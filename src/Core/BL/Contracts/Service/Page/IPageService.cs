using Common.Enumerations;
using DAL.Models;
using Shared.DTOs.Page;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Services.Page
{
    public interface IPageService
    {
        Task<IEnumerable<PageDto>> GetAllAsync();
        Task<PageDto> GetByIdAsync(Guid id);
        Task<PageDto> GetByTypeAsync(PageType pageType);
        Task<PagedResult<PageDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(PageDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}