using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Exceptions;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace DAL.Repositories.Catalog.Item
{
    public class ItemDetailsRepository : Repository<SpGetItemDetails>, IItemDetailsRepository
    {
        public ItemDetailsRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        /// <summary>
        /// Get item details - matches search result price
        /// </summary>
        public async Task<SpGetItemDetails> GetItemDetailsAsync(
            Guid itemCombinationId,
            CancellationToken cancellationToken = default)
        {
            if (itemCombinationId == Guid.Empty)
            {
                throw new ArgumentException("ItemCombinationId cannot be empty", nameof(itemCombinationId));
            }

            var parameters = new[]
            {
                new SqlParameter("@ItemCombinationId", SqlDbType.UniqueIdentifier) { Value = itemCombinationId }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetItemDetails>(
                    "SpGetItemDetails",
                    cancellationToken,  // ✅ ADDED: Pass cancellation token
                    parameters))
                    .FirstOrDefault();

                // ✅ ADDED: Explicit null check with exception
                if (result == null)
                {
                    throw new DataAccessException(
                        $"Item not found for ItemCombinationId: {itemCombinationId}",
                        null,
                        _logger);
                }

                return result;
            }
            catch (SqlException ex) when (ex.Number == 50000) // RAISERROR from SP
            {
                _logger.Warning(ex, "Item not found or invalid: {ItemCombinationId}", itemCombinationId);
                throw new DataAccessException("Item not found or is not active", ex, _logger);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting item details for ItemCombinationId: {ItemCombinationId}", itemCombinationId);
                throw new DataAccessException("Failed to retrieve item details", ex, _logger);
            }
        }

        /// <summary>
        /// Get combination details by selected attributes
        /// </summary>
        public async Task<SpGetItemDetails> GetCombinationByAttributesAsync(
            List<AttributeSelection> selectedAttributes,
            CancellationToken cancellationToken = default)
        {
            // ✅ ENHANCED: Better input validation
            if (selectedAttributes == null || !selectedAttributes.Any())
            {
                throw new ArgumentException("At least one attribute must be selected", nameof(selectedAttributes));
            }

            // Get selected attributes values IDs
            var selectedAttributeValueIds = selectedAttributes.Select(a => a.CombinationAttributeValueId).ToList();
            int expectedMatchCount = selectedAttributeValueIds.Count;

            // ✅ IMPROVED: Using nullable Guid instead of Guid.Empty
            Guid? itemCombinationId = null;

            try
            {
                // Find combination that matches ALL selected attribute values using a single optimized query
                itemCombinationId = await _dbContext.TbCombinationAttributes
                    .AsNoTracking()
                    .Where(c => selectedAttributeValueIds.Contains(c.AttributeValueId) && !c.IsDeleted)
                    .GroupBy(c => c.ItemCombinationId)
                    .Where(g => g.Count() == expectedMatchCount)
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync(cancellationToken);  // ✅ ADDED: Pass cancellation token

                // ✅ IMPROVED: Better null checking
                if (!itemCombinationId.HasValue)
                {
                    // If no exact match, get the last selected attribute's combination
                    var lastAttributeSelectionValue = selectedAttributes.FirstOrDefault(a => a.IsLastSelected)
                        ?? selectedAttributes.Last();

                    itemCombinationId = await _dbContext.TbCombinationAttributes
                        .AsNoTracking()
                        .Where(ca => ca.AttributeValueId == lastAttributeSelectionValue.CombinationAttributeValueId
                                  && !ca.IsDeleted)
                        .Select(ca => ca.ItemCombinationId)
                        .FirstOrDefaultAsync(cancellationToken);  // ✅ ADDED: Pass cancellation token

                    if (!itemCombinationId.HasValue)
                    {
                        throw new DataAccessException(
                            "Invalid attribute value selected - no matching combination found",
                            null,
                            _logger);
                    }
                }

                // ✅ IMPROVED: Validate combination exists and is not deleted
                var combinationExists = await _dbContext.TbItemCombinations
                    .AsNoTracking()
                    .AnyAsync(ic => ic.Id == itemCombinationId.Value && !ic.IsDeleted, cancellationToken);

                if (!combinationExists)
                {
                    throw new DataAccessException(
                        "Selected combination is not available",
                        null,
                        _logger);
                }

                // Call the main method with the found combination ID
                return await GetItemDetailsAsync(itemCombinationId.Value, cancellationToken);
            }
            catch (DataAccessException)
            {
                // Re-throw DataAccessException as-is
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting combination by attributes. Selected IDs: {AttributeValueIds}",
                    string.Join(", ", selectedAttributeValueIds));
                throw new DataAccessException("Failed to retrieve combination details", ex, _logger);
            }
        }

        /// <summary>
        /// ✅ NEW METHOD: Validate that all selected attributes belong to the same item
        /// </summary>
        private async Task<bool> ValidateAttributesBelongToSameItemAsync(
            List<Guid> attributeValueIds,
            CancellationToken cancellationToken = default)
        {
            var itemIds = await _dbContext.TbCombinationAttributes
                .AsNoTracking()
                .Where(ca => attributeValueIds.Contains(ca.AttributeValueId) && !ca.IsDeleted)
                .Join(_dbContext.TbItemCombinations,
                    ca => ca.ItemCombinationId,
                    ic => ic.Id,
                    (ca, ic) => ic.ItemId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return itemIds.Count == 1;
        }

        /// <summary>
        /// ✅ NEW METHOD: Get available attribute values for progressive selection
        /// </summary>
        public async Task<List<Guid>> GetAvailableAttributeValuesAsync(
            Guid itemId,
            List<Guid> selectedAttributeValueIds,
            CancellationToken cancellationToken = default)
        {
            if (itemId == Guid.Empty)
            {
                throw new ArgumentException("ItemId cannot be empty", nameof(itemId));
            }

            try
            {
                // Get combinations that match already selected attributes (if any)
                IQueryable<Guid> availableCombinations = _dbContext.TbItemCombinations
                    .AsNoTracking()
                    .Where(ic => ic.ItemId == itemId && !ic.IsDeleted)
                    .Select(ic => ic.Id);

                if (selectedAttributeValueIds != null && selectedAttributeValueIds.Any())
                {
                    // Filter combinations that have all selected attributes
                    foreach (var selectedValueId in selectedAttributeValueIds)
                    {
                        availableCombinations = availableCombinations
                            .Where(combinationId => _dbContext.TbCombinationAttributes
                                .Any(ca => ca.ItemCombinationId == combinationId
                                        && ca.AttributeValueId == selectedValueId
                                        && !ca.IsDeleted));
                    }
                }

                // Get all attribute values from these combinations
                var availableValueIds = await _dbContext.TbCombinationAttributes
                    .AsNoTracking()
                    .Where(ca => availableCombinations.Contains(ca.ItemCombinationId) && !ca.IsDeleted)
                    .Select(ca => ca.AttributeValueId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                return availableValueIds;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting available attribute values for ItemId: {ItemId}", itemId);
                throw new DataAccessException("Failed to retrieve available attribute values", ex, _logger);
            }
        }
    }
}