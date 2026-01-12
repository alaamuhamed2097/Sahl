using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order;
using Domains.Entities.Order.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.OrderProcessing;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureOrderMappings()
    {
        CreateMap<TbOrder, OrderDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.User.ProfileImagePath))
            .ReverseMap();
        CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
        CreateMap<TbCouponCode, CouponCodeDto>().ReverseMap();
    }
}
