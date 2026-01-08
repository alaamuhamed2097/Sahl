using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.GeneralModels;

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