using Common.Models.Filters;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models.ItemSearch;
using Domains.Views.Item;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;

namespace DAL.Repositories;

/// <summary>
/// Implementation of IItemSearchRepository
/// Uses SpSearchItemsMultiVendor stored procedure for optimal performance
/// Returns domain entities, not DTOs
/// </summary>
public class ItemSearchRepository : IItemSearchRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;

    public ItemSearchRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Execute stored procedure search and return domain entities
    /// </summary>
    public async Task<(List<SpSearchItemsMultiVendor> Items, int TotalCount)> SearchItemsAsync(
     ItemFilterDto filter,
     CancellationToken cancellationToken = default)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));

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
                ? string.Join(",", filter.VendorIds)
                : null;

            var conditionIds = filter.ConditionIds?.Any() == true
                ? string.Join(",", filter.ConditionIds)
                : null;

            // Convert attribute filter to pipe-separated format
            string attributeIds = null;
            string attributeValues = null;
            if (filter.AttributeValues?.Any() == true)
            {
                var attrIdList = new List<string>();
                var attrValueList = new List<string>();

                foreach (var kvp in filter.AttributeValues)
                {
                    attrIdList.Add(kvp.Key.ToString());
                    attrValueList.Add(string.Join(",", kvp.Value));
                }

                attributeIds = string.Join(",", attrIdList);
                attributeValues = string.Join("|", attrValueList);
            }

            // Prepare ALL SQL parameters
            var parameters = new[]
            {
            // Item-level filters
            new SqlParameter("@SearchTerm", (object)filter.SearchTerm ?? DBNull.Value),
            new SqlParameter("@CategoryIds", (object)categoryIds ?? DBNull.Value),
            new SqlParameter("@BrandIds", (object)brandIds ?? DBNull.Value),
            
            // Price filters
            new SqlParameter("@MinPrice", (object)filter.MinPrice ?? DBNull.Value),
            new SqlParameter("@MaxPrice", (object)filter.MaxPrice ?? DBNull.Value),
            
            // Rating filters
            new SqlParameter("@MinItemRating", (object)filter.MinItemRating ?? DBNull.Value),
            
            // Availability filters
            new SqlParameter("@InStockOnly", filter.InStockOnly ?? false),
            
            // Shipping filters
            new SqlParameter("@FreeShippingOnly", filter.FreeShippingOnly ?? false),
            
            // Vendor filters
            new SqlParameter("@VendorIds", (object)vendorIds ?? DBNull.Value),
            
            // Condition and warranty filters
            new SqlParameter("@ConditionIds", (object)conditionIds ?? DBNull.Value),
            new SqlParameter("@WithWarrantyOnly", filter.WithWarrantyOnly ?? false),
            
            // Attribute filters
            new SqlParameter("@AttributeIds", (object)attributeIds ?? DBNull.Value),
            new SqlParameter("@AttributeValues", (object)attributeValues ?? DBNull.Value),
            
            // Sorting
            new SqlParameter("@SortBy", filter.SortBy ?? "relevance"),
            
            // Pagination
            new SqlParameter("@PageNumber", filter.PageNumber),
            new SqlParameter("@PageSize", filter.PageSize)
        };

            var results = new List<SpSearchItemsMultiVendor>();
            int totalRecords = 0;

            // Execute stored procedure
            var connection = _context.Database.GetDbConnection();

            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync(cancellationToken);

                using var command = connection.CreateCommand();
                command.CommandText = "SpSearchItemsMultiVendor";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30;

                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using var reader = await command.ExecuteReaderAsync(cancellationToken);

                // Read first result set (search results)
                while (await reader.ReadAsync(cancellationToken))
                {
                    var item = MapToSearchResultEntity(reader);
                    results.Add(item);
                }

                // Read second result set (total count)
                if (await reader.NextResultAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
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

            return (results, totalRecords);
        }
        catch (SqlException sqlEx)
        {
            _logger.Error(sqlEx, "SQL error executing SpSearchItemsMultiVendor");
            throw new DataAccessException(
                "Database error occurred during search operation",
                sqlEx,
                _logger);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error executing SpSearchItemsMultiVendor stored procedure");
            throw new DataAccessException(
                "Failed to execute search operation",
                ex,
                _logger);
        }
    }

    /// <summary>
    /// Get available filter options
    /// </summary>
    public async Task<AvailableFiltersData> GetAvailableFiltersAsync(
        ItemFilterDto currentFilter,
        CancellationToken cancellationToken = default)
    {
        if (currentFilter == null)
            throw new ArgumentNullException(nameof(currentFilter));

        try
        {
            // Build base query with existing filters
            var query = from item in _context.TbItems
                        join offer in _context.TbOffers on item.Id equals offer.ItemId
                        join pricing in _context.TbOfferCombinationPricings on offer.Id equals pricing.OfferId
                        where item.IsActive && !item.IsDeleted
                            && offer.VisibilityScope == Common.Enumerations.Offer.OfferVisibilityScope.Active
                            && !offer.IsDeleted
                            && !pricing.IsDeleted
                        select new { item, offer, pricing };

            // Apply existing filters (except the one we're calculating)
            if (!string.IsNullOrEmpty(currentFilter.SearchTerm))
            {
                var searchLower = currentFilter.SearchTerm.ToLower();
                query = query.Where(x =>
                    x.item.TitleAr.ToLower().Contains(searchLower) ||
                    x.item.TitleEn.ToLower().Contains(searchLower));
            }

            if (currentFilter.MinPrice.HasValue)
                query = query.Where(x => x.pricing.SalesPrice >= currentFilter.MinPrice);

            if (currentFilter.MaxPrice.HasValue)
                query = query.Where(x => x.pricing.SalesPrice <= currentFilter.MaxPrice);

            // Get available categories with counts
            var categories = await query
                .GroupBy(x => new { x.item.CategoryId, x.item.Category.TitleAr, x.item.Category.TitleEn })
                .Select(g => new FilterCategoryOption
                {
                    Id = g.Key.CategoryId,
                    NameAr = g.Key.TitleAr,
                    NameEn = g.Key.TitleEn,
                    Count = g.Select(x => x.item.Id).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(20)
                .ToListAsync(cancellationToken);

            // Get available brands with counts
            var brands = await query
                .Where(x => x.item.BrandId != null)
                .GroupBy(x => new { x.item.BrandId, x.item.Brand.NameAr, x.item.Brand.NameEn })
                .Select(g => new FilterBrandOption
                {
                    Id = g.Key.BrandId,
                    NameAr = g.Key.NameAr,
                    NameEn = g.Key.NameEn,
                    Count = g.Select(x => x.item.Id).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(20)
                .ToListAsync(cancellationToken);

            // Get price range
            var priceStats = await query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    MinPrice = g.Min(x => x.pricing.SalesPrice),
                    MaxPrice = g.Max(x => x.pricing.SalesPrice)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return new AvailableFiltersData
            {
                Categories = categories,
                Brands = brands,
                Vendors = new List<FilterVendorOption>(),
                MinPrice = priceStats?.MinPrice,
                MaxPrice = priceStats?.MaxPrice
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting available filters");
            throw new DataAccessException("Failed to retrieve available filters", ex, _logger);
        }
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
    /// Map DataReader to SpSearchItemsMultiVendor entity
    /// </summary>
    private SpSearchItemsMultiVendor MapToSearchResultEntity(IDataReader reader)
    {
        var result = new SpSearchItemsMultiVendor
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
            FinalScore = GetDouble(reader, "FinalScore")
        };

        // Calculate average price
        result.AvgPrice = (result.MinPrice + result.MaxPrice) / 2;

        // Parse BestOfferData
        var bestOfferData = GetString(reader, "BestOfferData");
        if (!string.IsNullOrEmpty(bestOfferData))
        {
            // Store raw data - BL will parse and transform
            result.BestOfferDataRaw = bestOfferData;
        }

        return result;
    }

    private string GetString(IDataReader reader, string columnName)
    {
        try
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
        catch { return null; }
    }

    private Guid? GetNullableGuid(IDataReader reader, string columnName)
    {
        try
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetGuid(ordinal);
        }
        catch { return null; }
    }

    private double GetDouble(IDataReader reader, string columnName)
    {
        try
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDouble(ordinal);
        }
        catch { return 0; }
    }
}