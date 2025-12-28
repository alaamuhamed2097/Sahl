namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Payment verification request
    /// </summary>
    public class VerifyPaymentRequest
    {
        public Guid OrderId { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }
}
