using BL.Contracts.IMapper;
using BL.Contracts.Service.Review;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using DAL.Contracts.Repositories.Review;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Review;
using Resources;
using Serilog;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace BL.Services.Review
{
    public class ItemReviewService : BaseService<TbItemReview, ItemReviewDto>, IItemReviewService
    {
        private readonly ITableRepository<TbItem> _itemRepository;
        private readonly IItemReviewRepository _reviewRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public ItemReviewService(
            ITableRepository<TbItem> itemRepository,
            ITableRepository<TbItemReview> tableRepository,
            IItemReviewRepository reviewRepo,
            ICustomerRepository customerRepository,
            IBaseMapper mapper,
            ILogger logger)
            : base(tableRepository, mapper)
        {
            _reviewRepository = reviewRepo;
            _itemRepository = itemRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<ItemReviewResponseDto>> GetPageAsync(
            ItemReviewSearchCriteriaModel criteriaModel,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (criteriaModel == null)
                    throw new ArgumentNullException(nameof(criteriaModel));

                if (criteriaModel.PageNumber < 1)
                    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

                if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

                // Build filter expression
                Expression<Func<TbItemReview, bool>> filter = x => !x.IsDeleted;

                if (criteriaModel.ItemId.HasValue && criteriaModel.ItemId.Value != Guid.Empty)
                {
                    filter = filter.And(x => x.ItemId == criteriaModel.ItemId.Value);
                }

                if (criteriaModel.CustomerId.HasValue)
                {
                    filter = filter.And(x => x.CustomerId == criteriaModel.CustomerId.Value);
                }

                if (criteriaModel.RatingFrom.HasValue)
                {
                    filter = filter.And(x => x.Rating >= criteriaModel.RatingFrom.Value);
                }

                if (criteriaModel.RatingTo.HasValue)
                {
                    filter = filter.And(x => x.Rating <= criteriaModel.RatingTo.Value);
                }

                if (criteriaModel.Statuses.HasValue)
                {
                    filter = filter.And(x => x.Status == criteriaModel.Statuses.Value);
                }

                // Search term filter
                var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    filter = filter.And(x =>
                        (x.ReviewTitle != null && x.ReviewTitle.ToLower().Contains(searchTerm)) ||
                        (x.ReviewText != null && x.ReviewText.ToLower().Contains(searchTerm)) ||
                        (x.ReviewNumber != null && x.ReviewNumber.ToLower().Contains(searchTerm))
                    );
                }

                // Build ordering expression
                Func<IQueryable<TbItemReview>, IOrderedQueryable<TbItemReview>> orderByExpression = null;
                if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
                {
                    var isDesc = criteriaModel.SortDirection?.ToLowerInvariant() == "desc";

                    switch (criteriaModel.SortBy.ToLowerInvariant())
                    {
                        case "reviewtitle":
                            orderByExpression = isDesc
                                ? q => q.OrderByDescending(x => x.ReviewTitle)
                                : q => q.OrderBy(x => x.ReviewTitle);
                            break;
                        case "rating":
                            orderByExpression = isDesc
                                ? q => q.OrderByDescending(x => x.Rating)
                                : q => q.OrderBy(x => x.Rating);
                            break;
                        case "reviewnumber":
                            orderByExpression = isDesc
                                ? q => q.OrderByDescending(x => x.ReviewNumber)
                                : q => q.OrderBy(x => x.ReviewNumber);
                            break;
                        case "helpfulvotecount":
                            orderByExpression = isDesc
                                ? q => q.OrderByDescending(x => x.HelpfulVoteCount)
                                : q => q.OrderBy(x => x.HelpfulVoteCount);
                            break;
                        case "createddateutc":
                        default:
                            orderByExpression = isDesc
                                ? q => q.OrderByDescending(x => x.CreatedDateUtc)
                                : q => q.OrderBy(x => x.CreatedDateUtc);
                            break;
                    }
                }
                else
                {
                    orderByExpression = q => q.OrderByDescending(x => x.CreatedDateUtc);
                }

                var page = await _reviewRepository.GetPageAsync(
                    criteriaModel.PageNumber,
                    criteriaModel.PageSize,
                    filter,
                    orderBy: orderByExpression,
                    cancellationToken: cancellationToken
                );

                var dtos = _mapper.MapList<TbItemReview,ItemReviewResponseDto>(page.Items);
                return new PagedResult<ItemReviewResponseDto>(dtos, page.TotalRecords);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetPageAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an item review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier. Must not be <see cref="Guid.Empty"/>.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A <see cref="ItemReviewResponseDto"/> containing the review details.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="reviewId"/> is empty.</exception>
        /// <exception cref="Exception">Thrown when the review is not found or an error occurs.</exception>
        public async Task<ItemReviewResponseDto> FindReviewByIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (reviewId == Guid.Empty)
                    throw new ArgumentException("ReviewId cannot be empty.", nameof(reviewId));

                var review = await _reviewRepository.GetReviewDetailsAsync(reviewId, cancellationToken) 
                    ?? throw new KeyNotFoundException($"Review with ID {reviewId} was not found.");

                return _mapper.MapModel<TbItemReview, ItemReviewResponseDto>(review);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving review with ID {reviewId}.", ex);
            }
        }

        /// <summary>
        /// Calculates and returns the average rating score for a given Item,
        /// based only on approved reviews.
        /// </summary>
        /// <param name="ItemId">Item identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Average rating as decimal (e.g. 4.5).</returns>
        public async Task<decimal> CalculateItemAverageRatingAsync(
            Guid ItemId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _reviewRepository.GetAverageRatingAsync(ItemId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(CalculateItemAverageRatingAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves aggregated review statistics for a given Item, such as average rating and total review count.
        /// </summary>
        /// <param name="ItemId">Item identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>ItemReviewStatsDto containing metrics like average rating, total reviews.</returns>
        public async Task<ResponseItemReviewSummeryDto> GetItemReviewSummeryAsync(
           Guid ItemId,
           CancellationToken cancellationToken = default)
        {
            var averageRating = await _reviewRepository.GetAverageRatingAsync(ItemId, cancellationToken);
            var reviewCount = await _reviewRepository.GetReviewCountByItemIdAsync(ItemId, cancellationToken);
            var ratingDistribution = await _reviewRepository.GetRatingDistributionAsync(ItemId, cancellationToken);

            var sumnnery = new ResponseItemReviewSummeryDto
            {
                AverageRating = averageRating,
                ReviewCount = reviewCount,
                FiveStarCount = ratingDistribution.GetValueOrDefault(5, 0),
                FourStarCount = ratingDistribution.GetValueOrDefault(4, 0),
                ThreeStarCount = ratingDistribution.GetValueOrDefault(3, 0),
                TwoStarCount   = ratingDistribution.GetValueOrDefault(2, 0),
                OneStarCount   = ratingDistribution.GetValueOrDefault(1, 0)
            };

            // Calculate percentages
            if (reviewCount > 0)
            {
                sumnnery.FiveStarPercentage = Math.Round((decimal)sumnnery.FiveStarCount / reviewCount * 100, 2);
                sumnnery.FourStarPercentage = Math.Round((decimal)sumnnery.FourStarCount / reviewCount * 100, 2);
                sumnnery.ThreeStarPercentage = Math.Round((decimal)sumnnery.ThreeStarCount / reviewCount * 100, 2);
                sumnnery.TwoStarPercentage = Math.Round((decimal)sumnnery.TwoStarCount / reviewCount * 100, 2);
                sumnnery.OneStarPercentage = Math.Round((decimal)sumnnery.OneStarCount / reviewCount * 100, 2);
            }

            return sumnnery;
        }

        /// <summary>
        /// Creates a new review for a given Item by a customer.
        /// Validates that the customer hasn't already reviewed the same Item (optional),
        /// sets the default review status (e.g. Pending), and persists it in database.
        /// </summary>
        /// <param name="reviewDto">DTO containing rating, comments and associated ItemId.</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>The created review DTO with assigned Id and metadata (creation date, status).</returns>
        public async Task<ItemReviewResponseDto> CreateReviewAsync(
            ItemReviewDto reviewDto,
            Guid creatorId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validation
                (bool sucess, string errorMassage) isValid = IsReviewValid(reviewDto.Rating, reviewDto.ReviewTitle, reviewDto.ReviewText);
                if (!isValid.sucess)
                    throw new Exception(isValid.errorMassage);

                var customer = await _customerRepository.FindAsync(c => c.UserId == creatorId.ToString());

                // Mapping review
                var review = _mapper.MapModel<ItemReviewDto, TbItemReview>(reviewDto);
                review.ReviewNumber = generateNumber();
                review.CustomerId = customer.Id;
                review.Status = ReviewStatus.Approved;
                review.IsEdited = false;

                // Create review
                var result = await _reviewRepository.CreateAsync(review, creatorId, cancellationToken);
                if (!result.Success)
                    throw new Exception("Failed to submit review");

                // Update item average rating if review is auto-approved
                if (review.Status == ReviewStatus.Approved)
                    await UpdateItemAverageRatingAsync(review.ItemId, cancellationToken);

                return _mapper.MapModel<TbItemReview, ItemReviewResponseDto>(review);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(CreateReviewAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing review. Verifies that the review belongs to the current user.
        /// Updates provided fields (e.g. rating, comment), possibly resets status to Pending for re-approval.
        /// </summary>
        /// <param name="reviewDto">DTO containing updated review data (must include Id).</param>
        /// <param name="currentUserId">Guid of the user attempting update (used for ownership verification).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated review DTO.</returns>
        public async Task<ItemReviewResponseDto> UpdateReviewAsync(
            ItemReviewDto reviewDto,
            Guid currentUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var review = await _reviewRepository.GetReviewDetailsAsync(reviewDto.Id, cancellationToken)
                    ?? throw new NotFoundException($"Review with ID {reviewDto.Id} not found.", _logger);

                // Check ownership
                if (review.Customer.UserId != currentUserId.ToString())
                    throw new UnauthorizedAccessException("You can only edit your own reviews");

                // Validation
                (bool sucess, string errorMassage) isValid = IsReviewValid(review.Rating, review.ReviewTitle, review.ReviewText);
                if (!isValid.sucess)
                    throw new Exception(isValid.errorMassage);

                // Update fields
                review.Rating = reviewDto.Rating;
                review.ReviewTitle = reviewDto.ReviewTitle;
                review.ReviewText = reviewDto.ReviewText;
                review.IsEdited = true;

                var result = await _reviewRepository.UpdateAsync(review, currentUserId, cancellationToken);
                if (!result.Success)
                    throw new Exception("Failed to update review");

                // Update item average rating if review is approved
                if (review.Status == ReviewStatus.Approved)
                {
                    await UpdateItemAverageRatingAsync(review.ItemId, cancellationToken);
                }

                return _mapper.MapModel<TbItemReview, ItemReviewResponseDto>(review);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(UpdateReviewAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Approves the review with given Id, marking it as visible/approved.
        /// </summary>
        /// <param name="reviewId">Id of the review to approve.</param>
        /// <param name="adminId">Id of the admin performing approval (for audit logging).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if review was successfully approved; otherwise false.</returns>
        public async Task<bool> ChangeReviewStatus(
            Guid reviewId,
            ReviewStatus newStatus,
            string adminId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _reviewRepository.UpdateReviewStatus(reviewId, newStatus, adminId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(ChangeReviewStatus)}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a review permanently or flags it as deleted (soft-delete), after verifying ownership.
        /// </summary>
        /// <param name="reviewId">Id of the review to delete.</param>
        /// <param name="currentUserId">Guid of the user requesting deletion (must match review owner).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deletion was successful; otherwise false (e.g. not found or not authorized).</returns>
        public async Task<bool> DeleteReviewAsync(
            Guid reviewId,
            Guid currentUserId,
            bool isAdmin = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var review = await _reviewRepository.GetReviewDetailsAsync(reviewId, cancellationToken);

                if (review == null)
                    return false;

                // Check ownership
                if (!isAdmin && review.Customer.UserId != currentUserId.ToString())
                    throw new UnauthorizedAccessException("You can only delete your own reviews");

                return await _reviewRepository.SoftDeleteAsync(reviewId, currentUserId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(DeleteReviewAsync)}");
                throw;
            }
        }

        #region Helper Methods

        private async Task UpdateItemAverageRatingAsync(
            Guid itemId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Get the item
                var item = await _itemRepository.FindByIdAsync(itemId, cancellationToken);
                if (item == null)
                {
                    _logger.Warning($"Item with ID {itemId} not found while updating average rating");
                    return;
                }

                // Get average rating from approved reviews
                var averageRating = await _reviewRepository.GetAverageRatingAsync(itemId, cancellationToken);

                // Update the average rating
                item.AverageRating = averageRating > 0 ? Math.Round(averageRating, 2) : (decimal?)null;

                // Save the item
                await _itemRepository.UpdateAsync(item, Guid.Empty, cancellationToken);

                _logger.Information($"Updated average rating for item {itemId}: {item.AverageRating}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating average rating for item {itemId}");
                // Don't throw - this is a secondary operation
            }
        }

        private (bool, string) IsReviewValid(decimal rating, string reviewTitle, string reviewText)
        {
            if (rating < 1 || rating > 5)
                return (false, "Rating must be between 1 and 5");

            if (string.IsNullOrWhiteSpace(reviewTitle))
                return (false, "Review title is required");

            if (string.IsNullOrWhiteSpace(reviewText))
                return (false, "Review text is required");

            return (true, string.Empty);
        }

        private string generateNumber()
        {
            // Generate a unique 4-character string
            char[] randomNumber = new char[3];
            byte[] randomBytes = RandomNumberGenerator.GetBytes(4);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < 3; i++)
                randomNumber[i] = chars[randomBytes[i] % chars.Length];

            // Generation Time
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var egyptTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var formattedDate = egyptTime.ToString("MMdd");

            return $"REV-{formattedDate}-{new string(randomNumber)}";
        }

        #endregion
    }
}
