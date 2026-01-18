using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureVendorMappings()
    {
        CreateMap<TbVendor, VendorDto>()
            .IncludeMembers(s => s.User)
            .ForMember(
                desc => desc.AdministratorFirstName,
                opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(
                desc => desc.AdministratorLastName,
                opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(
                desc => desc.CountryName,
                opt => opt.MapFrom(src => src.City.State.Country.TitleEn)
            )
            .ForMember(
                desc => desc.StateName,
                opt => opt.MapFrom(src => src.City.State.TitleEn)
            )
            .ForMember(
                desc => desc.CityName,
                opt => opt.MapFrom(src => src.City.TitleEn)
            )
            .ReverseMap();

        CreateMap<TbVendor, VendorPreviewDto>()
            .IncludeMembers(s => s.User)
            .ForMember(
                desc => desc.AdministratorFirstName,
                opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(
                desc => desc.AdministratorLastName,
                opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(
                desc => desc.CountryName,
                opt => opt.MapFrom(src => src.City.State.Country.TitleEn)
            )
            .ForMember(
                desc => desc.StateName,
                opt => opt.MapFrom(src => src.City.State.TitleEn)
            )
            .ForMember(
                desc => desc.CityName,
                opt => opt.MapFrom(src => src.City.TitleEn)
            )
            .ReverseMap();

        CreateMap<ApplicationUser, VendorDto>().ReverseMap();
        CreateMap<ApplicationUser, VendorPreviewDto>().ReverseMap();
        CreateMap<TbVendor, VendorRegistrationRequestDto>().ReverseMap();
        CreateMap<VendorUpdateRequestDto, TbVendor>();
    }
}

