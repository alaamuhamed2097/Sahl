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
	public interface IOfferReviewService : IBaseService<TbOfferReview, OfferReviewDto>
	{
		Task<OfferReviewDto> SubmitReviewAsync(OfferReviewDto reviewDto, CancellationToken cancellationToken = default);
		Task<OfferReviewDto> EditReviewAsync(Guid reviewId, OfferReviewDto reviewDto, Guid currentUserId, CancellationToken cancellationToken = default);
		Task<bool> DeleteReviewAsync(Guid reviewId, Guid currentUserId, CancellationToken cancellationToken = default);
		Task<IEnumerable<OfferReviewDto>> GetReviewsByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);
		Task<PaginatedDataModel<OfferReviewDto>> GetPaginatedReviewsAsync(
			Guid? OfferId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<OfferReviewDto>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
		Task<bool> ApproveReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);
		Task<bool> RejectReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);
		Task<decimal> GetAverageRatingAsync(Guid OfferId, CancellationToken cancellationToken = default);
		Task<int> GetReviewCountAsync(Guid OfferId, CancellationToken cancellationToken = default);
	}
}
