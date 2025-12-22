namespace Shared.DTOs.Order
{
    /// <summary>
    /// Stage 5: Payment Processing Request
    /// </summary>
    public class ProcessPaymentRequest
    {
        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? PaymentGatewayReference { get; set; }
        public string? Notes { get; set; }
    }
}
