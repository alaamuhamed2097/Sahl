using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Review;
using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Review
{
    public class ReviewReportRepository : TableRepository<TbReviewReport>, IReviewReportRepository
    {
        public ReviewReportRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger) { }


        //public async Task<TbReviewReport> CreateReportAsync(
        //	TbReviewReport report,
        //	CancellationToken cancellationToken = default)
        //{
        //	try
        //	{
        //		report.Id = Guid.NewGuid();
        //		report.CreatedDateUtc = DateTime.UtcNow;
        //		report.CurrentState = (int)Common.Enumerations.EntityState.Active;
        //		report.Status = ReviewReportStatus.Pending;

        //		await _dbContext.Set<TbReviewReport>().AddAsync(report, cancellationToken);
        //		await _dbContext.SaveChangesAsync(cancellationToken);

        //		return report;
        //	}
        //	catch (Exception ex)
        //	{
        //		HandleException(nameof(CreateReportAsync),
        //			$"Error occurred while creating report.", ex);
        //		throw;
        //	}
        //}

        public async Task<TbReviewReport?> GetByIdAsync(
            Guid reportId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewReport>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == reportId
                        && !r.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetByIdAsync),
                    $"Error occurred while retrieving report {reportId}.", ex);
                return null;
            }
        }

        public async Task<bool> IsAlreadyReportedAsync(
            Guid reviewId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewReport>()
                    .AsNoTracking()
                    .AnyAsync(r => r.ItemReviewId == reviewId
                        && r.CustomerId == userId
                        && !r.IsDeleted,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(IsAlreadyReportedAsync),
                    $"Error occurred while checking if review already reported.", ex);
                return false;
            }
        }

        public async Task<IEnumerable<TbReviewReport>> GetReportsByReviewIdAsync(
            Guid reviewId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewReport>()
                    .AsNoTracking()
                    .Where(r => r.ItemReviewId == reviewId
                        && !r.IsDeleted)
                    .OrderByDescending(r => r.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetReportsByReviewIdAsync),
                    $"Error occurred while retrieving reports for review {reviewId}.", ex);
                return new List<TbReviewReport>();
            }
        }

        public async Task<IEnumerable<TbReviewReport>> GetReportsByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbReviewReport>()
                    .AsNoTracking()
                    .Where(r => r.CustomerId == userId
                        && !r.IsDeleted)
                    .OrderByDescending(r => r.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetReportsByUserIdAsync),
                    $"Error occurred while retrieving reports for user {userId}.", ex);
                return new List<TbReviewReport>();
            }
        }

        //public async Task<PaginatedDataModel<TbReviewReport>> GetAllReportsAsync(
        //	ReviewReportStatus? status = null,
        //	int pageNumber = 1,
        //	int pageSize = 10,
        //	CancellationToken cancellationToken = default)
        //{
        //	try
        //	{
        //		ValidatePaginationParameters(pageNumber, pageSize);

        //		var query = _dbContext.Set<TbReviewReport>()
        //			.AsNoTracking()
        //			.Where(r => !r.IsDeleted);

        //		if (status.HasValue)
        //			query = query.Where(r => r.Status == status.Value);

        //		var totalCount = await query.CountAsync(cancellationToken);

        //		var reports = await query
        //			.OrderByDescending(r => r.CreatedDateUtc)
        //			.Skip((pageNumber - 1) * pageSize)
        //			.Take(pageSize)
        //			.ToListAsync(cancellationToken);

        //		return new PaginatedDataModel<TbReviewReport>(reports, totalCount);
        //	}
        //	catch (Exception ex)
        //	{
        //		HandleException(nameof(GetAllReportsAsync),
        //			$"Error occurred while retrieving reports.", ex);
        //		return new PaginatedDataModel<TbReviewReport>(new List<TbReviewReport>(), 0);
        //	}
        //}

        //public async Task<bool> ChangeStatusAsync(
        //	Guid reportId,
        //	ReviewReportStatus newStatus,
        //	Guid updaterId,
        //	CancellationToken cancellationToken = default)
        //{
        //	try
        //	{
        //		var report = await _dbContext.Set<TbReviewReport>()
        //			.FirstOrDefaultAsync(r => r.Id == reportId, cancellationToken);

        //		if (report == null) return false;

        //		report.Status = newStatus;
        //		report.UpdatedDateUtc = DateTime.UtcNow;
        //		report.UpdatedBy = updaterId;

        //		_dbContext.Set<TbReviewReport>().Update(report);
        //		return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        //	}
        //	catch (Exception ex)
        //	{
        //		HandleException(nameof(ChangeStatusAsync),
        //			$"Error occurred while changing report status for report {reportId}.", ex);
        //		return false;
        //	}
        //}

    }
}
