using Shared.DTOs.Page;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Dashboard.Models.pagintion;

namespace Dashboard.Contracts.Page
{
    public interface IPageService
    {
        Task<ResponseModel<IEnumerable<PageDto>>> GetAllAsync();
        Task<ResponseModel<PageDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<PageDto>> GetBySlugAsync(string slug);
        Task<ResponseModel<bool>> SaveAsync(PageDto pageDto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> ToggleStatusAsync(Guid id);
        Task<ResponseModel<PaginatedDataModel<PageDto>>> SearchAsync(BaseSearchCriteriaModel searchCriteria);
    }
}