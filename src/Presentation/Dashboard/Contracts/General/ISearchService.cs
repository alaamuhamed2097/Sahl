using Dashboard.Models.pagintion;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.General
{
    public interface ISearchService<T>
    {
        /// <summary>
        /// Search and Pagination.
        /// </summary>
        Task<ResponseModel<PaginatedDataModel<T>>> SearchAsync(BaseSearchCriteriaModel model, string Endpoint);
    }
}