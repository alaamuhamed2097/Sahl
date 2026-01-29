using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Category
{
    public class AttributeOptionDto : BaseDto
    {
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceName = "TitleArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TitleAr { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessageResourceName = "TitleEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TitleEn { get; set; }

        public int DisplayOrder { get; set; }
        
        [StringLength(200)]
        public string? Value { get; set; }
        
        [JsonIgnore]
        public string Title
     => ResourceManager.CurrentLanguage == Language.Arabic ? (TitleAr ?? Value ?? "") : (TitleEn ?? Value ?? "");

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid AttributeId { get; set; }
    }
}
