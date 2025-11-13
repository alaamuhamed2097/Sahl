using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domins.Entities.Setting
{
    public class TbSetting : BaseSeo
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(4)]
        public string PhoneCode { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [StringLength(200)]
        public string? FacebookUrl { get; set; }

        [StringLength(200)]
        public string? InstagramUrl { get; set; }

        [StringLength(200)]
        public string? TwitterUrl { get; set; }

        [StringLength(200)]
        public string? LinkedInUrl { get; set; }

        [StringLength(20)]
        public string? WhatsAppNumber { get; set; }

        [StringLength(4)]
        public string? WhatsAppCode { get; set; }

        public string? MainBannerPath { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ShippingAmount { get; set; } = 0m;

        [Range(0, 100)]
        [DefaultValue(14)]
        public decimal OrderTaxPercentage { get; set; } = 14;

        [Range(0, double.MaxValue)]
        [DefaultValue(0)]
        public decimal OrderExtraCost { get; set; } = 0m;
    }
}
