namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Payment result DTO
    /// </summary>
    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public Guid? PaymentId { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentUrl { get; set; }
        public bool RequiresRedirect { get; set; }
    }
}
