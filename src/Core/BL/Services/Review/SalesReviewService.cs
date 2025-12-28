using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Services.Review;

public interface ISalesReviewService : IBaseService<TbSalesReview, SalesReviewDto>
{
}

public class SalesReviewService : BaseService<TbSalesReview, SalesReviewDto>, ISalesReviewService
{
    public SalesReviewService(ITableRepository<TbSalesReview> repository, IBaseMapper mapper)
        : base(repository, mapper)
    {
    }
}
