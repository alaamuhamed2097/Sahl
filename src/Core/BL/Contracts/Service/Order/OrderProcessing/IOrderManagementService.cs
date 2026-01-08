using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.ResponseOrderDetail;

namespace BL.Contracts.Service.Order.OrderProcessing;

/// <summary>
/// Service interface for managing orders (CRUD operations)
/// UPDATED: Cleaned up method signatures, removed unused methods
/// Note: Order creation is handled by IOrderCreationService
/// </summary>
public interface IOrderManagementService
{
    // ============================================
    // READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get order by ID
    /// </summary>
    Task<OrderDto?> GetOrderByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order by order number
    /// </summary>
    Task<OrderDto?> GetOrderByNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order with full details including shipments
    /// </summary>
    Task<OrderDto?> GetOrderWithShipmentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get customer orders with pagination
    /// Returns one DTO per order detail (for display in list)
    /// </summary>
    Task<List<OrderListItemDto>> GetCustomerOrdersAsync(
        string customerId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order items list by order ID
    /// </summary>
    Task<List<ResponseOrderItemDetailsDto>> GetListByOrderIdAsync(
        Guid orderId,
        string? userId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get detailed order information by order details ID
    /// </summary>
    Task<ResponseOrderDetailsDto?> GetOrderDetailsByIdAsync(
        Guid orderDetailsId,
        string userId,
        CancellationToken cancellationToken = default);

    // ============================================
    // WRITE OPERATIONS
    // ============================================

    /// <summary>
    /// Cancel order before shipping
    /// Can only cancel orders that are not yet shipped/delivered/cancelled
    /// </summary>
    Task<bool> CancelOrderAsync(
        Guid orderId,
        string reason,
        string? adminNotes = null,
        CancellationToken cancellationToken = default);

    // ============================================
    // VALIDATION & STATUS OPERATIONS
    // ============================================

    /// <summary>
    /// Get order completion status
    /// </summary>
    Task<OrderCompletionStatusDto> GetOrderCompletionStatusAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate order data
    /// </summary>
    Task<bool> ValidateOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}