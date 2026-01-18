using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.VendorItem;
using BL.Services.VendorItem;
using Common.Enumerations.User;
using Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.VendorItems
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/item-conditions")]
    public class VendorItemConditionsController : BaseController
    {
        private readonly IVendorItemConditionService  _vendorItemConditionService;

        public VendorItemConditionsController( IVendorItemConditionService vendorItemConditionService)
        {
            _vendorItemConditionService = vendorItemConditionService;
        }

        ///// <summary>
        ///// Retrieves all vendor items conditions 
        ///// </summary>
        ///// <remarks>
        ///// API Version: 1.0+
        ///// </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var items = await _vendorItemConditionService.GetAllAsync();

            if (items?.Any() != true)
                return Ok(CreateSuccessResponse(new List<VendorItemConditionDto>(),NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(items, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Retrieves a vendor item condition by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the vendor item condition.</param>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(CreateErrorResponse<VendorItemConditionDto>(NotifiAndAlertsResources.InvalidInputAlert));

            var item = await _vendorItemConditionService.FindByIdAsync(id);

            if (item == null)
                return NotFound(CreateErrorResponse<VendorItemConditionDto>(NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(item, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Searches vendor items conditions with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            ValidateBaseSearchCriteriaModel(criteria);

            var result = await _vendorItemConditionService.GetPageAsync(criteria);

            if (result?.Items?.Any() != true)
                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        }

        /// <summary>
        /// Adds a new vendor item condition.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody]  VendorItemConditionDto vendorItemConditionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.InvalidInputAlert));

            var saveResult = await _vendorItemConditionService.SaveAsync(vendorItemConditionDto, GuidUserId);
            if (!saveResult.Success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.SaveFailed));

            return Ok(CreateSuccessResponse<bool>(saveResult.Success, NotifiAndAlertsResources.SavedSuccessfully));
        }

        /// <summary>
        /// Deletes a vendor item by ID.
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

            var success = await _vendorItemConditionService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(CreateErrorResponse<bool>(NotifiAndAlertsResources.DeleteFailed));

            return Ok(CreateSuccessResponse(true, NotifiAndAlertsResources.DeletedSuccessfully));
        }
    }
}
