using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Services.General
{
    public class SearchService<T> : ISearchService<T> where T : class
    {
        private readonly IApiService _apiService;

        public SearchService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Search with optional filters.
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<T>>> SearchAsync(BaseSearchCriteriaModel model, string Endpoint)
        {
            try
            {
                var queryString = $"PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}";
                
                // Add sorting parameters if provided
                if (!string.IsNullOrWhiteSpace(model.SortBy))
                {
                    queryString += $"&SortBy={model.SortBy}&SortDirection={model.SortDirection}";
                }
                
                string url = $"{Endpoint}?{queryString}";
                return await _apiService.GetAsync<PaginatedDataModel<T>>(url);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PaginatedDataModel<T>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
