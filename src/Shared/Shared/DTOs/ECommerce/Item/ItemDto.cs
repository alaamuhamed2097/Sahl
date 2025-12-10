using Common.Enumerations.Visibility;
using Resources;
using Resources.Enumerations;
using Shared.Attributes;
using Shared.Contracts;
using Shared.DTOs.Base;
using Shared.DTOs.Currency;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Item
{
    public class ItemDto : BaseSeoDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = string.Empty;
        public string Title=> ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "ShortDescriptionArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ShortDescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "ShortDescriptionEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ShortDescriptionEn { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "DescriptionArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string DescriptionAr { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "DescriptionEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string DescriptionEn { get; set; } = string.Empty;

        [NotEmptyGuid]
        public Guid CategoryId { get; set; }

        [NotEmptyGuid]
        public Guid BrandId { get; set; }

        [NotEmptyGuid]
        public Guid UnitId { get; set; }

        // Convenience/display properties used by mappings and other services
        public string CategoryTitleAr { get; set; } = string.Empty;
        public string CategoryTitleEn { get; set; } = string.Empty;
        public string CategoryTitle=> ResourceManager.CurrentLanguage == Language.Arabic ? CategoryTitleAr : CategoryTitleEn;
        public string BrandTitleAr { get; set; } = string.Empty;
        public string BrandTitleEn { get; set; } = string.Empty;
        public string BrandTitle => ResourceManager.CurrentLanguage == Language.Arabic ? BrandTitleAr : BrandTitleEn;
        public string UnitTitleAr { get; set; } = string.Empty;
        public string UnitTitleEn { get; set; } = string.Empty;
        public string UnitTitle => ResourceManager.CurrentLanguage == Language.Arabic ? UnitTitleAr : UnitTitleEn;
        public string? VideoProviderTitleAr { get; set; }
        public string? VideoProviderTitleEn { get; set; } 
        public string? VideoProviderTitle => ResourceManager.CurrentLanguage == Language.Arabic ? VideoProviderTitleAr : VideoProviderTitleEn;

        public Guid? VideoProviderId { get; set; }
        public string? VideoUrl { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ThumbnailImage { get; set; } = null!;
        public string? Barcode { get; set; }
        public string? SKU { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
        public ProductVisibilityStatus VisibilityScope { get; set; }
        public DateTime CreatedDateUtc { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(1, ErrorMessageResourceName = "ImagesRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public List<ItemImageDto> Images { get; set; } = new();

       //public List<ItemCombinationDto> ItemCombinations { get; set; } = new();
       public List<ItemAttributeDto>? ItemAttributes { get; set; } = new();
    }
}
