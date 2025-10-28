using AutoMapper;

namespace BL.Mapper
{
    // Main mapping profile file (MappingProfile.cs)
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureUserMappings();
            ConfigureNotificationMappings();
        }
    }
}
