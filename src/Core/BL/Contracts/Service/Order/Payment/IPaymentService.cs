using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Contracts.Service.Order.Payment
{
    /// <summary>
    /// UPDATED IPaymentService - matches your existing structure
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Process payment for an order
        /// </summary>
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentProcessRequest request);

        /// <summary>
        /// Get payment status for an order
        /// </summary>
        Task<PaymentStatusDto> GetPaymentStatusAsync(Guid orderId);

        /// <summary>
        /// Get all payment attempts for an order
        /// </summary>
        Task<List<PaymentStatusDto>> GetOrderPaymentsAsync(Guid orderId);

        /// <summary>
        /// Get payment by ID
        /// </summary>
        Task<PaymentStatusDto?> GetPaymentByIdAsync(Guid paymentId);

        /// <summary>
        /// Verify payment transaction
        /// </summary>
        Task<bool> VerifyPaymentAsync(string transactionId);

        /// <summary>
        /// Process refund
        /// </summary>
        Task<RefundResultDto> ProcessRefundAsync(RefundProcessRequest request);

        /// <summary>
        /// Cancel payment
        /// </summary>
        Task<bool> CancelPaymentAsync(Guid paymentId, string reason);
    }
}