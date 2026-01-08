using DAL.Models;
using Domains.Entities.Order;

namespace DAL.Contracts.Repositories.Order;

/// <summary>
/// Repository interface for order operations
/// </summary>
public interface IOrderRepository : ITableRepository<TbOrder>
{
    /// <summary>
    /// Get order by ID
    /// </summary>
    Task<TbOrder?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get order with full details (items, address, payments)
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
    /// Get order by invoice ID
    /// </summary>
    Task<TbOrder?> GetByInvoiceIdAsync(
        string invoiceId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get customer orders with pagination
    /// </summary>
    Task<PagedResult<TbOrder>> GetCustomerOrdersPagedAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new order
    /// </summary>
    Task<TbOrder> CreateAsync(
        TbOrder order,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing order
    /// </summary>
    Task UpdateAsync(
        TbOrder order,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get orders by status
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
    /// Count today's orders for order number generation
    /// </summary>
    Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default);
}