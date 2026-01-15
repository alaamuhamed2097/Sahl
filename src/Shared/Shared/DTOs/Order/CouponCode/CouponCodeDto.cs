using Common.Enumerations.Order;
using Resources;

namespace Shared.DTOs.Order.CouponCode
{
    /// <summary>
    /// Main DTO for CouponCode CRUD operations
    /// </summary>
    public class CouponCodeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string Title => ResourceManager.CurrentLanguage == Resources.Enumerations.Language.English ? TitleEn : TitleAr;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public CouponCodeType PromoType { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MinimumProductPrice { get; set; }

        public Guid? VendorId { get; set; }
        public string? VendorName { get; set; }

        public decimal? PlatformSharePercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public int? UsageLimitPerUser { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFirstOrderOnly { get; set; }

        // Scopes (Categories or Items)
        public List<CouponScopeDto>? ScopeItems { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
