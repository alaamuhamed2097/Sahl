using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
       
        private void ConfigureReviewMappings()
        {
            CreateMap<TbItemReview, ItemReviewDto>().ReverseMap();
            CreateMap<TbSalesReview, SalesReviewDto>().ReverseMap();
            CreateMap<TbReviewVote, ReviewVoteDto>().ReverseMap();
            CreateMap<TbReviewReport, ReviewReportDto>().ReverseMap();
            CreateMap<TbVendorReview, VendorReviewDto>().ReverseMap();
		}
    }
}
