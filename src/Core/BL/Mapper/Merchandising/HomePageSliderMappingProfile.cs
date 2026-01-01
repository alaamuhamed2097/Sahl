using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.VideoProvider;
using Shared.DTOs.HomeSlider;
using Shared.DTOs.Media;

namespace BL.Mapper;

// Media mappings partial (MappingProfile.Media.cs)
public partial class MappingProfile
{
    private void ConfigureHomePageSliderMappingProfile()
    {
       
        CreateMap<TbHomePageSlider, HomePageSliderDto>()
            .ReverseMap();

    }
}
