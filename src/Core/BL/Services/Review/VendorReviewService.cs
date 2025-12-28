//using AutoMapper;
//using BL.Contracts.IMapper;
//using BL.Contracts.Service.Review;
//using BL.Service.Base;
//using Common.Enumerations.Review;
//using DAL.Contracts.Repositories;
//using DAL.Contracts.Repositories.Review;
//using DAL.Repositories;
//using Domains.Entities.ECommerceSystem.Review;
//using Serilog;
//using Shared.DTOs.Review;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BL.Service.Review
//{
//	public class VendorReviewService : BaseService<TbVendorReview, VendorReviewDto>, IVendorReviewService
//	{
//		private readonly IVendorReviewRepository _reviewRepository;
//		private readonly ITableRepository<TbVendorReview> _tableRepository;

//		private readonly IBaseMapper _mapper;
//		private readonly ILogger _logger;

//		public VendorReviewService(
//			IVendorReviewRepository reviewRepository,
//			IBaseMapper mapper,
//			ILogger logger,
//			ITableRepository<TbVendorReview> tableRepository) : base(tableRepository, mapper)
//		{
//			_reviewRepository = reviewRepository;
//			_mapper = mapper;
//			_logger = logger;
//			_tableRepository = tableRepository;
//		}


//		/// <summary>
//		/// Creates a new vendor review.
//		/// Validates that the customer hasn't already reviewed the vendor.
//		/// </summary>
//		/// <param name="reviewDto">Review data transfer object.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>The created review or null if failed.</returns>
//		public async Task<VendorReviewDto> CreateReviewAsync(
//			VendorReviewDto reviewDto,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				// Check if customer already reviewed this vendor
//				var hasReviewed = await _reviewRepository.HasCustomerReviewedVendorAsync(
//					reviewDto.CustomerId, reviewDto.VendorId, cancellationToken);

//				if (!hasReviewed)
//					throw new Exception("Failed to submit review");

//				var review = _mapper.MapModel<TbVendorReview, VendorReviewDto>(reviewDto);
//				review.CreatedDate = DateTime.UtcNow;
//				review.Status = ReviewStatus.Pending;
//				review.IsDeleted = false;

//				var created = await _reviewRepository.AddAsync(review);
//				var resultDto = _mapper.Map<VendorReviewDto>(created);

//				_logger.LogInformation("Review created successfully for Vendor {VendorId} by Customer {CustomerId}.",
//					reviewDto.VendorId, reviewDto.CustomerId);

//				return resultDto;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error creating vendor review for Vendor {VendorId}.",
//					reviewDto.VendorId);
//				return null;
//			}
//		}

//		/// <summary>
//		/// Updates an existing vendor review.
//		/// Marks the review as edited and resets status to pending.
//		/// </summary>
//		/// <param name="id">Review ID.</param>
//		/// <param name="reviewDto">Updated review data.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>The updated review or null if failed.</returns>
//		public async Task<VendorReviewDto> UpdateReviewAsync(
//			Guid id,
//			VendorReviewDto reviewDto,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var existingReview = await _reviewRepository.GetByIdAsync(id);
//				if (existingReview == null || existingReview.IsDeleted)
//				{
//					_logger.LogWarning("Review {ReviewId} not found or deleted.", id);
//					return null;
//				}

//				existingReview.Rating = reviewDto.Rating;
//				existingReview.ReviewText = reviewDto.ReviewText;
//				existingReview.IsEdited = true;
//				existingReview.UpdatedDate = DateTime.UtcNow;
//				existingReview.Status = ReviewStatus.Pending; // Re-submit for approval

//				var updated = await _reviewRepository.UpdateAsync(existingReview);
//				var resultDto = _mapper.Map<VendorReviewDto>(updated);

//				_logger.LogInformation("Review {ReviewId} updated successfully.", id);

//				return resultDto;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error updating vendor review {ReviewId}.", id);
//				return null;
//			}
//		}

//		/// <summary>
//		/// Soft deletes a vendor review.
//		/// </summary>
//		/// <param name="id">Review ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>True if deleted successfully, false otherwise.</returns>
//		public async Task<bool> DeleteReviewAsync(
//			Guid id,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var review = await _reviewRepository.GetByIdAsync(id);
//				if (review == null || review.IsDeleted)
//				{
//					_logger.LogWarning("Review {ReviewId} not found or already deleted.", id);
//					return false;
//				}

//				review.IsDeleted = true;
//				review.UpdatedDate = DateTime.UtcNow;
//				await _reviewRepository.UpdateAsync(review);

//				_logger.LogInformation("Review {ReviewId} deleted successfully.", id);

//				return true;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error deleting vendor review {ReviewId}.", id);
//				return false;
//			}
//		}

//		/// <summary>
//		/// Retrieves a review by its ID.
//		/// </summary>
//		/// <param name="id">Review ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>The review or null if not found.</returns>
//		public async Task<VendorReviewDto> GetReviewByIdAsync(
//			Guid id,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var review = await _reviewRepository.GetByIdAsync(id);
//				if (review == null || review.IsDeleted)
//				{
//					_logger.LogWarning("Review {ReviewId} not found.", id);
//					return null;
//				}

//				var reviewDto = _mapper.Map<VendorReviewDto>(review);
//				return reviewDto;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error retrieving vendor review {ReviewId}.", id);
//				return null;
//			}
//		}

//		/// <summary>
//		/// Retrieves all approved reviews for a specific vendor.
//		/// </summary>
//		/// <param name="vendorId">Vendor ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>List of approved reviews.</returns>
//		public async Task<IEnumerable<VendorReviewDto>> GetApprovedVendorReviewsAsync(
//			Guid vendorId,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var reviews = await _reviewRepository.GetReviewsByVendorIdAsync(
//					vendorId, cancellationToken);
//				var reviewDtos = _mapper.Map<IEnumerable<VendorReviewDto>>(reviews);

//				_logger.LogInformation("Retrieved {Count} approved reviews for Vendor {VendorId}.",
//					reviewDtos.Count(), vendorId);

//				return reviewDtos;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error retrieving approved reviews for Vendor {VendorId}.",
//					vendorId);
//				return Enumerable.Empty<VendorReviewDto>();
//			}
//		}

//		/// <summary>
//		/// Retrieves all reviews for a vendor with optional status filter.
//		/// </summary>
//		/// <param name="vendorId">Vendor ID.</param>
//		/// <param name="status">Optional review status filter.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>List of reviews.</returns>
//		public async Task<IEnumerable<VendorReviewDto>> GetVendorReviewsAsync(
//			Guid vendorId,
//			ReviewStatus? status = null,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var reviews = await _reviewRepository.GetVendorReviewsAsync(
//					vendorId, status, cancellationToken);
//				var reviewDtos = _mapper.Map<IEnumerable<VendorReviewDto>>(reviews);

//				_logger.LogInformation("Retrieved {Count} reviews for Vendor {VendorId} with status filter.",
//					reviewDtos.Count(), vendorId);

//				return reviewDtos;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error retrieving reviews for Vendor {VendorId}.", vendorId);
//				return Enumerable.Empty<VendorReviewDto>();
//			}
//		}

//		/// <summary>
//		/// Retrieves all reviews created by a specific customer.
//		/// </summary>
//		/// <param name="customerId">Customer ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>List of customer reviews.</returns>
//		public async Task<IEnumerable<VendorReviewDto>> GetCustomerReviewsAsync(
//			Guid customerId,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var reviews = await _reviewRepository.GetCustomerReviewsAsync(
//					customerId, cancellationToken);
//				var reviewDtos = _mapper.Map<IEnumerable<VendorReviewDto>>(reviews);

//				_logger.LogInformation("Retrieved {Count} reviews for Customer {CustomerId}.",
//					reviewDtos.Count(), customerId);

//				return reviewDtos;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error retrieving reviews for Customer {CustomerId}.",
//					customerId);
//				return Enumerable.Empty<VendorReviewDto>();
//			}
//		}

//		/// <summary>
//		/// Approves a pending review.
//		/// </summary>
//		/// <param name="id">Review ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>True if approved successfully, false otherwise.</returns>
//		public async Task<bool> ApproveReviewAsync(
//			Guid id,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var review = await _reviewRepository.GetByIdAsync(id);
//				if (review == null || review.IsDeleted)
//				{
//					_logger.LogWarning("Review {ReviewId} not found for approval.", id);
//					return false;
//				}

//				review.Status = ReviewStatus.Approved;
//				review.UpdatedDate = DateTime.UtcNow;
//				await _reviewRepository.UpdateAsync(review);

//				_logger.LogInformation("Review {ReviewId} approved successfully.", id);

//				return true;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error approving review {ReviewId}.", id);
//				return false;
//			}
//		}

//		/// <summary>
//		/// Rejects a pending review.
//		/// </summary>
//		/// <param name="id">Review ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>True if rejected successfully, false otherwise.</returns>
//		public async Task<bool> RejectReviewAsync(
//			Guid id,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var review = await _reviewRepository.GetByIdAsync(id);
//				if (review == null || review.IsDeleted)
//				{
//					_logger.LogWarning("Review {ReviewId} not found for rejection.", id);
//					return false;
//				}

//				review.Status = ReviewStatus.Rejected;
//				review.UpdatedDate = DateTime.UtcNow;
//				await _reviewRepository.UpdateAsync(review);

//				_logger.LogInformation("Review {ReviewId} rejected successfully.", id);

//				return true;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error rejecting review {ReviewId}.", id);
//				return false;
//			}
//		}

//		/// <summary>
//		/// Gets comprehensive rating statistics for a vendor.
//		/// </summary>
//		/// <param name="vendorId">Vendor ID.</param>
//		/// <param name="cancellationToken">Cancellation token.</param>
//		/// <returns>Rating statistics or null if failed.</returns>
//		public async Task<VendorReviewDto> GetVendorRatingStatsAsync(
//			Guid vendorId,
//			CancellationToken cancellationToken = default)
//		{
//			try
//			{
//				var avgRating = await _reviewRepository.GetVendorAverageRatingAsync(
//					vendorId, cancellationToken);
//				var totalReviews = await _reviewRepository.GetVendorReviewCountAsync(
//					vendorId, cancellationToken);
//				var distribution = await _reviewRepository.GetVendorRatingDistributionAsync(
//					vendorId, cancellationToken);

//				// Create a DTO to hold the stats
//				var stats = new VendorReviewDto
//				{
//					VendorId = vendorId,
//					Rating = avgRating,
//					// يمكنك إضافة properties جديدة في الـ DTO لو محتاج
//					// أو استخدام DTO منفصل للـ Stats
//				};

//				_logger.LogInformation("Retrieved rating stats for Vendor {VendorId}: Avg={Average}, Total={Total}.",
//					vendorId, avgRating, totalReviews);

//				return stats;
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error retrieving rating stats for Vendor {VendorId}.",
//					vendorId);
//				return null;
//			}
//		}
//	}
//}

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
using Resources;
using Serilog;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.Review
{
    public class VendorReviewService : BaseService<TbVendorReview, VendorReviewDto>, IVendorReviewService
    {
        private readonly IVendorReviewRepository _reviewRepo;
        private readonly ITableRepository<TbVendorReview> _tableRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public VendorReviewService(
            IBaseMapper mapper,
            IVendorReviewRepository reviewRepo,
            ILogger logger,
            ITableRepository<TbVendorReview> tableRepository)
            : base(tableRepository, mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _reviewRepo = reviewRepo;
            _tableRepository = tableRepository;
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
                var review = await _reviewRepo.GetReviewDetailsAsync(reviewId, cancellationToken);

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
            Guid customerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Validation
                if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
                    throw new ArgumentException("Rating must be between 1 and 5");

                if (string.IsNullOrWhiteSpace(reviewDto.ReviewText))
                    throw new ArgumentException("Review text is required");

                // Check if customer already reviewed this vendor
                var hasReviewed = await _reviewRepo.HasCustomerReviewedVendorAsync(
                    reviewDto.CustomerId,
                    reviewDto.VendorId,
                    cancellationToken);

                if (hasReviewed)
                    throw new InvalidOperationException("You have already reviewed this Vendor");

                // Create review
                var review = _mapper.MapModel<VendorReviewDto, TbVendorReview>(reviewDto);
                review.Status = ReviewStatus.Pending;
                review.IsEdited = false;
                var result = await _reviewRepo.CreateAsync(review, customerId, cancellationToken);

                if (!result.Success)
                    throw new Exception("Failed to submit review");

                review.Id = result.Id;
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
                var review = await _reviewRepo.FindByIdAsync(reviewDto.Id, cancellationToken);

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

                var result = await _reviewRepo.UpdateAsync(review, currentUserId, cancellationToken);

                if (!result.Success)
                    throw new Exception("Failed to update review");

                return _mapper.MapModel<TbVendorReview, VendorReviewDto>(review);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(UpdateReviewAsync)}");
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
                if (review.CustomerId != currentUserId)
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
                var reviews = await _reviewRepo.GetReviewsByVendorIdAsync(vendorId, cancellationToken);
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
                var reviews = await _reviewRepo.GetPendingReviewsAsync(cancellationToken);
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
                return await _reviewRepo.GetVendorAverageRatingAsync(vendorId, cancellationToken);
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
                return await _reviewRepo.GetVendorReviewCountAsync(vendorId, cancellationToken);
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
            var averageRating = await _reviewRepo.GetVendorAverageRatingAsync(vendorId, cancellationToken);
            var reviewCount = await _reviewRepo.GetVendorReviewCountAsync(vendorId, cancellationToken);
            var ratingDistribution = await _reviewRepo.GetVendorRatingDistributionAsync(vendorId, cancellationToken);

            var stats = new VendorReviewStatsDto
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
                var reviews = await _reviewRepo.GetVendorReviewsAsync(vendorId, status, cancellationToken);
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
                var reviews = await _reviewRepo.GetCustomerReviewsAsync(customerId, cancellationToken);
                return _mapper.MapList<TbVendorReview, VendorReviewDto>(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in {nameof(GetCustomerReviewsAsync)}");
                throw;
            }
        }
    }
}