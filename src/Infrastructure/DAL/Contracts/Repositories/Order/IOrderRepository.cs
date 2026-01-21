using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Domains.Entities.Order;

namespace DAL.Contracts.Repositories.Order;

/// <summary>
/// Repository interface for Order operations
/// Inherits all CRUD operations from ITableRepository<TbOrder>
/// Adds order-specific query methods
/// </summary>
public interface IOrderRepository : ITableRepository<TbOrder>
{
    Task<TbOrder?> GetOrderWithDetailsAsync(
       Guid orderId,
       CancellationToken cancellationToken = default);

    Task<TbOrder?> GetOrderWithShipmentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<TbOrder?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default);

    // REMOVED: GetByInvoiceIdAsync - No InvoiceId field anymore

    Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default);

    Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<List<TbOrder>> GetOrdersByStatusAsync(
        OrderProgressStatus status,
        CancellationToken cancellationToken = default);

    Task<List<TbOrder>> GetOrdersByPaymentStatusAsync(
        PaymentStatus paymentStatus,
        CancellationToken cancellationToken = default);

    Task<(List<TbOrder> Orders, int TotalCount)> GetCustomerOrdersWithPaginationAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<TbOrder?> GetOrderWithFullDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<(List<TbOrder> Orders, int TotalCount)> GetVendorOrdersWithPaginationAsync(
        string vendorId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

    Task<(List<TbOrder> Orders, int TotalCount)> SearchAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken = default);

    Task<(List<TbOrder> Orders, int TotalCount)> SearchOrdersAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

    Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default);
}