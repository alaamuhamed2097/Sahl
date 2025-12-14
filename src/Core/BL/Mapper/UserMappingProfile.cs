using Domains.Entities.Location;
using Shared.DTOs.Location;
using Shared.DTOs.User.Admin;
using Shared.DTOs.User.Customer;

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

            // Customer registration mapping
            CreateMap<CustomerRegistrationDto, ApplicationUser>()
                .ForMember(dest => dest.NormalizedPhone, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDateUtc, opt => opt.Ignore())
                .ForMember(dest => dest.UserState, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());

            CreateMap<ApplicationUser, CustomerRegistrationResponseDto>();

            // Add additional user-related mappings here
            CreateMap<TbCountry, CountryDto>().ReverseMap();
            CreateMap<TbState, StateDto>().ReverseMap();
            CreateMap<TbCity, CityDto>().ReverseMap();
        }
    }
}
