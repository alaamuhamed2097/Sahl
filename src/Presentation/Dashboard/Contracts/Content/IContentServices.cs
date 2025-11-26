using Dashboard.Models.pagintion;
using Shared.DTOs.Content;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Content
{
    public interface IContentAreaService
    {
        Task<ResponseModel<IEnumerable<ContentAreaDto>>> GetAllAsync();
        Task<ResponseModel<IEnumerable<ContentAreaDto>>> GetActiveAreasAsync();
        Task<ResponseModel<ContentAreaDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<ContentAreaDto>> GetByAreaCodeAsync(string areaCode);
        Task<ResponseModel<PaginatedDataModel<ContentAreaDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<ResponseModel<ContentAreaDto>> SaveAsync(ContentAreaDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> ToggleStatusAsync(Guid id);
    }

    public interface IMediaContentService
    {
        Task<ResponseModel<IEnumerable<MediaContentDto>>> GetAllAsync();
        Task<ResponseModel<IEnumerable<MediaContentDto>>> GetByAreaIdAsync(Guid contentAreaId);
        Task<ResponseModel<IEnumerable<MediaContentDto>>> GetByAreaCodeAsync(string areaCode);
        Task<ResponseModel<MediaContentDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<PaginatedDataModel<MediaContentDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<ResponseModel<MediaContentDto>> SaveAsync(MediaContentDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> ToggleStatusAsync(Guid id);
        Task<ResponseModel<bool>> UpdateDisplayOrderAsync(Guid id, int displayOrder);
    }
}
