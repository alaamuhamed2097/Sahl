using Domains.Entities.Catalog.Brand;
using Domains.Views.Brand;
using Shared.DTOs.Brand;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureBrandMapping()
        {
            CreateMap<TbBrand, BrandDto>().ReverseMap();
			// 1. Brand Overview
			CreateMap<VwBrandOverview, VwBrandOverviewDto>();

			// 2. Brand Products
			CreateMap<VwBrandProducts, VwBrandProductsDto>();

			// 3. Brand Registration Requests
			CreateMap<VwBrandRegistrationRequests, VwBrandRegistrationRequestsDto>();

			// 4. Brand Authorized Distributors
			CreateMap<VwBrandAuthorizedDistributors, VwBrandAuthorizedDistributorsDto>();

			// 5. Brand Sales Analysis
			CreateMap<VwBrandSalesAnalysis, VwBrandSalesAnalysisDto>();

			// 6. Brand Campaigns
			CreateMap<VwBrandCampaigns, VwBrandCampaignsDto>();
		}
    }
}