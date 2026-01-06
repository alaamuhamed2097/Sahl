
using BL.Contracts.IMapper;
using BL.Contracts.Service.Review;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Review;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Resources;
using Serilog;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.Review
{
    public class VendorReviewService : BaseService<TbVendorReview, VendorReviewDto>, IVendorReviewService
    {
        private readonly IVendorReviewRepository _vendorReviewRepo;
        private readonly ITableRepository<TbVendorReview> _tableRepository;
        private readonly ITableRepository<TbVendor> _vendorRepo;
        private readonly ITableRepository<TbOrderDetail> _orderDetailRepo;
		private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

		public VendorReviewService(
			IBaseMapper mapper,
			IVendorReviewRepository reviewRepo,
			ILogger logger,
			ITableRepository<TbVendorReview> tableRepository,
			ITableRepository<TbVendor> vendorRepo,
			ITableRepository<TbOrderDetail> orderDetailRepo)
			: base(tableRepository, mapper)
		{
			_mapper = mapper;
			_logger = logger;
			_vendorReviewRepo = reviewRepo;
			_tableRepository = tableRepository;
			_vendorRepo = vendorRepo;
			_orderDetailRepo = orderDetailRepo;
		}

		/// <summary>
		/// Retrieves the details of a specific Vendor review by its unique identifier.
		/// </summary>
		/// <param name="reviewId">The unique identifier of the Vendor review. Must not be <see cref="Guid.Empty"/>.</param>
		/// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
		/// <returns>
		/// A <see cref="VendorReviewDto"/> representing the review details if found; otherwise, <c>null</c>.
		/// </returns>
		/// <remarks>
		/// This method validates the input, fetches the review entity from the repository,
		/// maps it to a DTO using AutoMapper, and propagates any exceptions to the caller.
		/// </remarks>
		/// <exception cref="ArgumentException">Thrown when <paramref name="reviewId"/> is <see cref="Guid.Empty"/>.</exception>
		/// <exception cref="Exception">Thrown when an unexpected error occurs during retrieval.</exception>
		public async Task<VendorReviewDto?> GetReviewByIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            if (reviewId == Guid.Empty)
                throw new ArgumentException("ReviewId cannot be empty.", nameof(reviewId));

            try
            {
                var review = await _vendorReviewRepo.GetReviewDetailsAsync(reviewId, cancellationToken);

                if (review == null)
                    return null;

                return _mapper.MapModel<TbVendorReview, VendorReviewDto>(review);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving review with ID {reviewId}.", ex);
            }
        }

        /// <summary>
        /// Creates a new review for a given Vendor by a customer.
        /// Validates that the customer hasn't already reviewed the same Vendor,
        /// sets the default review status (e.g. Pending), and persists it in database.
        /// </summary>
        /// <param name="reviewDto">DTO containing rating, comments and associated VendorId.</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>The created review DTO with assigned Id and metadata (creation date, status).</returns>
        public async Task<VendorReviewDto> SubmitReviewAsync(
            VendorReviewDto reviewDto,
            Guid currntUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validation
                if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
                    throw new ArgumentException("Rating must be between 1 and 5");

                if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
                    throw new ArgumentException("Review text is required");

				// Check if customer has purchased from this vendor
				var hasPurchased = await _vendorReviewRepo.HasCustomerPurchasedFromVendorAsync(
					reviewDto.OrderDetailId,
					reviewDto.VendorId,
					cancellationToken);

				if (!hasPurchased)
					throw new InvalidOperationException("You can only review vendors you have purchased from");


				// Check if customer already reviewed this vendor
				var hasReviewed = await _vendorReviewRepo.HasCustomerReviewedVendorAsync(
                    reviewDto.CustomerId,
                    reviewDto.VendorId,
                    cancellationToken);

                if (hasReviewed)
                    throw new InvalidOperationException("You have already reviewed this Vendor");

				var isVerifiedPurchase = await _vendorReviewRepo.IsVerifiedPurchaseAsync(
                    reviewDto.CustomerId.ToString(),
                    reviewDto.OrderDetailId,
                    cancellationToken);
				// Create review
				var review = _mapper.MapModel<VendorReviewDto, TbVendorReview>(reviewDto);
                review.IsVerifiedPurchase = isVerifiedPurchase;
				review.Status = ReviewStatus.Pending;
                review.IsEdited = false;
                var result = await _vendorReviewRepo.CreateAsync(review, currntUserId, cancellationToken);

                if (!result.Success)
                    throw new Exception("Failed to submit review");

                review.Id = result.Id;
				// Update vendor average rating
				await UpdateVendorAverageRatingAsync(reviewDto.VendorId, currntUserId, cancellationToken);

				return _mapper.MapModel<TbVendorReview, VendorReviewDto>(review);
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
		 
		public async Task<VendorReviewDto> UpdateReviewAsync(
            VendorReviewDto reviewDto,
            Guid currentUserId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var review = await _vendorReviewRepo.FindByIdAsync(reviewDto.Id, cancellationToken);

                if (review == null)
                    throw new NotFoundException($"Review with ID {reviewDto.Id} not found.", _logger);

                // Check ownership
                if (review.CustomerId != currentUserId)
                    throw new UnauthorizedAccessException("You can only edit your own reviews");

                // Check if review can be edited
                if (review.Status != ReviewStatus.Approved && review.Status != ReviewStatus.Pending)
                    throw new InvalidOperationException("This review cannot be edited");

                // Validation
                if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
                    throw new ArgumentException("Rating must be between 1 and 5");

                if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
                    throw new ArgumentException("Review text is required");

                // Update fields
                review.Rating = reviewDto.Rating;
                review.ReviewText = reviewDto.ReviewText;
                review.IsEdited = true;

				// Save the updated review
				await _vendorReviewRepo.UpdateAsync(review, currentUserId, cancellationToken);

				// Update vendor average rating
				await UpdateVendorAverageRatingAsync(review.VendorId, currentUserId, cancellationToken);


				return _mapper.MapModel<TbVendorReview, VendorReviewDto>(review);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(UpdateReviewAsync)}");
                throw;
            }
        }


		//private async Task UpdateVendorAverageRatingAsync(Guid vendorId, CancellationToken cancellationToken = default)
		//{
		//	try
		//	{
		//		// Get average rating from approved reviews
		//		var averageRating = await _vendorReviewRepo.GetAverageRatingAsync(vendorId, cancellationToken);

		//		// Get the item
		//		var item = await _vendorRepo.FindByIdAsync(vendorId, cancellationToken);

		//		if (item == null)
		//		{
		//			_logger.Warning($"Item with ID {vendorId} not found while updating average rating");
		//			return;
		//		}

		//		// Update the average rating
		//		item.AverageRating = averageRating > 0 ? Math.Round(averageRating, 2) : (decimal?)null;

		//		// Save the item
		//		await _vendorRepo.UpdateAsync(item, Guid.Empty, cancellationToken);

		//		_logger.Information($"Updated average rating for item {vendorId}: {item.AverageRating}");
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.Error(ex, $"Error updating average rating for item {vendorId}");
		//		// Don't throw - this is a secondary operation
		//	}
		//}
		/// <summary>
		/// Updates the average rating for a vendor after a review is submitted or updated.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		private async Task UpdateVendorAverageRatingAsync(
			Guid vendorId,
            Guid currntUserId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				// Get the average rating for approved reviews only
				var averageRating = await _vendorReviewRepo.GetVendorAverageRatingAsync(
					vendorId,
					cancellationToken);

				// Get the vendor
				var vendor = await _vendorRepo.FindByIdAsync(vendorId, cancellationToken);
				if (vendor == null)
					throw new NotFoundException($"Vendor with ID {vendorId} not found.", _logger);

				// Update the average rating
				vendor.AverageRating = averageRating > 0 ? Math.Round(averageRating, 2) : null;

				// Save changes
				await _vendorRepo.UpdateAsync(vendor, currntUserId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error updating vendor average rating for vendor {vendorId}");
				// Don't throw - this shouldn't prevent the review operation from completing
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
                var review = await _vendorReviewRepo.FindByIdAsync(reviewId, cancellationToken);

                if (review == null)
                    return false;

                // Check ownership
                if (review.CustomerId != currentUserId)
                    throw new UnauthorizedAccessException("You can only delete your own reviews");

                return await _vendorReviewRepo.SoftDeleteAsync(reviewId, currentUserId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(DeleteReviewAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all approved reviews for a specific Vendor.
        /// </summary>
        /// <param name="vendorId">Vendor identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Enumerable of VendorReviewDto representing approved reviews for the Vendor.</returns>
        public async Task<IEnumerable<VendorReviewDto>> GetReviewsByVendorIdAsync(
            Guid vendorId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var reviews = await _vendorReviewRepo.GetReviewsByVendorIdAsync(vendorId, cancellationToken);
                return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetReviewsByVendorIdAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a paginated list of Vendor reviews based on a comprehensive set of search criteria.
        /// Ideal for building admin dashboards or customer-facing review listing pages with advanced filtering and sorting.
        /// </summary>
        /// <param name="criteriaModel">
        /// A model containing all search, sort, and pagination criteria.
        /// Allows filtering by:
        /// - A search term in the review text.
        /// - A specific Vendor ID.
        /// - A specific Customer ID.
        /// - A rating range (from RatingFrom to RatingTo).
        /// - A list of one or more review statuses (e.g., Pending, Approved).
        /// 
        /// Supports dynamic sorting by specifying a SortBy column (e.g., Rating, CreatedDateUtc) 
        /// and a SortDirection (asc or desc).
        /// 
        /// Supports pagination using PageNumber and PageSize.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>
        /// A <see cref="PagedResult{VendorReviewDto}"/> containing the list of matching reviews 
        /// and the total record count.
        /// </returns>
        public async Task<PagedResult<VendorReviewDto>> GetPaginatedReviewsAsync(
            VendorReviewSearchCriteriaModel criteriaModel,
            CancellationToken cancellationToken = default)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbVendorReview, bool>> filter = x => !x.IsDeleted;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    (x.ReviewText != null && x.ReviewText.ToLower().Contains(searchTerm))
                );
            }

            if (criteriaModel.VendorId.HasValue)
            {
                filter = filter.And(x => x.VendorId == criteriaModel.VendorId.Value);
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

            if (criteriaModel.Statuses != null && criteriaModel.Statuses.Any())
            {
                filter = filter.And(x => criteriaModel.Statuses.Contains(x.Status));
            }

            Func<IQueryable<TbVendorReview>, IOrderedQueryable<TbVendorReview>> orderByExpression = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                switch (criteriaModel.SortBy.ToLowerInvariant())
                {
                    case "rating":
                        orderByExpression = criteriaModel.SortDirection.ToLowerInvariant() == "desc"
                            ? q => q.OrderByDescending(x => x.Rating)
                            : q => q.OrderBy(x => x.Rating);
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

            var itemsDto = _mapper.MapList<TbVendorReview, VendorReviewDto>(items.Items);

            return new PagedResult<VendorReviewDto>(itemsDto, items.TotalRecords);
        }

        /// <summary>
        /// Retrieves reviews currently pending approval.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Enumerable of pending VendorReviewDto.</returns>
        public async Task<IEnumerable<VendorReviewDto>> GetPendingReviewsAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var reviews = await _vendorReviewRepo.GetPendingReviewsAsync(cancellationToken);
                return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
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
                var review = await _vendorReviewRepo.FindByIdAsync(reviewId, cancellationToken);

                if (review == null)
                    return false;

                review.Status = ReviewStatus.Approved;

                var result = await _vendorReviewRepo.UpdateAsync(review, adminId, cancellationToken);
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
                var review = await _vendorReviewRepo.FindByIdAsync(reviewId, cancellationToken);

                if (review == null)
                    return false;

                review.Status = ReviewStatus.Rejected;

                var result = await _vendorReviewRepo.UpdateAsync(review, adminId, cancellationToken);
                return result.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(RejectReviewAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Calculates and returns the average rating score for a given Vendor,
        /// based only on approved reviews.
        /// </summary>
        /// <param name="vendorId">Vendor identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Average rating as decimal (e.g. 4.5).</returns>
        public async Task<decimal> GetAverageRatingAsync(
            Guid vendorId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _vendorReviewRepo.GetVendorAverageRatingAsync(vendorId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetAverageRatingAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Counts total number of approved reviews for a given Vendor.
        /// </summary>
        /// <param name="vendorId">Vendor identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Total count of approved reviews.</returns>
        public async Task<int> GetReviewCountAsync(
            Guid vendorId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _vendorReviewRepo.GetVendorReviewCountAsync(vendorId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetReviewCountAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves aggregated review statistics for a given Vendor, such as average rating and total review count.
        /// </summary>
        /// <param name="vendorId">Vendor identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>VendorReviewStatsDto containing metrics like average rating, total reviews.</returns>
        public async Task<VendorReviewStatsDto> GetVendorReviewStatsAsync(
            Guid vendorId,
            CancellationToken cancellationToken = default)
        {

            var averageRating = await _vendorReviewRepo.GetVendorAverageRatingAsync(vendorId, cancellationToken);
            var reviewCount = await _vendorReviewRepo.GetVendorReviewCountAsync(vendorId, cancellationToken);
            var ratingDistribution = await _vendorReviewRepo.GetVendorRatingDistributionAsync(vendorId, cancellationToken);

            var stats = new VendorReviewStatsDto
            {
                VendorId = vendorId,
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

        /// <summary>
        /// Retrieves all reviews for a vendor with optional status filter.
        /// </summary>
        /// <param name="vendorId">Vendor ID.</param>
        /// <param name="status">Optional review status filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of reviews.</returns>
        public async Task<IEnumerable<VendorReviewDto>> GetVendorReviewsAsync(
            Guid vendorId,
            ReviewStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var reviews = await _vendorReviewRepo.GetVendorReviewsAsync(vendorId, status, cancellationToken);
                return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetVendorReviewsAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all reviews created by a specific customer.
        /// </summary>
        /// <param name="customerId">Customer ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of customer reviews.</returns>
        public async Task<IEnumerable<VendorReviewDto>> GetCustomerReviewsAsync(
            Guid customerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var reviews = await _vendorReviewRepo.GetCustomerReviewsAsync(customerId, cancellationToken);
                return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetCustomerReviewsAsync)}");
                throw;
            }
        }

		/// <summary>
		/// Retrieves all reviews for a vendor with verified and non-verified purchases.
		/// </summary>
		/// <param name="vendorId">Vendor ID.</param>
		/// <param name="isVerifiedPurchase">Optional filter for verified purchases. If null, returns all reviews.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>List of vendor reviews filtered by verification status.</returns>
		public async Task<IEnumerable<VendorReviewDto>> GetVendorReviewsByVerificationAsync(
			Guid vendorId,
			bool? isVerifiedPurchase = null,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _vendorReviewRepo.GetVendorReviewsByVerificationAsync(
					vendorId,
					isVerifiedPurchase,
					cancellationToken);

				return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetVendorReviewsByVerificationAsync)}");
				throw;
			}
		}
	}
}