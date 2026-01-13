using Common.Enumerations.Payment;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Contracts.Service.Order.Payment
{
    /// <summary>
    /// Service for processing order payments
    /// </summary>
    public interface IOrderPaymentProcessor
    {
        /// <summary>
        /// Process Cash on Delivery payment
        /// </summary>
        Task<PaymentResult> ProcessCashOnDeliveryAsync(
            Guid orderId,
            decimal amount,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Process Wallet payment
        /// </summary>
        Task<PaymentResult> ProcessWalletPaymentAsync(
            Guid orderId,
            decimal amount,
            string customerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Process Card payment via payment gateway
        /// </summary>
        Task<PaymentResult> ProcessCardPaymentAsync(
            Guid orderId,
            decimal amount,
            string customerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Process Mixed payment (Wallet + Card)
        /// </summary>
        Task<PaymentResult> ProcessMixedPaymentAsync(
            Guid orderId,
            decimal totalAmount,
            string customerId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify payment callback from gateway
        /// </summary>
        Task<bool> VerifyPaymentCallbackAsync(
            string gatewayTransactionId,
            bool isSuccess,
            string? failureReason = null,
            CancellationToken cancellationToken = default);
    }

}