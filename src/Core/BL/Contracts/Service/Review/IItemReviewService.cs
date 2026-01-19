using BL.Contracts.Service.Base;
using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Review
{
    public interface IItemReviewService : IBaseService<TbItemReview, ItemReviewDto>
    {
        /// <summary>
        /// Retrieves a paginated list of Item reviews with optional filtering by Item and review status.
        /// </summary>
        /// <param name="ItemId">Optional: Filter by Item ID.</param>
        /// <param name="status">Optional: Filter by review status (Pending, Approved, Rejected).</param>
        /// <param name="pageNumber">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A paginated data model containing filtered reviews.</returns>
        Task<PagedResult<ItemReviewResponseDto>> GetPageAsync(
            ItemReviewSearchCriteriaModel criteriaModel, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to retrieve.</param
        Task<ItemReviewResponseDto> FindReviewByIdAsync(
            Guid reviewId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates the average rating for a specific Item.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The average rating value.</returns>
        Task<decimal> CalculateItemAverageRatingAsync(
            Guid ItemId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves combined statistics for a specific Item, including average rating and total review count.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An object containing review statistics such as average rating and review count.</returns>
        Task<ResponseItemReviewSummeryDto> GetItemReviewSummeryAsync(
           Guid ItemId,
           CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new review for a specific Item.
        /// </summary>
        /// <param name="reviewDto">The review data to be submitted, including rating and comments.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The created review data.</returns>
        Task<ItemReviewResponseDto> CreateReviewAsync(
            ItemReviewDto reviewDto, 
            Guid creatorId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing review submitted by the current user.
        /// </summary>
        /// <param name="reviewDto">Updated review information.</param>
        /// <param name="currentUserId">The ID of the user making the update request. Used to verify ownership.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The updated review data.</returns>
        Task<ItemReviewResponseDto> UpdateReviewAsync(
            ItemReviewDto reviewDto, 
            Guid currentUserId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Approves a review, making it publicly visible.
        /// </summary>
        /// <param name="reviewId">The ID of the review to approve.</param>
        /// <param name="adminId">The ID of the admin performing the action.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if approval was successful; otherwise false.</returns>
        Task<bool> ChangeReviewStatus(
            Guid reviewId, 
            ReviewStatus reviewStatus,
            string adminId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a review if it belongs to the requesting user.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to delete.</param>
        /// <param name="currentUserId">The ID of the user performing the deletion (owner validation).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if deletion was successful; otherwise false.</returns>
        Task<bool> DeleteReviewAsync(
            Guid reviewId, 
            Guid currentUserId, 
            bool isAdmin = false, 
            CancellationToken cancellationToken = default);
    }
}