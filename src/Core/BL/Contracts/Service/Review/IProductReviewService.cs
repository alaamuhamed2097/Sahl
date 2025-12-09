using BL.Contracts.Service.Base;
using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Review
{
	public interface IProductReviewService : IBaseService<TbProductReview, ProductReviewDto>
	{
		Task<ProductReviewDto> SubmitReviewAsync(ProductReviewDto reviewDto, CancellationToken cancellationToken = default);
		Task<ProductReviewDto> EditReviewAsync(Guid reviewId, ProductReviewDto reviewDto, Guid currentUserId, CancellationToken cancellationToken = default);
		Task<bool> DeleteReviewAsync(Guid reviewId, Guid currentUserId, CancellationToken cancellationToken = default);
		Task<IEnumerable<ProductReviewDto>> GetReviewsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
		Task<PaginatedDataModel<ProductReviewDto>> GetPaginatedReviewsAsync(
			Guid? productId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<ProductReviewDto>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
		Task<bool> ApproveReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);
		Task<bool> RejectReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);
		Task<decimal> GetAverageRatingAsync(Guid productId, CancellationToken cancellationToken = default);
		Task<int> GetReviewCountAsync(Guid productId, CancellationToken cancellationToken = default);
	}
}
