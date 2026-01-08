using Domains.Entities.Order.Payment;

namespace DAL.Contracts.Repositories.Order;

/// <summary>
/// Repository interface for order payment operations
/// </summary>
public interface IOrderPaymentRepository
{
    /// <summary>
    /// Get primary payment for an order
    /// </summary>
    Task<TbOrderPayment?> GetOrderPaymentAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all payments for an order (including split payments)
    /// </summary>
    Task<List<TbOrderPayment>> GetOrderPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payment by gateway transaction ID
    /// </summary>
    Task<TbOrderPayment?> GetByGatewayTransactionIdAsync(
        string gatewayTransactionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payment by ID
    /// </summary>
    Task<TbOrderPayment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new payment record
    /// </summary>
    Task<TbOrderPayment> CreateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing payment record
    /// </summary>
    Task UpdateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get pending payments for an order
    /// </summary>
    Task<List<TbOrderPayment>> GetPendingPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}