using BL.Mapper;
using Domains.Entities.Pricing;
using Shared.DTOs.Pricing;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigurePricingMappings()
    {
        CreateMap<TbPricingSystemSetting, PricingSystemSettingDto>().ReverseMap();
    }
}
