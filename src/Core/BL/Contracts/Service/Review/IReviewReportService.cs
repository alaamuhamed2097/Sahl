using Common.Enumerations.Review;
using DAL.Models;
using DAL.ResultModels;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Contracts.Service.Review
{
	public interface IReviewReportService
	{
		Task<SaveResult> SubmitReportAsync(
		ReviewReportDto reportDto,
		string userId,
		CancellationToken cancellationToken = default);
		Task<ReviewReportDto?> GetReportByIdAsync(
		Guid reportId,
		CancellationToken cancellationToken = default);
		Task<PagedResult<ReviewReportDto>> GetPaginatedReviewReportsAsync(
		ReviewReportSearchCriteriaModel criteriaModel,
		CancellationToken cancellationToken = default);
		Task<IEnumerable<ReviewReportDto>> GetReportsByReviewIdAsync(
		Guid reviewId,
		CancellationToken cancellationToken = default);
		Task<SaveResult> ResolveReportAsync(
		Guid reportId,
		Guid adminId,
		CancellationToken cancellationToken = default);
		Task<bool> MarkReviewAsFlaggedAsync(Guid reviewId, string adminId, CancellationToken cancellationToken = default);
	}
}
