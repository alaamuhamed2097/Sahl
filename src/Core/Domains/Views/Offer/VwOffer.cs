namespace Domains.Views.Offer
{
    public class VwOffer
    {
        // Offer basic fields
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public int VisibilityScope { get; set; }
        public int FulfillmentType { get; set; }
        public decimal VendorRatingForThisItem { get; set; }
        public int VendorSalesCountForThisItem { get; set; }
        public bool IsBuyBoxWinner { get; set; }
        public Guid? WarrantyId { get; set; }
        public DateTime CreatedDateUtc { get; set; }

        // Vendor info
        public string VendorFullName { get; set; } = string.Empty;
        public string VendorCompanyName { get; set; } = string.Empty;
        public string VendorEmail { get; set; } = string.Empty;
        public string VendorFullPhoneNumber { get; set; } = string.Empty;

        // Item fields
        public string ItemTitleAr { get; set; } = string.Empty;
        public string ItemTitleEn { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public string BrandTitleAr { get; set; } = string.Empty;
        public string BrandTitleEn { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryTitleAr { get; set; } = string.Empty;
        public string CategoryTitleEn { get; set; } = string.Empty;
        public int PricingSystemType { get; set; }

        // JSON FIELDS (string)
        public string ItemImages { get; set; } = "[]";
        public string ItemAttributes { get; set; } = "[]";
        public string OfferCombinationsJson { get; set; } = "[]";

        // Warranty
        public int? WarrantyType { get; set; }
        public int? WarrantyPeriodMonths { get; set; }
        public string? WarrantyPolicy { get; set; }
    }


}
