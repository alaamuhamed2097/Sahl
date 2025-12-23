using Common.Enumerations.FieldType;
using Common.Enumerations.Pricing;

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
        public PricingSystemType PricingSystemType { get; set; }
        public string PricingSystemName { get; set; }

        public decimal AverageRating { get; set; }

        public List<ItemImageDto> GeneralImages { get; set; }
        public List<ItemAttributeDefinitionDto> Attributes { get; set; }

        // Default selected combination (matches search result price)
        public CurrentCombinationDto CurrentCombination { get; set; }

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
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
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

    public class CurrentCombinationDto
    {
        public Guid CombinationId { get; set; }
        public string? SKU { get; set; }
        public string? Barcode { get; set; }
        public bool IsDefault { get; set; }
        public List<PricingAttributeDto>? PricingAttributes { get; set; }
        public List<ItemImageDto>? Images { get; set; }
    }
}
