using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.SellerTier
{
    public class TbSellerTierBenefit : BaseEntity
    {
        [Required]
        [ForeignKey("SellerTier")]
        public Guid SellerTierId { get; set; }

        [Required]
        [StringLength(200)]
        public string BenefitNameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string BenefitNameAr { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(200)]
        public string? IconPath { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual TbSellerTier SellerTier { get; set; } = null!;
    }
}
