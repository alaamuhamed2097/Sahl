using Common.Enumerations.Review;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Review;
using Dashboard.Models.pagintion;
using Shared.DTOs.Review;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.Review
{
	public class VendorReviewService : IVendorReviewService
	{
		private readonly IApiService _apiService;

		public VendorReviewService(IApiService apiService)
		{
			_apiService = apiService;
		}

		public async Task<ResponseModel<VendorReviewDto>> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<VendorReviewDto>(
				ApiEndpoints.VendorReview.GetById(reviewId));
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetReviewsByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(
				ApiEndpoints.VendorReview.GetByVendorId(vendorId));
		}

		public async Task<ResponseModel<PaginatedDataModel<VendorReviewDto>>> SearchVendorReviews(AdminVendorReviewSearchCriteriaModel criteria, CancellationToken cancellationToken = default)
		{
			var queryParams = new List<string>
			{
				$"PageNumber={criteria.PageNumber}",
				$"PageSize={criteria.PageSize}"
			};

			if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
				queryParams.Add($"SearchTerm={Uri.EscapeDataString(criteria.SearchTerm)}");

			if (criteria.VendorId.HasValue)
				queryParams.Add($"VendorId={criteria.VendorId.Value}");

			if (criteria.CustomerId.HasValue)
				queryParams.Add($"CustomerId={criteria.CustomerId.Value}");

			if (criteria.RatingFrom.HasValue)
				queryParams.Add($"RatingFrom={criteria.RatingFrom.Value}");

			if (criteria.RatingTo.HasValue)
				queryParams.Add($"RatingTo={criteria.RatingTo.Value}");

			if (criteria.Statuses != null && criteria.Statuses.Any())
			{
				foreach (var status in criteria.Statuses)
				{
					queryParams.Add($"Statuses={status}");
				}
			}

			if (!string.IsNullOrWhiteSpace(criteria.SortBy))
				queryParams.Add($"SortBy={criteria.SortBy}");

			if (!string.IsNullOrWhiteSpace(criteria.SortDirection))
				queryParams.Add($"SortDirection={criteria.SortDirection}");

			var queryString = string.Join("&", queryParams);
			var url = $"{ApiEndpoints.VendorReview.Search}?{queryString}";

			return await _apiService.GetAsync<PaginatedDataModel<VendorReviewDto>>(url);
		}

		public async Task<ResponseModel<VendorReviewStatsDto>> GetVendorReviewStatsAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<VendorReviewStatsDto>(
				ApiEndpoints.VendorReview.GetStats(vendorId));
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(
				ApiEndpoints.VendorReview.GetPending);
		}

		public async Task<ResponseModel<bool>> ApproveReviewAsync(Guid reviewId, CancellationToken cancellationToken = default)
		{
			var reviewDto = new VendorReviewDto { Id = reviewId };
			return await _apiService.PostAsync<VendorReviewDto, bool>(
				ApiEndpoints.VendorReview.Approve,
				reviewDto);
		}

		public async Task<ResponseModel<bool>> RejectReviewAsync(Guid reviewId, CancellationToken cancellationToken = default)
		{
			var reviewDto = new VendorReviewDto { Id = reviewId };
			return await _apiService.PostAsync<VendorReviewDto, bool>(
				ApiEndpoints.VendorReview.Reject,
				reviewDto);
		}

		public async Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId, CancellationToken cancellationToken = default)
		{
			var reviewDto = new VendorReviewDto { Id = reviewId };
			return await _apiService.PostAsync<VendorReviewDto, bool>(
				ApiEndpoints.VendorReview.Delete,
				reviewDto);
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetVendorReviewsAsync(Guid vendorId, ReviewStatus? status = null, CancellationToken cancellationToken = default)
		{
			var url = ApiEndpoints.VendorReview.GetVendorReviews(vendorId);
			if (status.HasValue)
			{
				url += $"?status={status.Value}";
			}

			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(url);
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetCustomerReviewsAsync(Guid customerId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(
				ApiEndpoints.VendorReview.GetCustomerReviews(customerId));
		}

		public async Task<ResponseModel<decimal>> GetAverageRatingAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<decimal>(
				ApiEndpoints.VendorReview.GetAverageRating(vendorId));
		}

		public async Task<ResponseModel<int>> GetReviewCountAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<int>(
				ApiEndpoints.VendorReview.GetReviewCount(vendorId));
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetVerifiedReviewsAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(
				ApiEndpoints.VendorReview.GetVerified(vendorId));
		}

		public async Task<ResponseModel<IEnumerable<VendorReviewDto>>> GetNonVerifiedReviewsAsync(Guid vendorId, CancellationToken cancellationToken = default)
		{
			return await _apiService.GetAsync<IEnumerable<VendorReviewDto>>(
				ApiEndpoints.VendorReview.GetNonVerified(vendorId));
		}
	}
}