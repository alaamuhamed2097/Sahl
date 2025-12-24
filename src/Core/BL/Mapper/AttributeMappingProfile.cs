using Domains.Entities.Catalog.Attribute;
using Domains.Views.Category;
using Shared.DTOs.Catalog.Category;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Attribute-related mappings
    public partial class MappingProfile
    {
        private void ConfigureAttributeMappings()
        {
            // Attribute main mappings
            CreateMap<TbAttribute, AttributeDto>()
                .ReverseMap()
                 .ForMember(dest => dest.AttributeOptions, opt => opt.Ignore());

            // Attribute options
            CreateMap<TbAttributeOption, AttributeOptionDto>()
                .ReverseMap();
            // Attribute with options
            CreateMap<VwAttributeWithOptions, AttributeDto>()
                .ForMember<List<AttributeOptionDto>>(dest => dest.AttributeOptions, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.AttributeOptionsJson)
                        ? JsonSerializer.Deserialize<List<AttributeOptionDto>>(src.AttributeOptionsJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            }) ?? new List<AttributeOptionDto>()
                            .OrderBy(o => o.DisplayOrder).ToList()
                        : new List<AttributeOptionDto>()));
        }
    }
}
