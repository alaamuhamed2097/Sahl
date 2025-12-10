using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories
{
	public interface IOfferReviewRepository : ITableRepository<TbOfferReview>
	{
		Task<IEnumerable<TbOfferReview>> GetReviewsByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);
		Task<TbOfferReview?> GetReviewDetailsAsync(Guid reviewId, CancellationToken cancellationToken = default);
		Task<TbOfferReview?> GetCustomerReviewForOrderItemAsync(Guid orderItemId, Guid customerId, CancellationToken cancellationToken = default);
		Task<decimal> GetAverageRatingAsync(Guid OfferId, CancellationToken cancellationToken = default);
		Task<PaginatedDataModel<TbOfferReview>> GetPaginatedReviewsAsync(
			Guid? OfferId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<TbOfferReview>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
		Task<int> GetReviewCountByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);


	}
}
