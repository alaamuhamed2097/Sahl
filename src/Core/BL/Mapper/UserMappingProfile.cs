using Domains.Identity;
using Shared.DTOs.User.Admin;

namespace BL.Mapper
{
    // User mappings partial (MappingProfile.Users.cs)
    public partial class MappingProfile
    {
        private void ConfigureUserMappings()
        {
            // User account mappings
            CreateMap<ApplicationUser, AdminRegistrationDto>()
                .ReverseMap();

            CreateMap<AdminCreateDto, ApplicationUser>();
            CreateMap<ApplicationUser, AdminDto>();
            CreateMap<AdminUpdateDto, ApplicationUser>();
        }
    }
}
