using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Payment status DTO
    /// </summary>
    public class PaymentStatusDto
    {
        public Guid? PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool CanRetry { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
