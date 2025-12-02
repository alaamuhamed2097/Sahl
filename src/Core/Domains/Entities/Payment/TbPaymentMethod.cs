using System.ComponentModel.DataAnnotations;
using Common.Enumerations.Payment;

namespace Domains.Entities.Payment
{
    public class TbPaymentMethod : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        public PaymentMethod MethodType { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? ProviderDetails { get; set; }
    }
}
