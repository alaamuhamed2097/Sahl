using Domains.Entities.CouponCode;
using Domains.Entities.Order;
using Domains.Entities.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.DTOs.Order.Order;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureOrderMappings()
        {
            CreateMap<TbOrder, OrderDto>().ReverseMap();
            CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
            CreateMap<TbCouponCode, CouponCodeDto>().ReverseMap();
        }
    }
}
