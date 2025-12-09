using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories
{
	public interface IProductReviewRepository : ITableRepository<TbProductReview>
	{
		Task<IEnumerable<TbProductReview>> GetReviewsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
		Task<TbProductReview?> GetReviewDetailsAsync(Guid reviewId, CancellationToken cancellationToken = default);
		Task<TbProductReview?> GetCustomerReviewForOrderItemAsync(Guid orderItemId, Guid customerId, CancellationToken cancellationToken = default);
		Task<decimal> GetAverageRatingAsync(Guid productId, CancellationToken cancellationToken = default);
		Task<PaginatedDataModel<TbProductReview>> GetPaginatedReviewsAsync(
			Guid? productId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<TbProductReview>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
		Task<int> GetReviewCountByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);


	}
}
