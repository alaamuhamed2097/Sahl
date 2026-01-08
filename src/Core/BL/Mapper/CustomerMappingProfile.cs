using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.VideoProvider;
using Domains.Procedures;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Customer;
using Shared.DTOs.Media;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureCustomerMappings()
        {
            // Customer mappings
            CreateMap<TbCustomer, CustomerDto>()
                .ReverseMap();

            // Customer Item View mappings
            CreateMap<TbCustomerItemView, CustomerItemViewDto>()
                .ReverseMap();

            // Customer Recommended Items mappings
            CreateMap<SpGetCustomerRecommendedItems, SearchItemDto>()
                .ReverseMap();
        }
    }
}
