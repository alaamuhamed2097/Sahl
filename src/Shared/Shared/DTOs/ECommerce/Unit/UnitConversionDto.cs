using Resources;
using Resources.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Unit
{
    public class UnitConversionDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid ConversionUnitId { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        [JsonIgnore]
        public string Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal ConversionFactor { get; set; }

        [JsonIgnore]
        public Guid FromUnitId { get; set; }
        [JsonIgnore]
        public Guid ToUnitId { get; set; }

    }
}
