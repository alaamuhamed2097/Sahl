using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Review;
using Dashboard.Models.pagintion;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Net.Http.Json;

namespace Dashboard.Services.Review
{
	public class ReportReviewService : IReportReviewService
	{
		private readonly IApiService _apiService;

		public ReportReviewService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ResponseModel<ReviewReportDto>> GetReportByIdAsync(Guid reportId)
		{
			return await _apiService.GetAsync<ReviewReportDto>($"{ApiEndpoints.ReviewReport.Get}/{reportId}");
		}

		public async Task<ResponseModel<PaginatedDataModel<ReviewReportDto>>> SearchReportsAsync(ReviewReportSearchCriteriaModel criteriaModel)
		{
			var queryString = BuildQueryString(criteriaModel);
			return await _apiService.GetAsync<PaginatedDataModel<ReviewReportDto>>($"{ApiEndpoints.ReviewReport.Search}?{queryString}");
		}

		public async Task<ResponseModel<IEnumerable<ReviewReportDto>>> GetReportsByReviewIdAsync(Guid itemReviewId)
		{
			return await _apiService.GetAsync<IEnumerable<ReviewReportDto>>($"{ApiEndpoints.ReviewReport.GetByItemReviewId}/{itemReviewId}");
		}

		public async Task<ResponseModel<string>> ResolveReportAsync(Guid reportId)
		{
			var dto = new ReviewReportDto { Id = reportId };
			return await _apiService.PostAsync<ReviewReportDto, string>(ApiEndpoints.ReviewReport.Resolve, dto);
		}

		public async Task<ResponseModel<bool>> MarkReviewAsFlaggedAsync(Guid reviewId)
		{
			var dto = new ItemReviewDto { Id = reviewId };
			return await _apiService.PostAsync<ItemReviewDto, bool>(ApiEndpoints.ReviewReport.MarkAsFlagged, dto);
		}

		private string BuildQueryString(ReviewReportSearchCriteriaModel criteria)
		{
			var parameters = new List<string>();

			if (criteria.PageNumber > 0)
				parameters.Add($"PageNumber={criteria.PageNumber}");
			if (criteria.PageSize > 0)
				parameters.Add($"PageSize={criteria.PageSize}");
			if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
				parameters.Add($"SearchTerm={Uri.EscapeDataString(criteria.SearchTerm)}");
			if (!string.IsNullOrWhiteSpace(criteria.SortBy))
				parameters.Add($"SortBy={criteria.SortBy}");
			if (!string.IsNullOrWhiteSpace(criteria.SortDirection))
				parameters.Add($"SortDirection={criteria.SortDirection}");
			//if (criteria.ItemReviewId.HasValue)
			//	parameters.Add($"ItemReviewId={criteria.ItemReviewId}");
			if (criteria.Status.HasValue)
				parameters.Add($"Status={(int)criteria.Status}");

			return string.Join("&", parameters);
		}
	}
}

