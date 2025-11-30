using Domains.Entities.ECommerceSystem.Support;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Support;
using Shared.DTOs.Review;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureSupportMappings()
        {
            CreateMap<TbDispute, DisputeDto>().ReverseMap();
            CreateMap<TbDisputeMessage, DisputeMessageDto>().ReverseMap();
            CreateMap<TbSupportTicket, SupportTicketDto>().ReverseMap();
            CreateMap<TbSupportTicketMessage, SupportTicketMessageDto>().ReverseMap();
        }

        private void ConfigureReviewMappings()
        {
            CreateMap<TbProductReview, ProductReviewDto>().ReverseMap();
            CreateMap<TbSalesReview, SalesReviewDto>().ReverseMap();
            CreateMap<TbDeliveryReview, DeliveryReviewDto>().ReverseMap();
            CreateMap<TbReviewVote, ReviewVoteDto>().ReverseMap();
            CreateMap<TbReviewReport, ReviewReportDto>().ReverseMap();
        }
    }
}
