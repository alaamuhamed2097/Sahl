using Common.Enumerations.Payment;
using Domains.Entities.Order.Payment;

namespace DAL.Contracts.Repositories.Order;

/// <summary>
/// FINAL IOrderPaymentRepository Interface
/// ✅ All methods for optimized payment queries
/// ✅ Includes methods with details for performance
/// </summary>
public interface IOrderPaymentRepository : ITableRepository<TbOrderPayment>
{
    /// <summary>
    /// Get single order payment (latest) without details
    /// </summary>
    Task<TbOrderPayment?> GetOrderPaymentAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ✅ Get single order payment WITH details (Order + PaymentMethod)
    /// Optimized for PaymentService.GetPaymentStatusAsync
    /// </summary>
    Task<TbOrderPayment?> GetOrderPaymentWithDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all order payments with details
    /// </summary>
    Task<List<TbOrderPayment>> GetOrderPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ✅ Get payment by ID WITH details
    /// Optimized for PaymentService.GetPaymentByIdAsync
    /// </summary>
    Task<TbOrderPayment?> GetPaymentWithDetailsAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payment by gateway transaction ID
    /// </summary>
    Task<TbOrderPayment?> GetByGatewayTransactionIdAsync(
        string gatewayTransactionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ✅ Get payment by transaction ID WITH details
    /// Optimized for PaymentService.VerifyPaymentAsync
    /// </summary>
    Task<TbOrderPayment?> GetByTransactionIdAsync(
        string transactionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payment by ID without details
    /// </summary>
    Task<TbOrderPayment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create payment record
    /// </summary>
    Task<TbOrderPayment> CreateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update payment record
    /// </summary>
    Task UpdateAsync(
        TbOrderPayment payment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get pending payments for order
    /// </summary>
    Task<List<TbOrderPayment>> GetPendingPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get completed payments for order
    /// Used for payment summary calculations
    /// </summary>
    Task<List<TbOrderPayment>> GetCompletedPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get payments by payment method type
    /// Useful for filtering wallet/card payments
    /// </summary>
    Task<List<TbOrderPayment>> GetPaymentsByMethodTypeAsync(
        Guid orderId,
        PaymentMethodType methodType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get failed payments for order
    /// Useful for retry logic
    /// </summary>
    Task<List<TbOrderPayment>> GetFailedPaymentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}