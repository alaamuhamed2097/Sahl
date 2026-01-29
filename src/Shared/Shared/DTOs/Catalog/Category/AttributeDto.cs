using Common.Enumerations.FieldType;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Category
{
    public class AttributeDto : BaseDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleArLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleAr { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "TitleEnLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string TitleEn { get; set; } = null!;
        public bool IsRangeFieldType { get; set; } = false;
        public DateTime CreatedDateUtc { get; set; }

        [JsonIgnore]
        public string Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        [Range((int)FieldType.Text, (int)FieldType.Color, ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public FieldType FieldType { get; set; }

        //[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        //[Range(1, int.MaxValue, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public int? MaxLength { get; set; }
        public List<AttributeOptionDto>? AttributeOptions { get; set; }
    }
}
