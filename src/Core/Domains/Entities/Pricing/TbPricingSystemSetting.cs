using Common.Enumerations.Pricing;
using Domains.Entities.Catalog.Category;
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

        public virtual ICollection<TbCategory> Categories { get; set; } = new HashSet<TbCategory>();
    }
}
