using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.ResponseOrderDetail;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order;

/// <summary>
/// Controller for order management
/// UPDATED: Uses both OrderManagementService (CRUD) and OrderCreationService (creation)
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderManagementService _orderManagementService;
    private readonly IOrderCreationService _orderCreationService;

    public OrderController(
        IOrderManagementService orderManagementService,
        IOrderCreationService orderCreationService)
    {
        _orderManagementService = orderManagementService ?? throw new ArgumentNullException(nameof(orderManagementService));
        _orderCreationService = orderCreationService ?? throw new ArgumentNullException(nameof(orderCreationService));
    }

    // ============================================
    // CREATE OPERATIONS
    // ============================================

    /// <summary>
    /// Create Order from Cart (NEW - Using OrderCreationService)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer role.<br/>
    /// Creates order with payment processing.
    /// </remarks>
    [HttpPost("create")]
    [Authorize(Roles = nameof(UserRole.Customer))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _orderCreationService.CreateOrderAsync(request);

        if (!result.Success)
        {
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = result.Message
            });
        }

        return CreatedAtAction(
            nameof(GetOrderById),
            new { orderId = result.OrderId },
            new ResponseModel<CreateOrderResult>
            {
                Success = true,
                Message = "Order created successfully",
                Data = result
            });
    }

    // ============================================
    // READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get My Orders
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer role.<br/>
    /// Returns customer orders with pagination.
    /// </remarks>
    [HttpGet("my-orders")]
    [Authorize(Roles = nameof(UserRole.Customer))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCustomerOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var orders = await _orderManagementService.GetCustomerOrdersAsync(
            UserId,
            pageNumber,
            pageSize);

        return Ok(new ResponseModel<List<OrderListItemDto>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = orders
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
        var order = await _orderManagementService.GetOrderByIdAsync(orderId);

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found"
            });
        }

        // Only customers are restricted to their own orders
        if (RoleName == nameof(UserRole.Customer) && order.UserId != UserId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<string>
            {
                Success = false,
                Message = "You do not have permission to access this order"
            });
        }

        return Ok(new ResponseModel<OrderDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = order
        });
    }

    /// <summary>
    /// Get Order Items List
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer, Admin, or Vendor role.
    /// </remarks>
    [HttpGet("list-order-details/{orderId}")]
    [Authorize(Roles = nameof(UserRole.Customer) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Vendor))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetListByOrdersId([FromRoute] Guid orderId)
    {
        // For customers, pass userId for security check
        string? userId = RoleName == nameof(UserRole.Customer) ? UserId : null;

        var orderItems = await _orderManagementService.GetListByOrderIdAsync(orderId, userId);

        if (orderItems == null || !orderItems.Any())
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order items not found"
            });
        }

        return Ok(new ResponseModel<List<ResponseOrderItemDetailsDto>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = orderItems
        });
    }

    /// <summary>
    /// Get Order Details by ID
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer, Admin, or Vendor role.<br/>
    /// Customers can only view their own orders.
    /// </remarks>
    [HttpGet("{orderId}/details")]
    [Authorize(Roles = nameof(UserRole.Customer) + "," + nameof(UserRole.Admin) + "," + nameof(UserRole.Vendor))]
    [ProducesResponseType(typeof(ResponseModel<ResponseOrderDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderDetailsById(Guid orderId)
    {
        // Only customers need userId check
        string? userId = RoleName == nameof(UserRole.Customer) ? UserId : null;

        var orderDetails = await _orderManagementService.GetOrderDetailsByIdAsync(
            orderId,
            userId ?? string.Empty);

        if (orderDetails == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found"
            });
        }

        return Ok(new ResponseModel<ResponseOrderDetailsDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = orderDetails
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
        var order = await _orderManagementService.GetOrderByNumberAsync(orderNumber);

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found"
            });
        }

        return Ok(new ResponseModel<OrderDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = order
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
        var order = await _orderManagementService.GetOrderWithShipmentsAsync(orderId);

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found"
            });
        }

        return Ok(new ResponseModel<OrderDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = order
        });
    }

    // ============================================
    // WRITE OPERATIONS
    // ============================================

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
    public async Task<IActionResult> CancelOrder(
        Guid orderId,
        [FromBody] CancelOrderRequest request)
    {
        var result = await _orderManagementService.CancelOrderAsync(orderId, request.Reason);

        if (!result)
        {
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = "Order cannot be cancelled at this stage"
            });
        }

        return Ok(new ResponseModel<object>
        {
            Success = true,
            Message = "Order cancelled successfully",
            Data = new { OrderId = orderId }
        });
    }
}

/// <summary>
/// Request DTO for cancelling an order
/// </summary>
public class CancelOrderRequest
{
    public string Reason { get; set; } = string.Empty;
}