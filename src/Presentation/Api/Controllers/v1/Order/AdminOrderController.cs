using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order;

/// <summary>
/// Controller for Admin order operations
/// Handles: Search all orders, manage orders, change status, view statistics
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin/orders")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminOrderController : BaseController
{
    private readonly IAdminOrderService _adminOrderService;

    public AdminOrderController(IAdminOrderService adminOrderService)
    {
        _adminOrderService = adminOrderService ?? throw new ArgumentNullException(nameof(adminOrderService));
    }

    // ============================================
    // READ OPERATIONS
    // ============================================

    /// <summary>
    /// Search Orders with advanced filtering
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Returns paginated orders with search and filter support.<br/>
    /// Search supports: text search, status:X, payment:Y filters.
    /// </remarks>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SearchOrders(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = "createdDateUtc",
        [FromQuery] string? sortDirection = "desc")
    {
        try
        {
            var (orders, totalRecords) = await _adminOrderService.SearchOrdersAsync(
                searchTerm,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection);

            return Ok(new ResponseModel<AdminOrderPagedResult>
            {
                Success = true,
                Message = "Orders retrieved successfully",
                Data = new AdminOrderPagedResult
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
            return BadRequest(new ResponseModel<AdminOrderPagedResult>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Get Order Details by ID (Admin view)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Returns full order details including customer info, payments, shipments.
    /// </remarks>
    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetOrderDetails(Guid orderId)
    {
        var order = await _adminOrderService.GetAdminOrderDetailsAsync(orderId);

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found"
            });
        }

        return Ok(new ResponseModel<AdminOrderDetailsDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = order
        });
    }

    /// <summary>
    /// Get Today's Orders Count
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Returns count of orders created today.
    /// </remarks>
    [HttpGet("statistics/today-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTodayOrdersCount()
    {
        var count = await _adminOrderService.CountTodayOrdersAsync(DateTime.UtcNow.Date);

        return Ok(new ResponseModel<int>
        {
            Success = true,
            Message = "Today's orders count retrieved successfully",
            Data = count
        });
    }

    // ============================================
    // WRITE OPERATIONS
    // ============================================

    /// <summary>
    /// Change Order Status
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Changes order status with validation.<br/>
    /// Valid transitions: Pending → Confirmed → Processing → Shipped → Delivered → Completed
    /// </remarks>
    [HttpPost("{orderId}/change-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeOrderStatus(
        [FromBody] ChangeOrderStatusRequest request)
    {
        try
        {
            if (request.OrderId == Guid.Empty)
            {
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid order ID",
                    Data = false
                });
            }

            var result = await _adminOrderService.ChangeOrderStatusAsync(
                request.OrderId,
                request.NewStatus,
                request.Notes,
                UserId); // Admin user ID

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
    /// Change Shipment Status for an Order (Admin)
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Validates that the shipment belongs to the order and records status history.
    /// </remarks>
    [HttpPut("{orderId}/shipments/{shipmentId}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateShipmentStatus(
        Guid orderId,
        Guid shipmentId,
        [FromBody] UpdateShipmentStatusRequest request)
    {
        try
        {
            if (shipmentId != request.ShipmentId)
            {
                return BadRequest(new ResponseModel<ShipmentDto>
                {
                    Success = false,
                    Message = "Shipment ID mismatch"
                });
            }

            request.OrderId = orderId;

            var result = await _adminOrderService.UpdateShipmentStatusAsync(
                orderId,
                request,
                UserId);

            if (!result.Success)
            {
                if (result.Message == "Order not found" || result.Message == "Shipment not found for this order")
                {
                    return NotFound(result);
                }

                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel<ShipmentDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Update Order Details
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Admin role.<br/>
    /// Updates order details like delivery date.
    /// </remarks>
    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateOrder(
        Guid orderId,
        [FromBody] UpdateOrderRequest request)
    {
        try
        {
            if (orderId != request.OrderId)
            {
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order ID mismatch",
                    Data = false
                });
            }

            var result = await _adminOrderService.UpdateOrderAsync(request);

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
    /// Paginated result model for admin orders
    /// </summary>
    public class AdminOrderPagedResult
    {
        public List<AdminOrderListDto> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
