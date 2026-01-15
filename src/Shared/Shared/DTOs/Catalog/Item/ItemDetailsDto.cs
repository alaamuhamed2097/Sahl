using Common.Enumerations.Pricing;

namespace Shared.DTOs.Catalog.Item
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

        public bool IsMultiVendor { get; set; }
        public PricingStrategyType PricingSystemType { get; set; }
        public string PricingSystemName { get; set; }

        public decimal AverageRating { get; set; }

        public List<ImageDto> GeneralImages { get; set; }
        public List<ItemAttributeDefinitionDto> Attributes { get; set; }

        // Default selected combination (matches search result price)
        public CurrentCombinationDto CurrentCombination { get; set; }

        // Pricing for default combination
        public PricingDto Pricing { get; set; }
    }
}
