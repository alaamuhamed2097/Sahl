using DAL.Models;
using Shared.DTOs.Content;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Content
{
    public interface IContentAreaService
    {
        Task<IEnumerable<ContentAreaDto>> GetAllAsync();
        Task<IEnumerable<ContentAreaDto>> GetActiveAreasAsync();
        Task<ContentAreaDto?> GetByIdAsync(Guid id);
        Task<ContentAreaDto?> GetByAreaCodeAsync(string areaCode);
        Task<PaginatedDataModel<ContentAreaDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(ContentAreaDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);
    }

    public interface IMediaContentService
    {
        Task<IEnumerable<MediaContentDto>> GetAllAsync();
        Task<IEnumerable<MediaContentDto>> GetByContentAreaIdAsync(Guid contentAreaId);
        Task<IEnumerable<MediaContentDto>> GetActiveMediaByAreaCodeAsync(string areaCode);
        Task<MediaContentDto?> GetByIdAsync(Guid id);
        Task<PaginatedDataModel<MediaContentDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(MediaContentDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);
        Task<bool> UpdateDisplayOrderAsync(Guid id, int displayOrder, Guid userId);
    }
}
