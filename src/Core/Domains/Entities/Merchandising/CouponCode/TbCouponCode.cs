using Common.Enumerations.Order;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising.CouponCode
{
    public class TbCouponCode : BaseEntity
    {
        // Basic Information
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        // Promotion Type
        [Required]
        public CouponCodeType PromoType { get; set; }

        // Discount Configuration
        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxDiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumOrderAmount { get; set; }

        // Scope Conditions - REMOVED CategoryId & ProductId (use Scopes instead)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumProductPrice { get; set; }

        // Vendor (who created & who bears cost)
        public Guid? VendorId { get; set; }

        // Co-funded configuration
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal? PlatformSharePercentage { get; set; }

        // Scopes (Categories or Products)
        public virtual ICollection<TbCouponCodeScope> CouponScopes { get; set; } = new List<TbCouponCodeScope>();

        // Validity Period
        public DateTime StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Usage Limits
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; } = 0;
        public int? UsageLimitPerUser { get; set; }

        // Status Flags
        [Required]
        public bool IsActive { get; set; } = true;

        public bool IsFirstOrderOnly { get; set; } = false;

        // Navigation Properties
        [ForeignKey(nameof(VendorId))]
        public virtual TbVendor? Vendor { get; set; }

        public virtual ICollection<TbOrder> Orders { get; set; } = new List<TbOrder>();
    }
}