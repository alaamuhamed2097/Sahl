using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment
{
    public class PaymentInfoDto
    {
        public PaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
