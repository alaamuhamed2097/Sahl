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
    /// Get All Orders (Admin/Dashboard)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Returns all orders for admin dashboard.
    /// </remarks>
    [HttpGet("all")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderManagementService.GetAllOrdersAsync();

            return Ok(new ResponseModel<IEnumerable<OrderDto>>
            {
                Success = true,
                Message = "Data retrieved successfully",
                Data = orders
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel<IEnumerable<OrderDto>>
            {
                Success = false,
                Message = ex.Message,
                Data = Enumerable.Empty<OrderDto>()
            });
        }
    }

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
    /// Search Orders (Admin/Dashboard) - GET variant
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Returns paginated orders with search and filter support.
    /// </remarks>
    [HttpGet("search")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SearchOrdersGet(
        [FromQuery] string? searchTerm = "",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = "createdDateUtc",
        [FromQuery] string? sortDirection = "desc")
    {
        try
        {
            var (orders, totalRecords) = await _orderManagementService.SearchOrdersAsync(
                searchTerm,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection);

            return Ok(new ResponseModel<PaginatedModel<OrderDto>>
            {
                Success = true,
                Message = "Orders retrieved successfully",
                Data = new PaginatedModel<OrderDto>
                {
                    Items = orders,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel<PaginatedModel<OrderDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
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

    /// <summary>
    /// Update Order (Admin Dashboard)
    /// Used by Details.razor to save order changes (e.g., delivery date)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Updates order details like delivery date.
    /// </remarks>
    [HttpPut("{orderId}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateOrder(
        Guid orderId,
        [FromBody] OrderDto orderDto)
    {
        try
        {
            if (orderId != orderDto.Id)
            {
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order ID mismatch",
                    Data = false
                });
            }

            var result = await _orderManagementService.SaveAsync(orderDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message,
                Data = false
            });
        }
    }

    /// <summary>
    /// Change Order Status (Admin Dashboard)
    /// Used by Details.razor to change order status (Pending → Accepted → InProgress → Shipping → Delivered)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Changes order status with validation.
    /// </remarks>
    [HttpPost("{orderId}/change-status")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeOrderStatus(
        Guid orderId,
        [FromBody] ChangeOrderStatusRequest request)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid order ID",
                    Data = false
                });
            }

            // Create OrderDto with the new status
            var orderDto = new OrderDto
            {
                Id = orderId,
                CurrentState = request.NewStatus
            };

            var result = await _orderManagementService.ChangeOrderStatusAsync(orderDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message,
                Data = false
            });
        }
    }
}

/// <summary>
/// Request DTO for searching orders
/// </summary>
public class OrderSearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "createdDateUtc";
    public string SortDirection { get; set; } = "desc";
}

/// <summary>
/// Paginated model for API responses
/// </summary>
public class PaginatedModel<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

/// <summary>
/// Request DTO for cancelling an order
/// </summary>
public class CancelOrderRequest
{
    public string Reason { get; set; } = string.Empty;
}