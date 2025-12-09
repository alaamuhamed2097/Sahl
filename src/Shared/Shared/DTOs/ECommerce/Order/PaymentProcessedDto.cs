using Common.Enumerations.Payment;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 5: Payment Processing Response
    /// </summary>
    public class PaymentProcessedDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime ProcessedAt { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
