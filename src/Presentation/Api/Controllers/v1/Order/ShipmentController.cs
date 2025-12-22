using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Shipment;
using System.Security.Claims;
using BL.Contracts.Service.Order;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly ILogger<ShipmentController> _logger;

        public ShipmentController(IShipmentService shipmentService, ILogger<ShipmentController> logger)
        {
            _shipmentService = shipmentService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 4: Split Order into Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin or System role.
        /// </remarks>
        [HttpPost("split-order/{orderId}")]
        [Authorize(Roles = "Admin,System")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<ShipmentDto>>> SplitOrderIntoShipments(Guid orderId)
        {
            try
            {
                _logger.LogInformation($"Admin splitting order {orderId} into shipments");

                var shipments = await _shipmentService.SplitOrderIntoShipmentsAsync(orderId);
                return Ok(shipments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error splitting order into shipments");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Shipment by ID
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{shipmentId}")]
        [Authorize(Roles = "Customer,Admin,Vendor,ShippingCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShipmentDto>> GetShipmentById(Guid shipmentId)
        {
            try
            {
                _logger.LogInformation($"Retrieving shipment {shipmentId}");

                var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);
                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shipment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Shipment by Shipment Number
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("number/{shipmentNumber}")]
        [Authorize(Roles = "Customer,Admin,Vendor,ShippingCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShipmentDto>> GetShipmentByNumber(string shipmentNumber)
        {
            try
            {
                _logger.LogInformation($"Retrieving shipment by number {shipmentNumber}");

                var shipment = await _shipmentService.GetShipmentByNumberAsync(shipmentNumber);
                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shipment by number");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Order Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ShipmentDto>>> GetOrderShipments(Guid orderId)
        {
            try
            {
                _logger.LogInformation($"Retrieving shipments for order {orderId}");

                var shipments = await _shipmentService.GetOrderShipmentsAsync(orderId);
                return Ok(shipments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order shipments");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update Shipment Status
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin, Vendor, or ShippingCompany role.
        /// </remarks>
        [HttpPut("{shipmentId}/status")]
        [Authorize(Roles = "Admin,Vendor,ShippingCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ShipmentDto>> UpdateShipmentStatus(Guid shipmentId, [FromBody] UpdateShipmentStatusRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} updating shipment {shipmentId} status to {request.NewStatus}");

                var shipment = await _shipmentService.UpdateShipmentStatusAsync(
                    shipmentId,
                    request.NewStatus,
                    request.Location,
                    request.Notes);

                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating shipment status");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Assign Tracking Number to Shipment
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin, Vendor, or ShippingCompany role.
        /// </remarks>
        [HttpPut("{shipmentId}/tracking")]
        [Authorize(Roles = "Admin,Vendor,ShippingCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShipmentDto>> AssignTrackingNumber(Guid shipmentId, [FromBody] AssignTrackingNumberRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} assigning tracking number {request.TrackingNumber} to shipment {shipmentId}");

                var shipment = await _shipmentService.AssignTrackingNumberAsync(
                    shipmentId,
                    request.TrackingNumber,
                    request.EstimatedDeliveryDate);

                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning tracking number");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Track Shipment by Tracking Number
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("track/{trackingNumber}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShipmentTrackingDto>> TrackShipment(string trackingNumber)
        {
            try
            {
                _logger.LogInformation($"Tracking shipment with tracking number {trackingNumber}");

                var tracking = await _shipmentService.TrackShipmentAsync(trackingNumber);
                return Ok(tracking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking shipment");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Vendor Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Vendor role.
        /// </remarks>
        [HttpGet("vendor/my-shipments")]
        [Authorize(Roles = "Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ShipmentDto>>> GetVendorShipments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var vendorId = User.FindFirst("VendorId")?.Value;
                if (string.IsNullOrEmpty(vendorId))
                    return BadRequest(new { message = "Vendor ID not found in token" });

                _logger.LogInformation($"Vendor {vendorId} retrieving their shipments (page {pageNumber}, size {pageSize})");

                var shipments = await _shipmentService.GetVendorShipmentsAsync(
                    Guid.Parse(vendorId),
                    pageNumber,
                    pageSize);

                return Ok(shipments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vendor shipments");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
