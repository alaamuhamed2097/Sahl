using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Microsoft.Data.SqlClient;
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
        public async Task<SpGetItemDetails> GetItemDetailsAsync(Guid itemId)
        {
            var parameters = new[]
            {
                new SqlParameter("@ItemId", SqlDbType.UniqueIdentifier) { Value = itemId }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetItemDetails>("SpGetItemDetails", default, parameters))
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting item details for ItemId: {ItemId}", itemId);
                throw new DataAccessException("Failed to retrieve item details", ex, _logger);
            }
        }

        /// <summary>
        /// Get combination details by selected attributes
        /// </summary>
        public async Task<SpGetAvailableOptionsForSelection> GetCombinationByAttributesAsync(
            Guid itemId,
            List<AttributeSelection> selectedAttributes)
        {
            if (selectedAttributes == null || !selectedAttributes.Any())
            {
                throw new ArgumentException("At least one attribute must be selected", nameof(selectedAttributes));
            }

            // Convert to JSON format expected by stored procedure
            var attributesJson = JsonSerializer.Serialize(
                selectedAttributes.Select(a => new { AttributeId = a.AttributeId, ValueId = a.ValueId })
            );

            var parameters = new[]
            {
                new SqlParameter("@ItemId", SqlDbType.UniqueIdentifier) { Value = itemId },
                new SqlParameter("@AttributeValuesJson", SqlDbType.NVarChar, -1) { Value = attributesJson }
            };

            try
            {
                var result = (await ExecuteStoredProcedureAsync<SpGetAvailableOptionsForSelection>
                    ("SpGetCombinationByAttributes", default, parameters)).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting combination by attributes for ItemId: {ItemId}", itemId);
                throw new DataAccessException("Failed to retrieve combination details", ex, _logger);
            }
        }
    }
}