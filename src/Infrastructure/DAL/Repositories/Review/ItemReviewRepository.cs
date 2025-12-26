using Common.Enumerations.Review;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories.Review
{
	public class ItemReviewRepository : TableRepository<TbItemReview>, IItemReviewRepository
	{
		public ItemReviewRepository(ApplicationDbContext dbContext, ILogger logger)
			: base(dbContext, logger) { }

		/// <summary>
		/// Retrieves all approved (visible) reviews for a given Item.
		/// Filters by ItemId, ensures the review is approved and not soft-deleted.
		/// </summary>
		/// <param name="ItemId">ID of the Item to retrieve reviews for.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A list of approved reviews sorted by newest first.</returns>
		public async Task<IEnumerable<TbItemReview>> GetReviewsByItemIdAsync(
			Guid ItemId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbItemReview>()
					.AsNoTracking()
					.Where(r => r.ItemId == ItemId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted)
					.OrderByDescending(r => r.CreatedDateUtc)
					.ToListAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewsByItemIdAsync),
					$"Error occurred while retrieving reviews for Item {ItemId}.", ex);
				return new List<TbItemReview>();
			}
		}

		/// <summary>
		/// Retrieves full details of a specific review including related votes and reports.
		/// Only includes records that are not soft-deleted.
		/// </summary>
		/// <param name="reviewId">ID of the review.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The review with related data, or null if not found.</returns>
		public async Task<TbItemReview?> GetReviewDetailsAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbItemReview>()
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
		///// <summary>
		///// Retrieves a customer's review for a specific order item.
		///// Useful for preventing duplicate reviews for the same purchased item.
		///// </summary>
		///// <param name="orderItemId">Order item ID.</param>
		///// <param name="customerId">Customer ID.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>The review if found; otherwise null.</returns>
		//public async Task<TbItemReview?> GetCustomerReviewForOrderItemAsync(
			
		//	Guid customerId,
		//	CancellationToken cancellationToken = default)
		//{
		//	try
		//	{
		//		return await _dbContext.Set<TbItemReview>()
		//			.AsNoTracking()
		//			.FirstOrDefaultAsync(r => 
		//				 r.CustomerId == customerId
		//				&& !r.IsDeleted,
		//				cancellationToken);
		//	}
		//	catch (Exception ex)
		//	{
		//		HandleException(nameof(GetCustomerReviewForOrderItemAsync),
		//			$"Error occurred while checking customer review for order item.", ex);
		//		return null;
		//	}
		//}
		/// <summary>
		/// Calculates the average rating for a specific Item
		/// considering only approved, non-deleted reviews.
		/// </summary>
		/// <param name="ItemId">Item ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The average rating, or 0 if no reviews exist.</returns>
		public async Task<decimal> GetAverageRatingAsync(
			Guid ItemId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _dbContext.Set<TbItemReview>()
					.AsNoTracking()
					.Where(r => r.ItemId == ItemId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted)
					.Select(r => r.Rating)
					.ToListAsync(cancellationToken);

				return reviews.Any() ? reviews.Average() : 0;
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetAverageRatingAsync),
					$"Error occurred while calculating average rating for Item {ItemId}.", ex);
				return 0;
			}
		}
		
		/// <summary>
		/// Retrieves paginated list of reviews with optional filters (ItemId and Status).
		/// Useful for admin panels and customer history views.
		/// </summary>
		/// <param name="ItemId">Optional filter by Item.</param>
		/// <param name="status">Optional filter by review status.</param>
		/// <param name="pageNumber">Page index starting from 1.</param>
		/// <param name="pageSize">Number of items per page.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A paginated data model wrapping reviews and total count.</returns>
		

		/// <summary>
		/// Retrieves all reviews currently pending admin approval.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>A list of pending reviews sorted by creation date.</returns>
		public async Task<IEnumerable<TbItemReview>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbItemReview>()
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
				return new List<TbItemReview>();
			}
		}



		/// <summary>
		/// Counts the number of approved, non-deleted reviews for a given Item.
		/// Used for statistics and rating calculations.
		/// </summary>
		/// <param name="ItemId">Item ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The count of approved reviews.</returns>
		public async Task<int> GetReviewCountByItemIdAsync(
			Guid ItemId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbItemReview>()
					.AsNoTracking()
					.CountAsync(r => r.ItemId == ItemId
						&& r.Status == ReviewStatus.Approved
						&& !r.IsDeleted,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewCountByItemIdAsync),
					$"Error occurred while counting reviews for Item {ItemId}.", ex);
				return 0;
			}
		}


		/// <summary>
		/// Returns the distribution of ratings (1-5 stars) for a specific Item.
		/// Groups ratings and returns a dictionary: Key = rating, Value = count.
		/// </summary>
		/// <param name="ItemId">Item ID.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Dictionary of rating value and count.</returns>
		public async Task<Dictionary<int, int>> GetRatingDistributionAsync(
		   Guid ItemId,
		   CancellationToken cancellationToken = default)
		{
			var reviews = await _dbContext.Set<TbItemReview>()
				.AsNoTracking()
				.Where(r => r.ItemId == ItemId
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
