using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Text.Json;

namespace DAL.Repositories.Item
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
            // Get selected attributes valus IDs
            List<Guid> selectedAttributeValueIds = selectedAttributes.Select(a => a.CombinationAttributeValueId).ToList();
            // Determine the last selected attribute
            var selectedAttributeValues =await _dbContext.TbCombinationAttributesValues.AsNoTracking().Where(c=> selectedAttributeValueIds.Contains(c.Id)).ToListAsync() ;
            var lastAttributeSelectionValue = selectedAttributes.FirstOrDefault(a => a.IsLastSelected) ?? selectedAttributes.First();
            var lastSelectedAttribute = selectedAttributeValues.FirstOrDefault(ca => ca.Id == lastAttributeSelectionValue.CombinationAttributeValueId) 
                 ?? throw new DataAccessException("Invalid attribute value selected", null, _logger);

            // Get ItemCombinationId based on selected attributes
            Guid ItemCombinationId  ;
            if (selectedAttributeValues.TrueForAll(s => s.ItemCombinationId == lastSelectedAttribute.ItemCombinationId))
                ItemCombinationId = lastSelectedAttribute.ItemCombinationId;
            else
                ItemCombinationId = lastSelectedAttribute.ItemCombinationId;

            var parameters = new[]
            {
                new SqlParameter("@ItemCombinationId", SqlDbType.UniqueIdentifier) { Value = ItemCombinationId }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetItemDetails>("SpGetItemDetails", default, parameters))
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting combination by attributes for this selection ");
                throw new DataAccessException("Failed to retrieve combination details", ex, _logger);
            }
        }
    }
}