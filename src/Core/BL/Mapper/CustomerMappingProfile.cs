using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.VideoProvider;
using Shared.DTOs.Customer;
using Shared.DTOs.Media;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureCustomerMappings()
    {
        
        CreateMap<TbCustomer, CustomerDto>()
            .ReverseMap();

    }
}
