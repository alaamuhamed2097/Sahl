using Api.Controllers.v1.Base;
using Api.Extensions;
using Asp.Versioning;
using BL.Service.VendorItem;
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
    public class VendorItemController : BaseController
    {
        private readonly IVendorItemService _vendorItemService;

        public VendorItemController(
            IVendorItemService vendorItemService)
        {
            _vendorItemService = vendorItemService;
        }

        ///// <summary>
        ///// Retrieves all vendor items with currency conversion based on client location.
        ///// </summary>
        ///// <remarks>
        ///// API Version: 1.0+
        ///// </remarks>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Get()
        //{
        //    var clientIp = HttpContext.GetClientIpAddress();
        //    var shouldApplyConversion = ShouldApplyCurrencyConversion();
        //    var items = await _vendorItemService.GetAllAsync();

        //    if (items?.Any() != true)
        //        return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

        //    return Ok(CreateSuccessResponse(items, NotifiAndAlertsResources.DataRetrieved));
        //}

        /// <summary>
        /// Retrieves a vendor item by ID with currency conversion based on client location.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the vendor item.</param>
        //[HttpGet("{id:guid}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Get(Guid id)
        //{
        //    if (id == Guid.Empty)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

        //    //var clientIp = HttpContext.GetClientIpAddress();
        //    //var shouldApplyConversion = ShouldApplyCurrencyConversion();
        //    var item = await _vendorItemService.FindByIdAsync(id);

        //    if (item == null)
        //        return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

        //    return Ok(CreateSuccessResponse(item, NotifiAndAlertsResources.DataRetrieved));
        //}

        /// <summary>
        /// Retrieves all vendors items by item combination ID .
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The item combination ID.</param>
        [HttpGet("{itemCombinationId}/vendors-items")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVendorsItems(Guid itemCombinationId)
        {
            if (itemCombinationId == Guid.Empty)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            var item = await _vendorItemService.FindByItemCombinationIdAsync(itemCombinationId);

            if (item == null)
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(item, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Searches vendor items with pagination and filtering, with currency conversion based on client location.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        //[HttpGet("search")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Search([FromQuery] ItemSearchCriteriaModel criteria)
        //{
        //    ValidateAndNormalizePagination(criteria);

        //    var clientIp = HttpContext.GetClientIpAddress();
        //    var shouldApplyConversion = ShouldApplyCurrencyConversion();
        //    var result = await _vendorItemService.GetPage(criteria);

        //    if (result?.Items?.Any() != true)
        //        return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

        //    return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        //}


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
        /// Adds a new vendor item.
        /// </summary>
        //[HttpPost("save")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        //public async Task<IActionResult> Save([FromBody] ItemDto itemDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

        //    var success = await _vendorItemService.Save(itemDto, GuidUserId);
        //    if (!success)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

        //    return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        //}

        /// <summary>
        /// Updates an existing vendor item .
        /// </summary>
        //[HttpPost("update/{id:guid}")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        //public async Task<IActionResult> Update(Guid id, [FromBody] ItemDto itemDto)
        //{
        //    if (id == Guid.Empty || !ModelState.IsValid)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

        //    itemDto.Id = id;
        //    var success = await _vendorItemService.Save(itemDto, GuidUserId);
        //    if (!success)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

        //    return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
        //}

        /// <summary>
        /// Deletes a vendor item by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        //[HttpPost("delete")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        //public async Task<IActionResult> Delete([FromBody] Guid id)
        //{
        //    if (id == Guid.Empty)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

        //    var success = await _vendorItemService.DeleteAsync(id, GuidUserId);
        //    if (!success)
        //        return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.DeleteFailed));

        //    return Ok(CreateSuccessResponse(true, NotifiAndAlertsResources.DeletedSuccessfully));
        //}

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
