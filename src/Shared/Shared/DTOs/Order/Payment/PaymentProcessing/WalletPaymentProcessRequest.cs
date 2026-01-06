namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    public class WalletPaymentProcessRequest : IPaymentProcessRequest
    {
        public Guid WalletId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
