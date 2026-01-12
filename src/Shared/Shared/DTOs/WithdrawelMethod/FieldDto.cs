using Common.Enumerations.FieldType;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.WithdrawelMethod
{
    public class FieldDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = null!;

        [JsonIgnore]
        public string Title
          => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]

        public FieldType FieldType { get; set; }
        public Guid WithdrawalMethodId { get; set; }

    }
}