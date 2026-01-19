using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Review;

namespace DAL.Contracts.Repositories.Review
{
	public interface IItemReviewRepository : ITableRepository<TbItemReview>
	{
		/// <summary>
		/// Retrieves all approved reviews for a specific Item.
		/// </summary>
		/// <param name="ItemId">The ID of the Item.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A collection of TbItemReview entries.</returns>
		Task<IEnumerable<TbItemReview>> GetReviewsByItemIdAsync(
			Guid ItemId, 
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the full details of a specific review.
		/// </summary>
		/// <param name="reviewId">The ID of the review.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The review entity if found; otherwise null.</returns>
		Task<TbItemReview?> GetReviewDetailsAsync(
			Guid reviewId, 
			CancellationToken cancellationToken = default);
		
		/// <summary>
		/// Calculates the average rating for a specific Item.
		/// </summary>
		/// <param name="ItemId">The ID of the Item.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The average rating value.</returns>
		Task<decimal> GetAverageRatingAsync(
			Guid ItemId, 
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Counts the number of approved reviews for a specific Item.
		/// </summary>
		/// <param name="ItemId">The ID of the Item.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>Total number of reviews for the Item.</returns>
		Task<int> GetReviewCountByItemIdAsync(
			Guid ItemId, 
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the distribution of ratings (1–5 stars) for a specific product.
		/// </summary>
		/// <param name="productId">The ID of the product.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A dictionary where key = rating value, value = count.</returns>
		Task<Dictionary<int, int>> GetRatingDistributionAsync(
			Guid productId, 
			CancellationToken cancellationToken = default);

		Task<bool> UpdateReviewStatus(
			Guid reviewId, 
			ReviewStatus newStatus, 
			string adminId, 
			CancellationToken cancellationToken = default);
	}
}
