using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.CouponCode
{
    /// <summary>
    /// Coupon code entity for discount management
    /// </summary>
    public class TbCouponCode : BaseEntity
    {
        // Basic Info
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        // Discount Configuration
        [Required]
        public int DiscountType { get; set; } // 1=Percentage, 2=FixedAmount

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxDiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumOrderAmount { get; set; }

        // Date Range
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Usage Limits
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; } = 0;
        public int? UsageLimitPerUser { get; set; }

        // Status
        [Required]
        public bool IsActive { get; set; } = true;

        // Optional: First order only
        public bool IsFirstOrderOnly { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<TbOrder> Orders { get; set; } = new List<TbOrder>();
    }
}