using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Services.Order;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Order;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Stage 3: Create Order from Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("create")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderFromCartRequest request)
        {
            var order = await _orderService.CreateOrderFromCartAsync(UserId, request);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = order.OrderId },
                new ResponseModel<OrderCreatedResponseDto>
                {
                    Success = true,
                    Message = "Order created successfully.",
                    Data = order
                });
        }

        /// <summary>
        /// Get Order by ID
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("{orderId}")]
        [Authorize(Roles = nameof(UserRole.Customer) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Vendor))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Order not found."
                });
            }

            // Only customers are restricted to their own orders
            if (RoleName == nameof(UserRole.Customer) && order.UserId != UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
                {
                    Success = false,
                    Message = "You do not have permission to access this order."
                });
            }

            return Ok(new ResponseModel<OrderDto>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = order
            });
        }

        /// <summary>
        /// Get Order by Order Number
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("number/{orderNumber}")]
        [Authorize(Roles = nameof(UserRole.Customer) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Vendor))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            if (order == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Order not found."
                });
            }

            return Ok(new ResponseModel<OrderDto>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = order
            });
        }

        /// <summary>
        /// Get Customer Orders with Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.
        /// </remarks>
        [HttpGet("my-orders")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetCustomerOrdersAsync(UserId, pageNumber, pageSize);

            return Ok(new ResponseModel<List<OrderListItemDto>>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = orders
            });
        }

        /// <summary>
        /// Get Order with Shipments and Details
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer, Admin, or Vendor role.
        /// </remarks>
        [HttpGet("{orderId}/with-shipments")]
        [Authorize(Roles = nameof(UserRole.Customer) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Vendor))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderWithShipments(Guid orderId)
        {
            var order = await _orderService.GetOrderWithShipmentsAsync(orderId);
            if (order == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Order not found."
                });
            }

            return Ok(new ResponseModel<OrderDto>
            {
                Success = true,
                Message = "Data retrieved successfully.",
                Data = order
            });
        }

        /// <summary>
        /// Cancel Order Before Shipping
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.
        /// </remarks>
        [HttpPost("{orderId}/cancel")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CancelOrder(Guid orderId, [FromBody] CancelOrderRequest request)
        {
            var result = await _orderService.CancelOrderAsync(orderId, request.Reason);

            if (!result)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Order cannot be cancelled at this stage."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Order cancelled successfully.",
                Data = new { OrderId = orderId }
            });
        }
    }

    public class CancelOrderRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}