using Common.Enumerations.Review;
using Dashboard.Models.pagintion;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Review
{
	public interface IItemReviewService
	{
        /// <summary>
        /// Search reviews with pagination and filters
        /// </summary>
        Task<ResponseModel<PaginatedDataModel<ItemReviewResponseDto>>> SearchReviewsAsync(ItemReviewSearchCriteriaModel criteria);

		/// <summary>
		/// Get review by ID
		/// </summary>
		Task<ResponseModel<ItemReviewResponseDto>> GetReviewByIdAsync(Guid reviewId);

		/// <summary>
		/// Get Item review statistics
		/// </summary>
		Task<ResponseModel<ResponseItemReviewSummeryDto>> GetItemReviewSummeryAsync(Guid itemId);
        
		Task<ResponseModel<bool>> ChangeReviewStatusAsync(Guid reviewId, ReviewStatus newStatus);
		
		/// <summary>
		/// Delete a review
		/// </summary>
		Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId);
	}
}