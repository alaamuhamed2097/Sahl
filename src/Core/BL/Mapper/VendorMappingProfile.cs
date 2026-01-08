using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Views.Vendor;
using Shared.DTOs.Vendor;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureVendorMappings()
    {
        CreateMap<TbVendor, VendorDto>().ReverseMap();
        CreateMap<TbVendor, RegisterVendorRequestDto>().ReverseMap();
        CreateMap<VwVendorPublicDetailsDto, VwVendorPublicDetailsDto>();
        CreateMap<VwVendorOwnerDetails, VwVendorOwnerDetailsDto>();
        CreateMap<VwVendorAdminDetails, VwVendorAdminDetailsDto>();
    }
}

