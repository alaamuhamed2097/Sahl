using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Review
{
	public interface IReviewVoteRepository : ITableRepository<TbReviewVote>
	{
		Task<bool> HasCustomerVotedAsync(Guid reviewId, Guid customerId, CancellationToken cancellationToken = default);
		Task<TbReviewVote?> GetVoteAsync(Guid reviewId, Guid customerId, CancellationToken cancellationToken = default);
		//Task<TbReviewVote> CreateVoteAsync(TbReviewVote vote, CancellationToken cancellationToken = default);
		//Task<bool> DeleteVoteAsync(Guid reviewId, Guid customerId, CancellationToken cancellationToken = default);
		Task UpdateReviewVoteCountsAsync(Guid reviewId, CancellationToken cancellationToken = default);
		Task<(int helpful, int notHelpful)> GetVoteCountsAsync(Guid reviewId, CancellationToken cancellationToken = default);
	}
}
