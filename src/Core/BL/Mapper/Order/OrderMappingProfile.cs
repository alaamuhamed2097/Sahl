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
        CreateMap<TbOrder, OrderDto>().ReverseMap();
        CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
        CreateMap<TbCouponCode, CouponCodeDto>().ReverseMap();
    }
}
