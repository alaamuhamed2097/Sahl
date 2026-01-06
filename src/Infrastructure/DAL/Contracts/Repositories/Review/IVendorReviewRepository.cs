using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Review
{
	public interface IVendorReviewRepository : ITableRepository<TbVendorReview>
	{
		//Task<TbVendorReview?> GetReviewByOrderDetailAsync(Guid orderDetailId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves detailed information for a specific review including related entities.
		/// </summary>
		/// <param name="reviewId">Review identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Review entity with related data if found; otherwise null.</returns>
		Task<TbVendorReview?> GetReviewDetailsAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all approved reviews for a specific vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of approved vendor reviews.</returns>
		Task<IEnumerable<TbVendorReview>> GetReviewsByVendorIdAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all reviews for a vendor with optional status filter.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="status">Optional review status filter (Pending, Approved, Rejected).</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of vendor reviews matching the criteria.</returns>
		Task<IEnumerable<TbVendorReview>> GetVendorReviewsAsync(
			Guid vendorId,
			ReviewStatus? status = null,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all reviews created by a specific customer.
		/// </summary>
		/// <param name="customerId">Customer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of customer's reviews.</returns>
		Task<IEnumerable<TbVendorReview>> GetCustomerReviewsAsync(
			Guid customerId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves all reviews with Pending status awaiting approval.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of pending reviews.</returns>
		Task<IEnumerable<TbVendorReview>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks if a customer has already reviewed a specific vendor.
		/// </summary>
		/// <param name="customerId">Customer identifier.</param>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>True if customer has already reviewed the vendor; otherwise false.</returns>
		Task<bool> HasCustomerReviewedVendorAsync(
			Guid customerId,
			Guid vendorId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Calculates the average rating for a vendor based on approved reviews only.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Average rating (0.0 to 5.0). Returns 0 if no approved reviews exist.</returns>
		Task<decimal> GetVendorAverageRatingAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Counts the total number of approved reviews for a vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Total count of approved reviews.</returns>
		Task<int> GetVendorReviewCountAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves the distribution of ratings (1-5 stars) for a vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// Dictionary with rating values (1-5) as keys and their counts as values.
		/// Example: { {5, 10}, {4, 5}, {3, 2}, {2, 1}, {1, 0} }
		/// </returns>
		Task<Dictionary<int, int>> GetVendorRatingDistributionAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default);
		Task<bool> HasCustomerPurchasedFromVendorAsync(
		Guid? orderId,
		Guid vendorId, CancellationToken cancellationToken = default);
		Task<decimal> GetAverageRatingAsync(
			Guid ItemId,
			CancellationToken cancellationToken = default);

		Task<IEnumerable<TbVendorReview>> GetVendorReviewsByVerificationAsync(
			Guid vendorId,
			bool? isVerifiedPurchase = null,
			CancellationToken cancellationToken = default);

		Task<bool> IsVerifiedPurchaseAsync(
string customerId,
Guid? orderDetailId,
CancellationToken cancellationToken = default);


	}
}

