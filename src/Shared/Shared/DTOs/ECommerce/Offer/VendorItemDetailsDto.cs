using Common.Enumerations.Offer;
using Shared.DTOs.Base;
using Shared.DTOs.Catalog.Item;

namespace Shared.DTOs.ECommerce.Offer
{
    public class VendorItemDetailsDto 
    {
        // Offer basic fields
        public Guid VendorItemId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public int FulfillmentType { get; set; }
		public OfferVisibilityScope VisibilityScope { get; set; }

		public bool IsBuyBoxWinner { get; set; }
        public string StockStatus { get; set; }
        public Guid OfferConditionId { get; set; }
        public string ConditionNameAr { get; set; }
        public string ConditionNameEn { get; set; }
        public bool IsConditionNew { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public int AvailableQuantity { get; set; }

        // Warranty
        public Guid? WarrantyId { get; set; }
        public int? WarrantyType { get; set; }
        public int? WarrantyPeriodMonths { get; set; }
        public string? WarrantyPolicy { get; set; }

        // Vendor info
        public Guid VendorId { get; set; }
        public string VendorFullName { get; set; } = string.Empty;

        // Item combination details
        public Guid ItemCombinationId { get; set; }
        public string? Barcode { get; set; }
        public string? SKU { get; set; }
        public bool IsDefault { get; set; }

        // Item fields
        public Guid ItemId { get; set; }
        public string ItemTitleAr { get; set; } = string.Empty;
        public string ItemTitleEn { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public string BrandTitleAr { get; set; } = string.Empty;
        public string BrandTitleEn { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryTitleAr { get; set; } = string.Empty;
        public string CategoryTitleEn { get; set; } = string.Empty;
        public int PricingSystemType { get; set; }

        // Collections
        public List<ImageDto>? BaseItemImages { get; set; } = new List<ImageDto>();
        public List<ImageDto>? ItemCombinationImages { get; set; } = new List<ImageDto>();
        public List<PricingAttributeDto>? ConbinationAttributes { get; set; } = new List<PricingAttributeDto>();
    }
}
