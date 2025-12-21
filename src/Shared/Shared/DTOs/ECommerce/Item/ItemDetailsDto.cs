using Common.Enumerations.FieldType;
using Common.Enumerations.Offer;

namespace Shared.DTOs.ECommerce.Item
{
    /// <summary>
    /// Main response for GET /api/items/{id}
    /// </summary>
    public class ItemDetailsDto
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string ThumbnailImage { get; set; }

        public CategoryInfoDto Category { get; set; }
        public BrandInfoDto Brand { get; set; }

        public bool HasCombinations { get; set; }
        public bool IsMultiVendor { get; set; }
        public int PricingSystemType { get; set; }
        public string PricingSystemName { get; set; }

        public decimal AverageRating { get; set; }

        public List<ItemImageDto> GeneralImages { get; set; }
        public List<ItemAttributeDefinitionDto> Attributes { get; set; }

        // Default selected combination (matches search result price)
        public DefaultCombinationDto DefaultCombination { get; set; }

        // Pricing for default combination
        public PricingDto Pricing { get; set; }
    }

    public class CategoryInfoDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class BrandInfoDto
    {
        public Guid? Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string LogoUrl { get; set; }
    }

    //public class ItemImageDto
    //{
    //    public string ImageUrl { get; set; }
    //    public int DisplayOrder { get; set; }
    //    public bool IsDefault { get; set; }
    //}

    // <summary>
    // Can be either pricing attribute(with options) or spec attribute(with value)
    // </summary>
    public class ItemAttributeDefinitionDto
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public FieldType FieldType { get; set; }
        public int DisplayOrder { get; set; }
        public string Value { get; set; }
    }

    public class ItemAttributeOptionDto
    {
        public Guid ValueId { get; set; }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class AttributeValueDto
    {
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
    }

    public class DefaultCombinationDto
    {
        public Guid CombinationId { get; set; }
        public string? SKU { get; set; }
        public string? Barcode { get; set; }
        public bool IsDefault { get; set; }
        public List<SelectedAttributeDto>? SelectedAttributes { get; set; }
        public List<ItemImageDto>? Images { get; set; }
    }

    public class SelectedAttributeDto
    {
        public Guid AttributeId { get; set; }
        public string AttributeNameAr { get; set; } = null!;
        public string AttributeNameEn { get; set; } = null!;
        public Guid CombinationValueId { get; set; }
        public string Value { get; set; }
    }

    public class PricingDto
    {
        public int VendorCount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public BestPriceOfferDto BestOffer { get; set; } = new BestPriceOfferDto();
    }

    public class BestPriceOfferDto
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
        public List<QuantityTierDto>? QuantityTiers { get; set; }
    }

    public class QuantityTierDto
    {
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    /// <summary>
    /// Response for POST /api/items/{id}/combination
    /// </summary>
    public class CombinationDetailsDto
    {
        public Guid? CombinationId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; }

        public List<SelectedAttributeDto> SelectedAttributes { get; set; }
        public List<ItemImageDto> Images { get; set; }
        public List<VendorOfferDto> Offers { get; set; }
        public CombinationSummaryDto Summary { get; set; }

        /// <summary>
        /// If incomplete selection, shows missing attributes
        /// </summary>
        public List<MissingAttributeDto> MissingAttributes { get; set; }
    }

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

    public class CombinationSummaryDto
    {
        public int TotalVendors { get; set; }
        public bool IsMultiVendor { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal AvgPrice { get; set; }
        public int TotalStock { get; set; }
    }

    public class MissingAttributeDto
    {
        public Guid AttributeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Request body for POST /api/items/{id}/combination
    /// </summary>
    public class GetCombinationRequest
    {
        public List<AttributeSelectionDto> SelectedAttributes { get; set; }
    }

    public class AttributeSelectionDto
    {
        public Guid AttributeId { get; set; }
        public Guid ValueId { get; set; }
    }
}
