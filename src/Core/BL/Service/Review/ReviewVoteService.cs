using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;

namespace BL.Service.Review
{
    public interface IReviewVoteService : IBaseService<TbReviewVote, ReviewVoteDto>
    {
    }

    public class ReviewVoteService : BaseService<TbReviewVote, ReviewVoteDto>, IReviewVoteService
    {
        public ReviewVoteService(ITableRepository<TbReviewVote> repository, IBaseMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
