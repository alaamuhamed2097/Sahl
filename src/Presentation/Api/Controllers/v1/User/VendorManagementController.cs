using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Vendor;
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

namespace Api.Controllers.v1.User
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class VendorManagementController : BaseController
    {
        private readonly IVendorManagementService _vendorService;
		private readonly IWarehouseService _warehouseService;


		public VendorManagementController(IVendorManagementService vendorService, IWarehouseService warehouseService)
		{
			_vendorService = vendorService;
			_warehouseService = warehouseService;
		}

		/// <summary>
		/// Retrieves all vendors.
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		[HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendor = await _vendorService.GetAllAsync();

            return Ok(new ResponseModel<IEnumerable<VendorDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = vendor
            });
        }

        /// <summary>
        /// Retrieves a vendor by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the vendor.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });

            var vendor = await _vendorService.FindByIdAsync(id);
            if (vendor == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<VendorDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = vendor
            });
        }

        /// <summary>
        /// Searches vendors with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteriaModel">Search criteria including pagination and filters.</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
            criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

            var result = await _vendorService.SearchAsync(criteriaModel);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<VendorDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound)),
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<VendorDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = result
            });
        }

        /// <summary>
        /// Adds a new vendor or updates an existing one.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="dto">The vendor data.</param>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] VendorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid vendor data."
                });

            var result = await _vendorService.SaveAsync(dto, GuidUserId);
            if (!result.Success)
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

		[HttpPost("add-warehouse")]
		[Authorize(Roles = nameof(UserRole.Vendor))]
		public async Task<IActionResult> AddWarehouse([FromBody] WarehouseDto dto)
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
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.WarehouseAlreadyExists))
				});
            

			return Ok(new ResponseModel<string>
			{
				Success = true,
				Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
			});
		}
		
        /// <summary>
		/// Get all vendors with their user information
		/// </summary>
		[HttpGet("with-users")]
		[ProducesResponseType(typeof(IEnumerable<VendorWithUserDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VendorWithUserDto>>> GetVendorsWithUsers()
		{
			try
			{
				var vendors = await _warehouseService.GetVendorUsersAsync();
				return Ok(vendors);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while retrieving vendors");
			}
		}
		
        /// <summary>
		/// Deletes a vendor by ID (soft delete).
		/// </summary>
		/// <remarks>
		/// API Version: 1.0+
		/// </remarks>
		/// <param name="id">The ID of the vendor to delete.</param>
		[HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid vendor ID."
                });

            var success = await _vendorService.DeleteAsync(id, GuidUserId);
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
    }
}
