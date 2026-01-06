using Dashboard.Models.pagintion;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Review
{
	public interface IReportReviewService
	{
		Task<ResponseModel<ReviewReportDto>> GetReportByIdAsync(Guid reportId);
		Task<ResponseModel<PaginatedDataModel<ReviewReportDto>>> SearchReportsAsync(ReviewReportSearchCriteriaModel criteriaModel);
		Task<ResponseModel<IEnumerable<ReviewReportDto>>> GetReportsByReviewIdAsync(Guid itemReviewId);
		Task<ResponseModel<string>> ResolveReportAsync(Guid reportId);
		Task<ResponseModel<bool>> MarkReviewAsFlaggedAsync(Guid reviewId);
	}
}
