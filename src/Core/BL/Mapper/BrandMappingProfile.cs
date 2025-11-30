using Domains.Entities.Catalog.Brand;
using Shared.DTOs.Brand;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureBrandMapping()
        {
            CreateMap<TbBrand, BrandDto>().ReverseMap();
        }
    }
}