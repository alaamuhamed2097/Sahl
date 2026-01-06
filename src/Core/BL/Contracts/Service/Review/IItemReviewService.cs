using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Review
{
    public interface IItemReviewService : IBaseService<TbItemReview, ItemReviewDto>
    {
        Task<ItemReviewDto?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a new review for a specific Item.
        /// </summary>
        /// <param name="reviewDto">The review data to be submitted, including rating and comments.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The created review data.</returns>
        Task<ResponseItemReviewDto> SubmitReviewAsync(ItemReviewDto reviewDto, Guid creatorId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Updates an existing review submitted by the current user.
        /// </summary>
        /// <param name="reviewDto">Updated review information.</param>
        /// <param name="currentUserId">The ID of the user making the update request. Used to verify ownership.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The updated review data.</returns>
        Task<ResponseItemReviewDto> updateReviewAsync(ItemReviewDto reviewDto, Guid currentUserId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Deletes a review if it belongs to the requesting user.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to delete.</param>
        /// <param name="currentUserId">The ID of the user performing the deletion (owner validation).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if deletion was successful; otherwise false.</returns>
        Task<bool> DeleteReviewAsync(Guid reviewId, Guid currentUserId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves all approved reviews for a specific Item.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of reviews belonging to the specified Item.</returns>
        Task<IEnumerable<ResponseItemReviewDto>> GetReviewsByItemIdAsync(Guid ItemId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves a paginated list of Item reviews with optional filtering by Item and review status.
        /// </summary>
        /// <param name="ItemId">Optional: Filter by Item ID.</param>
        /// <param name="status">Optional: Filter by review status (Pending, Approved, Rejected).</param>
        /// <param name="pageNumber">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A paginated data model containing filtered reviews.</returns>
        Task<PagedResult<ResponseItemReviewDto>> GetPaginatedReviewsAsync(ItemReviewSearchCriteriaModel criteriaModel, CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves all pending reviews that require admin approval.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of pending reviews.</returns>
        Task<IEnumerable<ResponseItemReviewDto>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Approves a review, making it publicly visible.
        /// </summary>
        /// <param name="reviewId">The ID of the review to approve.</param>
        /// <param name="adminId">The ID of the admin performing the action.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if approval was successful; otherwise false.</returns>
        Task<bool> ApproveReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Rejects a review and keeps it hidden from public display.
        /// </summary>
        /// <param name="reviewId">The ID of the review to reject.</param>
        /// <param name="adminId">The ID of the admin performing the rejection.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if rejection was successful; otherwise false.</returns>
        Task<bool> RejectReviewAsync(Guid reviewId, Guid adminId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Calculates the average rating for a specific Item.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The average rating value.</returns>
        Task<decimal> GetAverageRatingAsync(Guid ItemId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Counts the total number of approved reviews for a specific Item.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The number of reviews.</returns>
        Task<int> GetReviewCountAsync(Guid ItemId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves combined statistics for a specific Item, including average rating and total review count.
        /// </summary>
        /// <param name="ItemId">The ID of the Item.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An object containing review statistics such as average rating and review count.</returns>
        Task<ResponseItemReviewStatsDto> GetItemReviewStatsAsync(
           Guid ItemId,
           CancellationToken cancellationToken = default);

    }
}