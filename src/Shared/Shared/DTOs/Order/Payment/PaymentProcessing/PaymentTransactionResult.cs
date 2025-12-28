namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Result of payment transaction
    /// </summary>
    public class PaymentTransactionResult
    {
        public bool IsSuccess { get; set; }
        public Guid? OrderId { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentUrl { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
