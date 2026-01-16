using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Customer;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Customer;

namespace Api.Controllers.v1.User
{
    /// <summary>
    /// Customer Management Controller for Admin Dashboard
    /// Handles customer profile viewing, status management, and order/wallet history
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _customerService.GetAllAsync();
                if (result.Success)
                    return Ok(CreateSuccessResponse(result.Data, "Customers retrieved successfully"));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer details</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _customerService.GetByIdAsync(id);
                if (result.Success && result.Data != null)
                    return Ok(CreateSuccessResponse(result.Data, "Customer retrieved successfully"));
                return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Search customers with pagination and filtering (GET request)
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <param name="searchTerm">Search term for filtering</param>
        /// <param name="sortBy">Column name to sort by</param>
        /// <param name="sortDirection">Sort direction: 'asc' or 'desc' (default: 'asc')</param>
        /// <returns>Paginated list of customers</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchGet(
            [FromQuery] BaseSearchCriteriaModel criteria)
        {
            ValidateBaseSearchCriteriaModel(criteria);

            var result = await _customerService.SearchAsync(criteria);
            if (result.Success && result.Data != null)
                return Ok(CreateSuccessResponse(result.Data, "Search completed successfully"));

            return Ok(CreateSuccessResponse(result.Data ?? new AdvancedPagedResult<CustomerDto>(), "No results found"));
        }

        /// <summary>
        /// Search customers with pagination and filtering (POST request)
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Paginated list of customers</returns>
        [HttpPost("search")]
        public async Task<IActionResult> SearchPost([FromBody] BaseSearchCriteriaModel criteria)
        {
            try
            {
                if (criteria == null)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

                ValidateBaseSearchCriteriaModel(criteria);

                var result = await _customerService.SearchAsync(criteria);
                if (result.Success && result.Data != null)
                    return Ok(CreateSuccessResponse(result.Data, "Search completed successfully"));

                return Ok(CreateSuccessResponse(result.Data ?? new AdvancedPagedResult<CustomerDto>(), "No results found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Save or update customer
        /// </summary>
        /// <param name="dto">Customer data</param>
        /// <returns>Updated customer</returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] CustomerDto dto)
        {
            if (dto == null)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            try
            {
                var result = await _customerService.SaveAsync(dto);
                if (result.Success)
                    return Ok(CreateSuccessResponse(result.Data, NotifiAndAlertsResources.SavedSuccessfully));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Delete customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Success or error response</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

            try
            {
                var result = await _customerService.DeleteAsync(id);
                if (result.Success)
                    return Ok(CreateSuccessResponse(true, NotifiAndAlertsResources.DeletedSuccessfully));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Change customer account status (Lock, Suspend, Activate, etc.)
        /// </summary>
        /// <param name="request">Request containing customerId and new status</param>
        /// <returns>Success or error response</returns>
        [HttpPost("changeStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        {
            try
            {
                if (request?.CustomerId == null || request.CustomerId == Guid.Empty)
                    return BadRequest(CreateErrorResponse("Customer ID is required"));

                var result = await _customerService.ChangeStatusAsync(request.CustomerId, request.Status);
                if (result.Success)
                    return Ok(CreateSuccessResponse(true, "Account status updated successfully"));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer account status
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Current account status</returns>
        [HttpGet("getStatus/{id:guid}")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            try
            {
                var result = await _customerService.GetStatusAsync(id);
                if (result.Success)
                    return Ok(CreateSuccessResponse(result.Data, "Status retrieved successfully"));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer order history
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="pageNumber">Page number (optional, default 1)</param>
        /// <param name="pageSize">Page size (optional, default 10)</param>
        /// <returns>Paginated order history</returns>
        [HttpPost("{id:guid}/orders")]
        public async Task<IActionResult> GetOrderHistory(
            Guid id,
            [FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                //var searchCriteria = new BaseSearchCriteriaModel
                //{
                //    PageNumber = pageNumber,
                //    PageSize = pageSize,
                //    SearchTerm = string.Empty
                //};

                ValidateBaseSearchCriteriaModel(criteria);

                var result = await _customerService.GetOrderHistoryAsync(id, criteria);
                if (result.Success && result.Data != null)
                    return Ok(CreateSuccessResponse(result.Data, "Orders retrieved successfully"));

                return Ok(CreateSuccessResponse(new AdvancedPagedResult<OrderHistoryDto>(), "No orders found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer wallet transaction history
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="pageNumber">Page number (optional, default 1)</param>
        /// <param name="pageSize">Page size (optional, default 10)</param>
        /// <returns>Paginated wallet history</returns>
        [HttpGet("{id:guid}/wallet-history")]
        public async Task<IActionResult> GetWalletHistory(
            Guid id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var searchCriteria = new BaseSearchCriteriaModel
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = string.Empty
                };

                ValidateBaseSearchCriteriaModel(searchCriteria);

                var result = await _customerService.GetWalletHistoryAsync(id, searchCriteria);
                if (result.Success && result.Data != null)
                    return Ok(CreateSuccessResponse(result.Data, "Wallet history retrieved successfully"));

                return Ok(CreateSuccessResponse(new AdvancedPagedResult<object>(), "No transactions found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer wallet balance
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Current wallet balance</returns>
        [HttpGet("{id:guid}/wallet-balance")]
        public async Task<IActionResult> GetWalletBalance(Guid id)
        {
            try
            {
                var result = await _customerService.GetWalletBalanceAsync(id);
                if (result.Success)
                    return Ok(CreateSuccessResponse(result.Data, "Wallet balance retrieved successfully"));
                return BadRequest(CreateErrorResponse(result.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// Get customer info for select/dropdown
        /// </summary>
        /// <returns>List of customers for dropdown</returns>
        [HttpGet("forSelect")]
        [AllowAnonymous]
        public async Task<IActionResult> GetForSelect()
        {
            try
            {
                var result = await _customerService.GetAllAsync();
                if (result.Success && result.Data != null)
                {
                    var selectList = result.Data.Select(c => new { value = c.Id, label = $"{c.FirstName} {c.LastName}" });
                    return Ok(CreateSuccessResponse(selectList, "Select list retrieved successfully"));
                }
                return Ok(CreateSuccessResponse(new List<object>(), "No customers available"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse(ex.Message));
            }
        }
    }

    /// <summary>
    /// Request model for changing customer status
    /// </summary>
    public class ChangeStatusRequest
    {
        public Guid CustomerId { get; set; }
        public UserStateType Status { get; set; }
    }
}
