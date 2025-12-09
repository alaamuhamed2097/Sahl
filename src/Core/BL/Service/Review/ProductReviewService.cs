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
   

    public class ProductReviewService : BaseService<TbProductReview, ProductReviewDto>, IProductReviewService
    {
		private readonly IProductReviewRepository _reviewRepo;
		private readonly IBaseMapper _mapper;
		private readonly ILogger _logger;
		public ProductReviewService(
			IBaseMapper mapper,
			ITableRepository<TbProductReview> repository,
			IProductReviewRepository reviewRepo,
			ILogger logger)
			: base(repository, mapper)
		{
			_mapper = mapper;
			_logger = logger;
			_reviewRepo = reviewRepo;
		}

		//public ProductReviewService(
		//	IProductReviewRepository reviewRepo,
		//	IMapper mapper,
		//	ILogger logger)
		//{
		//	_reviewRepo = reviewRepo;
		//	_mapper = mapper;
		//	_logger = logger;
		//}

		public async Task<ProductReviewDto> SubmitReviewAsync(
			ProductReviewDto reviewDto,
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
						throw new InvalidOperationException("You have already reviewed this product");

					reviewDto.IsVerifiedPurchase = true;
				}

				// Create review
				var review = _mapper.MapModel<ProductReviewDto,TbProductReview >(reviewDto);
				review.Status = ReviewStatus.Pending;

				var result = await _reviewRepo.CreateAsync(review, reviewDto.CustomerID, cancellationToken);

				if (!result.Success)
					throw new Exception("Failed to submit review");

				review.Id = result.Id;
				return _mapper.MapModel<TbProductReview, ProductReviewDto>(review);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(SubmitReviewAsync)}");
				throw;
			}
		}

		public async Task<ProductReviewDto> EditReviewAsync(
			Guid reviewId,
			ProductReviewDto reviewDto,
			Guid currentUserId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);

				if (review == null)
					throw new NotFoundException($"Review with ID {reviewId} not found.", _logger);

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

				return _mapper.MapModel<TbProductReview,ProductReviewDto>(review);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(EditReviewAsync)}");
				throw;
			}
		}

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

		public async Task<IEnumerable<ProductReviewDto>> GetReviewsByProductIdAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _reviewRepo.GetReviewsByProductIdAsync(productId, cancellationToken);
				return _mapper.MapList<TbProductReview,ProductReviewDto> (reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReviewsByProductIdAsync)}");
				throw;
			}
		}

		public async Task<PaginatedDataModel<ProductReviewDto>> GetPaginatedReviewsAsync(
			Guid? productId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var result = await _reviewRepo.GetPaginatedReviewsAsync(
					productId, status, pageNumber, pageSize, cancellationToken);

				var reviewDtos = _mapper.MapList<TbProductReview,ProductReviewDto>(result.Items);
				return new PaginatedDataModel<ProductReviewDto>(reviewDtos, result.TotalRecords);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetPaginatedReviewsAsync)}");
				throw;
			}
		}

		public async Task<IEnumerable<ProductReviewDto>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _reviewRepo.GetPendingReviewsAsync(cancellationToken);
				return _mapper.MapList< TbProductReview,ProductReviewDto > (reviews);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetPendingReviewsAsync)}");
				throw;
			}
		}

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

		public async Task<decimal> GetAverageRatingAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _reviewRepo.GetAverageRatingAsync(productId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetAverageRatingAsync)}");
				throw;
			}
		}

		public async Task<int> GetReviewCountAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _reviewRepo.GetReviewCountByProductIdAsync(productId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReviewCountAsync)}");
				throw;
			}
		}

		public override bool Equals(object? obj)
		{
			return obj is ProductReviewService service &&
				   EqualityComparer<IBaseMapper>.Default.Equals(_mapper, service._mapper);
		}
	}
}
