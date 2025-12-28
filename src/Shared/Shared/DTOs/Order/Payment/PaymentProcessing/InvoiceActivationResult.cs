namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Result of invoice activation
    /// </summary>
    public class InvoiceActivationResult
    {
        public bool IsActivated { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? OrderId { get; set; }
    }
}
