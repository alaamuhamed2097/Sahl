using DAL.ApplicationContext;
using DAL.Repositories.Item;
using Domains.Entities.Catalog.Item;
using Domains.Views.Item;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.Item
{
    /// <summary>
    /// Repository implementation for advanced multi-vendor item search operations
    /// Uses stored procedures and views for optimal performance
    /// </summary>
    public class ItemSearchRepository : IItemSearchRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemSearchRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Search items using the SpSearchItemsMultiVendor stored procedure
        /// Handles full-text search, multi-criteria filtering, sorting, and pagination
        /// </summary>
        public async Task<PagedResult<ItemSearchResultDto>> SearchItemsAsync(ItemSearchFilterDto filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            ValidateFilter(filter);

            // Convert lists to comma-separated strings for SQL parameters
            var categoryIds = filter.CategoryIds?.Any() == true
                ? string.Join(",", filter.CategoryIds)
                : null;

            var brandIds = filter.BrandIds?.Any() == true
                ? string.Join(",", filter.BrandIds)
                : null;

            var vendorIds = filter.VendorIds?.Any() == true
                ? string.Join(",", filter.VendorIds)
                : null;

            // Prepare SQL parameters
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

            var results = new List<ItemSearchResultDto>();
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
                    var result = MapToItemSearchResult(reader);
                    results.Add(result);
                }

                // Read second result set (total count)
                using (reader)
                {
                    var hasNextSet = false;
                    try
                    {
                        // Try to get the next result set
                        if (reader is SqlDataReader sqlReader)
                        {
                            hasNextSet = sqlReader.NextResult();
                        }
                    }
                    catch
                    {
                        // If it fails, totalRecords remains 0
                    }

                    if (hasNextSet && await reader.ReadAsync())
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

            return new PagedResult<ItemSearchResultDto>
            {
                Items = results,
                TotalCount = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = totalRecords > 0 ? (int)Math.Ceiling(totalRecords / (double)filter.PageSize) : 0
            };
        }

        /// <summary>
        /// Get best prices for multiple items using the VwItemBestPrices denormalized view
        /// Optimized for quick catalog price displays with minimal queries
        /// </summary>
        public async Task<List<VwItemBestPrice>> GetItemBestPricesAsync(List<Guid> itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<VwItemBestPrice>();

            return await _context.Set<VwItemBestPrice>()
                .Where(p => itemIds.Contains(p.ItemId))
                .ToListAsync();
        }

        /// <summary>
        /// Get available filter options based on search results
        /// </summary>
        public async Task<AvailableFiltersDto> GetAvailableFiltersAsync(ItemSearchFilterDto currentFilter)
        {
            if (currentFilter == null)
                throw new ArgumentNullException(nameof(currentFilter));

            // Build base query joining items with offers and pricing
            var query = from item in _context.TbItems
                        join offer in _context.TbOffers on item.Id equals offer.ItemId
                        join pricing in _context.TbOfferCombinationPricings on offer.Id equals pricing.OfferId
                        select new { item, offer, pricing };

            // Apply base filters
            query = query.Where(x => x.offer.VisibilityScope == 0);

            // Apply text search filter if provided
            if (!string.IsNullOrEmpty(currentFilter.SearchTerm))
            {
                var searchLower = currentFilter.SearchTerm.ToLower();
                query = query.Where(x =>
                    x.item.TitleAr.ToLower().Contains(searchLower) ||
                    x.item.TitleEn.ToLower().Contains(searchLower) ||
                    x.item.ShortDescriptionAr.ToLower().Contains(searchLower) ||
                    x.item.ShortDescriptionEn.ToLower().Contains(searchLower));
            }

            // Apply category filter
            if (currentFilter.CategoryIds?.Any() == true)
            {
                query = query.Where(x => currentFilter.CategoryIds.Contains(x.item.CategoryId));
            }

            // Apply brand filter
            if (currentFilter.BrandIds?.Any() == true)
            {
                query = query.Where(x => currentFilter.BrandIds.Contains(x.item.BrandId));
            }

            // Apply price filters
            if (currentFilter.MinPrice.HasValue)
            {
                query = query.Where(x => x.pricing.SalesPrice >= currentFilter.MinPrice.Value);
            }

            if (currentFilter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.pricing.SalesPrice <= currentFilter.MaxPrice.Value);
            }

            // Apply vendor filter
            if (currentFilter.VendorIds?.Any() == true)
            {
                query = query.Where(x => currentFilter.VendorIds.Contains(x.offer.VendorId));
            }

            // Apply stock filter
            if (currentFilter.InStockOnly == true)
            {
                query = query.Where(x => x.pricing.AvailableQuantity > 0);
            }

            // Get available categories
            var categories = await query
                .Select(x => new { x.item.CategoryId, x.item.Category.TitleAr, x.item.Category.TitleEn })
                .Distinct()
                .GroupBy(x => new { x.CategoryId, x.TitleAr, x.TitleEn })
                .Select(g => new FilterOptionDto
                {
                    Id = g.Key.CategoryId,
                    NameAr = g.Key.TitleAr,
                    NameEn = g.Key.TitleEn,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(50)
                .ToListAsync();

            // Get available brands
            var brands = await query
                .Where(x => x.item.BrandId != Guid.Empty)
                .Select(x => new { x.item.BrandId, x.item.Brand.NameAr, x.item.Brand.NameEn })
                .Distinct()
                .GroupBy(x => new { x.BrandId, x.NameAr, x.NameEn })
                .Select(g => new FilterOptionDto
                {
                    Id = g.Key.BrandId,
                    NameAr = g.Key.NameAr,
                    NameEn = g.Key.NameEn,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(50)
                .ToListAsync();

            // Get price range statistics
            var priceStats = await query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    MinPrice = g.Min(x => x.pricing.SalesPrice),
                    MaxPrice = g.Max(x => x.pricing.SalesPrice),
                    AvgPrice = g.Average(x => x.pricing.SalesPrice)
                })
                .FirstOrDefaultAsync();

            return new AvailableFiltersDto
            {
                Categories = categories,
                Brands = brands,
                PriceRange = priceStats != null ? new PriceRangeDto
                {
                    MinPrice = priceStats.MinPrice,
                    MaxPrice = priceStats.MaxPrice,
                    AvgPrice = priceStats.AvgPrice
                } : null
            };
        }

        /// <summary>
        /// Maps DataReader result to ItemSearchResultDto
        /// </summary>
        private ItemSearchResultDto MapToItemSearchResult(IDataReader reader)
        {
            var result = new ItemSearchResultDto
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
                    ? (Guid?)null
                    : reader.GetGuid(reader.GetOrdinal("BrandId")),
                ThumbnailImage = reader.GetString(reader.GetOrdinal("ThumbnailImage")),
                CreatedDateUtc = reader.GetDateTime(reader.GetOrdinal("CreatedDateUtc")),
                MinPrice = reader.GetDecimal(reader.GetOrdinal("MinPrice")),
                MaxPrice = reader.GetDecimal(reader.GetOrdinal("MaxPrice")),
                OffersCount = reader.GetInt32(reader.GetOrdinal("OffersCount")),
                FastestDelivery = reader.GetInt32(reader.GetOrdinal("FastestDelivery"))
            };

            // Parse BestOfferData
            var bestOfferData = reader.GetString(reader.GetOrdinal("BestOfferData"));
            if (!string.IsNullOrEmpty(bestOfferData))
            {
                try
                {
                    var parts = bestOfferData.Split('|');
                    if (parts.Length >= 7)
                    {
                        var salesPrice = decimal.Parse(parts[2]);
                        var originalPrice = decimal.Parse(parts[3]);

                        result.BestOffer = new BestOfferDto
                        {
                            OfferId = Guid.Parse(parts[0]),
                            VendorId = Guid.Parse(parts[1]),
                            Price = salesPrice,
                            OriginalPrice = originalPrice,
                            AvailableQuantity = int.Parse(parts[4]),
                            IsFreeShipping = parts[5] == "1",
                            EstimatedDeliveryDays = int.Parse(parts[6])
                        };

                        if (originalPrice > salesPrice && originalPrice > 0)
                        {
                            result.BestOffer.DiscountPercentage = Math.Round(
                                ((originalPrice - salesPrice) / originalPrice) * 100, 2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing BestOfferData: {ex.Message}");
                }
            }

            return result;
        }

        /// <summary>
        /// Validates search filter parameters
        /// </summary>
        private void ValidateFilter(ItemSearchFilterDto filter)
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
