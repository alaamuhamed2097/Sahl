using Domains.Entities;
using Shared.DTOs.ECommerce;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureOrderMappings()
        {
            CreateMap<TbShippingCompany, ShippingCompanyDto>().ReverseMap();
        }
    }
}
