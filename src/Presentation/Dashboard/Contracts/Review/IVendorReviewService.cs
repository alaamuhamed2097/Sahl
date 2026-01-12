namespace Dashboard.Contracts.Review
{
	using Common.Enumerations.Review;
	using Dashboard.Models.pagintion;
	using Shared.DTOs.Review;
	using Shared.GeneralModels;
	using Shared.GeneralModels.SearchCriteriaModels;


		public interface IVendorReviewService
		{
			/// <summary>
			/// Get review by ID with full details
			/// </summary>
			Task<ResponseModel<VendorReviewDto>> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get all approved reviews for a vendor
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetReviewsByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Search vendor reviews with pagination and filters
		/// </summary>
		Task<ResponseModel<PaginatedDataModel<VendorReviewDto>>> SearchVendorReviews(VendorReviewSearchCriteriaModel criteria, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get vendor review statistics
			/// </summary>
			Task<ResponseModel<VendorReviewStatsDto>> GetVendorReviewStatsAsync(Guid vendorId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get pending reviews for moderation
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);

			/// <summary>
			/// Approve a review
			/// </summary>
			Task<ResponseModel<bool>> ApproveReviewAsync(Guid reviewId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Reject a review
			/// </summary>
			Task<ResponseModel<bool>> RejectReviewAsync(Guid reviewId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Delete a review
			/// </summary>
			Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get all reviews for a vendor with optional status filter
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetVendorReviewsAsync(Guid vendorId, ReviewStatus? status = null, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get all reviews created by a specific customer
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetCustomerReviewsAsync(Guid customerId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get average rating for a vendor
			/// </summary>
			Task<ResponseModel<decimal>> GetAverageRatingAsync(Guid vendorId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get review count for a vendor
			/// </summary>
			Task<ResponseModel<int>> GetReviewCountAsync(Guid vendorId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get verified purchase reviews for a vendor
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetVerifiedReviewsAsync(Guid vendorId, CancellationToken cancellationToken = default);

			/// <summary>
			/// Get non-verified purchase reviews for a vendor
			/// </summary>
			Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetNonVerifiedReviewsAsync(Guid vendorId, CancellationToken cancellationToken = default);
		}
	}
