using Api.Controllers.Base;
using Asp.Versioning;
using BL.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Order;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, Serilog.ILogger serilogLogger)
            : base(serilogLogger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 3: Create Order from Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} creating order with delivery address {request.DeliveryAddressId}");

                var order = await _orderService.CreateOrderAsync(customerId, request);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Order by ID
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                _logger.LogInformation($"User {userId} retrieving order {orderId}");

                var order = await _orderService.GetOrderByIdAsync(orderId);

                if (userRole == "Customer" && order.UserId != userId)
                    return Forbid();

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Order by Order Number
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("number/{orderNumber}")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderByNumber(string orderNumber)
        {
            try
            {
                _logger.LogInformation($"Retrieving order by number {orderNumber}");

                var order = await _orderService.GetOrderByNumberAsync(orderNumber);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order by number");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Customer Orders with Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<OrderListItemDto>>> GetCustomerOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} retrieving their orders (page {pageNumber}, size {pageSize})");

                var orders = await _orderService.GetCustomerOrdersAsync(customerId, pageNumber, pageSize);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer orders");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Order with Shipments and Details
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("{orderId}/with-shipments")]
        [Authorize(Roles = "Customer,Admin,Vendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderWithShipments(Guid orderId)
        {
            try
            {
                _logger.LogInformation($"Retrieving order {orderId} with shipments");

                var order = await _orderService.GetOrderWithShipmentsAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order with shipments");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cancel Order Before Shipping
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("{orderId}/cancel")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<object>> CancelOrder(Guid orderId, [FromBody] CancelOrderRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} cancelling order {orderId} with reason: {request.Reason}");

                var result = await _orderService.CancelOrderAsync(orderId, request.Reason);

                if (!result)
                    return BadRequest(new { message = "Order cannot be cancelled at this stage" });

                return Ok(new { message = "Order cancelled successfully", orderId = orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order");
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CancelOrderRequest
    {
        public string Reason { get; set; } = null!;
    }
}
