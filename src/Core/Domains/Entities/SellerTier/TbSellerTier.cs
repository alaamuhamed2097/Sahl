using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.SellerTier
{
    public class TbSellerTier : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string TierCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TierNameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TierNameAr { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [Required]
        public int MinimumOrders { get; set; }

        public int? MaximumOrders { get; set; }

        [Required]
        public decimal CommissionReductionPercentage { get; set; } = 0m;

        public bool HasPrioritySupport { get; set; }

        public bool HasBuyBoxBoost { get; set; }

        public bool HasFeaturedListings { get; set; }

        public bool HasAdvancedAnalytics { get; set; }

        public bool HasDedicatedAccountManager { get; set; }

        [StringLength(50)]
        public string? BadgeColor { get; set; }

        [StringLength(200)]
        public string? BadgeIconPath { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<TbSellerTierBenefit> Benefits { get; set; } = new HashSet<TbSellerTierBenefit>();
        public ICollection<TbVendorTierHistory> VendorTierHistories { get; set; } = new HashSet<TbVendorTierHistory>();
    }
}
