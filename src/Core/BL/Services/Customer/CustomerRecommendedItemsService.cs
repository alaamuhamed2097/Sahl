using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using Common.Filters;
using DAL.Contracts.Repositories.Customer;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Procedures;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Customer;
using System.Linq.Expressions;

namespace BL.Services.Customer
{
    /// <summary>
    /// Service for managing customer Recommended Items
    /// Uses custom repository for customer-scoped operations
    /// </summary>
    public class CustomerRecommendedItemsService : ICustomerRecommendedItemsService
    {
        private readonly ICustomerRecommendedItemsRepository _customerRecommendedItemsRepository;
        protected readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public CustomerRecommendedItemsService(

            ILogger logger, IBaseMapper mapper, ICustomerRecommendedItemsRepository customerRecommendedItemsRepository, ICustomerRepository customerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
            _customerRecommendedItemsRepository = customerRecommendedItemsRepository;
            _customerRepository = customerRepository;
        }
        public async Task<PagedSpSearchResultDto> SearchCustomerRecommendedItemsAsync(
          BaseSearchCriteriaModel filter,
          string? userId,
          CancellationToken cancellationToken = default)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            try
            {
                // Validate criteria
                ValidateAndSanitizeFilter(filter);
                
                // Validate userId
                if(userId == null)
                    return new PagedSpSearchResultDto();
                
                // get CustomerId
                var customer = await _customerRepository.GetCustomerByUserIdAsync(userId);
                if (customer == null)
                    return new PagedSpSearchResultDto();

                // Prepare customer filter
                var customerFilter = new CustomerFilterQuery()
                {
                    CustomerId = customer.Id,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    SearchTerm = filter.SearchTerm,
                    SortBy = filter.SortBy,
                    SortDirection = filter.SortDirection
                };
                var result = await _customerRecommendedItemsRepository.GetCustomerRecommendedItemsAsync(customerFilter);

                var dtos = result.Items.Select(entity => _mapper.MapModel<SpGetCustomerRecommendedItems, SearchItemDto>(entity)).ToList();

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
                _logger.Error(ex, "Error in Search Customer Recommended Items");
                throw;
            }
        }

        // Helper methods

        /// <summary>
        /// validate and sanitize filter parameters
        /// </summary>
        /// <param name="filter"></param>
        private void ValidateAndSanitizeFilter(BaseSearchCriteriaModel filter)
        {
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;

            if (filter.PageSize < 1 || filter.PageSize > 100)
                filter.PageSize = 20;

            var validSorts = new[] { "relevance", "price", "rating", "newest" };
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
                if (item.AverageRating.HasValue && item.AverageRating >= 4.5m)
                {
                    item.Badges.Add(new BadgeDto
                    {
                        TextAr = $"⭐ {item.AverageRating:F1}",
                        TextEn = $"⭐ {item.AverageRating:F1}",
                        Type = "rating",
                        Variant = "warning"
                    });
                }
            }
        }
    }
}