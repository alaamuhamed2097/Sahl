using AutoMapper;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Review;
using BL.Service.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Serilog;
using Shared.DTOs.Review;

namespace BL.Service.Review
{
   

    public class OfferReviewService : BaseService<TbOfferReview, OfferReviewDto>, IOfferReviewService
    {
		private readonly IOfferReviewRepository _reviewRepo;
		private readonly IBaseMapper _mapper;
		private readonly ILogger _logger;
		public OfferReviewService(
			IBaseMapper mapper,
			ITableRepository<TbOfferReview> repository,
			IOfferReviewRepository reviewRepo,
			ILogger logger)
			: base(repository, mapper)
		{
			_mapper = mapper;
			_logger = logger;
			_reviewRepo = reviewRepo;
		}

		//public OfferReviewService(
		//	IOfferReviewRepository reviewRepo,
		//	IMapper mapper,
		//	ILogger logger)
		//{
		//	_reviewRepo = reviewRepo;
		//	_mapper = mapper;
		//	_logger = logger;
		//}

		/// <summary>
		/// Creates a new review for a given offer by a customer.
		/// Validates that the customer hasn't already reviewed the same offer (optional),
		/// sets the default review status (e.g. Pending), and persists it in database.
		/// </summary>
		/// <param name="reviewDto">DTO containing rating, comments and associated OfferId.</param>
		/// <param name="cancellationToken">Cancellation token to cancel operation.</param>
		/// <returns>The created review DTO with assigned Id and metadata (creation date, status).</returns>

		public async Task<OfferReviewDto> SubmitReviewAsync(
			OfferReviewDto reviewDto,
			CancellationToken cancellationToken = default)
		{
			try
			{
				// Validation
				if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
					throw new ArgumentException("Rating must be between 1 and 5");

				if (string.IsNullOrWhiteSpace(reviewDto.ReviewTitle))
					throw new ArgumentException("Review title is required");

				if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
					throw new ArgumentException("Review text is required");

				// Check if customer already reviewed this order item
				if (reviewDto.OrderItemID.HasValue)
				{
					var existingReview = await _reviewRepo.GetCustomerReviewForOrderItemAsync(
						reviewDto.OrderItemID.Value,
						reviewDto.CustomerID,
						cancellationToken);

					if (existingReview != null)
						throw new InvalidOperationException("You have already reviewed this Offer");

					reviewDto.IsVerifiedPurchase = true;
				}

				// Create review
				var review = _mapper.MapModel<OfferReviewDto,TbOfferReview >(reviewDto);
				review.Status = ReviewStatus.Pending;

				var result = await _reviewRepo.CreateAsync(review, reviewDto.CustomerID, cancellationToken);

				if (!result.Success)
					throw new Exception("Failed to submit review");

				review.Id = result.Id;
				return _mapper.MapModel<TbOfferReview, OfferReviewDto>(review);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(SubmitReviewAsync)}");
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

		public async Task<OfferReviewDto> updateReviewAsync(
			
			OfferReviewDto reviewDto,
			Guid currentUserId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewDto.Id, cancellationToken);

				if (review == null)
					throw new NotFoundException($"Review with ID {reviewDto.Id} not found.", _logger);

				// Check ownership
				if (review.CustomerID != currentUserId)
					throw new UnauthorizedAccessException("You can only edit your own reviews");

				// Check if review can be edited
				if (review.Status != ReviewStatus.Approved && review.Status != ReviewStatus.Pending)
					throw new InvalidOperationException("This review cannot be edited");

				// Validation
				if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
					throw new ArgumentException("Rating must be between 1 and 5");

				if (string.IsNullOrWhiteSpace(reviewDto.ReviewTitle))
					throw new ArgumentException("Review title is required");

				if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
					throw new ArgumentException("Review text is required");

				// Update fields
				review.Rating = reviewDto.Rating;
				review.ReviewTitle = reviewDto.ReviewTitle;
				review.ReviewText = reviewDto.ReviewText;
				review.IsEdited = true;

				var result = await _reviewRepo.UpdateAsync(review, currentUserId, cancellationToken);

				if (!result.Success)
					throw new Exception("Failed to update review");

				return _mapper.MapModel<TbOfferReview,OfferReviewDto>(review);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(updateReviewAsync)}");
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
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);

				if (review == null)
					return false;

				// Check ownership
				if (review.CustomerID != currentUserId)
					throw new UnauthorizedAccessException("You can only delete your own reviews");

				return await _reviewRepo.SoftDeleteAsync(reviewId, currentUserId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(DeleteReviewAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Retrieves all approved reviews for a specific offer.
		/// </summary>
		/// <param name="OfferId">Offer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Enumerable of OfferReviewDto representing approved reviews for the offer.</returns>
		public async Task<IEnumerable<OfferReviewDto>> GetReviewsByOfferIdAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _reviewRepo.GetReviewsByOfferIdAsync(OfferId, cancellationToken);
				return _mapper.MapList<TbOfferReview,OfferReviewDto> (reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReviewsByOfferIdAsync)}");
				throw;
			}
		}

		/// <summary>
		/// Retrieves a paginated list of offer reviews, filtered optionally by offer and review status.
		/// Useful for admin or listing pages.
		/// </summary>
		/// <param name="OfferId">Optional filter by offer id.</param>
		/// <param name="status">Optional filter by review status (Pending/Approved/Rejected).</param>
		/// <param name="pageNumber">Page number (default 1).</param>
		/// <param name="pageSize">Page size (default 10).</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Paginated data model containing matching reviews.</returns>
		public async Task<PaginatedDataModel<OfferReviewDto>> GetPaginatedReviewsAsync(
			Guid? OfferId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var result = await _reviewRepo.GetPaginatedReviewsAsync(
					OfferId, status, pageNumber, pageSize, cancellationToken);

				var reviewDtos = _mapper.MapList<TbOfferReview,OfferReviewDto>(result.Items);
				return new PaginatedDataModel<OfferReviewDto>(reviewDtos, result.TotalRecords);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetPaginatedReviewsAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Retrieves reviews currently pending approval.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Enumerable of pending OfferReviewDto.</returns>
		public async Task<IEnumerable<OfferReviewDto>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _reviewRepo.GetPendingReviewsAsync(cancellationToken);
				return _mapper.MapList< TbOfferReview,OfferReviewDto > (reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetPendingReviewsAsync)}");
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
		public async Task<bool> ApproveReviewAsync(
			Guid reviewId,
			Guid adminId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);

				if (review == null)
					return false;

				review.Status = ReviewStatus.Approved;

				var result = await _reviewRepo.UpdateAsync(review, adminId, cancellationToken);
				return result.Success;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(ApproveReviewAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Rejects the review with given Id, marking it as rejected/hidden.
		/// </summary>
		/// <param name="reviewId">Id of the review to reject.</param>
		/// <param name="adminId">Id of the admin performing rejection (for audit logging).</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>True if review was successfully rejected; otherwise false.</returns>
		public async Task<bool> RejectReviewAsync(
			Guid reviewId,
			Guid adminId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);

				if (review == null)
					return false;

				review.Status = ReviewStatus.Rejected;

				var result = await _reviewRepo.UpdateAsync(review, adminId, cancellationToken);
				return result.Success;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(RejectReviewAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Calculates and returns the average rating score for a given offer,
		/// based only on approved reviews.
		/// </summary>
		/// <param name="OfferId">Offer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Average rating as decimal (e.g. 4.5).</returns>
		public async Task<decimal> GetAverageRatingAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _reviewRepo.GetAverageRatingAsync(OfferId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetAverageRatingAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Counts total number of approved reviews for a given offer.
		/// </summary>
		/// <param name="OfferId">Offer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Total count of approved reviews.</returns>
		public async Task<int> GetReviewCountAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _reviewRepo.GetReviewCountByOfferIdAsync(OfferId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReviewCountAsync)}");
				throw;
			}
		}
		/// <summary>
		/// Retrieves aggregated review statistics for a given offer, such as average rating and total review count.
		/// </summary>
		/// <param name="offerId">Offer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>OfferReviewStatsDto containing metrics like average rating, total reviews.</returns>
		public async Task<OfferReviewStatsDto> GetOfferReviewStatsAsync(
		   Guid OfferId,
		   CancellationToken cancellationToken = default)
		{
			var averageRating = await _reviewRepo.GetAverageRatingAsync(OfferId, cancellationToken);
			var reviewCount = await _reviewRepo.GetReviewCountByOfferIdAsync(OfferId, cancellationToken);
			var ratingDistribution = await _reviewRepo.GetRatingDistributionAsync(OfferId, cancellationToken);

			var stats = new OfferReviewStatsDto
			{
				AverageRating = averageRating,
				ReviewCount = reviewCount,
				FiveStarCount = ratingDistribution.GetValueOrDefault(5, 0),
				FourStarCount = ratingDistribution.GetValueOrDefault(4, 0),
				ThreeStarCount = ratingDistribution.GetValueOrDefault(3, 0),
				TwoStarCount = ratingDistribution.GetValueOrDefault(2, 0),
				OneStarCount = ratingDistribution.GetValueOrDefault(1, 0)
			};

			// Calculate percentages
			if (reviewCount > 0)
			{
				stats.FiveStarPercentage = Math.Round((decimal)stats.FiveStarCount / reviewCount * 100, 2);
				stats.FourStarPercentage = Math.Round((decimal)stats.FourStarCount / reviewCount * 100, 2);
				stats.ThreeStarPercentage = Math.Round((decimal)stats.ThreeStarCount / reviewCount * 100, 2);
				stats.TwoStarPercentage = Math.Round((decimal)stats.TwoStarCount / reviewCount * 100, 2);
				stats.OneStarPercentage = Math.Round((decimal)stats.OneStarCount / reviewCount * 100, 2);
			}

			return stats;
		}


		//public override bool Equals(object? obj)
		//{
		//	return obj is OfferReviewService service &&
		//		   EqualityComparer<IBaseMapper>.Default.Equals(_mapper, service._mapper);
		//}

		
	}
}
