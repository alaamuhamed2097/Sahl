namespace Shared.DTOs.ECommerce.Payment
{
    public class PaymentProcessRequest
    {
        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
    }

    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? TransactionId { get; set; }
        public Guid? OrderId { get; set; }
        public string? ErrorCode { get; set; }
    }

    public class RefundProcessRequest
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
        public string Reason { get; set; } = null!;
        public decimal RefundAmount { get; set; }
    }

    public class RefundResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public decimal RefundAmount { get; set; }
        public string RefundStatus { get; set; } = null!;
    }

    public class PaymentStatusDto
    {
        public Guid OrderId { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
