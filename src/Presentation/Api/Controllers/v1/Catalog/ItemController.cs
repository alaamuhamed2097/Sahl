using Api.Controllers.v1.Base;
using Api.Extensions;
using Asp.Versioning;
using BL.Contracts.Service.Catalog.Item;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Parameters;

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
                return NotFound(CreateErrorResponse<IEnumerable<ItemDto>>(NotifiAndAlertsResources.NoDataFound));

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
                return BadRequest(CreateErrorResponse<ItemDto>(NotifiAndAlertsResources.InvalidInputAlert));

            //var clientIp = HttpContext.GetClientIpAddress();
            //var shouldApplyConversion = ShouldApplyCurrencyConversion();
            var item = await _itemService.FindByIdAsync(id);

            if (item == null)
                return NotFound(CreateErrorResponse<ItemDto>(NotifiAndAlertsResources.NoDataFound));

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
        /// Searches new items requests with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search/requests")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchNewItemRequests([FromQuery] ItemSearchCriteriaModel criteria)
        {
            ValidateAndNormalizePagination(criteria);

            var result = await _itemService.GetNewItemRequestsPage(criteria);

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
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Vendor)}")]
        public async Task<IActionResult> Save([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.InvalidInputAlert));

            var success = await _itemService.SaveAsync(itemDto, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<bool>(success, NotifiAndAlertsResources.SavedSuccessfully));
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        [HttpPost("update/{id:guid}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Vendor)}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ItemDto itemDto)
        {
            if (id == Guid.Empty || !ModelState.IsValid)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.InvalidInputAlert));

            itemDto.Id = id;
            var success = await _itemService.SaveAsync(itemDto, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        [HttpPost("update/status")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateItemVisibilityRequest updateItemVisibility)
        {
            if (updateItemVisibility.ItemId == Guid.Empty || !ModelState.IsValid)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.InvalidInputAlert));

            var result = await _itemService.UpdateVisibilityScope(updateItemVisibility, GuidUserId);
            if (!result.Success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<bool>(result.Success, NotifiAndAlertsResources.SavedSuccessfully));
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
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.InvalidInputAlert));

            var success = await _itemService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.DeleteFailed));

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
        #endregion
    }
}
