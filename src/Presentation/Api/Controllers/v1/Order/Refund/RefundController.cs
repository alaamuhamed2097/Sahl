using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Location;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.Payment.Refund;
using Shared.DTOs.Order.ResponseOrderDetail;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    /// <summary>
    /// Controller for refund management
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/refunds")]
    [Authorize]
    public class RefundController : BaseController
    {
        private readonly IRefundService _refundService;

        public RefundController(
            IRefundService refundService)
        {
            _refundService = refundService;
        }
        
        /// <summary>
        /// Get refund by number.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRefundByNumber([FromQuery] string number)
        {
            // Validate input
            if (string.IsNullOrEmpty(number))
                return BadRequest("Refund number is required.");
            
            // Retrieve refund by number
            var refund = await _refundService.GetRefundRequestByNumberAsync(number);

            // Check if refund exists
            if (refund == null)
                return NotFound();

            // Return ok response with refund data
            return Ok(refund);
        }

        /// <summary>
        /// Searches refunds with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] RefundSearchCriteria criteria)
        {
            ValidateBaseSearchCriteriaModel(criteria);

            var result = await _refundService.GetRefundsPageAsync(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<RefundRequestDto>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<RefundRequestDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Create refund request.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Creates customer refund request.
        /// </remarks>
        [HttpPost("create")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRefundRequest([FromBody] CreateRefundRequestDto request)
        {
            var result = await _refundService.CreateRefundRequestAsync(request,UserId);

            if (!result.IsSuccess)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = result.ErrorMessage ?? "Failed to create refund request",
                });
            }

            return Ok(
                new ResponseModel<string>
                {
                    Success = true,
                    Message = "Refund request created successfully",
                    Data = result.RefundNumber 
                });
        }

    }
}