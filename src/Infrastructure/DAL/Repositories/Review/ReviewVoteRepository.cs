using BL.Contracts.GeneralService;
using Common.Enumerations.Review;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Review;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Review
{
    public class ReviewVoteRepository : TableRepository<TbReviewVote>, IReviewVoteRepository
    {

        public ReviewVoteRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger) { }

        public async Task<bool> HasCustomerVotedAsync(
            Guid reviewId,
            Guid customerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewVote>()
                    .AsNoTracking()
                    .AnyAsync(v => v.ItemReviewId == reviewId
                        && v.CustomerId == customerId
                        && !v.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(HasCustomerVotedAsync),
                    $"Error occurred while checking if customer voted.", ex);
                return false;
            }
        }

        public async Task<TbReviewVote?> GetVoteAsync(
            Guid reviewId,
            Guid customerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewVote>()
                    .FirstOrDefaultAsync(v => v.ItemReviewId == reviewId
                        && v.CustomerId == customerId
                        && !v.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetVoteAsync),
                    $"Error occurred while retrieving vote.", ex);
                return null;
            }
        }

        //public async Task<TbReviewVote> CreateVoteAsync(
        //	TbReviewVote vote,
        //	CancellationToken cancellationToken = default)
        //{
        //	try
        //	{
        //		vote.Id = Guid.NewGuid();
        //		vote.CreatedDateUtc = DateTime.UtcNow;
        //		vote.CurrentState = (int)Common.Enumerations.EntityState.Active;

        //		await _dbContext.Set<TbReviewVote>().AddAsync(vote, cancellationToken);
        //		//await _dbContext.SaveChangesAsync(cancellationToken);

        //		return vote;
        //	}
        //	catch (Exception ex)
        //	{
        //		HandleException(nameof(CreateVoteAsync),
        //			$"Error occurred while creating vote.", ex);
        //		throw;
        //	}
        //}

        //public async Task<bool> DeleteVoteAsync(
        //	Guid reviewId,
        //	Guid customerId,
        //	CancellationToken cancellationToken = default)
        //{
        //	try
        //	{
        //		var vote = await GetVoteAsync(reviewId, customerId, cancellationToken);

        //		if (vote == null)
        //			return false;

        //		vote.CurrentState = (int)Common.Enumerations.EntityState.Deleted;
        //		vote.UpdatedDateUtc = DateTime.UtcNow;

        //		_dbContext.Set<TbReviewVote>().Update(vote);
        //		return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        //	}
        //	catch (Exception ex)
        //	{
        //		HandleException(nameof(DeleteVoteAsync),
        //			$"Error occurred while deleting vote.", ex);
        //		return false;
        //	}
        //}

        public async Task UpdateReviewVoteCountsAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var review = await _dbContext.Set<TbItemReview>()
                    .FirstOrDefaultAsync(r => r.Id == reviewId, cancellationToken);

                if (review == null) return;

                var counts = await GetVoteCountsAsync(reviewId, cancellationToken);

                review.HelpfulCount = counts.helpful;
                review.NotHelpfulCount = counts.notHelpful;
                review.UpdatedDateUtc = DateTime.UtcNow;

                _dbContext.Set<TbItemReview>().Update(review);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateReviewVoteCountsAsync),
                    $"Error occurred while updating vote counts for review {reviewId}.", ex);
            }
        }

        public async Task<(int helpful, int notHelpful)> GetVoteCountsAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var votes = await _dbContext.Set<TbReviewVote>()
                    .AsNoTracking()
                    .Where(v => v.ItemReviewId == reviewId
                        && !v.IsDeleted)
                    .ToListAsync(cancellationToken);

                var helpfulCount = votes.Count(v => v.VoteType == VoteType.Helpful);
                var notHelpfulCount = votes.Count(v => v.VoteType == VoteType.NotHelpful);

                return (helpfulCount, notHelpfulCount);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetVoteCountsAsync),
                    $"Error occurred while getting vote counts.", ex);
                return (0, 0);
            }
        }
    }
}
