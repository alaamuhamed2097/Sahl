using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Warehouse;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Warehouse
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /// <summary>
        /// Retrieves all warehouses.
        /// </summary>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            var warehouses = await _warehouseService.GetAllAsync();

            return Ok(new ResponseModel<IEnumerable<WarehouseDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = warehouses
            });
        }
		[HttpGet("vendors")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> GetVendors()
		{
			var vendors = await _warehouseService.GetVendorsAsync();

			return Ok(new ResponseModel<IEnumerable<VendorDto>>
			{
				Success = true,
				Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
				Data = vendors
			});
		}

		[HttpGet("multi-vendor-enabled")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> IsMultiVendorEnabled()
		{
			var isEnabled = await _warehouseService.IsMultiVendorEnabledAsync();

			return Ok(new ResponseModel<bool>
			{
				Success = true,
				Data = isEnabled
			});
		}


		/// <summary>
		/// Retrieves active warehouses only.
		/// </summary>
		/// <remarks>
		/// Requires Admin role.
		/// 
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet("active")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetActive()
        {
            var warehouses = await _warehouseService.GetActiveWarehousesAsync();

            return Ok(new ResponseModel<IEnumerable<WarehouseDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = warehouses
            });
        }

        /// <summary>
        /// Retrieves a warehouse by ID.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });

            var warehouse = await _warehouseService.GetByIdAsync(id);
            if (warehouse == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<WarehouseDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = warehouse
            });
        }

        /// <summary>
        /// Searches warehouses with pagination and filtering.
        /// </summary>
        /// <param name="criteriaModel">Search criteria including pagination and filters.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] WarehouseSearchCriteriaModel criteriaModel)
        {
            criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
            criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

            var result = await _warehouseService.SearchAsync(criteriaModel);

            return Ok(new ResponseModel<PagedResult<WarehouseDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = result
            });
        }
		[HttpGet("search-vendor")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> SearchVendor([FromQuery] WarehouseSearchCriteriaModel criteriaModel)
		{
			criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
			criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

			var result = await _warehouseService.SearchVendorAsync(criteriaModel);

			return Ok(new ResponseModel<PagedResult<WarehouseDto>>
			{
				Success = true,
				Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
				Data = result
			});
		}
		/// <summary>
		/// Adds a new warehouse or updates an existing one.
		/// </summary>
		/// <param name="dto">The warehouse data.</param>
		/// <remarks>
		/// Requires Admin role.
		/// 
		/// API Version: 1.0+
		/// </remarks>
		[HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] WarehouseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid warehouse data."
                });

            var success = await _warehouseService.SaveAsync(dto, GuidUserId);
            if (!success)
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SaveFailed))
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
            });
        }

        /// <summary>
        /// Deletes a warehouse by ID (soft delete).
        /// </summary>
        /// <param name="id">The ID of the warehouse to delete.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid warehouse ID."
                });

            var success = await _warehouseService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeleteFailed))
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeletedSuccessfully))
            });
        }

        /// <summary>
        /// Toggles the active status of a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <remarks>
        /// Requires Admin role.
        /// 
        /// API Version: 1.0+
        /// </remarks>
        [HttpPost("toggle-status")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid warehouse ID."
                });

            var success = await _warehouseService.ToggleActiveStatusAsync(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Failed to update warehouse status."
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Warehouse status updated successfully."
            });
        }
    }
}
