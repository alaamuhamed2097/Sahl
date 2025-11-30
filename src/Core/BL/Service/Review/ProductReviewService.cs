using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Service.Review
{
    public interface IProductReviewService : IBaseService<TbProductReview, ProductReviewDto>
    {
    }

    public class ProductReviewService : BaseService<TbProductReview, ProductReviewDto>, IProductReviewService
    {
        public ProductReviewService(ITableRepository<TbProductReview> repository, IBaseMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
