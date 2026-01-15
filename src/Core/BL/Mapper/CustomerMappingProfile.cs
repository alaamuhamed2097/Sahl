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
            // Customer mappings with User property mapping
            CreateMap<TbCustomer, CustomerDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : string.Empty))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User != null ? src.User.LastName : string.Empty))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.User != null ? src.User.UserState : 0))
                .ForMember(dest => dest.LastLoginDate, opt => opt.MapFrom(src => src.User != null ? src.User.LastLoginDate : null))
                .ForMember(dest => dest.WalletBalance, opt => opt.MapFrom(src => src.User != null && src.User.CustomerWallets != null ? src.User.CustomerWallets.Sum(w => w.Balance) : 0))
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.User != null && src.User.Orders != null ? src.User.Orders.Count : 0))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Don't overwrite User on reverse map

            // Customer Item View mappings
            CreateMap<TbCustomerItemView, CustomerItemViewDto>()
                .ReverseMap();

            // Customer Recommended Items mappings
            CreateMap<SpGetCustomerRecommendedItems, SearchItemDto>()
                .ReverseMap();
        }
    }
}
