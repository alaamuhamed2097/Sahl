using BL.Contracts.GeneralService;
using Common.Enumerations.Review;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Review;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories.Review
{
    public class ItemReviewRepository : TableRepository<TbItemReview>, IItemReviewRepository
    {
        public ItemReviewRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger) { }

        public override async Task<PagedResult<TbItemReview>> GetPageAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<TbItemReview, bool>> filter = null, 
            Func<IQueryable<TbItemReview>, IOrderedQueryable<TbItemReview>> orderBy = null, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidatePaginationParameters(pageNumber, pageSize);

                IQueryable<TbItemReview> query = _dbContext.Set<TbItemReview>()
                    .Include(r => r.ReviewVotes.Where(v => !v.IsDeleted))
                    .Include(r => r.ReviewReports.Where(rr => !rr.IsDeleted))
                    .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                    .AsNoTracking();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                int totalCount = await query.CountAsync(cancellationToken);
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                var data = await query.ToListAsync(cancellationToken);

                return new PagedResult<TbItemReview>(data, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(TbItemReview).Name}.");
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(TbItemReview).Name}.",
                    ex,
                    _logger
                );
            }
        }

        /// <summary>
        /// Retrieves all approved (visible) reviews for a given Item.
        /// Filters by ItemId, ensures the review is approved and not soft-deleted.
        /// </summary>
        /// <param name="ItemId">ID of the Item to retrieve reviews for.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of approved reviews sorted by newest first.</returns>
        public async Task<IEnumerable<TbItemReview>> GetReviewsByItemIdAsync(
            Guid ItemId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbItemReview>()
                    .AsNoTracking()
                    .Where(r => r.ItemId == ItemId
                        && r.Status == ReviewStatus.Approved
                        && !r.IsDeleted)
                    .OrderByDescending(r => r.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetReviewsByItemIdAsync),
                    $"Error occurred while retrieving reviews for Item {ItemId}.", ex);
                return new List<TbItemReview>();
            }
        }

        /// <summary>
        /// Retrieves full details of a specific review including related votes and reports.
        /// Only includes records that are not soft-deleted.
        /// </summary>
        /// <param name="reviewId">ID of the review.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The review with related data, or null if not found.</returns>
        public async Task<TbItemReview?> GetReviewDetailsAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbItemReview>()
                    .Include(r => r.ReviewVotes.Where(v => !v.IsDeleted))
                    .Include(r => r.ReviewReports.Where(rr => !rr.IsDeleted))
                    .Include(r => r.Customer)
                    .ThenInclude(c => c.User) 
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        r => r.Id == reviewId && !r.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetReviewDetailsAsync),
                    $"Error occurred while retrieving review details for ID {reviewId}.", ex);
                return null;
            }
        }
        
        /// <summary>
        /// Calculates the average rating for a specific Item
        /// considering only approved, non-deleted reviews.
        /// </summary>
        /// <param name="ItemId">Item ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The average rating, or 0 if no reviews exist.</returns>
        public async Task<decimal> GetAverageRatingAsync(
            Guid ItemId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var reviews = await _dbContext.Set<TbItemReview>()
                    .AsNoTracking()
                    .Where(r => r.ItemId == ItemId
                        && r.Status == ReviewStatus.Approved
                        && !r.IsDeleted)
                    .Select(r => r.Rating)
                    .ToListAsync(cancellationToken);

                return reviews.Any() ? reviews.Average() : 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAverageRatingAsync),
                    $"Error occurred while calculating average rating for Item {ItemId}.", ex);
                return 0;
            }
        }

        /// <summary>
        /// Counts the number of approved, non-deleted reviews for a given Item.
        /// Used for statistics and rating calculations.
        /// </summary>
        /// <param name="ItemId">Item ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The count of approved reviews.</returns>
        public async Task<int> GetReviewCountByItemIdAsync(
            Guid ItemId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbItemReview>()
                    .AsNoTracking()
                    .CountAsync(r => r.ItemId == ItemId
                        && r.Status == ReviewStatus.Approved
                        && !r.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetReviewCountByItemIdAsync),
                    $"Error occurred while counting reviews for Item {ItemId}.", ex);
                return 0;
            }
        }

        /// <summary>
        /// Returns the distribution of ratings (1-5 stars) for a specific Item.
        /// Groups ratings and returns a dictionary: Key = rating, Value = count.
        /// </summary>
        /// <param name="ItemId">Item ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Dictionary of rating value and count.</returns>
        public async Task<Dictionary<int, int>> GetRatingDistributionAsync(
           Guid ItemId,
           CancellationToken cancellationToken = default)
        {
            var reviews = await _dbContext.Set<TbItemReview>()
                .AsNoTracking()
                .Where(r => r.ItemId == ItemId
                    && r.Status == ReviewStatus.Approved
                    && !r.IsDeleted)
                .Select(r => r.Rating)
                .ToListAsync(cancellationToken);

            return reviews
                .GroupBy(r => (int)Math.Round(r))
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<bool> UpdateReviewStatus(
            Guid reviewId, 
            ReviewStatus newStatus, 
            string adminId, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _dbContext.Set<TbItemReview>().FindAsync(new object[] { reviewId }, cancellationToken);

                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(TbReviewReport).Name} with ID {reviewId} not found.", _logger);

                entity.Status = newStatus;
                entity.UpdatedBy = new Guid(adminId);
                entity.UpdatedDateUtc = DateTime.UtcNow;

                _dbContext.Set<TbItemReview>().Update(entity);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(UpdateIsDeletedAsync),
                    $"Database update error while updating IsDeleted for entity type {typeof(TbItemReview).Name}, ID {reviewId}.", dbEx);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateIsDeletedAsync),
                    $"Error occurred while updating IsDeleted for entity type {typeof(TbItemReview).Name}, ID {reviewId}.", ex);
                return false;
            }
        }
    }
}
