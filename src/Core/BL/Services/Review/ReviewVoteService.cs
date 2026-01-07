using BL.Contracts.IMapper;
using BL.Contracts.Service.Base;
using BL.Contracts.Service.Review;
using BL.Services.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Review;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Review;
using Shared.GeneralModels.Models;
using System.ComponentModel.DataAnnotations;

namespace BL.Services.Review;


public class ReviewVoteService : BaseService<TbReviewVote, ReviewVoteDto>, IReviewVoteService
{
	private readonly IReviewVoteRepository _voteRepo;
	private readonly IItemReviewRepository _reviewRepo;
	private readonly IBaseMapper _mapper;
	private readonly ILogger _logger;
	public ReviewVoteService(ITableRepository<TbReviewVote> repository, IBaseMapper mapper, ILogger logger, IReviewVoteRepository voteRepo, IItemReviewRepository reviewRepo)
		: base(repository, mapper)
	{
		_logger = logger;
		_voteRepo = voteRepo;
		_reviewRepo = reviewRepo;
	}

	public async Task<bool> AddHelpfulVoteAsync(
		ReviewVoteDto reviewVoteDto,
		Guid userId,
		CancellationToken cancellationToken = default)
	{
		try
		{
			return await AddVoteAsync(reviewVoteDto, userId, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(AddHelpfulVoteAsync)}");
			throw;
		}
	}
	public async Task<bool> AddNotHelpfulVoteAsync(
	   ReviewVoteDto reviewVoteDto,
	   Guid userId,
	   CancellationToken cancellationToken = default)
	{
		try
		{
			return await AddVoteAsync(reviewVoteDto, userId, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(AddNotHelpfulVoteAsync)}");
			throw;
		}
	}


	private async Task<bool> AddVoteAsync(
		ReviewVoteDto reviewVoteDto,
		Guid userId,
		CancellationToken cancellationToken = default)
	{
		try
		{
			if (reviewVoteDto == null)
				throw new ArgumentNullException(nameof(reviewVoteDto));

			if (reviewVoteDto.ReviewID == Guid.Empty)
				throw new ValidationException("ReviewID is required");

			if (reviewVoteDto.CustomerID == Guid.Empty)
				throw new ValidationException("CustomerID is required");
			//var customerGuid = Guid.Parse(customerId);

			// Check if review exists
			var review = await _reviewRepo.FindByIdAsync(reviewVoteDto.ReviewID, cancellationToken);
			if (review == null)
				throw new KeyNotFoundException($"Review with ID {reviewVoteDto.ReviewID} not found");
			if (review.Status != ReviewStatus.Approved)
				throw new InvalidOperationException("You can only vote on approved reviews");

			if (reviewVoteDto.CustomerID == Guid.Empty)
				throw new ValidationException("CustomerID is required");

			// Prevent voting on own review
			//if (review.CustomerID == reviewVoteDto.CustomerID)
			//	throw new InvalidOperationException("You cannot vote on your own review");

			// Check if already voted
			var existingVote = await _voteRepo.GetVoteAsync(reviewVoteDto.ReviewID, reviewVoteDto.CustomerID, cancellationToken);

			if (existingVote != null)
			{
				// Update existing vote if different
				if (existingVote.VoteType != reviewVoteDto.VoteType)
				{
					existingVote.VoteType = reviewVoteDto.VoteType;
					var updateResult = await _voteRepo.UpdateAsync(existingVote, userId, cancellationToken);

					if (!updateResult.Success)
						throw new Exception("Failed to update vote");
				}
				else
				{
					throw new InvalidOperationException("You have already voted this way on this review");
				}
			}
			else
			{
				
				var vote = new TbReviewVote
				{
					ItemReviewId = reviewVoteDto.ReviewID,
					CustomerId = reviewVoteDto.CustomerID,
					VoteType = reviewVoteDto.VoteType
				};
				await _voteRepo.CreateAsync(vote, userId, cancellationToken);
			}

			await _voteRepo.UpdateReviewVoteCountsAsync(reviewVoteDto.ReviewID, cancellationToken);

			return true;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(AddVoteAsync)}");
			throw;
		}
	}
	public async Task<ResponseReviewVoteCountsDto> GetVoteCountsByReviewIdAsync(
	Guid reviewId,
	CancellationToken cancellationToken)
	{
		var votCount = await _voteRepo.GetVoteCountsAsync(reviewId, cancellationToken);

		return new ResponseReviewVoteCountsDto
		{
			ReviewID = reviewId,
			HelpfulVotesCount = votCount.helpful,
			NotHelpfulVotesCount = votCount.notHelpful

		};
	}
	public async Task<bool> RemoveVoteAsync(
		   Guid reviewId,
		   Guid userId,
		   CancellationToken cancellationToken = default)
	{
		try
		{
			var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);
			if (review.Status != ReviewStatus.Approved)
				throw new InvalidOperationException("Cannot remove vote from unapproved review");

			//var customerGuid = Guid.Parse(customerId);
			var result = await _voteRepo.SoftDeleteAsync(reviewId, userId, cancellationToken);

			if (result)
			{
				await _voteRepo.UpdateReviewVoteCountsAsync(reviewId, cancellationToken);
			}

			return result;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(RemoveVoteAsync)}");
			throw;
		}
	}
}
