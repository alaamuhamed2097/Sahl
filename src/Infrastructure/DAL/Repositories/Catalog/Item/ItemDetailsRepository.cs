using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Exceptions;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Text.Json;

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
        public async Task<SpGetItemDetails> GetItemDetailsAsync(Guid itemCombinationId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ItemCombinationId", SqlDbType.UniqueIdentifier) { Value = itemCombinationId }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetItemDetails>("SpGetItemDetails", default, parameters))
                .FirstOrDefault();

                return result;
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
        public async Task<SpGetItemDetails> GetCombinationByAttributesAsync(List<AttributeSelection> selectedAttributes)
        {
            if (selectedAttributes == null || !selectedAttributes.Any())
            {
                throw new ArgumentException("At least one attribute must be selected", nameof(selectedAttributes));
            }

            // Get selected attributes values IDs
            var selectedAttributeValueIds = selectedAttributes.Select(a => a.CombinationAttributeValueId).ToList();
            int expectedMatchCount = selectedAttributeValueIds.Count;

            // Find combination that matches ALL selected attribute values using a single optimized query
            var itemCombinationId = await _dbContext.TbCombinationAttributes.AsNoTracking()
                .Where(c => selectedAttributeValueIds.Contains(c.AttributeValueId))
                .GroupBy(c => c.ItemCombinationId)
                .Where(g => g.Count() == expectedMatchCount)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            // If no exact match, get the last selected attribute's combination
            if (itemCombinationId == Guid.Empty)
            {
                var lastAttributeSelectionValue = selectedAttributes.FirstOrDefault(a => a.IsLastSelected)
                    ?? selectedAttributes.Last();

                itemCombinationId = await _dbContext.TbCombinationAttributes.AsNoTracking()
                    .Where(ca => ca.AttributeValueId == lastAttributeSelectionValue.CombinationAttributeValueId)
                    .Select(ca => ca.ItemCombinationId)
                    .FirstOrDefaultAsync();

                if (itemCombinationId == Guid.Empty)
                {
                    throw new DataAccessException("Invalid attribute value selected", null, _logger);
                }
            }

            var parameters = new[]
            {
                new SqlParameter("@ItemCombinationId", SqlDbType.UniqueIdentifier) { Value = itemCombinationId }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetItemDetails>("SpGetItemDetails", default, parameters))
                    .FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting combination by attributes for this selection");
                throw new DataAccessException("Failed to retrieve combination details", ex, _logger);
            }
        }
    }
}