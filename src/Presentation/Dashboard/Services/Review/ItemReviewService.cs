//using Dashboard.Constants;
//using Dashboard.Contracts.General;
//using Dashboard.Contracts.Review;
//using Dashboard.Models.pagintion;
//using Shared.DTOs.Review;
//using Shared.GeneralModels;
//using Shared.GeneralModels.SearchCriteriaModels;

//namespace Dashboard.Services.Review
//{
//	public class ItemReviewService : IItemReviewService
//	{
//		private readonly IApiService _apiService;

//		public ItemReviewService(IApiService apiService)
//		{
//			_apiService = apiService;
//		}

//		/// <summary>
//		/// Update an existing review
//		/// </summary>
//		public async Task<ResponseModel<ResponseItemReviewDto>> UpdateReviewAsync(ItemReviewDto reviewDto)
//		{
//			if (reviewDto == null) throw new ArgumentNullException(nameof(reviewDto));

//			try
//			{
//				return await _apiService.PostAsync<ItemReviewDto, ResponseItemReviewDto>(
//					$"{ApiEndpoints.ItemReview.Update}",
//					reviewDto);
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<ResponseItemReviewDto>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Get review by ID
//		/// </summary>
//		public async Task<ResponseModel<ItemReviewDto>> GetReviewByIdAsync(Guid reviewId)
//		{
//			try
//			{
//				return await _apiService.GetAsync<ItemReviewDto>(
//					$"{ApiEndpoints.ItemReview.Get}/{reviewId}");
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<ItemReviewDto>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Delete a review
//		/// </summary>
//		public async Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId)
//		{
//			try
//			{
//				var reviewDto = new ItemReviewDto { Id = reviewId };
//				return await _apiService.PostAsync<ItemReviewDto, bool>(
//					$"{ApiEndpoints.ItemReview.Delete}",
//					reviewDto);
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Get all reviews for a specific Item
//		/// </summary>
//		public async Task<ResponseModel<IEnumerable<ResponseItemReviewDto>>> GetReviewsByItemIdAsync(Guid itemId)
//		{
//			try
//			{
//				return await _apiService.GetAsync<IEnumerable<ResponseItemReviewDto>>(
//					$"{ApiEndpoints.ItemReview.GetByItemId}/{itemId}");
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<IEnumerable<ResponseItemReviewDto>>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Search reviews with pagination and filters
//		/// </summary>
//		public async Task<ResponseModel<PaginatedDataModel<ResponseItemReviewDto>>> SearchReviewsAsync(ItemReviewSearchCriteriaModel criteria)
//		{
//			try
//			{
//				var queryString = $"PageNumber={criteria.PageNumber}&PageSize={criteria.PageSize}";

//				if (!string.IsNullOrEmpty(criteria.SearchTerm))
//					queryString += $"&SearchTerm={criteria.SearchTerm}";

//				if (criteria.ItemId.HasValue)
//					queryString += $"&ItemId={criteria.ItemId}";

//				//if (criteria.Rating.HasValue)
//				//	queryString += $"&Rating={criteria.Rating}";

//				string url = $"{ApiEndpoints.ItemReview.Search}?{queryString}";

//				return await _apiService.GetAsync<PaginatedDataModel<ResponseItemReviewDto>>(url);
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<PaginatedDataModel<ResponseItemReviewDto>>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}


//		/// <summary>
//		/// Get Item review statistics
//		/// </summary>
//		public async Task<ResponseModel<ResponseItemReviewStatsDto>> GetItemReviewStatsAsync(Guid itemId)
//		{
//			try
//			{
//				return await _apiService.GetAsync<ResponseItemReviewStatsDto>(
//					$"{ApiEndpoints.ItemReview.GetStats}/{itemId}");
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<ResponseItemReviewStatsDto>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Get pending reviews for moderation
//		/// </summary>
//		public async Task<ResponseModel<IEnumerable<ResponseItemReviewDto>>> GetPendingReviewsAsync()
//		{
//			try
//			{
//				return await _apiService.GetAsync<IEnumerable<ResponseItemReviewDto>>(
//					$"{ApiEndpoints.ItemReview.GetPending}");
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<IEnumerable<ResponseItemReviewDto>>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Approve a review
//		/// </summary>
//		public async Task<ResponseModel<bool>> ApproveReviewAsync(Guid reviewId)
//		{
//			try
//			{
//				var reviewDto = new ItemReviewDto { Id = reviewId };
//				return await _apiService.PostAsync<ItemReviewDto, bool>(
//					$"{ApiEndpoints.ItemReview.Approve}",
//					reviewDto);
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}

//		/// <summary>
//		/// Reject a review
//		/// </summary>
//		public async Task<ResponseModel<bool>> RejectReviewAsync(Guid reviewId)
//		{
//			try
//			{
//				var reviewDto = new ItemReviewDto { Id = reviewId };
//				return await _apiService.PostAsync<ItemReviewDto, bool>(
//					$"{ApiEndpoints.ItemReview.Reject}",
//					reviewDto);
//			}
//			catch (Exception ex)
//			{
//				return new ResponseModel<bool>
//				{
//					Success = false,
//					Message = ex.Message
//				};
//			}
//		}
//	}
//}
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
	public class ItemReviewService : IItemReviewService
	{
		private readonly IApiService _apiService;

		public ItemReviewService(IApiService apiService)
		{
			_apiService = apiService;
		}


        /// <summary>
        /// Search reviews with pagination and filters
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<ItemReviewResponseDto>>> SearchReviewsAsync(ItemReviewSearchCriteriaModel criteria)
        {
            try
            {
                var queryString = $"PageNumber={criteria.PageNumber}&PageSize={criteria.PageSize}";

                if (!string.IsNullOrEmpty(criteria.SearchTerm))
                    queryString += $"&SearchTerm={Uri.EscapeDataString(criteria.SearchTerm)}";

                if (!string.IsNullOrEmpty(criteria.SortBy))
                    queryString += $"&SortBy={criteria.SortBy}";

                if (!string.IsNullOrEmpty(criteria.SortDirection))
                    queryString += $"&SortDirection={criteria.SortDirection}";

                if (criteria.ItemId.HasValue)
                    queryString += $"&ItemId={criteria.ItemId}";

                string url = $"{ApiEndpoints.ItemReview.Search}?{queryString}";


                var response = await _apiService.GetAsync<PaginatedDataModel<ItemReviewResponseDto>>(url);

                if (!response.Success || response.Data == null)
                {
                    return new ResponseModel<PaginatedDataModel<ItemReviewResponseDto>>
                    {
                        Success = false,
                        Message = response.Message ?? "Error retrieving data",
                        StatusCode = response.StatusCode
                    };
                }


                var paginatedData = new PaginatedDataModel<ItemReviewResponseDto>(
                    response.Data.Items ?? new List<ItemReviewResponseDto>(),
                    response.Data.TotalRecords
                );

                return new ResponseModel<PaginatedDataModel<ItemReviewResponseDto>>
                {
                    Success = true,
                    Message = response.Message,
                    Data = paginatedData,
                    StatusCode = response.StatusCode
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<ItemReviewResponseDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Get review by ID
        /// </summary>
        public async Task<ResponseModel<ItemReviewResponseDto>> GetReviewByIdAsync(Guid reviewId)
		{
			try
			{
				return await _apiService.GetAsync<ItemReviewResponseDto>(
					$"{ApiEndpoints.ItemReview.Get}/{reviewId}");
			}
			catch (Exception ex)
			{
				return new ResponseModel<ItemReviewResponseDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Delete a review
		/// </summary>
		public async Task<ResponseModel<bool>> DeleteReviewAsync(Guid reviewId)
		{
			try
			{
				return (await _apiService.DeleteAsync<bool>(
					$"{ApiEndpoints.ItemReview.Delete}/{reviewId}"));
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Get Item review statistics
		/// </summary>
		public async Task<ResponseModel<ResponseItemReviewSummeryDto>> GetItemReviewSummeryAsync(Guid itemId)
		{
			try
			{
				return await _apiService.GetAsync<ResponseItemReviewSummeryDto>(
					$"{ApiEndpoints.ItemReview.GetStats}/{itemId}");
			}
			catch (Exception ex)
			{
				return new ResponseModel<ResponseItemReviewSummeryDto>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}

		/// <summary>
		/// Approve a review
		/// </summary>
		public async Task<ResponseModel<bool>> ChangeReviewStatusAsync(Guid reviewId, ReviewStatus newStatus)
		{
			try
			{
				var reviewDto = new ItemReviewDto { Id = reviewId };
				return await _apiService.PutAsync<ItemReviewDto, bool>(
					$"{ApiEndpoints.ItemReview.ChangeStatus}/{reviewId}",
					reviewDto);
			}
			catch (Exception ex)
			{
				return new ResponseModel<bool>
				{
					Success = false,
					Message = ex.Message
				};
			}
		}
	}
}