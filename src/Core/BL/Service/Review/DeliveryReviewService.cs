using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Service.Review
{
    public interface IDeliveryReviewService : IBaseService<TbDeliveryReview, DeliveryReviewDto>
    {
    }

    public class DeliveryReviewService : BaseService<TbDeliveryReview, DeliveryReviewDto>, IDeliveryReviewService
    {
        public DeliveryReviewService(ITableRepository<TbDeliveryReview> repository, IBaseMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
