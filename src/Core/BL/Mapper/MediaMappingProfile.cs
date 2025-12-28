using Domains.Entities.VideoProvider;
using Shared.DTOs.Media;

namespace BL.Mapper;

// Media mappings partial (MappingProfile.Media.cs)
public partial class MappingProfile
{
    private void ConfigureMediaMappings()
    {
        // Video providers
        CreateMap<TbVideoProvider, VideoProviderDto>()
            .ReverseMap();

        // Add additional media-related mappings here
        // CreateMap<TbImageGallery, ImageGalleryDto>().ReverseMap();
    }
}
