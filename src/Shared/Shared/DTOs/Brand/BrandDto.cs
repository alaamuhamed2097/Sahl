using Resources;
using Resources.Enumerations;
using Shared.Attributes;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Brand
{
    public class BrandDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(50, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NameEn { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(50, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string NameAr { get; set; } = string.Empty;

        [JsonIgnore]
        public string Name => ResourceManager.CurrentLanguage == Language.Arabic ? NameAr : NameEn;

        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? DescriptionEn { get; set; }

        [StringLength(100, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? DescriptionAr { get; set; }

        [JsonIgnore]
        public string? Description => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;

        public int DisplayOrder { get; set; } = 0;

        [Url(ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? WebsiteUrl { get; set; }

        // Required only on Create (custom validation)
        [RequiredImageOnCreate]
        public string? Base64Image { get; set; } // Base64 from Blazor input

        public string? LogoPath { get; set; } = string.Empty;

        public DateTime? CreatedDateUtc { get; set; }

        [JsonIgnore]
        public string CreatedDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(CreatedDateUtc ?? new DateTime(),
             TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"))
             .ToString("d MMM yyyy");
    }
}