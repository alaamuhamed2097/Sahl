namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Refund process request
    /// </summary>
    public class RefundProcessRequest
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
