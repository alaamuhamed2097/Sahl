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
	public class VendorReviewRepository : TableRepository<TbVendorReview>, IVendorReviewRepository
	{
		public VendorReviewRepository(ApplicationDbContext dbContext, ILogger logger)
			: base(dbContext, logger) { }

		/// <summary>
		/// Retrieves detailed information for a specific review including related entities.
		/// </summary>
		/// <param name="reviewId">Review identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Review entity with related data if found; otherwise null.</returns>
		public async Task<TbVendorReview?> GetReviewDetailsAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.Include(r => r.Customer)
				.Include(r => r.Vendor)
				.FirstOrDefaultAsync(r => r.Id == reviewId && !r.IsDeleted, cancellationToken);
		}

		/// <summary>
		/// Retrieves all approved reviews for a specific vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of approved vendor reviews.</returns>
		public async Task<IEnumerable<TbVendorReview>> GetReviewsByVendorIdAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.Include(r => r.Customer)
				.Where(r => r.VendorId == vendorId
					&& r.Status == ReviewStatus.Approved
					&& !r.IsDeleted)
				.OrderByDescending(r => r.CreatedDateUtc)
				.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Retrieves all reviews for a vendor with optional status filter.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="status">Optional review status filter.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of vendor reviews matching the criteria.</returns>
		public async Task<IEnumerable<TbVendorReview>> GetVendorReviewsAsync(
			Guid vendorId,
			ReviewStatus? status = null,
			CancellationToken cancellationToken = default)
		{
			var query = _dbContext.TbVendorReviews
				.Include(r => r.Customer)
				.Where(r => r.VendorId == vendorId && !r.IsDeleted);

			if (status.HasValue)
			{
				query = query.Where(r => r.Status == status.Value);
			}

			return await query
				.OrderByDescending(r => r.CreatedDateUtc)
				.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Retrieves all reviews created by a specific customer.
		/// </summary>
		/// <param name="customerId">Customer identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of customer's reviews.</returns>
		public async Task<IEnumerable<TbVendorReview>> GetCustomerReviewsAsync(
			Guid customerId,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.Include(r => r.Vendor)
				.Where(r => r.CustomerId == customerId && !r.IsDeleted)
				.OrderByDescending(r => r.CreatedDateUtc)
				.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Retrieves all reviews with Pending status awaiting approval.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Collection of pending reviews.</returns>
		public async Task<IEnumerable<TbVendorReview>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.Include(r => r.Customer)
				.Include(r => r.Vendor)
				.Where(r => r.Status == ReviewStatus.Pending && !r.IsDeleted)
				.OrderBy(r => r.CreatedDateUtc)
				.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Checks if a customer has already reviewed a specific vendor.
		/// </summary>
		/// <param name="customerId">Customer identifier.</param>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>True if customer has already reviewed the vendor; otherwise false.</returns>
		public async Task<bool> HasCustomerReviewedVendorAsync(
			Guid customerId,
			Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.AnyAsync(r => r.CustomerId == customerId
					&& r.VendorId == vendorId
					&& !r.IsDeleted,
					cancellationToken);
		}

		/// <summary>
		/// Calculates the average rating for a vendor based on approved reviews only.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Average rating (0.0 to 5.0). Returns 0 if no approved reviews exist.</returns>
		public async Task<decimal> GetVendorAverageRatingAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			var reviews = await _dbContext.TbVendorReviews
				.Where(r => r.VendorId == vendorId
					&& r.Status == ReviewStatus.Approved
					&& !r.IsDeleted)
				.ToListAsync(cancellationToken);

			if (!reviews.Any())
				return 0;

			return Math.Round((decimal)reviews.Average(r => r.Rating), 2);
		}

		/// <summary>
		/// Counts the total number of approved reviews for a vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>Total count of approved reviews.</returns>
		public async Task<int> GetVendorReviewCountAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default)
		{
			return await _dbContext.TbVendorReviews
				.CountAsync(r => r.VendorId == vendorId
					&& r.Status == ReviewStatus.Approved
					&& !r.IsDeleted,
					cancellationToken);
		}

		/// <summary>
		/// Retrieves a review by order detail ID.
		/// </summary>
		/// <param name="orderDetailId">ID of the OrderDetail.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>The review if found, null otherwise.</returns>
		public async Task<TbVendorReview?> GetReviewByOrderDetailAsync(
			Guid orderDetailId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbVendorReview>()
					.AsNoTracking()
					.Include(r => r.Customer)
					.FirstOrDefaultAsync(r => r.OrderDetailId == orderDetailId && !r.IsDeleted,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewByOrderDetailAsync),
					$"Error occurred while retrieving review for OrderDetail {orderDetailId}.", ex);
				return null;
			}
		}
		/// <summary>
		/// Retrieves the distribution of ratings (1-5 stars) for a vendor.
		/// </summary>
		/// <param name="vendorId">Vendor identifier.</param>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>
		/// Dictionary with rating values (1-5) as keys and their counts as values.
		/// Example: { {5, 10}, {4, 5}, {3, 2}, {2, 1}, {1, 0} }
		/// </returns>
		public async Task<Dictionary<int, int>> GetVendorRatingDistributionAsync(
			Guid vendorId,
			CancellationToken cancellationToken = default)
		{

			var distribution = await _dbContext.TbVendorReviews
							.Where(r => r.VendorId == vendorId
								&& r.Status == ReviewStatus.Approved
								&& !r.IsDeleted)
							.GroupBy(r => (int)r.Rating)
							.Select(g => new { Rating = g.Key, Count = g.Count() })
							.ToListAsync(cancellationToken);

			// Initialize all ratings (1-5) with 0 count
			var result = new Dictionary<int, int>
			{
				{ 5, 0 },
				{ 4, 0 },
				{ 3, 0 },
				{ 2, 0 },
				{ 1, 0 }
			};

			// Fill in actual counts
			foreach (var item in distribution)
			{
				if (result.ContainsKey(item.Rating))
				{
					result[item.Rating] = item.Count;
				}
			}

			return result;
		}
		///// <summary>
		///// Retrieves all approved (visible) reviews for a given Vendor.
		///// Filters by VendorId, ensures the review is approved and not soft-deleted.
		///// </summary>
		///// <param name="vendorId">ID of the Vendor to retrieve reviews for.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>A list of approved reviews sorted by newest first.</returns>
		//public async Task<IEnumerable<TbVendorReview>> GetReviewsByVendorIdAsync(
		//    Guid vendorId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        return await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .Include(r => r.Customer)
		//            .Where(r => r.VendorId == vendorId
		//                && r.Status == ReviewStatus.Approved
		//                && !r.IsDeleted)
		//            .OrderByDescending(r => r.CreatedDateUtc)
		//            .ToListAsync(cancellationToken);
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetReviewsByVendorIdAsync),
		//            $"Error occurred while retrieving reviews for Vendor {vendorId}.", ex);
		//        return new List<TbVendorReview>();
		//    }
		//}

		///// <summary>
		///// Retrieves all reviews for a given Vendor with optional status filter.
		///// </summary>
		///// <param name="vendorId">ID of the Vendor.</param>
		///// <param name="status">Optional review status filter.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>A list of reviews sorted by newest first.</returns>
		//public async Task<IEnumerable<TbVendorReview>> GetVendorReviewsAsync(
		//    Guid vendorId,
		//    ReviewStatus? status = null,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        var query = _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .Include(r => r.Customer)
		//            .Include(r => r.OrderDetail)
		//            .Where(r => r.VendorId == vendorId && !r.IsDeleted);

		//        if (status.HasValue)
		//            query = query.Where(r => r.Status == status.Value);

		//        return await query
		//            .OrderByDescending(r => r.CreatedDateUtc)
		//            .ToListAsync(cancellationToken);
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetVendorReviewsAsync),
		//            $"Error occurred while retrieving reviews for Vendor {vendorId} with status filter.", ex);
		//        return new List<TbVendorReview>();
		//    }
		//}

		///// <summary>
		///// Retrieves all reviews created by a specific customer.
		///// </summary>
		///// <param name="customerId">ID of the Customer.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>A list of customer reviews sorted by newest first.</returns>
		//public async Task<IEnumerable<TbVendorReview>> GetCustomerReviewsAsync(
		//    Guid customerId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        return await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .Include(r => r.Vendor)
		//            .Where(r => r.CustomerId == customerId && !r.IsDeleted)
		//            .OrderByDescending(r => r.CreatedDateUtc)
		//            .ToListAsync(cancellationToken);
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetCustomerReviewsAsync),
		//            $"Error occurred while retrieving reviews for Customer {customerId}.", ex);
		//        return new List<TbVendorReview>();
		//    }
		//}


		///// <summary>
		///// Checks if a customer has already reviewed a specific vendor.
		///// </summary>
		///// <param name="customerId">ID of the Customer.</param>
		///// <param name="vendorId">ID of the Vendor.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>True if the customer has reviewed the vendor, false otherwise.</returns>
		//public async Task<bool> HasCustomerReviewedVendorAsync(
		//    Guid customerId,
		//    Guid vendorId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        return await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .AnyAsync(r => r.CustomerId == customerId 
		//                && r.VendorId == vendorId 
		//                && !r.IsDeleted,
		//                cancellationToken);
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(HasCustomerReviewedVendorAsync),
		//            $"Error occurred while checking if Customer {customerId} reviewed Vendor {vendorId}.", ex);
		//        return false;
		//    }
		//}

		///// <summary>
		///// Calculates the average rating for a vendor based on approved reviews.
		///// </summary>
		///// <param name="vendorId">ID of the Vendor.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>The average rating or 0 if no reviews exist.</returns>
		//public async Task<decimal> GetVendorAverageRatingAsync(
		//    Guid vendorId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        var reviews = await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .Where(r => r.VendorId == vendorId 
		//                && r.Status == ReviewStatus.Approved 
		//                && !r.IsDeleted)
		//            .Select(r => r.Rating)
		//            .ToListAsync(cancellationToken);

		//        return reviews.Any() ? reviews.Average() : 0;
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetVendorAverageRatingAsync),
		//            $"Error occurred while calculating average rating for Vendor {vendorId}.", ex);
		//        return 0;
		//    }
		//}

		///// <summary>
		///// Gets the total count of approved reviews for a vendor.
		///// </summary>
		///// <param name="vendorId">ID of the Vendor.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>The count of approved reviews.</returns>
		//public async Task<int> GetVendorReviewCountAsync(
		//    Guid vendorId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        return await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .CountAsync(r => r.VendorId == vendorId 
		//                && r.Status == ReviewStatus.Approved 
		//                && !r.IsDeleted,
		//                cancellationToken);
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetVendorReviewCountAsync),
		//            $"Error occurred while counting reviews for Vendor {vendorId}.", ex);
		//        return 0;
		//    }
		//}

		///// <summary>
		///// Gets the rating distribution for a vendor (count per rating value).
		///// </summary>
		///// <param name="vendorId">ID of the Vendor.</param>
		///// <param name="cancellationToken">Cancellation token.</param>
		///// <returns>Dictionary with rating as key and count as value.</returns>
		//public async Task<Dictionary<int, int>> GetVendorRatingDistributionAsync(
		//    Guid vendorId,
		//    CancellationToken cancellationToken = default)
		//{
		//    try
		//    {
		//        var reviews = await _dbContext.Set<TbVendorReview>()
		//            .AsNoTracking()
		//            .Where(r => r.VendorId == vendorId 
		//                && r.Status == ReviewStatus.Approved 
		//                && !r.IsDeleted)
		//            .Select(r => (int)r.Rating)
		//            .ToListAsync(cancellationToken);

		//        return reviews
		//            .GroupBy(r => r)
		//            .ToDictionary(g => g.Key, g => g.Count());
		//    }
		//    catch (Exception ex)
		//    {
		//        HandleException(nameof(GetVendorRatingDistributionAsync),
		//            $"Error occurred while getting rating distribution for Vendor {vendorId}.", ex);
		//        return new Dictionary<int, int>();
		//    }
		//}

	}
}
