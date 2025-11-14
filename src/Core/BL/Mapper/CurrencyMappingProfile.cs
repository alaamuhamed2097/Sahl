using Domains.Entities.Currency;
using Shared.DTOs.Currency;

namespace BL.Mapper
{
    /// <summary>
    /// Currency mappings partial (MappingProfile.Currency.cs)
    /// </summary>
    public partial class MappingProfile
    {
        private void ConfigureCurrencyMappings()
        {
            // Currency main mappings
            CreateMap<TbCurrency, CurrencyDto>()
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedDateUtc))
                .ReverseMap()
                .ForMember(dest => dest.UpdatedDateUtc, opt => opt.MapFrom(src => src.LastUpdated ?? DateTime.UtcNow));

            // Currency conversion mappings (if needed)
            CreateMap<CurrencyConversionDto, CurrencyConversionDto>()
                .ReverseMap();
        }
    }
}