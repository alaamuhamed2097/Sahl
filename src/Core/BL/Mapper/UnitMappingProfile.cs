using Domains.Entities.Catalog.Unit;
using Domains.Views.Unit;
using Shared.DTOs.Catalog.Unit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BL.Mapper
{
    // Unit mappings partial (MappingProfile.Units.cs)
    public partial class MappingProfile
    {
        private void ConfigureUnitMappings()
        {
            // Unit main mappings
            CreateMap<TbUnit, UnitDto>()
                .ReverseMap();

            // Unit conversions
            CreateMap<TbUnitConversion, UnitConversionDto>()
                .ReverseMap();
            // Category with attributes
            CreateMap<VwUnitWithConversionsUnits, UnitDto>()
                .ForMember(dest => dest.ConversionUnitsFrom, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ConversionUnitsFromJson)
                        ? JsonSerializer.Deserialize<List<ConversionUnitDto>>(src.ConversionUnitsFromJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        : new List<ConversionUnitDto>()))
            .ForMember(dest => dest.ConversionUnitsTo, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.ConversionUnitsToJson)
                        ? JsonSerializer.Deserialize<List<ConversionUnitDto>>(src.ConversionUnitsToJson,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            })
                        : new List<ConversionUnitDto>()));
        }
    }
}
