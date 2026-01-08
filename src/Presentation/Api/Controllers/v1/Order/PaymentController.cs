using Asp.Versioning;
using BL.Contracts.Service.Order.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    /// <summary>
    /// FIXED PaymentController - matches IPaymentService interface
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Process payment for an order
        /// </summary>
        [HttpPost("process")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(PaymentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentResult>> ProcessPayment(
            [FromBody] OrderPaymentProcessRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                    return Unauthorized(new PaymentResult
                    {
                        Success = false,
                        Message = "Customer not authenticated",
                        ErrorCode = "UNAUTHORIZED"
                    });
                }

                _logger.LogInformation(
                    "Customer {CustomerId} processing payment for order {OrderId}",
                    customerId,
                    request.OrderId
                );

                var result = await _paymentService.ProcessPaymentAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return BadRequest(new PaymentResult
                {
                    Success = false,
                    Message = "Payment processing failed",
                    ErrorCode = "PAYMENT_ERROR"
                });
            }
        }

        /// <summary>
        /// Get payment status for an order
        /// </summary>
        [HttpGet("status/{orderId}")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(typeof(PaymentStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving payment status for order {OrderId}", orderId);

                var status = await _paymentService.GetPaymentStatusAsync(orderId);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment status");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all payment attempts for an order
        /// </summary>
        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,Vendor")]
        [ProducesResponseType(typeof(List<PaymentStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<PaymentStatusDto>>> GetOrderPayments(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving all payments for order {OrderId}", orderId);

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
        /// Get payment by ID
        /// </summary>
        [HttpGet("{paymentId}")]
        [Authorize(Roles = "Admin,Vendor")]
        [ProducesResponseType(typeof(PaymentStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaymentStatusDto>> GetPayment(Guid paymentId)
        {
            try
            {
                _logger.LogInformation("Retrieving payment {PaymentId}", paymentId);

                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);

                if (payment == null)
                {
                    return NotFound(new { message = $"Payment {paymentId} not found" });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verify payment transaction
        /// </summary>
        [HttpPost("verify")]
        [Authorize(Roles = "Admin,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> VerifyPayment(
            [FromBody] VerifyPaymentRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Verifying payment for order {OrderId} with transaction {TransactionId}",
                    request.OrderId,
                    request.TransactionId
                );

                var isValid = await _paymentService.VerifyPaymentAsync(request.TransactionId);

                return Ok(new
                {
                    valid = isValid,
                    orderId = request.OrderId,
                    transactionId = request.TransactionId,
                    message = isValid
                        ? "Payment verified successfully"
                        : "Payment verification failed"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying payment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process refund for a payment
        /// </summary>
        [HttpPost("refund")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(RefundResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefundResultDto>> ProcessRefund(
            [FromBody] RefundProcessRequest request)
        {
            try
            {
                var adminUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                _logger.LogInformation(
                    "Admin {AdminId} processing refund for order {OrderId}",
                    adminUserId,
                    request.OrderId
                );

                var result = await _paymentService.ProcessRefundAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund");
                return BadRequest(new RefundResultDto
                {
                    Success = false,
                    Message = "Refund processing failed",
                    RefundStatus = "ERROR"
                });
            }
        }

        /// <summary>
        /// Cancel pending payment
        /// </summary>
        [HttpPost("{paymentId}/cancel")]
        [Authorize(Roles = "Customer,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> CancelPayment(
            Guid paymentId,
            [FromBody] CancelPaymentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                _logger.LogInformation(
                    "User {UserId} cancelling payment {PaymentId}",
                    userId,
                    paymentId
                );

                var success = await _paymentService.CancelPaymentAsync(paymentId, request.Reason);

                if (!success)
                {
                    return BadRequest(new { message = "Failed to cancel payment" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Payment cancelled successfully",
                    paymentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}