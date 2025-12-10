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
		cancellationToken
	);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetReviewDetailsAsync),
					$"Error occurred while retrieving review details for ID {reviewId}.", ex);
				return null;
			}
		}

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

		public async Task<PaginatedDataModel<TbOfferReview>> GetPaginatedReviewsAsync(
			Guid? OfferId = null,
			ReviewStatus? status = null,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			try
			{
				ValidatePaginationParameters(pageNumber, pageSize);

				var query = _dbContext.Set<TbOfferReview>()
					.AsNoTracking()
					.Where(r => !r.IsDeleted);

				if (OfferId.HasValue)
					query = query.Where(r => r.OfferID == OfferId.Value);

				if (status.HasValue)
					query = query.Where(r => r.Status == status.Value);

				var totalCount = await query.CountAsync(cancellationToken);

				var reviews = await query
					.OrderByDescending(r => r.CreatedDateUtc)
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync(cancellationToken);

				return new PaginatedDataModel<TbOfferReview>(reviews, totalCount);
			}
			catch (Exception ex)
			{
				HandleException(nameof(GetPaginatedReviewsAsync),
					$"Error occurred while retrieving paginated reviews.", ex);
				return new PaginatedDataModel<TbOfferReview>(new List<TbOfferReview>(), 0);
			}
		}

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
	}
}
