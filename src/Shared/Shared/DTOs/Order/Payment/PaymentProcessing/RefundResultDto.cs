namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Refund result DTO
    /// </summary>
    public class RefundResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string RefundStatus { get; set; } = string.Empty;
        public Guid? RefundId { get; set; }
        public string? RefundTransactionId { get; set; }
        public decimal RefundAmount { get; set; }
    }
}
