using Common.Enumerations.FieldType;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Category
{
    public class CategoryAttributeDto : BaseDto
    {
        public Guid CategoryId { get; set; } = Guid.Empty;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public Guid AttributeId { get; set; }

        //[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        //[StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TitleAr { get; set; }

        //[Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        //[StringLength(200, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string? TitleEn { get; set; }

        [JsonIgnore]
        public string? Title
        => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        public bool IsRangeFieldType { get; set; }
        public bool AffectsPricing { get; set; }

        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }

        public FieldType FieldType { get; set; }


        public int? MaxLength { get; set; }


        [JsonPropertyName("AttributeOptionsJson")]
        public object? AttributeOptionsJson { get; set; } = "[]";


        public List<AttributeOptionDto>? AttributeOptions
        {
            get
            {
                try
                {
                    if (AttributeOptionsJson is string jsonString && !string.IsNullOrEmpty(jsonString))
                        return JsonSerializer.Deserialize<List<AttributeOptionDto>>(jsonString)?.OrderBy(o => o.DisplayOrder).ToList() ?? new();

                    if (AttributeOptionsJson is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                        return JsonSerializer.Deserialize<List<AttributeOptionDto>>(jsonElement.GetRawText())?.OrderBy(o => o.DisplayOrder).ToList() ?? new();

                    return new List<AttributeOptionDto>();
                }
                catch
                {
                    return new List<AttributeOptionDto>();
                }
            }
        }
    }
}
