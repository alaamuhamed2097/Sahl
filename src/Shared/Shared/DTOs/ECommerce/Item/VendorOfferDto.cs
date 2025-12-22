using Common.Enumerations.Offer;

namespace Shared.DTOs.ECommerce.Item
{
    public class VendorOfferDto
    {
        public Guid OfferId { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorNameAr { get; set; }
        public decimal VendorRating { get; set; }
        public string VendorLogoUrl { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int AvailableQuantity { get; set; }
        public StockStatus StockStatus { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal ShippingCost { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsBuyBoxWinner { get; set; }
        public bool HasWarranty { get; set; }
        public string ConditionNameAr { get; set; }
        public string ConditionNameEn { get; set; }
        public string WarrantyTypeAr { get; set; }
        public string WarrantyTypeEn { get; set; }
        public int? WarrantyPeriodMonths { get; set; }
        public int MinOrderQuantity { get; set; }
        public int MaxOrderQuantity { get; set; }
        public List<QuantityTierDto>? QuantityTiers { get; set; }
        public int OfferRank { get; set; }
    }
}
