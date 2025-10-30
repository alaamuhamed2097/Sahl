using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Unit
{
    public class UnitDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = null!;

        [JsonIgnore]
        public string Title
         => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
        public List<ConversionUnitDto>? ConversionUnitsFrom { get; set; }
        public List<ConversionUnitDto>? ConversionUnitsTo { get; set; }
    }
}
