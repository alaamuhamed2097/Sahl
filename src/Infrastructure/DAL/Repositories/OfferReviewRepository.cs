using Common.Enumerations.Review;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
	public class OfferReviewRepository : TableRepository<TbOfferReview>, IOfferReviewRepository
	{
		public OfferReviewRepository(ApplicationDbContext dbContext, ILogger logger)
			: base(dbContext, logger) { }

		/// <summary>
		/// Retrieves all approved (visible) reviews for a given offer.
		/// Filters by OfferId, ensures the review is approved and not soft-deleted.
		/// </summary>
		/// <param name="OfferId">ID of the offer to retrieve reviews for.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A list of approved reviews sorted by newest first.</returns>
		public async Task<IEnumerable<TbOfferReview>> GetReviewsByOfferIdAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.Where(r => r.OfferID == OfferId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted)
					.OrderByDescending(r => r.CreatedDateUtc)
					.ToListAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewsByOfferIdAsync),
					$"Error occurred while retrieving reviews for Offer {OfferId}.", ex);
				return new List<TbOfferReview>();
			}
		}

		/// <summary>
		/// Retrieves full details of a specific review including related votes and reports.
		/// Only includes records that are not soft-deleted.
		/// </summary>
		/// <param name="reviewId">ID of the review.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The review with related data, or null if not found.</returns>
		public async Task<TbOfferReview?> GetReviewDetailsAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.Include(r => r.ReviewVotes.Where(v => !v.IsDeleted))
					.Include(r => r.ReviewReports.Where(rr => !rr.IsDeleted))
					.FirstOrDefaultAsync(
						r => r.Id == reviewId && !r.IsDeleted,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewDetailsAsync),
					$"Error occurred while retrieving review details for ID {reviewId}.", ex);
				return null;
			}
		}
		/// <summary>
		/// Retrieves a customer's review for a specific order item.
		/// Useful for preventing duplicate reviews for the same purchased item.
		/// </summary>
		/// <param name="orderItemId">Order item ID.</param>
		/// <param name="customerId">Customer ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The review if found; otherwise null.</returns>
		public async Task<TbOfferReview?> GetCustomerReviewForOrderItemAsync(
			Guid orderItemId,
			Guid customerId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.FirstOrDefaultAsync(r => r.OrderItemID == orderItemId
						&& r.CustomerID == customerId
						&& !r.IsDeleted,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetCustomerReviewForOrderItemAsync),
					$"Error occurred while checking customer review for order item.", ex);
				return null;
			}
		}
		/// <summary>
		/// Calculates the average rating for a specific offer
		/// considering only approved, non-deleted reviews.
		/// </summary>
		/// <param name="OfferId">Offer ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The average rating, or 0 if no reviews exist.</returns>
		public async Task<decimal> GetAverageRatingAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.Where(r => r.OfferID == OfferId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted)
					.Select(r => r.Rating)
					.ToListAsync(cancellationToken);

				return reviews.Any() ? reviews.Average() : 0;
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetAverageRatingAsync),
					$"Error occurred while calculating average rating for Offer {OfferId}.", ex);
				return 0;
			}
		}
		
		/// <summary>
		/// Retrieves paginated list of reviews with optional filters (OfferId and Status).
		/// Useful for admin panels and customer history views.
		/// </summary>
		/// <param name="OfferId">Optional filter by offer.</param>
		/// <param name="status">Optional filter by review status.</param>
		/// <param name="pageNumber">Page index starting from 1.</param>
		/// <param name="pageSize">Number of items per page.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A paginated data model wrapping reviews and total count.</returns>
		//public async Task<PaginatedDataModel<TbOfferReview>> GetPaginatedReviewsAsync(
		//	Guid? OfferId = null,
		//	ReviewStatus? status = null,
		//	int pageNumber = 1,
		//	int pageSize = 10,
		//	CancellationToken cancellationToken = default)
		//{
		//	try
		//	{
		//		ValidatePaginationParameters(pageNumber, pageSize);

		//		var query = _dbContext.Set<TbOfferReview>()
		//			.AsNoTracking()
		//			.Where(r => !r.IsDeleted);

		//		if (OfferId.HasValue)
		//			query = query.Where(r => r.OfferID == OfferId.Value);

		//		if (status.HasValue)
		//			query = query.Where(r => r.Status == status.Value);

		//		var totalCount = await query.CountAsync(cancellationToken);

		//		var reviews = await query
		//			.OrderByDescending(r => r.CreatedDateUtc)
		//			.Skip((pageNumber - 1) * pageSize)
		//			.Take(pageSize)
		//			.ToListAsync(cancellationToken);

		//		return new PaginatedDataModel<TbOfferReview>(reviews, totalCount);
		//	}
		//	catch (Exception ex)
		//	{
		//		HandleException(nameof(GetPaginatedReviewsAsync),
		//			$"Error occurred while retrieving paginated reviews.", ex);
		//		return new PaginatedDataModel<TbOfferReview>(new List<TbOfferReview>(), 0);
		//	}
		//}


		/// <summary>
		/// Retrieves all reviews currently pending admin approval.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A list of pending reviews sorted by creation date.</returns>
		public async Task<IEnumerable<TbOfferReview>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.Where(r => r.Status == ReviewStatus.Pending
						&& !r.IsDeleted)
					.OrderBy(r => r.CreatedDateUtc)
					.ToListAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetPendingReviewsAsync),
					$"Error occurred while retrieving pending reviews.", ex);
				return new List<TbOfferReview>();
			}
		}



		/// <summary>
		/// Counts the number of approved, non-deleted reviews for a given offer.
		/// Used for statistics and rating calculations.
		/// </summary>
		/// <param name="OfferId">Offer ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The count of approved reviews.</returns>
		public async Task<int> GetReviewCountByOfferIdAsync(
			Guid OfferId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.CountAsync(r => r.OfferID == OfferId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewCountByOfferIdAsync),
					$"Error occurred while counting reviews for Offer {OfferId}.", ex);
				return 0;
			}
		}


		/// <summary>
		/// Returns the distribution of ratings (1-5 stars) for a specific offer.
		/// Groups ratings and returns a dictionary: Key = rating, Value = count.
		/// </summary>
		/// <param name="OfferId">Offer ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Dictionary of rating value and count.</returns>
		public async Task<Dictionary<int, int>> GetRatingDistributionAsync(
		   Guid OfferId,
		   CancellationToken cancellationToken = default)
		{
			var reviews = await _dbContext.Set<TbOfferReview>()
				.AsNoTracking()
				.Where(r => r.OfferID == OfferId
					&& r.Status == ReviewStatus.Approved
					&& !r.IsDeleted)
				.Select(r => r.Rating)
				.ToListAsync(cancellationToken);

			return reviews
				.GroupBy(r => (int)Math.Round(r))
				.ToDictionary(g => g.Key, g => g.Count());
		}


	}
}
