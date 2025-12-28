using BL.Contracts.Service.Base;
using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Review;

	public interface IOfferReviewService : IBaseService<TbOfferReview, OfferReviewDto>
	{
		Task<OfferReviewDto?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Submits a new review for a specific offer.
		/// </summary>
		/// <param name="reviewDto">The review data to be submitted, including rating and comments.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The created review data.</returns>
		Task<OfferReviewDto> SubmitReviewAsync(OfferReviewDto reviewDto, CancellationToken cancellationToken = default);


		/// <summary>
		/// Updates an existing review submitted by the current user.
		/// </summary>
		/// <param name="reviewDto">Updated review information.</param>
		/// <param name="currentUserId">The ID of the user making the update request. Used to verify ownership.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The updated review data.</returns>
		Task<OfferReviewDto> updateReviewAsync(OfferReviewDto reviewDto, Guid currentUserId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Deletes a review if it belongs to the requesting user.
		/// </summary>
		/// <param name="reviewId">The unique identifier of the review to delete.</param>
		/// <param name="currentUserId">The ID of the user performing the deletion (owner validation).</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>True if deletion was successful; otherwise false.</returns>
		Task<bool> DeleteReviewAsync(Guid reviewId, Guid currentUserId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves all approved reviews for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A list of reviews belonging to the specified offer.</returns>
		Task<IEnumerable<OfferReviewDto>> GetReviewsByOfferIdAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves a paginated list of offer reviews with optional filtering by offer and review status.
		/// </summary>
		/// <param name="OfferId">Optional: Filter by offer ID.</param>
		/// <param name="status">Optional: Filter by review status (Pending, Approved, Rejected).</param>
		/// <param name="pageNumber">Page number (default: 1).</param>
		/// <param name="pageSize">Number of items per page (default: 10).</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A paginated data model containing filtered reviews.</returns>
		Task<PagedResult<OfferReviewDto>> GetPaginatedReviewsAsync(OfferReviewSearchCriteriaModel criteriaModel, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves all pending reviews that require admin approval.
		/// </summary>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>A list of pending reviews.</returns>
		Task<IEnumerable<OfferReviewDto>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);


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
		/// Calculates the average rating for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The average rating value.</returns>
		Task<decimal> GetAverageRatingAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Counts the total number of approved reviews for a specific offer.
		/// </summary>
		/// <param name="OfferId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>The number of reviews.</returns>
		Task<int> GetReviewCountAsync(Guid OfferId, CancellationToken cancellationToken = default);


		/// <summary>
		/// Retrieves combined statistics for a specific offer, including average rating and total review count.
		/// </summary>
		/// <param name="offerId">The ID of the offer.</param>
		/// <param name="cancellationToken">Token to cancel the operation.</param>
		/// <returns>An object containing review statistics such as average rating and review count.</returns>
		Task<OfferReviewStatsDto> GetOfferReviewStatsAsync(
		   Guid offerId,
		   CancellationToken cancellationToken = default);

	}
