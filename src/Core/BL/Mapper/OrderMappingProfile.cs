using Domains.Entities;
using Domains.Entities.PromoCode;
using Shared.DTOs.ECommerce;
using Shared.DTOs.ECommerce.PromoCode;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureOrderMappings()
        {
            CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
            CreateMap<TbPromoCode, PromoCodeDto>().ReverseMap();
        }
    }
}
