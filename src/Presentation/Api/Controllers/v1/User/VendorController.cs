using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.Vendor;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.User
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VendorController : BaseController
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
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
                return Ok(new ResponseModel<PaginatedDataModel<VendorDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound)),
                    Data = result
                });
            }

            return Ok(new ResponseModel<PaginatedDataModel<VendorDto>>
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
