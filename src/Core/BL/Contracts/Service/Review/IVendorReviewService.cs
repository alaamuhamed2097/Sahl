using BL.Contracts.Service.Base;
using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Review
{
	public interface IVendorReviewService : IBaseService<TbVendorReview, VendorReviewDto>
	{
		
			/// <summary>
			/// Retrieves the details of a specific Vendor review by its unique identifier.
			/// </summary>
			/// <param name="reviewId">The unique identifier of the review.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>Review DTO if found; otherwise null.</returns>
			Task<VendorReviewDto?> GetReviewByIdAsync(
				Guid reviewId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Creates a new vendor review. Validates that the customer hasn't already reviewed the vendor.
			/// </summary>
			/// <param name="reviewDto">Review data transfer object.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>The created review DTO.</returns>
			Task<VendorReviewDto> SubmitReviewAsync(
				VendorReviewDto reviewDto,
				Guid customerId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Updates an existing vendor review. Verifies ownership before updating.
			/// </summary>
			/// <param name="reviewDto">Updated review data.</param>
			/// <param name="currentUserId">ID of the user attempting the update.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>The updated review DTO.</returns>
			Task<VendorReviewDto> UpdateReviewAsync(
				VendorReviewDto reviewDto,
				Guid currentUserId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Soft deletes a vendor review. Verifies ownership before deletion.
			/// </summary>
			/// <param name="reviewId">Review ID.</param>
			/// <param name="currentUserId">ID of the user attempting deletion.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>True if deleted successfully; otherwise false.</returns>
			Task<bool> DeleteReviewAsync(
				Guid reviewId,
				Guid currentUserId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves all approved reviews for a specific vendor.
			/// </summary>
			/// <param name="vendorId">Vendor identifier.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>List of approved vendor reviews.</returns>
			Task<IEnumerable<VendorReviewDto>> GetReviewsByVendorIdAsync(
				Guid vendorId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves a paginated list of vendor reviews with advanced filtering and sorting.
			/// </summary>
			/// <param name="criteriaModel">Search criteria including filters, sorting, and pagination.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>Paginated result containing reviews and total count.</returns>
			Task<PagedResult<VendorReviewDto>> GetPaginatedReviewsAsync(
				VendorReviewSearchCriteriaModel criteriaModel,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves all vendor reviews with optional status filter.
			/// </summary>
			/// <param name="vendorId">Vendor ID.</param>
			/// <param name="status">Optional review status filter.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>List of vendor reviews.</returns>
			Task<IEnumerable<VendorReviewDto>> GetVendorReviewsAsync(
				Guid vendorId,
				ReviewStatus? status = null,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves all reviews created by a specific customer.
			/// </summary>
			/// <param name="customerId">Customer ID.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>List of customer reviews.</returns>
			Task<IEnumerable<VendorReviewDto>> GetCustomerReviewsAsync(
				Guid customerId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves all reviews currently pending approval.
			/// </summary>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>List of pending reviews.</returns>
			Task<IEnumerable<VendorReviewDto>> GetPendingReviewsAsync(
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Approves a pending review.
			/// </summary>
			/// <param name="reviewId">Review ID.</param>
			/// <param name="adminId">Admin ID performing the approval.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>True if approved successfully; otherwise false.</returns>
			Task<bool> ApproveReviewAsync(
				Guid reviewId,
				Guid adminId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Rejects a pending review.
			/// </summary>
			/// <param name="reviewId">Review ID.</param>
			/// <param name="adminId">Admin ID performing the rejection.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>True if rejected successfully; otherwise false.</returns>
			Task<bool> RejectReviewAsync(
				Guid reviewId,
				Guid adminId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Calculates the average rating for a vendor based on approved reviews.
			/// </summary>
			/// <param name="vendorId">Vendor identifier.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>Average rating (0.0 to 5.0).</returns>
			Task<decimal> GetAverageRatingAsync(
				Guid vendorId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Counts the total number of approved reviews for a vendor.
			/// </summary>
			/// <param name="vendorId">Vendor identifier.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>Total review count.</returns>
			Task<int> GetReviewCountAsync(
				Guid vendorId,
				CancellationToken cancellationToken = default);

			/// <summary>
			/// Retrieves comprehensive rating statistics for a vendor.
			/// Includes average rating, total count, and rating distribution with percentages.
			/// </summary>
			/// <param name="vendorId">Vendor identifier.</param>
			/// <param name="cancellationToken">Cancellation token.</param>
			/// <returns>Vendor review statistics DTO.</returns>
			Task<VendorReviewStatsDto> GetVendorReviewStatsAsync(
				Guid vendorId,
				CancellationToken cancellationToken = default);
		}
	}
	
