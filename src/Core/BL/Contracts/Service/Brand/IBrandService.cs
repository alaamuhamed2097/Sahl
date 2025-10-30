using DAL.Models;
using Shared.DTOs.Brand;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Brand
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<BrandDto>> GetFavoritesAsync();
        Task<PaginatedDataModel<BrandDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(BrandDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> MarkAsFavoriteAsync(Guid brandId, Guid userId);
    }
}