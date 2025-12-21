using Asp.Versioning;
using BL.Contracts.Service.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Shipment;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IFulfillmentService _fulfillmentService;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(
            IDeliveryService deliveryService,
            IFulfillmentService fulfillmentService,
            ILogger<DeliveryController> logger)
        {
            _deliveryService = deliveryService;
            _fulfillmentService = fulfillmentService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 6: Process Fulfillment (FBA)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or System role.
        /// </remarks>
        [HttpPost("{shipmentId}/process-fba")]
        [Authorize(Roles = "Admin,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<object>> ProcessFBAShipment(Guid shipmentId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} processing FBA shipment {shipmentId}");

                await _fulfillmentService.ProcessFBAShipmentAsync(shipmentId);
                return Ok(new { message = "FBA shipment processed successfully", shipmentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing FBA shipment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process FBM Shipment
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Vendor role.
        /// </remarks>
        [HttpPost("{shipmentId}/process-fbm")]
        [Authorize(Roles = "Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<object>> ProcessFBMShipment(Guid shipmentId)
        {
            try
            {
                var vendorId = User.FindFirst("VendorId")?.Value;
                _logger.LogInformation($"Vendor {vendorId} processing FBM shipment {shipmentId}");

                await _fulfillmentService.ProcessFBMShipmentAsync(shipmentId);
                return Ok(new { message = "FBM shipment processed successfully", shipmentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing FBM shipment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Stage 7: Confirm Delivery
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin, ShippingCompany, or System role.
        /// </remarks>
        [HttpPost("{shipmentId}/complete-delivery")]
        [Authorize(Roles = "Admin,ShippingCompany,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> CompleteDelivery(Guid shipmentId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} confirming delivery for shipment {shipmentId}");

                var result = await _deliveryService.CompleteDeliveryAsync(shipmentId);

                if (!result)
                    return BadRequest(new { message = "Failed to complete delivery" });

                return Ok(new { message = "Delivery confirmed successfully", shipmentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming delivery");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Stage 8: Confirm Order Completion
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or System role.
        /// </remarks>
        [HttpPost("order/{orderId}/complete")]
        [Authorize(Roles = "Admin,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> ConfirmOrderCompletion(Guid orderId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} confirming completion for order {orderId}");

                var result = await _deliveryService.ConfirmOrderCompletionAsync(orderId);

                if (!result)
                    return BadRequest(new { message = "Order cannot be completed at this stage" });

                return Ok(new { message = "Order completed successfully", orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming order completion");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Shipment Delivery Information
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{shipmentId}/delivery-info")]
        [Authorize(Roles = "Customer,Admin,Vendor,ShippingCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShipmentDto>> GetDeliveryInfo(Guid shipmentId)
        {
            try
            {
                _logger.LogInformation($"Retrieving delivery information for shipment {shipmentId}");

                var shipmentInfo = await _deliveryService.GetShipmentDeliveryInfoAsync(shipmentId);
                return Ok(shipmentInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting delivery information");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Initiate Return Request
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("{shipmentId}/initiate-return")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<object>> InitiateReturn(Guid shipmentId, [FromBody] InitiateReturnRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"Customer {customerId} initiating return for shipment {shipmentId}. Reason: {request.Reason}");

                var result = await _deliveryService.InitiateReturnAsync(shipmentId, request.Reason);

                if (!result)
                    return BadRequest(new { message = "Return cannot be initiated at this stage" });

                return Ok(new { message = "Return request initiated successfully", shipmentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating return");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Process Return Request
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or Vendor role.
        /// </remarks>
        [HttpPost("{shipmentId}/process-return")]
        [Authorize(Roles = "Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> ProcessReturn(Guid shipmentId, [FromBody] ProcessReturnRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} processing return for shipment {shipmentId}. Approved: {request.Approved}");

                var result = await _deliveryService.ProcessReturnAsync(shipmentId, request.Approved);

                if (!result)
                    return BadRequest(new { message = "Failed to process return" });

                var status = request.Approved ? "approved" : "rejected";
                return Ok(new { message = $"Return {status} successfully", shipmentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing return");
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class InitiateReturnRequest
    {
        public string Reason { get; set; } = null!;
    }

    public class ProcessReturnRequest
    {
        public bool Approved { get; set; }
        public string? AdminNotes { get; set; }
    }
}
