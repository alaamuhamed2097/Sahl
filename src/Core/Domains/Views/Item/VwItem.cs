using Common.Enumerations.Offer;
using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domains.Views.Item
{
    public class VwItem
    {
        // ✅ الحقول الأساسية
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryTitleAr { get; set; }
        public string CategoryTitleEn { get; set; }
        public Guid UnitId { get; set; }
        public string UnitTitleAr { get; set; }
        public string UnitTitleEn { get; set; }
        public string ThumbnailImage { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; }
        public Guid? VideoProviderId { get; set; }
        public string? VideoProviderTitleAr { get; set; }
        public string? VideoProviderTitleEn { get; set; }
        public string? VideoLink { get; set; }
        public bool IsNewArrival { get; set; }
        public Guid? ByBoxOfferId { get; set; }
        public Guid? ByBoxUserId { get; set; }
        public string? ItemImagesJson { get; set; }
        public string? CombinationsJson { get; set; }

        // ✅ البيانات الجديدة المضافة لدعم الفلاتر المتقدمة
        
        // البراند
        public Guid? BrandId { get; set; }
        public string BrandNameAr { get; set; }
        public string BrandNameEn { get; set; }

        // التقييم
        public decimal ItemAverageRating { get; set; } = 0;
        public int ItemRatingCount { get; set; } = 0;

        // بيانات الـ Offer الرئيسي
        public Guid VendorId { get; set; }
        public string VendorNameAr { get; set; }
        public string VendorNameEn { get; set; }
        public decimal VendorAverageRating { get; set; } = 0;
        public bool IsVerifiedVendor { get; set; } = false;
        public bool IsPrimeVendor { get; set; } = false;

        // السعر والعروض
        public decimal OfferPrice { get; set; }
        public decimal? OfferOriginalPrice { get; set; }
        public bool IsOnSale { get; set; } = false;
        public decimal DiscountPercentage { get; set; } = 0;

        // التوفر والمخزون
        public bool IsInStock { get; set; } = true;
        public int AvailableQuantity { get; set; } = 0;
        public StorgeLocation StorgeLocation { get; set; }

        // الشحن
        public bool IsFreeShipping { get; set; } = false;
        public decimal ShippingCost { get; set; } = 0;
        public int EstimatedDeliveryDays { get; set; } = 0;
        public int MaxDeliveryDays { get; set; } = 30;

        // الحالة والضمان
        public Guid? OfferConditionId { get; set; }
        public string ConditionNameAr { get; set; }
        public string ConditionNameEn { get; set; }
        public bool HasWarranty { get; set; } = false;
        public int WarrantyMonths { get; set; } = 0;

        // Buy Box
        public bool IsBuyBoxWinner { get; set; } = false;

        // الإحصائيات
        public int TotalSalesCount { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;
    }
}