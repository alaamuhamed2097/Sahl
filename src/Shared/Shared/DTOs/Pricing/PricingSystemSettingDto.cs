using Common.Enumerations.Pricing;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Pricing
{
    public class PricingSystemSettingDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string SystemNameAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string SystemNameEn { get; set; } = null!;

        [JsonIgnore]
        public string SystemName => ResourceManager.CurrentLanguage == Language.Arabic ? SystemNameAr : SystemNameEn;

        [Required]
        public PricingSystemType SystemType { get; set; }

        public bool IsEnabled { get; set; } = true;

        public int DisplayOrder { get; set; }
    }
}
