namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Payment process request
    /// </summary>
    public class PaymentProcessRequest
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
    }
}
