using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.OrderProcessing.CustomerOrder;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order;

/// <summary>
/// Controller for Customer order operations
/// Handles: View orders, order details, cancel orders
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/customer/orders")]
[Authorize(Roles = nameof(UserRole.Customer))]
public class CustomerOrderController : BaseController
{
    private readonly ICustomerOrderService _customerOrderService;
    private readonly IOrderCreationService _orderCreationService;

    public CustomerOrderController(
        ICustomerOrderService customerOrderService,
        IOrderCreationService orderCreationService)
    {
        _customerOrderService = customerOrderService ?? throw new ArgumentNullException(nameof(customerOrderService));
        _orderCreationService = orderCreationService ?? throw new ArgumentNullException(nameof(orderCreationService));
    }

    // ============================================
    // CREATE OPERATIONS
    // ============================================

    /// <summary>
    /// Create Order from Cart
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer role.<br/>
    /// Creates order with payment processing.
    /// </remarks>
    [HttpPost("create")]
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
    /// Get My Orders with pagination
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer role.<br/>
    /// Returns customer orders with pagination.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _customerOrderService.GetCustomerOrdersListAsync(
            UserId,
            pageNumber,
            pageSize);

        return Ok(new ResponseModel<AdvancedPagedResult<CustomerOrderListDto>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    /// Get Order by ID (Customer view)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Customer role.<br/>
    /// Customer can only view their own orders.
    /// </remarks>
    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = await _customerOrderService.GetCustomerOrderDetailsAsync(
            orderId,
            UserId);

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found or you don't have permission to view it"
            });
        }

        return Ok(new ResponseModel<CustomerOrderDetailsDto>
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
    /// Requires Customer role.<br/>
    /// Only orders in Pending or Confirmed status can be cancelled.
    /// </remarks>
    [HttpPost("{orderId}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CancelOrder(
        Guid orderId,
        [FromBody] CancelOrderRequest request)
    {
        var result = await _customerOrderService.CancelOrderAsync(
            orderId,
            UserId,
            request.Reason);

        if (!result.Success)
        {
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = result.Message
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