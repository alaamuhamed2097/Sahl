using Common.Enumerations.FieldType;
using Common.Enumerations.Offer;
using System.Text.Json.Serialization;

namespace Domains.Procedures
{

    /// <summary>
    /// Main result from SpGetItemDetails
    /// Single row with JSON columns
    /// </summary>
    public class SpGetItemDetails
    {
        // Basic Item Info
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string ThumbnailImage { get; set; }

        // Category Info
        public Guid CategoryId { get; set; }
        public string CategoryNameAr { get; set; }
        public string CategoryNameEn { get; set; }
        public int PricingSystemType { get; set; }
        public string PricingSystemNameAr { get; set; }
        public string PricingSystemNameEn { get; set; }

        // Brand Info
        public Guid? BrandId { get; set; }
        public string? BrandNameAr { get; set; }
        public string? BrandNameEn { get; set; }
        public string? BrandLogoUrl { get; set; }

        // Flags
        public bool IsMultiVendor { get; set; }

        // Reviews
        public decimal AverageRating { get; set; }

        // JSON Columns (to be parsed)
        public string? GeneralImagesJson { get; set; }
        public string? AttributesJson { get; set; }
        public string? CurrentCombinationJson { get; set; }
        public string? PricingJson { get; set; }
    }

    // ========== Parsed Objects (from JSON) ==========

    public class ItemImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
    }

    public class AttributeInfo
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public FieldType FieldType { get; set; }
        public int DisplayOrder { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
    }

    public class AttributeOption
    {
        public Guid ValueId { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CurrentCombination
    {
        public Guid CombinationId { get; set; }
        public string? SKU { get; set; }
        public string? Barcode { get; set; }
        public bool IsDefault { get; set; }
        public Guid CreatedBy { get; set; }

        [JsonPropertyName("PricingAttributesJson")]
        public List<PricingAttribute>? PricingAttributes { get; set; }
        [JsonPropertyName("ImagesJson")]
        public List<ItemImage>? Images { get; set; }
    }

    public class PricingAttribute
    {
        public Guid AttributeId { get; set; }
        public string AttributeNameAr { get; set; } = null!;
        public string AttributeNameEn { get; set; } = null!;
        public Guid CombinationValueId { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
        public bool IsSelected { get; set; }
    }

    public class PricingInfo
    {
        public int VendorCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        [JsonPropertyName("BestOfferJson")]
        public string? BestOffer { get; set; }
    }

    public class BestOffer
    {
        public Guid OfferId { get; set; }
        public Guid VendorId { get; set; }
        public string? VendorName { get; set; }
        public decimal? VendorRating { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int AvailableQuantity { get; set; }
        public StockStatus StockStatus { get; set; }
        public bool IsFreeShipping { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsBuyBoxWinner { get; set; }
        public int MinOrderQuantity { get; set; }
        public int MaxOrderQuantity { get; set; }
        [JsonPropertyName("QuantityTiersJson")]
        public List<QuantityTier>? QuantityTiers { get; set; }
    }

    public class QuantityTier
    {
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class VendorOffer
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
        public List<QuantityTier> QuantityTiers { get; set; }
        public int OfferRank { get; set; }
    }

    public class CombinationSummary
    {
        public int TotalVendors { get; set; }
        public bool IsMultiVendor { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public int TotalStock { get; set; }
    }

    public class MissingAttribute
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Status { get; set; }
    }
}
