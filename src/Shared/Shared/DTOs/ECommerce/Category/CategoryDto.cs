using Resources;
using Resources.Enumerations;
using Shared.Attributes;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Category
{
    public class CategoryDto : BaseDto
    {
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = null!;

        [JsonIgnore]
        public string Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        //[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid ParentId { get; set; } = Guid.Empty;

        public bool IsFinal { get; set; }
        public bool IsHomeCategory { get; set; }

        public bool IsFeaturedCategory { get; set; }

        public bool IsMainCategory { get; set; }
        public bool PriceRequired { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public int DisplayOrder { get; set; }
        public string? TreeViewSerial { get; set; }

        [RequiredImage(nameof(ImageUrl), 5, 1)]
        public string? ImageUrl { get; set; }

        [ConditionalRequiredImage(nameof(IsMainCategory), true, nameof(Icon), 5, 1, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? Icon { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public List<CategoryAttributeDto>? CategoryAttributes { get; set; }
    }
}