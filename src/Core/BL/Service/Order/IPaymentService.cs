using Shared.DTOs.ECommerce.Payment;

namespace BL.Services.Order
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(PaymentProcessRequest request);
        Task<PaymentStatusDto> GetPaymentStatusAsync(Guid orderId);
        Task<RefundResultDto> ProcessRefundAsync(RefundProcessRequest request);
        Task<bool> VerifyPaymentAsync(Guid orderId, string transactionId);
        Task<List<PaymentStatusDto>> GetOrderPaymentsAsync(Guid orderId);
    }
}
