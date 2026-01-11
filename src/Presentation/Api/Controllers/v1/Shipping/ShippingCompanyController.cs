using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.ShippingCompny;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Shipping
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class ShippingCompanyController : BaseController
    {
        private readonly IShippingCompanyService _shippingCompanyService;

        public ShippingCompanyController(IShippingCompanyService shippingCompanyService)
        {
            _shippingCompanyService = shippingCompanyService;
        }

        /// <summary>
        /// Retrieves all shipping companies.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var companies = await _shippingCompanyService.GetAllAsync();
            if (companies == null || !companies.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No companies found."
                });

            return Ok(new ResponseModel<IEnumerable<ShippingCompanyDto>>
            {
                Success = true,
                Message = "companies retrieved successfully.",
                Data = companies
            });
        }

        /// <summary>
        /// Retrieves a shipping company by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid company ID."
                });

            var company = await _shippingCompanyService.FindByIdAsync(id);
            if (company == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "company not found."
                });

            return Ok(new ResponseModel<ShippingCompanyDto>
            {
                Success = true,
                Message = "company retrieved successfully.",
                Data = company
            });
        }

        /// <summary>
        /// Searches shipping companies with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _shippingCompanyService.GetPage(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<ShippingCompanyDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<ShippingCompanyDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Saves a shipping company.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] ShippingCompanyDto shippingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid shipping companies data."
                });

            var success = await _shippingCompanyService.Save(shippingDto, GuidUserId);
            if (!success)
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully
            });
        }

        /// <summary>
        /// Deletes a shipping company by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid company ID."
                });

            var success = await _shippingCompanyService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully
            });
        }
    }
}
