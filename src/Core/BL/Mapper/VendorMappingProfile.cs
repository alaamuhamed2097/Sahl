using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Views.Vendor;
using Shared.DTOs.Vendor;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureVendorMappings()
    {
        CreateMap<TbVendor, VendorDto>()
            .ForMember(
                desc => desc.AdministratorFirstName,
                opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(
                desc => desc.AdministratorLastName,
                opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(
                desc => desc.Email,
                opt => opt.MapFrom(src => src.User.Email))
            .ForMember(
                desc => desc.UserStatus,
                opt => opt.MapFrom(src => src.User.UserState))
            .ReverseMap();

        CreateMap<TbVendor, VendorRegistrationRequestDto>().ReverseMap();
        CreateMap<VendorUpdateRequestDto, TbVendor>();
        CreateMap<VwVendorPublicDetailsDto, VwVendorPublicDetailsDto>();
        CreateMap<VwVendorOwnerDetails, VwVendorOwnerDetailsDto>();
        CreateMap<VwVendorAdminDetails, VwVendorAdminDetailsDto>();
    }
}

