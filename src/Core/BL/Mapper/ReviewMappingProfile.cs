using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Mapper;

public partial class MappingProfile
{
    //      private void ConfigureReviewMappings()
    //      {
    //          CreateMap<TbOfferReview, OfferReviewDto>().ReverseMap();
    //          CreateMap<TbReviewReport, ReviewReportDto>().ReverseMap();
    //}
    private void ConfigureReviewMappings()
    {
        CreateMap<TbOfferReview, OfferReviewDto>().ReverseMap();
        CreateMap<TbSalesReview, SalesReviewDto>().ReverseMap();
        CreateMap<TbDeliveryReview, DeliveryReviewDto>().ReverseMap();
        CreateMap<TbReviewVote, ReviewVoteDto>().ReverseMap();
        CreateMap<TbReviewReport, ReviewReportDto>().ReverseMap();
    }
}
