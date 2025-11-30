using System.ComponentModel.DataAnnotations;

namespace Domains.Entities.Loyalty
{
    public class TbLoyaltyTier : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string TierNameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TierNameAr { get; set; } = string.Empty;

        [Required]
        public int MinimumOrdersPerYear { get; set; }

        [Required]
        public int MaximumOrdersPerYear { get; set; }

        [Required]
        public decimal PointsMultiplier { get; set; } = 1.0m;

        public decimal CashbackPercentage { get; set; } = 0m;

        public bool HasFreeShipping { get; set; }

        public bool HasPrioritySupport { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(50)]
        public string? BadgeColor { get; set; }

        [StringLength(200)]
        public string? BadgeIconPath { get; set; }

        public ICollection<TbCustomerLoyalty> CustomerLoyalties { get; set; } = new HashSet<TbCustomerLoyalty>();
    }
}
