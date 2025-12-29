using Common.Enumerations.Order;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// DTO for creating a new coupon code
    /// </summary>
    public class CreateCouponCodeDto
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arabic title is required")]
        [StringLength(200, ErrorMessage = "Arabic title cannot exceed 200 characters")]
        public string TitleAr { get; set; } = string.Empty;

        [Required(ErrorMessage = "English title is required")]
        [StringLength(200, ErrorMessage = "English title cannot exceed 200 characters")]
        public string TitleEn { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        public string? DescriptionAr { get; set; }

        [StringLength(500, ErrorMessage = "English description cannot exceed 500 characters")]
        public string? DescriptionEn { get; set; }

        [Required(ErrorMessage = "Promo type is required")]
        public CouponCodeType PromoType { get; set; }

        [Required(ErrorMessage = "Discount type is required")]
        public DiscountType DiscountType { get; set; }

        [Required(ErrorMessage = "Discount value is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount value must be greater than 0")]
        public decimal DiscountValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max discount amount must be non-negative")]
        public decimal? MaxDiscountAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum order amount must be non-negative")]
        public decimal? MinimumOrderAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum product price must be non-negative")]
        public decimal? MinimumProductPrice { get; set; }

        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? VendorId { get; set; }

        [Range(0, 100, ErrorMessage = "Platform share percentage must be between 0 and 100")]
        public decimal? PlatformSharePercentage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1")]
        public int? UsageLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Usage limit per user must be at least 1")]
        public int? UsageLimitPerUser { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFirstOrderOnly { get; set; } = false;

        // For ProductGroup scope
        public List<int>? ProductIds { get; set; }
    }
}
