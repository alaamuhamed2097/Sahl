using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.Customer;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Brand;
using Shared.DTOs.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.User
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService, Serilog.ILogger logger) 
            : base(logger)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customer = await _customerService.GetAllAsync();

            return Ok(new ResponseModel<IEnumerable<CustomerDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = customer
            });
        }

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the customer.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });

            var customer = await _customerService.FindByIdAsync(id);
            if (customer == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<CustomerDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = customer
            });
        }

        /// <summary>
        /// Searches customers with pagination and filtering.
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

            var result = await _customerService.SearchAsync(criteriaModel);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PaginatedDataModel<CustomerDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound)),
                    Data = result
                });
            }

            return Ok(new ResponseModel<PaginatedDataModel<CustomerDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = result
            });
        }

        /// <summary>
        /// Adds a new customer or updates an existing one.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="dto">The customer data.</param>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid customer data."
                });

            var result = await _customerService.SaveAsync(dto, GuidUserId);
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
        /// Deletes a customer by ID (soft delete).
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="id">The ID of the customer to delete.</param>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid customer ID."
                });

            var success = await _customerService.DeleteAsync(id, GuidUserId);
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
