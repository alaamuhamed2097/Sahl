using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.OrderProcessing.VendorOrder;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order;

/// <summary>
/// Controller for Vendor order operations
/// Handles: View vendor orders, manage fulfillment
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/vendor/orders")]
[Authorize(Roles = nameof(UserRole.Vendor))]
public class VendorOrderController : BaseController
{
    private readonly IVendorOrderService _vendorOrderService;

    public VendorOrderController(IVendorOrderService vendorOrderService)
    {
        _vendorOrderService = vendorOrderService ?? throw new ArgumentNullException(nameof(vendorOrderService));
    }

    // ============================================
    // READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get Vendor Orders with pagination and search
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Vendor role.<br/>
    /// Returns only orders containing vendor's items.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetVendorOrders(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = "createdDateUtc",
        [FromQuery] string? sortDirection = "desc")
    {
        var result = await _vendorOrderService.GetVendorOrdersAsync(
            UserId, // VendorId from authenticated user
            searchTerm,
            pageNumber,
            pageSize,
            sortBy,
            sortDirection);

        return Ok(new ResponseModel<AdvancedPagedResult<VendorOrderListDto>>
        {
            Success = true,
            Message = "Vendor orders retrieved successfully",
            Data = result
        });
    }

    /// <summary>
    /// Get Vendor Order Details by ID
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+<br/>
    /// Requires Vendor role.<br/>
    /// Shows only items belonging to this vendor.
    /// </remarks>
    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetVendorOrderDetails(Guid orderId)
    {
        var order = await _vendorOrderService.GetVendorOrderDetailsAsync(
            orderId,
            UserId); // VendorId from authenticated user

        if (order == null)
        {
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "Order not found or contains no items from your store"
            });
        }

        return Ok(new ResponseModel<VendorOrderDetailsDto>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = order
        });
    }
}
