using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// Payment process request
    /// </summary>
    public class IPaymentProcessRequest
    {
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentProcessType ProcessType { get; set; }
    }
}
