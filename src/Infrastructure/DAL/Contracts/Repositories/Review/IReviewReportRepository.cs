using Common.Enumerations.Review;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts.Repositories.Review
{
	public interface IReviewReportRepository : ITableRepository<TbReviewReport>
	{
		//Task<TbReviewReport> CreateReportAsync(TbReviewReport report, CancellationToken cancellationToken = default);
		Task<TbReviewReport?> GetByIdAsync(Guid reportId, CancellationToken cancellationToken = default);
		Task<bool> IsAlreadyReportedAsync(Guid reviewId, Guid userId, CancellationToken cancellationToken = default);
		Task<IEnumerable<TbReviewReport>> GetReportsByReviewIdAsync(Guid reviewId, CancellationToken cancellationToken = default);
		Task<IEnumerable<TbReviewReport>> GetReportsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
		//Task<PaginatedDataModel<TbReviewReport>> GetAllReportsAsync(
		//	ReviewReportStatus? status = null,
		//	int pageNumber = 1,
		//	int pageSize = 10,
		//	CancellationToken cancellationToken = default);
		//Task<bool> ChangeStatusAsync(Guid reportId, ReviewReportStatus newStatus, Guid updaterId, CancellationToken cancellationToken = default);
	}
}
