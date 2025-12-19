using Common.Filters;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Domains.Views.Item;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Text.Json;

namespace DAL.Repositories.Item
{
    /// <summary>
    /// Repository implementation for advanced multi-vendor item search operations
    /// Uses stored procedures and views for optimal performance
    /// </summary>
    public class ItemSearchRepository : Repository<SpGetAvailableSearchFilters>, IItemSearchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ItemSearchRepository(ApplicationDbContext context, ILogger logger)
            : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Search items using the SpSearchItemsMultiVendor stored procedure
        /// Handles full-text search, multi-criteria filtering, sorting, and pagination
        /// </summary>
        public async Task<AdvancedPagedResult<SpSearchItemsMultiVendor>> SearchItemsAsync(ItemFilterQuery filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            ValidateFilter(filter);

            // Handle attribute filters - convert to comma-separated and pipe-separated strings
            var attributeIds = filter.AttributeIds?.Any() == true
                ? string.Join(",", filter.AttributeIds)
                : null;

            var attributeValues = filter.AttributeValues?.Any() == true
                ? string.Join("|", filter.AttributeValues)
                : null;

            // Prepare SQL parameters matching the stored procedure signature
            var parameters = new[]
            {
                new SqlParameter("@SearchTerm", (object)filter.SearchTerm ?? DBNull.Value),
                new SqlParameter("@CategoryId", (object)filter.CategoryId ?? DBNull.Value),
                new SqlParameter("@VendorId", (object)filter.VendorId ?? DBNull.Value),
                new SqlParameter("@BrandId", (object)filter.BrandId ?? DBNull.Value),
                new SqlParameter("@MinPrice", (object)filter.MinPrice ?? DBNull.Value),
                new SqlParameter("@MaxPrice", (object)filter.MaxPrice ?? DBNull.Value),
                new SqlParameter("@MinItemRating", (object)filter.MinItemRating ?? DBNull.Value),
                new SqlParameter("@InStockOnly", filter.InStockOnly ?? false),
                new SqlParameter("@FreeShippingOnly", filter.FreeShippingOnly ?? false),
                new SqlParameter("@ConditionId", (object)filter.ConditionId ?? DBNull.Value),
                new SqlParameter("@WithWarrantyOnly", filter.WithWarrantyOnly ?? false),
                new SqlParameter("@AttributeIds", (object)attributeIds ?? DBNull.Value),
                new SqlParameter("@AttributeValues", (object)attributeValues ?? DBNull.Value),
                new SqlParameter("@SortBy", filter.SortBy ?? "relevance"),
                new SqlParameter("@PageNumber", filter.PageNumber),
                new SqlParameter("@PageSize", filter.PageSize)
            };

            var results = new List<SpSearchItemsMultiVendor>();
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

                // Read the result set (search results with TotalRecords in each row)
                while (await reader.ReadAsync())
                {
                    var result = MapToSearchEntity(reader);
                    results.Add(result);

                    // Get total records from first row (it's in every row)
                    if (totalRecords == 0)
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

            // Return using the same pattern as the original code
            return new AdvancedPagedResult<SpSearchItemsMultiVendor>
            {
                Items = results,
                TotalRecords = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize)
            };
        }

        /// <summary>
        /// Get available filter options
        /// </summary>
        public async Task<SearchFilters> GetAvailableFiltersAsync(
        AvailableFiltersQuery filtersQuery)
        {
            var parameters = new[]
            {
            new SqlParameter("@SearchTerm", filtersQuery.SearchTerm ?? (object)DBNull.Value),
            new SqlParameter("@CategoryId", filtersQuery.CategoryId ?? (object)DBNull.Value),
            new SqlParameter("@VendorId", filtersQuery.VendorId ?? (object)DBNull.Value)
        };

            var result = (await ExecuteStoredProcedureAsync<SpGetAvailableSearchFilters>("SpGetAvailableSearchFilters", default, parameters))
                .FirstOrDefault();

            if (result == null)
                return new SearchFilters();

            // Parse JSON
            return new SearchFilters
            {
                Categories = string.IsNullOrEmpty(result.CategoriesJson)
                    ? new List<CategoryFilter>()
                    : JsonSerializer.Deserialize<List<CategoryFilter>>(result.CategoriesJson),

                Brands = string.IsNullOrEmpty(result.BrandsJson)
                    ? new List<BrandFilter>()
                    : JsonSerializer.Deserialize<List<BrandFilter>>(result.BrandsJson),

                Vendors = string.IsNullOrEmpty(result.VendorsJson)
                    ? new List<VendorFilter>()
                    : JsonSerializer.Deserialize<List<VendorFilter>>(result.VendorsJson),

                PriceRange = string.IsNullOrEmpty(result.PriceRangeJson)
                    ? new PriceRangeFilter()
                    : JsonSerializer.Deserialize<PriceRangeFilter>(result.PriceRangeJson),

                Attributes = string.IsNullOrEmpty(result.AttributesJson)
                    ? new List<AttributeFilter>()
                    : JsonSerializer.Deserialize<List<AttributeFilter>>(result.AttributesJson),

                Conditions = string.IsNullOrEmpty(result.ConditionsJson)
                    ? new List<ConditionFilter>()
                    : JsonSerializer.Deserialize<List<ConditionFilter>>(result.ConditionsJson)
            };
        }

        /// <summary>
        /// Get best prices - returns view entities
        /// </summary>
        public async Task<List<VwItemBestPrice>> GetItemBestPricesAsync(
            List<Guid> itemIds,
            CancellationToken cancellationToken = default)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<VwItemBestPrice>();

            try
            {
                return await _context.VwItemBestPrices
                    .Where(x => itemIds.Contains(x.ItemId))
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting best prices for items");
                throw new DataAccessException("Failed to retrieve best prices", ex, _logger);
            }
        }

        /// <summary>
        /// Maps DataReader result to SpSearchItemsMultiVendor domain entity
        /// Matches exact stored procedure output columns
        /// </summary>
        private SpSearchItemsMultiVendor MapToSearchEntity(IDataReader reader)
        {
            return new SpSearchItemsMultiVendor
            {
                ItemId = reader.GetGuid(reader.GetOrdinal("ItemId")),
                TitleAr = reader.GetString(reader.GetOrdinal("TitleAr")),
                TitleEn = reader.GetString(reader.GetOrdinal("TitleEn")),
                ShortDescriptionAr = reader.IsDBNull(reader.GetOrdinal("ShortDescriptionAr"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShortDescriptionAr")),
                ShortDescriptionEn = reader.IsDBNull(reader.GetOrdinal("ShortDescriptionEn"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("ShortDescriptionEn")),
                CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                BrandId = reader.IsDBNull(reader.GetOrdinal("BrandId"))
                    ? null
                    : reader.GetGuid(reader.GetOrdinal("BrandId")),
                BrandNameAr = reader.IsDBNull(reader.GetOrdinal("BrandNameAr"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("BrandNameAr")),
                BrandNameEn = reader.IsDBNull(reader.GetOrdinal("BrandNameEn"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("BrandNameEn")),
                ThumbnailImage = reader.GetString(reader.GetOrdinal("ThumbnailImage")),
                CreatedDateUtc = reader.GetDateTime(reader.GetOrdinal("CreatedDateUtc")),
                ItemRating = reader.IsDBNull(reader.GetOrdinal("AverageRating"))
                    ? null
                    : reader.GetDecimal(reader.GetOrdinal("AverageRating")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                SalesPrice = reader.GetDecimal(reader.GetOrdinal("SalesPrice")),
                AvailableQuantity = reader.GetInt32(reader.GetOrdinal("AvailableQuantity")),
                StockStatus = reader.GetString(reader.GetOrdinal("StockStatus")),
                IsFreeShipping = reader.GetBoolean(reader.GetOrdinal("IsFreeShipping"))
            };
        }

        /// <summary>
        /// Validates search filter parameters
        /// </summary>
        private void ValidateFilter(ItemFilterQuery filter)
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