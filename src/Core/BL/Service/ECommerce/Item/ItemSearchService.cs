using BL.Contracts.Service.ECommerce.Item;
using Common.Enumerations.Fulfillment;
using Common.Models.Filters;
using DAL.Contracts.Repositories;
using Domains.Views.Item;
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

        /// <summary>
        /// Search items with validation and business logic
        /// Converts domain entities to DTOs
        /// </summary>
        public async Task<PagedSpSearchResultDto> SearchItemsAsync(
            ItemFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            try
            {
                // Validate and sanitize
                ValidateAndSanitizeFilter(filter);

                // Execute search - gets domain entities
                var (entities, totalCount) = await _searchRepository.SearchItemsAsync(filter, cancellationToken);

                // Convert entities to DTOs
                var dtos = entities.Select(entity => MapEntityToDto(entity)).ToList();

                // Enrich DTOs with badges
                EnrichResultsWithBadges(dtos);

                return new PagedSpSearchResultDto
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = totalCount > 0
                        ? (int)Math.Ceiling(totalCount / (double)filter.PageSize)
                        : 0
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SearchItemsAsync");
                throw;
            }
        }

        /// <summary>
        /// Get available filter options
        /// Converts domain data to DTOs
        /// </summary>
        public async Task<AvailableSearchFiltersDto> GetAvailableFiltersAsync(
            ItemFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            try
            {
                // Get domain data
                var filterData = await _searchRepository.GetAvailableFiltersAsync(filter, cancellationToken);

                // Convert to DTOs
                return new AvailableSearchFiltersDto
                {
                    Categories = filterData.Categories.Select(c => new FilterOptionDto
                    {
                        Id = c.Id,
                        NameAr = c.NameAr,
                        NameEn = c.NameEn,
                        Count = c.Count
                    }).ToList(),
                    Brands = filterData.Brands.Select(b => new FilterOptionDto
                    {
                        Id = b.Id,
                        NameAr = b.NameAr,
                        NameEn = b.NameEn,
                        Count = b.Count
                    }).ToList(),
                    Vendors = filterData.Vendors.Select(v => new FilterOptionDto
                    {
                        Id = v.Id,
                        NameAr = v.NameAr,
                        NameEn = v.NameEn,
                        Count = v.Count
                    }).ToList(),
                    PriceRange = new PriceRangeDto
                    {
                        MinPrice = filterData.MinPrice ?? 0,
                        MaxPrice = filterData.MaxPrice ?? 0
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetAvailableFiltersAsync");
                throw;
            }
        }

        /// <summary>
        /// Get best prices for multiple items
        /// Converts view entities to DTOs
        /// </summary>
        public async Task<List<ItemBestPriceDto>> GetItemBestPricesAsync(
            List<Guid> itemIds,
            CancellationToken cancellationToken = default)
        {
            if (itemIds == null || !itemIds.Any())
                return new List<ItemBestPriceDto>();

            try
            {
                // Get view entities from DAL
                var viewEntities = await _searchRepository.GetItemBestPricesAsync(itemIds, cancellationToken);

                // Convert to DTOs
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
        /// Map SpSearchItemsMultiVendor entity to SpSearchItemsResultDto
        /// </summary>
        private SpSearchItemsResultDto MapEntityToDto(SpSearchItemsMultiVendor entity)
        {
            var dto = new SpSearchItemsResultDto
            {
                ItemId = entity.ItemId,
                TitleAr = entity.TitleAr,
                TitleEn = entity.TitleEn,
                ShortDescriptionAr = entity.ShortDescriptionAr,
                ShortDescriptionEn = entity.ShortDescriptionEn,
                CategoryId = entity.CategoryId,
                BrandId = entity.BrandId,
                ThumbnailImage = entity.ThumbnailImage,
                CreatedDateUtc = entity.CreatedDateUtc,
                MinPrice = entity.MinPrice,
                MaxPrice = entity.MaxPrice,
                AvgPrice = entity.AvgPrice,
                OffersCount = entity.OffersCount,
                FastestDelivery = entity.FastestDelivery,
                FinalScore = entity.FinalScore
            };

            // Parse best offer data if available
            if (!string.IsNullOrEmpty(entity.BestOfferDataRaw))
            {
                dto.BestOffer = ParseBestOfferData(entity.BestOfferDataRaw);
            }

            return dto;
        }

        /// <summary>
        /// Parse best offer data from pipe-separated string
        /// Format: OfferId|VendorId|SalesPrice|OriginalPrice|AvailableQuantity|IsFreeShipping|HandlingTimeInDays|IsBuyBoxWinner|FulfillmentType
        /// </summary>
        private BestOfferDetailsDto ParseBestOfferData(string bestOfferData)
        {
            try
            {
                var parts = bestOfferData.Split('|');
                if (parts.Length < 9)
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
                    EstimatedDeliveryDays = int.Parse(parts[6]),  // HandlingTimeInDays
                    IsBuyBoxWinner = parts[7] == "1",
                    FulfillmentType = (FulfillmentType)int.Parse(parts[8])
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
        /// Validate and sanitize filter parameters
        /// </summary>
        private void ValidateAndSanitizeFilter(ItemFilterDto filter)
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

            var validSorts = new[] { "relevance", "price_asc", "price_desc", "newest", "rating" };
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
        /// Add badges to search results based on scores and properties
        /// </summary>
        private void EnrichResultsWithBadges(List<SpSearchItemsResultDto> items)
        {
            foreach (var item in items)
            {
                item.Badges = new List<string>();

                if (item.FinalScore > 0.8)
                    item.Badges.Add("مميز");

                if (item.IsNew)
                    item.Badges.Add("جديد");

                if (item.BestOffer?.DiscountPercentage > 20)
                    item.Badges.Add($"خصم {item.BestOffer.DiscountPercentage:F0}%");

                if (item.OffersCount > 5)
                    item.Badges.Add($"{item.OffersCount} عروض");

                if (item.BestOffer?.IsBuyBoxWinner == true)
                    item.Badges.Add("الأفضل مبيعاً");

                if (item.BestOffer?.IsFreeShipping == true)
                    item.Badges.Add("شحن مجاني");
            }
        }
    }
}
