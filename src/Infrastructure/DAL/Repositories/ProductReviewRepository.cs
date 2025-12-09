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
	public class ProductReviewRepository : TableRepository<TbProductReview>, IProductReviewRepository
	{
		public ProductReviewRepository(ApplicationDbContext dbContext, ILogger logger)
			: base(dbContext, logger) { }

		public async Task<IEnumerable<TbProductReview>> GetReviewsByProductIdAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.Where(r => r.ProductID == productId
						&& r.Status == ReviewStatus.Approved
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active)
					.OrderByDescending(r => r.CreatedDateUtc)
					.ToListAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewsByProductIdAsync),
					$"Error occurred while retrieving reviews for product {productId}.", ex);
				return new List<TbProductReview>();
			}
		}

		public async Task<TbProductReview?> GetReviewDetailsAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.Include(r => r.ReviewVotes.Where(v => v.CurrentState == (int)Common.Enumerations.EntityState.Active))
					.Include(r => r.ReviewReports.Where(rr => rr.CurrentState == (int)Common.Enumerations.EntityState.Active))
					.FirstOrDefaultAsync(r => r.Id == reviewId
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewDetailsAsync),
					$"Error occurred while retrieving review details for ID {reviewId}.", ex);
				return null;
			}
		}

		public async Task<TbProductReview?> GetCustomerReviewForOrderItemAsync(
			Guid orderItemId,
			Guid customerId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.FirstOrDefaultAsync(r => r.OrderItemID == orderItemId
						&& r.CustomerID == customerId
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetCustomerReviewForOrderItemAsync),
					$"Error occurred while checking customer review for order item.", ex);
				return null;
			}
		}

		public async Task<decimal> GetAverageRatingAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reviews = await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.Where(r => r.ProductID == productId
						&& r.Status == ReviewStatus.Approved
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active)
					.Select(r => r.Rating)
					.ToListAsync(cancellationToken);

				return reviews.Any() ? reviews.Average() : 0;
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetAverageRatingAsync),
					$"Error occurred while calculating average rating for product {productId}.", ex);
				return 0;
			}
		}

		public async Task<PaginatedDataModel<TbProductReview>> GetPaginatedReviewsAsync(
			Guid? productId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			try
			{
				ValidatePaginationParameters(pageNumber, pageSize);

				var query = _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.Where(r => r.CurrentState == (int)Common.Enumerations.EntityState.Active);

				if (productId.HasValue)
					query = query.Where(r => r.ProductID == productId.Value);

				if (status.HasValue)
					query = query.Where(r => r.Status == status.Value);

				var totalCount = await query.CountAsync(cancellationToken);

				var reviews = await query
					.OrderByDescending(r => r.CreatedDateUtc)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync(cancellationToken);

				return new PaginatedDataModel<TbProductReview>(reviews, totalCount);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetPaginatedReviewsAsync),
					$"Error occurred while retrieving paginated reviews.", ex);
				return new PaginatedDataModel<TbProductReview>(new List<TbProductReview>(), 0);
			}
		}

		public async Task<IEnumerable<TbProductReview>> GetPendingReviewsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.Where(r => r.Status == ReviewStatus.Pending
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active)
					.OrderBy(r => r.CreatedDateUtc)
					.ToListAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetPendingReviewsAsync),
					$"Error occurred while retrieving pending reviews.", ex);
				return new List<TbProductReview>();
			}
		}

		public async Task<int> GetReviewCountByProductIdAsync(
			Guid productId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				return await _dbContext.Set<TbProductReview>()
					.AsNoTracking()
					.CountAsync(r => r.ProductID == productId
						&& r.Status == ReviewStatus.Approved
						&& r.CurrentState == (int)Common.Enumerations.EntityState.Active,
						cancellationToken);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewCountByProductIdAsync),
					$"Error occurred while counting reviews for product {productId}.", ex);
				return 0;
			}
		}
	}
}
