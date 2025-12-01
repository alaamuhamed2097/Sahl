using Common.Enumerations.Pricing;
using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Pricing
{
    public class TbPricingSystemSetting : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string SystemNameAr { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string SystemNameEn { get; set; } = null!;

        [Required]
        public PricingSystemType SystemType { get; set; }

        public bool IsEnabled { get; set; } = true;

        public int DisplayOrder { get; set; }
    }
}
