using BL.Contracts.Service.ECommerce.Item;
using DAL.ApplicationContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.ECommerce.Item;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Service.ECommerce.Item
{
    /// <summary>
    /// Implementation of IItemSearchService
    /// Executes SpSearchItemsMultiVendor stored procedure for optimal search performance
    /// </summary>
    public class ItemSearchService : IItemSearchService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ItemSearchService(ApplicationDbContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Execute stored procedure search with comprehensive filtering
        /// </summary>
        public async Task<PagedSpSearchResultDto> SearchItemsAsync(ItemFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            ValidateFilter(filter);

            try
            {
                // Convert list parameters to comma-separated strings
                var categoryIds = filter.CategoryIds?.Any() == true
                    ? string.Join(",", filter.CategoryIds)
                    : null;

                var brandIds = filter.BrandIds?.Any() == true
                    ? string.Join(",", filter.BrandIds)
                    : null;

                var vendorIds = filter.VendorIds?.Any() == true
                    ? string.Join(",", filter.VendorIds.Select(v => $"'{v}'"))
                    : null;

                var storageLocations = filter.StorageLocations?.Any() == true
                    ? string.Join(",", filter.StorageLocations.Cast<int>())
                    : null;

                var conditionIds = filter.ConditionIds?.Any() == true
                    ? string.Join(",", filter.ConditionIds)
                    : null;

                // Prepare SQL parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@SearchTerm", (object)filter.SearchTerm ?? DBNull.Value),
                    new SqlParameter("@CategoryIds", (object)categoryIds ?? DBNull.Value),
                    new SqlParameter("@BrandIds", (object)brandIds ?? DBNull.Value),
                    new SqlParameter("@MinPrice", (object)filter.MinPrice ?? DBNull.Value),
                    new SqlParameter("@MaxPrice", (object)filter.MaxPrice ?? DBNull.Value),
                    new SqlParameter("@VendorIds", (object)vendorIds ?? DBNull.Value),
                    new SqlParameter("@InStockOnly", filter.InStockOnly ?? false),
                    new SqlParameter("@FreeShippingOnly", filter.FreeShippingOnly ?? false),
                    new SqlParameter("@OnSaleOnly", filter.OnSaleOnly ?? false),
                    new SqlParameter("@BuyBoxWinnersOnly", filter.BuyBoxWinnersOnly ?? false),
                    new SqlParameter("@MaxDeliveryDays", (object)filter.MaxDeliveryDays ?? DBNull.Value),
                    new SqlParameter("@SortBy", filter.SortBy ?? "newest"),
                    new SqlParameter("@PageNumber", filter.PageNumber),
                    new SqlParameter("@PageSize", filter.PageSize)
                };

                var results = new List<SpSearchItemsResultDto>();
                int totalRecords = 0;

                // Execute stored procedure
                var connection = _context.Database.GetDbConnection();

                try
                {
                    await connection.OpenAsync();

                    using var command = connection.CreateCommand();
                    command.CommandText = "SpSearchItemsMultiVendor";
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 30;

                    // Add parameters
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    using var reader = await command.ExecuteReaderAsync();

                    // Read first result set (search results)
                    while (await reader.ReadAsync())
                    {
                        var item = MapToSearchResult(reader);
                        results.Add(item);
                    }

                    // Read second result set (total count)
                    if (reader is SqlDataReader sqlReader)
                    {
                        if (sqlReader.NextResult() && await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        await connection.CloseAsync();
                    }
                }

                return new PagedSpSearchResultDto
                {
                    Items = results,
                    TotalCount = totalRecords,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = totalRecords > 0 ? (int)Math.Ceiling(totalRecords / (double)filter.PageSize) : 0
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error executing SpSearchItemsMultiVendor stored procedure");
                throw;
            }
        }

        /// <summary>
        /// Get available filter options based on current search criteria
        /// </summary>
        public async Task<AvailableSearchFiltersDto> GetAvailableFiltersAsync(ItemFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            try
            {
                // This would be implemented with a dedicated stored procedure
                // or LINQ query for filter aggregation
                // For now, returning empty filters
                // TODO: Implement GetAvailableFilters stored procedure

                return new AvailableSearchFiltersDto
                {
                    Categories = new List<FilterOptionDto>(),
                    Brands = new List<FilterOptionDto>(),
                    Vendors = new List<FilterOptionDto>(),
                    PriceRange = new PriceRangeDto()
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting available filters");
                throw;
            }
        }

        /// <summary>
        /// Get best prices for multiple items
        /// </summary>
        public async Task<List<object>> GetItemBestPricesAsync(List<Guid> itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<object>();

            try
            {
                // TODO: Implement best prices query or stored procedure
                // For now returning empty list
                return new List<object>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting best prices");
                throw;
            }
        }

        /// <summary>
        /// Map stored procedure result to DTO
        /// </summary>
        private SpSearchItemsResultDto MapToSearchResult(IDataReader reader)
        {
            var result = new SpSearchItemsResultDto
            {
                ItemId = reader.GetGuid(reader.GetOrdinal("ItemId")),
                TitleAr = GetString(reader, "TitleAr"),
                TitleEn = GetString(reader, "TitleEn"),
                ShortDescriptionAr = GetString(reader, "ShortDescriptionAr"),
                ShortDescriptionEn = GetString(reader, "ShortDescriptionEn"),
                CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                BrandId = GetNullableGuid(reader, "BrandId"),
                ThumbnailImage = GetString(reader, "ThumbnailImage"),
                CreatedDateUtc = reader.GetDateTime(reader.GetOrdinal("CreatedDateUtc")),
                MinPrice = reader.GetDecimal(reader.GetOrdinal("MinPrice")),
                MaxPrice = reader.GetDecimal(reader.GetOrdinal("MaxPrice")),
                OffersCount = reader.GetInt32(reader.GetOrdinal("OffersCount")),
                FastestDelivery = reader.GetInt32(reader.GetOrdinal("FastestDelivery"))
            };

            // Parse BestOfferData
            var bestOfferData = GetString(reader, "BestOfferData");
            if (!string.IsNullOrEmpty(bestOfferData))
            {
                result.BestOffer = ParseBestOfferData(bestOfferData);
            }

            return result;
        }

        /// <summary>
        /// Parse best offer data from pipe-separated string
        /// Format: OfferId|VendorId|SalesPrice|OriginalPrice|AvailableQuantity|IsFreeShipping|EstimatedDeliveryDays
        /// </summary>
        private BestOfferDetailsDto ParseBestOfferData(string bestOfferData)
        {
            try
            {
                var parts = bestOfferData.Split('|');
                if (parts.Length < 7)
                    return null;

                var salesPrice = decimal.Parse(parts[2]);
                var originalPrice = decimal.Parse(parts[3]);

                var offer = new BestOfferDetailsDto
                {
                    OfferId = Guid.Parse(parts[0]),
                    VendorId = Guid.Parse(parts[1]),
                    SalesPrice = salesPrice,
                    OriginalPrice = originalPrice,
                    AvailableQuantity = int.Parse(parts[4]),
                    IsFreeShipping = parts[5] == "1",
                    EstimatedDeliveryDays = int.Parse(parts[6])
                };

                // Calculate discount percentage
                if (originalPrice > salesPrice && originalPrice > 0)
                {
                    offer.DiscountPercentage = Math.Round(
                        ((originalPrice - salesPrice) / originalPrice) * 100, 2);
                }

                return offer;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error parsing BestOfferData: {BestOfferData}", bestOfferData);
                return null;
            }
        }

        /// <summary>
        /// Get string value safely from reader
        /// </summary>
        private string GetString(IDataReader reader, string columnName)
        {
            try
            {
                var ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get nullable GUID value safely from reader
        /// </summary>
        private Guid? GetNullableGuid(IDataReader reader, string columnName)
        {
            try
            {
                var ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetGuid(ordinal);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Validate filter parameters
        /// </summary>
        private void ValidateFilter(ItemFilterDto filter)
        {
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;

            if (filter.PageSize < 1 || filter.PageSize > 100)
                filter.PageSize = 20;

            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
            {
                throw new ArgumentException("MinPrice cannot be greater than MaxPrice");
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = filter.SortBy.ToLower();
            }
        }
    }
}
