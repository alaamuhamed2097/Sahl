using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Review
{
	public interface IOfferReviewRepository : ITableRepository<TbOfferReview>
	{

		/// <summary>
		/// Retrieves all approved reviews for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A collection of TbOfferReview entries.</returns>
		Task<IEnumerable<TbOfferReview>> GetReviewsByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves the full details of a specific review.
		/// </summary>
		/// <param name="reviewId">The ID of the review.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The review entity if found; otherwise null.</returns>
		Task<TbOfferReview?> GetReviewDetailsAsync(Guid reviewId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves the review created by a specific customer for a specific order item.
		/// </summary>
		/// <param name="orderItemId">The ID of the order item.</param>
		/// <param name="customerId">The ID of the customer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The corresponding review if exists; otherwise null.</returns>
		Task<TbOfferReview?> GetCustomerReviewForOrderItemAsync(Guid orderItemId, Guid customerId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Calculates the average rating for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The average rating value.</returns>
		Task<decimal> GetAverageRatingAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves a paginated list of reviews with optional filtering by offer and status.
		/// </summary>
		/// <param name="OfferId">Optional filter: offer id.</param>
		/// <param name="status">Optional filter: review status.</param>
		/// <param name="pageNumber">Page number (default: 1).</param>
		/// <param name="pageSize">Page size (default: 10).</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A paginated model containing reviews and metadata.</returns>
		//Task<PaginatedDataModel<TbOfferReview>> GetPaginatedReviewsAsync(
		//	Guid? OfferId = null,
		//	ReviewStatus? status = null,
		//	int pageNumber = 1,
		//	int pageSize = 10,
		//	CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves all reviews pending moderation.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A collection of pending reviews.</returns>
		Task<IEnumerable<TbOfferReview>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);


		/// <summary>
		/// Counts the number of approved reviews for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>Total number of reviews for the offer.</returns>
		Task<int> GetReviewCountByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves the distribution of ratings (1–5 stars) for a specific product.
		/// </summary>
		/// <param name="productId">The ID of the product.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A dictionary where key = rating value, value = count.</returns>
		Task<Dictionary<int, int>> GetRatingDistributionAsync(Guid productId, CancellationToken cancellationToken = default);


	}
}
