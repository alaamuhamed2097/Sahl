using Domains.Entities.Testimonial;
using Domains.Identity;
using Domins.Entities.Location;
using Shared.DTOs.Location;
using Shared.DTOs.Testimonial;
using Shared.DTOs.User.Admin;

namespace BL.Mapper
{
    // User mappings partial (MappingProfile.Users.cs)
    public partial class MappingProfile
    {
        private void ConfigureUserMappings()
        {
            // User account mappings
            CreateMap<ApplicationUser, AdminRegistrationDto>().ReverseMap();
            CreateMap<ApplicationUser, AdminProfileDto>().ReverseMap();

            CreateMap<AdminCreateDto, ApplicationUser>();
            CreateMap<ApplicationUser, AdminDto>();
            CreateMap<AdminUpdateDto, ApplicationUser>();

            // Add additional user-related mappings here
            CreateMap<TbCountry, CountryDto>().ReverseMap();
            CreateMap<TbState, StateDto>().ReverseMap();
            CreateMap<TbCity, CityDto>().ReverseMap();

            // Add Testimonial mappings
            CreateMap<TbTestimonial, TestimonialDto>().ReverseMap();
        }
    }
}
