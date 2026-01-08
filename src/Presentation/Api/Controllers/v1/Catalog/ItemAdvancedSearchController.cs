using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Catalog.Item;
using Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Catalog
{
    /// <summary>
    /// Advanced item search controller using optimized stored procedure
    /// Provides high-performance multi-vendor item search with complex filtering
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemAdvancedSearchController : BaseController
    {
        private readonly IItemSearchService _itemSearchService;

        public ItemAdvancedSearchController(IItemSearchService itemSearchService)
        {
            _itemSearchService = itemSearchService ?? throw new ArgumentNullException(nameof(itemSearchService));
        }

        /// <summary>
        /// Advanced item search with multi-vendor filtering and optimization
        /// Uses SpSearchItemsMultiVendor stored procedure for optimal performance
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Available sort options: "price_asc", "price_desc", "rating", "vendor_rating", "fastest_delivery", "most_sold", "newest"
        /// 
        /// Supports filtering by:
        /// - Text search (searches titles and descriptions in both Arabic and English)
        /// - Categories (multiple selection)
        /// - Brands (multiple selection)
        /// - Price range (MinPrice to MaxPrice)
        /// - Stock status (in stock only)
        /// - Free shipping
        /// - On sale items
        /// - Buy Box winners
        /// - Delivery time
        /// - Vendors (multiple selection)
        /// - Verified/Prime vendors
        /// - Warranty status
        /// - Offer conditions
        /// - Item and vendor ratings
        /// 
        /// Response includes:
        /// - Item details with best offer information
        /// - Price aggregation (min, max, average)
        /// - Available offers count
        /// - Fastest delivery time
        /// - Pagination info with total count
        /// </remarks>
        /// <param name="filter">Advanced search filter criteria</param>
        [HttpPost("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromBody] ItemFilterQuery filter)
        {
            if (filter == null)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            // Validate and normalize filter parameters
            ValidateAndNormalizeItemFilter(filter);

            // Execute stored procedure search
            var result = await _itemSearchService.SearchItemsAsync(filter);

            if (result?.Items?.Any() != true)
                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Get available filter options for search interface
        /// Returns categories, brands, vendors, and price range based on current filters
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// 
        /// Returns:
        /// - Top 50 categories with item counts
        /// - Top 50 brands with item counts
        /// - Available vendors with item counts
        /// - Price range statistics (min, max, average)
        /// 
        /// All filter options respect the currently applied filters
        /// </remarks>
        /// <param name="filter">Current search filter to apply</param>
        [HttpPost("filters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableFilters([FromBody] AvailableFiltersQuery filter)
        {
            if (filter == null)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            try
            {
                // Get available filter options
                var availableFilters = await _itemSearchService.GetAvailableFiltersAsync(filter);

                return Ok(CreateSuccessResponse(availableFilters, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse($"Error retrieving filters: {ex.Message}"));
            }
        }

        /// <summary>
        /// Get best prices for multiple items
        /// Optimized for quick bulk price lookups
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Useful for displaying prices in catalog/listing views
        /// </remarks>
        /// <param name="itemIds">List of item IDs to get prices for</param>
        [HttpPost("best-prices")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBestPrices([FromBody] List<Guid> itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return BadRequest(CreateErrorResponse("Item IDs are required"));

            try
            {
                var bestPrices = await _itemSearchService.GetItemBestPricesAsync(itemIds);

                if (bestPrices?.Any() != true)
                    return Ok(CreateSuccessResponse(bestPrices, NotifiAndAlertsResources.NoDataFound));

                return Ok(CreateSuccessResponse(bestPrices, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse($"Error retrieving prices: {ex.Message}"));
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Validates and normalizes item filter parameters
        /// </summary>
        private static void ValidateAndNormalizeItemFilter(ItemFilterQuery filter)
        {
            // Validate pagination
            filter.PageNumber = Math.Max(filter.PageNumber, 1);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            // Validate price range
            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
            {
                var temp = filter.MinPrice;
                filter.MinPrice = filter.MaxPrice;
                filter.MaxPrice = temp;
            }

            // Validate rating range (0-5)
            if (filter.MinItemRating.HasValue)
                filter.MinItemRating = Math.Max(0, Math.Min(filter.MinItemRating.Value, 5));

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                filter.SortBy = filter.SortBy.ToLower().Trim();
            }
        }

        #endregion
    }
}