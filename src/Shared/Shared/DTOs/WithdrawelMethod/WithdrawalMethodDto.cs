using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using Shared.DTOs.WithdrawelMethod;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.WithdrawalMethod
{
    public class WithdrawalMethodDto : BaseDto
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
        public string? ImageFile { get; set; }

        [RequiredImage(nameof(ImageFile), 5, 1)]
        public string? ImagePath { get; set; }

        public List<FieldDto> Fields { get; set; } = new List<FieldDto>();
    }
}
