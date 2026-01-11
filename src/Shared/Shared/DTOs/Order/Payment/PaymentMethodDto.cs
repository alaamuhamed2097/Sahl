using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment
{
    public class PaymentMethodDto
    {
        public Guid Id { get; set; }
        public string TitleEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public PaymentMethodType MethodType { get; set; }
        public bool IsActive { get; set; }
        public string? ProviderDetails { get; set; }
    }
}
