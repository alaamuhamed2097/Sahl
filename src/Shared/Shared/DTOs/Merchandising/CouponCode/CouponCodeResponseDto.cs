using Common.Enumerations.Order;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// DTO for coupon code response
    /// </summary>
    public class CouponCodeResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public CouponCodeType PromoType { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MinimumProductPrice { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
        public decimal? PlatformSharePercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public int? UsageLimitPerUser { get; set; }
        public bool IsActive { get; set; }
        public bool IsFirstOrderOnly { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
