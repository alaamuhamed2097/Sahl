using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Dashboard.Models.pagintion;

namespace Dashboard.Contracts.Review
{
	public interface IItemReviewService
	{
		/// <summary>
		/// Update an existing review
		/// </summary>
		Task<ResponseModel<ResponseItemReviewDto>> UpdateReviewAsync(ItemReviewDto reviewDto);

		/// <summary>
		/// Get review by ID
		/// </summary>
		Task<ResponseModel<ResponseItemReviewDto>> GetReviewByIdAsync(Guid reviewId);

		/// <summary>
		/// Delete a review
		/// </summary>
		Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId);

		/// <summary>
		/// Get all reviews for a specific Item
		/// </summary>
		Task<ResponseModel<IEnumerable<ResponseItemReviewDto>>> GetReviewsByItemIdAsync(Guid itemId);

		/// <summary>
		/// Search reviews with pagination and filters
		/// </summary>
		Task<ResponseModel<PaginatedDataModel<ResponseItemReviewDto>>> SearchReviewsAsync(ItemReviewSearchCriteriaModel criteria);

		/// <summary>
		/// Get Item review statistics
		/// </summary>
		Task<ResponseModel<ResponseItemReviewStatsDto>> GetItemReviewStatsAsync(Guid itemId);

		/// <summary>
		/// Get pending reviews for moderation
		/// </summary>
		Task<ResponseModel<IEnumerable<ResponseItemReviewDto>>> GetPendingReviewsAsync();

		/// <summary>
		/// Approve a review
		/// </summary>
		Task<ResponseModel<bool>> ApproveReviewAsync(Guid reviewId);

		/// <summary>
		/// Reject a review
		/// </summary>
		Task<ResponseModel<bool>> RejectReviewAsync(Guid reviewId);
	}
}