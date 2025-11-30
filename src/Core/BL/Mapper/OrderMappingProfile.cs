using Domains.Entities.CouponCode;
using Domains.Entities.Shipping;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.CouponCode;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureOrderMappings()
        {
            CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
            CreateMap<TbCouponCode, CouponCodeDto>().ReverseMap();
        }
    }
}
