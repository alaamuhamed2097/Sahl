using BL.Contracts.Service.ECommerce.Item;
using Common.Filters;
using DAL.Contracts.Repositories;
using Domains.Procedures;
using Serilog;
using Shared.DTOs.ECommerce.Item;

namespace BL.Service.ECommerce.Item
{
    /// <summary>
    /// Implementation of IItemSearchService
    /// Transforms domain entities from DAL to DTOs for presentation layer
    /// </summary>
    public class ItemSearchService : IItemSearchService
    {
        private readonly IItemSearchRepository _searchRepository;
        private readonly ILogger _logger;

        public ItemSearchService(
            IItemSearchRepository searchRepository,
            ILogger logger)
        {
            _searchRepository = searchRepository ?? throw new ArgumentNullException(nameof(searchRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedSpSearchResultDto> SearchItemsAsync(
            ItemFilterQuery filter,
            CancellationToken cancellationToken = default)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            try
            {
                ValidateAndSanitizeFilter(filter);

                var result = await _searchRepository.SearchItemsAsync(filter);

                var dtos = result.Items.Select(entity => MapEntityToDto(entity)).ToList();

                EnrichResultsWithBadges(dtos);

                return new PagedSpSearchResultDto
                {
                    Items = dtos,
                    TotalCount = result.TotalRecords,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = result.TotalPages
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SearchItemsAsync");
                throw;
            }
        }

        public async Task<AvailableSearchFiltersDto> GetAvailableFiltersAsync(
            AvailableFiltersQuery filtersQuery)
        {
            if (filtersQuery == null)
                throw new ArgumentNullException(nameof(filtersQuery));

            if (filtersQuery.SearchTerm == null && filtersQuery.CategoryId == null && filtersQuery.VendorId == null)
                throw new ArgumentNullException("At least one search parameter must be provided");

            try
            {
                var filterData = await _searchRepository.GetAvailableFiltersAsync(filtersQuery);

                return new AvailableSearchFiltersDto
                {
                    Categories = filterData.Categories.Select(c => new FilterOptionDto
                    {
                        Id = c.Id,
                        NameAr = c.TitleAr,
                        NameEn = c.TitleEn,
                        Count = c.ItemCount
                    }).ToList(),

                    Brands = filterData.Brands.Select(b => new FilterOptionDto
                    {
                        Id = b.Id,
                        NameAr = b.NameAr,
                        NameEn = b.NameEn,
                        Count = b.ItemCount
                    }).ToList(),

                    Vendors = filterData.Vendors.Select(v => new FilterOptionDto
                    {
                        Id = v.Id,
                        NameAr = v.StoreNameAr ?? v.StoreName,
                        NameEn = v.StoreName,
                        Count = v.ItemCount
                    }).ToList(),

                    PriceRange = new PriceRangeDto
                    {
                        MinPrice = filterData.PriceRange.MinPrice,
                        MaxPrice = filterData.PriceRange.MaxPrice
                    },

                    Attributes = filterData.Attributes.Select(a => new AttributeFilterDto
                    {
                        AttributeId = a.AttributeId,
                        NameAr = a.NameAr,
                        NameEn = a.NameEn,
                        DisplayOrder = a.DisplayOrder,
                        Values = a.Values.Select(v => new AttributeValueFilterDto
                        {
                            ValueId = v.ValueId,
                            ValueAr = v.ValueAr,
                            ValueEn = v.ValueEn,
                            Count = v.ItemCount
                        }).ToList()
                    }).ToList(),

                    Conditions = filterData.Conditions.Select(c => new FilterOptionDto
                    {
                        Id = c.Id,
                        NameAr = c.NameAr,
                        NameEn = c.NameEn,
                        Count = c.ItemCount
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetAvailableFiltersAsync");
                throw;
            }
        }

        public async Task<List<ItemBestPriceDto>> GetItemBestPricesAsync(
            List<Guid> itemIds,
            CancellationToken cancellationToken = default)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<ItemBestPriceDto>();

            try
            {
                var viewEntities = await _searchRepository.GetItemBestPricesAsync(itemIds);

                return viewEntities.Select(entity => new ItemBestPriceDto
                {
                    ItemId = entity.ItemId,
                    BestPrice = entity.BestPrice,
                    TotalStock = entity.TotalStock,
                    TotalOffers = entity.TotalOffers,
                    BuyBoxRatio = (double)entity.BuyBoxRatio
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetItemBestPricesAsync");
                throw;
            }
        }

        /// <summary>
        /// Map SpSearchItemsMultiVendor entity to SearchItemDto
        /// Now includes brand names
        /// </summary>
        private SearchItemDto MapEntityToDto(SpSearchItemsMultiVendor entity)
        {
            var dto = new SearchItemDto
            {
                ItemId = entity.ItemId,
                TitleAr = entity.TitleAr,
                TitleEn = entity.TitleEn,
                ShortDescriptionAr = entity.ShortDescriptionAr,
                ShortDescriptionEn = entity.ShortDescriptionEn,
                CategoryId = entity.CategoryId,
                BrandId = entity.BrandId,
                BrandNameAr = entity.BrandNameAr,
                BrandNameEn = entity.BrandNameEn,
                ThumbnailImage = entity.ThumbnailImage,
                CreatedDateUtc = entity.CreatedDateUtc,
                ItemRating = entity.ItemRating,
                Price = entity.Price,
                SalesPrice = entity.SalesPrice,
                AvailableQuantity = entity.AvailableQuantity,
                StockStatus = entity.StockStatus,
                IsFreeShipping = entity.IsFreeShipping
            };

            // Calculate discount percentage
            if (entity.Price > entity.SalesPrice && entity.Price > 0)
            {
                dto.DiscountPercentage = Math.Round(
                    ((entity.Price - entity.SalesPrice) / entity.Price) * 100, 2);
            }

            // Check if new (created within last 30 days)
            dto.IsNew = (DateTime.UtcNow - entity.CreatedDateUtc).TotalDays <= 30;

            return dto;
        }

        private void ValidateAndSanitizeFilter(ItemFilterQuery filter)
        {
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;

            if (filter.PageSize < 1 || filter.PageSize > 100)
                filter.PageSize = 20;

            if (filter.MinPrice.HasValue && filter.MinPrice < 0)
                filter.MinPrice = 0;

            if (filter.MaxPrice.HasValue && filter.MaxPrice < 0)
                filter.MaxPrice = null;

            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
            {
                throw new ArgumentException("MinPrice cannot be greater than MaxPrice");
            }

            var validSorts = new[] { "relevance", "price_asc", "price_desc", "newest arrival", "best seller", "customer review", "featured" };
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = filter.SortBy.ToLower();
                if (!validSorts.Contains(filter.SortBy))
                {
                    _logger.Warning("Invalid sort value: {SortBy}, defaulting to 'relevance'", filter.SortBy);
                    filter.SortBy = "relevance";
                }
            }
            else
            {
                filter.SortBy = "relevance";
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                filter.SearchTerm = filter.SearchTerm.Trim();
                if (filter.SearchTerm.Length > 255)
                {
                    filter.SearchTerm = filter.SearchTerm.Substring(0, 255);
                }
            }
        }

        /// <summary>
        /// Add badges to search results based on item properties (Bilingual)
        /// </summary>
        private void EnrichResultsWithBadges(List<SearchItemDto> items)
        {
            foreach (var item in items)
            {
                item.Badges = new List<BadgeDto>();

                // New item badge
                if (item.IsNew)
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = "جديد",
                        TextEn = "New",
                        Type = "new",
                        Variant = "info"
                    });
                }

                // Discount badge
                if (item.DiscountPercentage >= 20)
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = $"خصم {item.DiscountPercentage:F0}%",
                        TextEn = $"{item.DiscountPercentage:F0}% OFF",
                        Type = "discount",
                        Variant = "danger"
                    });
                }

                // Stock status badges
                if (item.StockStatus == "InStock")
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = "متوفر",
                        TextEn = "In Stock",
                        Type = "stock",
                        Variant = "success"
                    });
                }
                else if (item.StockStatus == "LimitedStock")
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = "كمية محدودة",
                        TextEn = "Limited Stock",
                        Type = "stock",
                        Variant = "warning"
                    });
                }
                else if (item.StockStatus == "ComingSoon")
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = "قريباً",
                        TextEn = "Coming Soon",
                        Type = "stock",
                        Variant = "info"
                    });
                }

                // Free shipping badge
                if (item.IsFreeShipping)
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = "شحن مجاني",
                        TextEn = "Free Shipping",
                        Type = "shipping",
                        Variant = "success"
                    });
                }

                // Rating badge (for high ratings)
                if (item.ItemRating.HasValue && item.ItemRating >= 4.5m)
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = $"⭐ {item.ItemRating:F1}",
                        TextEn = $"⭐ {item.ItemRating:F1}",
                        Type = "rating",
                        Variant = "warning"
                    });
                }
            }
        }
    }
}