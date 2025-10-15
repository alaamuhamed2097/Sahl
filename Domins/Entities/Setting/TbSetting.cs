using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domains.Entities.Setting
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
        public decimal WithdrawalLimit { get; set; } = 1000m;

        [Range(0, double.MaxValue)]
        public decimal WithdrawalFeePersentage { get; set; } = 1m;

        [Range(0, double.MaxValue)]
        public decimal DirectSaleTargetAmount { get; set; } = 10000m;

        [Range(0, int.MaxValue)]
        public int RequiredMarketersCount { get; set; } = 2;
        
        [Range(0, 100)]
        public decimal Level1Percentage { get; set; } = 0;

        [Range(0, 100)]
        public decimal Level2Percentage { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal ShippingAmount { get; set; } = 0m;
    }
}
