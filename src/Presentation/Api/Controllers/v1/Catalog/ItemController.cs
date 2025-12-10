using Api.Controllers.v1.Base;
using Api.Extensions;
using Asp.Versioning;
using BL.Contracts.Service.ECommerce.Item;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Catalog
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(
            IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Retrieves all items with currency conversion based on client location.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var items = await _itemService.GetAllAsync();

            if (items?.Any() != true)
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(items, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Retrieves an item by ID with currency conversion based on client location.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the item.</param>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var item = await _itemService.FindByIdAsync(id);

            if (item == null)
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(item, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Searches items with pagination and filtering, with currency conversion based on client location.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] ItemSearchCriteriaModel criteria)
        {
            ValidateAndNormalizePagination(criteria);

            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var result = await _itemService.GetPage(criteria);

            if (result?.Items?.Any() != true)
                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Searches items with advanced filters for customer website.
        /// Supports filtering by price, rating, availability, vendor, attributes, and more.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Available sort options: "price_asc", "price_desc", "rating", "vendor_rating", "fastest_delivery", "most_sold", "newest"
        /// </remarks>
        /// <param name="filter">Advanced filter criteria including pagination parameters.</param>
        [HttpPost("search/advanced")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchWithFilters([FromBody] ItemFilterDto filter)
        {
            if (filter == null)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            ValidateAndNormalizeItemFilter(filter);

            var clientIp = HttpContext.GetClientIpAddress();
            var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var result = await _itemService.GetPageWithFiltersAsync(filter);

            if (result?.Items?.Any() != true)
                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        }
        ///// <summary>
        ///// Retrieves best seller items with currency conversion.
        ///// </summary>
        //[HttpGet("best-sellers")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetBestSellers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    return await GetItemsByCategory(new ItemSearchCriteriaModel
        //    {
        //        IsBestSeller = true,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    });
        //}

        ///// <summary>
        ///// Retrieves recommended items with currency conversion.
        ///// </summary>
        //[HttpGet("recommended")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetRecommended([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    return await GetItemsByCategory(new ItemSearchCriteriaModel
        //    {
        //        IsRecommended = true,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    });
        //}

        /// <summary>
        /// Adds a new item.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            var success = await _itemService.Save(itemDto, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        [HttpPost("update/{id:guid}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Update(Guid id, [FromBody] ItemDto itemDto)
        {
            if (id == Guid.Empty || !ModelState.IsValid)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            itemDto.Id = id;
            var success = await _itemService.Save(itemDto, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        }

        /// <summary>
        /// Deletes an item by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            var success = await _itemService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.DeleteFailed));

            return Ok(CreateSuccessResponse(true, NotifiAndAlertsResources.DeletedSuccessfully));
        }

        #region Private Helper Methods

        /// <summary>
        /// Determines whether currency conversion should be applied based on user role.
        /// Currency conversion is applied only for Customer role or unlogged users.
        /// </summary>
        private bool ShouldApplyCurrencyConversion()
        {
            if (RoleName == nameof(UserRole.Admin))
                return false;

            return true;
        }

        /// <summary>
        /// Validates and normalizes pagination parameters
        /// </summary>
        private static void ValidateAndNormalizePagination(ItemSearchCriteriaModel criteria)
        {
            criteria.PageNumber = Math.Max(criteria.PageNumber, 1);
            criteria.PageSize = Math.Clamp(criteria.PageSize, 1, 100);
        }

        /// <summary>
        /// Validates and normalizes item filter parameters
        /// </summary>
        private static void ValidateAndNormalizeItemFilter(ItemFilterDto filter)
        {
            filter.PageNumber = Math.Max(filter.PageNumber, 1);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);
            
            // Validate price range
            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
            {
                var temp = filter.MinPrice;
                filter.MinPrice = filter.MaxPrice;
                filter.MaxPrice = temp;
            }

            // Validate rating range
            if (filter.MinItemRating.HasValue)
                filter.MinItemRating = Math.Max(0, Math.Min(filter.MinItemRating.Value, 5));

            if (filter.MinVendorRating.HasValue)
                filter.MinVendorRating = Math.Max(0, Math.Min(filter.MinVendorRating.Value, 5));

            // Validate delivery days
            if (filter.MaxDeliveryDays.HasValue)
                filter.MaxDeliveryDays = Math.Max(filter.MaxDeliveryDays.Value, 1);

            // Validate quantity
            if (filter.MinAvailableQuantity.HasValue)
                filter.MinAvailableQuantity = Math.Max(filter.MinAvailableQuantity.Value, 0);
        }

        /// <summary>
        /// Creates a standardized success response
        /// </summary>
        private ResponseModel<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ResponseModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        private ResponseModel<string> CreateErrorResponse(string message)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = message
            };
        }

        #endregion
    }
}
