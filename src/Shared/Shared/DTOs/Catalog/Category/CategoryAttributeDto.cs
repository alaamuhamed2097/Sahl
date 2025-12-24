using Common.Enumerations.FieldType;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Catalog.Category
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
        public object? AttributeOptionsJson { get; set; } = new List<AttributeOptionDto>();

        private List<AttributeOptionDto>? _attributeOptions;
        private bool _optionsParsed = false;

        [JsonIgnore]
        public List<AttributeOptionDto>? AttributeOptions
        {
            get
            {
                if (_optionsParsed)
                    return _attributeOptions;

                try
                {
                    // If it's already a List, return it directly
                    if (AttributeOptionsJson is List<AttributeOptionDto> list)
                    {
                        Console.WriteLine($"[AttributeOptions] SUCCESS: Already a List with {list.Count} options for {TitleAr ?? TitleEn}");
                        _attributeOptions = list;
                        _optionsParsed = true;
                        return _attributeOptions;
                    }

                    Console.WriteLine($"[AttributeOptions] Parsing for {TitleAr ?? TitleEn}");
                    Console.WriteLine($"[AttributeOptions] Type: {AttributeOptionsJson?.GetType().Name}");

                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    if (AttributeOptionsJson is string jsonString && !string.IsNullOrEmpty(jsonString) && jsonString != "[]")
                    {
                        Console.WriteLine($"[AttributeOptions] Parsing from string (length: {jsonString.Length})");
                        _attributeOptions = JsonSerializer.Deserialize<List<AttributeOptionDto>>(jsonString, jsonOptions)?.OrderBy(o => o.DisplayOrder).ToList() ?? new();
                        Console.WriteLine($"[AttributeOptions] SUCCESS: Parsed {_attributeOptions.Count} options from string");
                    }
                    else if (AttributeOptionsJson is JsonElement jsonElement)
                    {
                        Console.WriteLine($"[AttributeOptions] Parsing from JsonElement (Kind: {jsonElement.ValueKind})");
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                        {
                            var rawText = jsonElement.GetRawText();
                            Console.WriteLine($"[AttributeOptions] Raw JSON (first 200 chars): {rawText.Substring(0, Math.Min(200, rawText.Length))}");
                            _attributeOptions = JsonSerializer.Deserialize<List<AttributeOptionDto>>(rawText, jsonOptions)?.OrderBy(o => o.DisplayOrder).ToList() ?? new();
                            Console.WriteLine($"[AttributeOptions] SUCCESS: Parsed {_attributeOptions.Count} options from JsonElement");
                        }
                        else
                        {
                            Console.WriteLine($"[AttributeOptions] WARNING: JsonElement is not an array");
                            _attributeOptions = new List<AttributeOptionDto>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[AttributeOptions] WARNING: No valid format, returning empty list");
                        _attributeOptions = new List<AttributeOptionDto>();
                    }

                    _optionsParsed = true;
                    return _attributeOptions;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AttributeOptions] ERROR: {ex.Message}");
                    Console.WriteLine($"[AttributeOptions] Stack: {ex.StackTrace}");
                    _attributeOptions = new List<AttributeOptionDto>();
                    _optionsParsed = true;
                    return _attributeOptions;
                }
            }
        }
    }
}
