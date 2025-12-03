using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Payment;
using BL.Services.Order;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 5: Process Payment
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("process")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaymentResultDto>> ProcessPayment([FromBody] PaymentProcessRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} processing payment for order {request.OrderId} amount {request.Amount}");

                var result = await _paymentService.ProcessPaymentAsync(request);
                
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return BadRequest(new PaymentResultDto
                {
                    Success = false,
                    Message = ex.Message,
                    ErrorCode = "PAYMENT_ERROR"
                });
            }
        }

        /// <summary>
        /// Get Payment Status for Order
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("status/{orderId}")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus(Guid orderId)
        {
            try
            {
                _logger.LogInformation($"Retrieving payment status for order {orderId}");

                var paymentStatus = await _paymentService.GetPaymentStatusAsync(orderId);
                return Ok(paymentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get All Payments for Order
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or Vendor role.
        /// </remarks>
        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PaymentStatusDto>>> GetOrderPayments(Guid orderId)
        {
            try
            {
                _logger.LogInformation($"Retrieving all payments for order {orderId}");

                var payments = await _paymentService.GetOrderPaymentsAsync(orderId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order payments");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verify Payment
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or System role.
        /// </remarks>
        [HttpPost("verify")]
        [Authorize(Roles = "Admin,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            try
            {
                _logger.LogInformation($"Verifying payment for order {request.OrderId} with transaction {request.TransactionId}");

                var isValid = await _paymentService.VerifyPaymentAsync(request.OrderId, request.TransactionId);
                
                return Ok(new
                {
                    valid = isValid,
                    message = isValid ? "Payment verified successfully" : "Payment verification failed"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process Refund
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or Vendor role.
        /// </remarks>
        [HttpPost("refund")]
        [Authorize(Roles = "Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RefundResultDto>> ProcessRefund([FromBody] RefundProcessRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} processing refund for order {request.OrderId} amount {request.RefundAmount}");

                var result = await _paymentService.ProcessRefundAsync(request);
                
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund");
                return BadRequest(new RefundResultDto
                {
                    Success = false,
                    Message = ex.Message,
                    RefundStatus = "FAILED"
                });
            }
        }
    }

    public class VerifyPaymentRequest
    {
        public Guid OrderId { get; set; }
        public string TransactionId { get; set; } = null!;
    }
}
