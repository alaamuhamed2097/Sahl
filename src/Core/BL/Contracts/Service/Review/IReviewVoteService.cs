using Shared.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Review;

	public interface IReviewVoteService
	{
		Task<bool> AddHelpfulVoteAsync(
			ReviewVoteDto reviewVoteDto,
			Guid userId,
			CancellationToken cancellationToken = default);
	//Task<bool> AddNotHelpfulVoteAsync(Guid reviewId, string customerId, CancellationToken cancellationToken = default);
	Task<ResponseReviewVoteCountsDto> GetVoteCountsByReviewIdAsync(Guid reviewId, CancellationToken cancellationToken);

	Task<bool> RemoveVoteAsync(
		   Guid reviewId,
		   Guid userId,
		   CancellationToken cancellationToken = default);
	}
