using Domains.Entities.Order;

namespace DAL.Contracts.Repositories.Order;

/// <summary>
/// Repository interface for Order operations
/// Inherits all CRUD operations from ITableRepository<TbOrder>
/// Adds order-specific query methods
/// </summary>
public interface IOrderRepository : ITableRepository<TbOrder>
{
    // ============================================
    // ORDER-SPECIFIC READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get order with full details (includes OrderDetails, Shipments, Payments, etc.)
    /// </summary>
    Task<TbOrder?> GetOrderWithDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order with shipments
    /// </summary>
    Task<TbOrder?> GetOrderWithShipmentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order by order number
    /// </summary>
    Task<TbOrder?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order by invoice ID
    /// </summary>
    Task<TbOrder?> GetByInvoiceIdAsync(
        string invoiceId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get orders by customer ID (all orders)
    /// </summary>
    Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get orders by customer ID with pagination
    /// </summary>
    Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get orders by order status
    /// </summary>
    Task<List<TbOrder>> GetOrdersByStatusAsync(
        Common.Enumerations.Order.OrderProgressStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get orders by payment status
    /// </summary>
    Task<List<TbOrder>> GetOrdersByPaymentStatusAsync(
        Common.Enumerations.Payment.PaymentStatus paymentStatus,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Count orders created today for order number generation
    /// </summary>
    Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default);
}