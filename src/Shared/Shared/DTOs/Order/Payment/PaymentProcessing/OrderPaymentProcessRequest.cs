namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    public class OrderPaymentProcessRequest : IPaymentProcessRequest
    {
        public Guid OrderId { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
    }
}
