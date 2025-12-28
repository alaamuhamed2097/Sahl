namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Cancel payment request
    /// </summary>
    public class CancelPaymentRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
