using AutoMapper;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Review;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Review;
using DAL.Exceptions;
using DAL.Models;
using DAL.Repositories;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Serilog;
using Shared.DTOs.ECommerce.Item;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;


namespace BL.Service.Review
{


	public class OfferReviewService : BaseService<TbOfferReview, OfferReviewDto>, IOfferReviewService
	{
		private readonly IOfferReviewRepository _reviewRepo;
		private readonly ITableRepository<TbOfferReview> _tableRepository;
		private readonly IBaseMapper _mapper;
		private readonly ILogger _logger;
		public OfferReviewService(
			IBaseMapper mapper,
			IOfferReviewRepository reviewRepo,
			ILogger logger,
			ITableRepository<TbOfferReview> tableRepository)
			: base(tableRepository, mapper)
		{
			_mapper = mapper;
			_logger = logger;
			_reviewRepo = reviewRepo;
			_tableRepository = tableRepository;
		}

		/// <summary>
		/// Retrieves the details of a specific offer review by its unique identifier.
		/// </summary>
		/// <param name="reviewId">The unique identifier of the offer review. Must not be <see cref="Guid.Empty"/>.</param>
		/// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
		/// <returns>
		/// An <see cref="OfferReviewDto"/> representing the review details if found; otherwise, <c>null</c>.
		/// </returns>
		/// <remarks>
		/// This method validates the input, fetches the review entity from the repository,
		/// maps it to a DTO using AutoMapper, and propagates any exceptions to the caller.
		/// </remarks>
		/// <exception cref="ArgumentException">Thrown when <paramref name="reviewId"/> is <see cref="Guid.Empty"/>.</exception>
		/// <exception cref="Exception">Thrown when an unexpected error occurs during retrieval.</exception>
		public async Task<OfferReviewDto?> GetReviewByIdAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			if (reviewId == Guid.Empty)
				throw new ArgumentException("ReviewId cannot be empty.", nameof(reviewId));

			try
			{
				var review = await _reviewRepo.GetReviewDetailsAsync(reviewId, cancellationToken);

				if (review == null)
					return null;

				return _mapper.MapModel<TbOfferReview, OfferReviewDto>(review);
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while retrieving review with ID {reviewId}.", ex);
			}
		}

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
				var review = _mapper.MapModel<OfferReviewDto, TbOfferReview>(reviewDto);
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

				return _mapper.MapModel<TbOfferReview, OfferReviewDto>(review);
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
				return _mapper.MapList<TbOfferReview, OfferReviewDto>(reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReviewsByOfferIdAsync)}");
				throw;
			}
		}

		/// <summary>
		/// Retrieves a paginated list of offer reviews based on a comprehensive set of search criteria.
		/// Ideal for building admin dashboards or customer-facing review listing pages with advanced filtering and sorting.
		/// </summary>
		/// <param name="criteriaModel">
		/// A model containing all search, sort, and pagination criteria.
		/// Allows filtering by:
		/// - A search term in the review title or text.
		/// - A specific Offer ID.
		/// - A specific Customer ID.
		/// - A rating range (from RatingFrom to RatingTo).
		/// - Whether the purchase is verified (IsVerifiedPurchase).
		/// - A list of one or more review statuses (e.g., Pending, Approved).
		/// 
		/// Supports dynamic sorting by specifying a SortBy column (e.g., Rating, CreatedDateUtc) 
		/// and a SortDirection (asc or desc).
		/// 
		/// Supports pagination using PageNumber and PageSize.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>
		/// A <see cref="PaginatedDataModel{OfferReviewDto}"/> containing the list of matching reviews 
		/// and the total record count.
		/// </returns>
		public async Task<PaginatedDataModel<OfferReviewDto>> GetPaginatedReviewsAsync(OfferReviewSearchCriteriaModel criteriaModel, CancellationToken cancellationToken = default)
		{
			if (criteriaModel == null)
				throw new ArgumentNullException(nameof(criteriaModel));

			if (criteriaModel.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

			if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

			Expression<Func<TbOfferReview, bool>> filter = x => !x.IsDeleted;

			var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				filter = filter.And(x =>
					(x.ReviewTitle != null && x.ReviewTitle.ToLower().Contains(searchTerm)) ||
					(x.ReviewText != null && x.ReviewText.ToLower().Contains(searchTerm))
				);
			}

			if (criteriaModel.OfferId.HasValue)
			{
				filter = filter.And(x => x.OfferID == criteriaModel.OfferId.Value);
			}

			if (criteriaModel.CustomerId.HasValue)
			{
				filter = filter.And(x => x.CustomerID == criteriaModel.CustomerId.Value);
			}

			if (criteriaModel.RatingFrom.HasValue)
			{
				filter = filter.And(x => x.Rating >= criteriaModel.RatingFrom.Value);
			}

			if (criteriaModel.RatingTo.HasValue)
			{
				filter = filter.And(x => x.Rating <= criteriaModel.RatingTo.Value);
			}

			if (criteriaModel.IsVerifiedPurchase.HasValue)
			{
				filter = filter.And(x => x.IsVerifiedPurchase == criteriaModel.IsVerifiedPurchase.Value);
			}

			if (criteriaModel.Statuses != null && criteriaModel.Statuses.Any())
			{
				filter = filter.And(x => criteriaModel.Statuses.Contains(x.Status));
			}

			Func<IQueryable<TbOfferReview>, IOrderedQueryable<TbOfferReview>> orderByExpression = null;

			if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
			{
				switch (criteriaModel.SortBy.ToLowerInvariant())
				{
					case "rating":
						orderByExpression = criteriaModel.SortDirection.ToLowerInvariant() == "desc"
							? q => q.OrderByDescending(x => x.Rating)
							: q => q.OrderBy(x => x.Rating);
						break;
					case "helpfulcount":
						orderByExpression = criteriaModel.SortDirection.ToLowerInvariant() == "desc"
							? q => q.OrderByDescending(x => x.HelpfulCount)
							: q => q.OrderBy(x => x.HelpfulCount);
						break;
					case "createddateutc":
					default:
						orderByExpression = criteriaModel.SortDirection.ToLowerInvariant() == "desc"
							? q => q.OrderByDescending(x => x.CreatedDateUtc)
							: q => q.OrderBy(x => x.CreatedDateUtc);
						break;
				}
			}
			else
			{
				orderByExpression = q => q.OrderByDescending(x => x.CreatedDateUtc);
			}

			var items = await _tableRepository.GetPageAsync(
			criteriaModel.PageNumber,
			criteriaModel.PageSize,
			filter,
			orderBy: orderByExpression
		);

			var itemsDto = _mapper.MapList<TbOfferReview, OfferReviewDto>(items.Items);

			return new PaginatedDataModel<OfferReviewDto>(itemsDto, items.TotalRecords);
		}
	
		
		
		//public async Task<PaginatedDataModel<OfferReviewDto>> GetPaginatedReviewsAsync(OfferReviewSearchCriteriaModel criteriaModel, CancellationToken cancellationToken = default)
		//{
		//	if (criteriaModel == null)
		//		throw new ArgumentNullException(nameof(criteriaModel));

		//	if (criteriaModel.PageNumber < 1)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

		//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
		//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

		//	// Base filter
		//	Expression<Func<TbOfferReview, bool>> filter = x => !x.IsDeleted;

		//	// Combine expressions manually
		//	var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
		//	if (!string.IsNullOrWhiteSpace(searchTerm))
		//	{
		//		filter = filter.And(x =>
		//			(x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
		//			(x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
		//			(x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm)) ||
		//			(x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm))
		//		);
		//	}

		//	if (criteriaModel.CategoryIds?.Any() == true)
		//	{
		//		filter = filter.And(x => criteriaModel.CategoryIds.Contains(x.CategoryId));
		//	}

		//	// New Item Flags Filters
		//	if (criteriaModel.IsNewArrival.HasValue)
		//	{
		//		filter = filter.And(x => x.CreatedDateUtc.Date >= DateTime.UtcNow.AddDays(-3).Date);
		//	}

		//	// Get paginated data from repository
		//	var items = await _tableRepository.GetPageAsync(
		//		criteriaModel.PageNumber,
		//		criteriaModel.PageSize,
		//		filter,
		//		orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
		//	);

		//	var itemsDto = _mapper.MapList<TbOfferReview, OfferReviewDto>(items.Items);

		//	return new PaginatedDataModel<OfferReviewDto>(itemsDto, items.TotalRecords);
		//}





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
				return _mapper.MapList<TbOfferReview, OfferReviewDto>(reviews);
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
