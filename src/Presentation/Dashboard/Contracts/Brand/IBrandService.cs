using Dashboard.Models.pagintion;
using Shared.DTOs.Brand;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Brand
{
    public interface IBrandService
    {
        /// <summary>
        /// Get all brands.
        /// </summary>
        Task<ResponseModel<IEnumerable<BrandDto>>> GetAllAsync();

        /// <summary>
        /// Get brand by ID.
        /// </summary>
        Task<ResponseModel<BrandDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Get favorite brands.
        /// </summary>
        Task<ResponseModel<IEnumerable<BrandDto>>> GetFavoritesAsync();

        /// <summary>
        /// Search brands with pagination and filtering.
        /// </summary>
        Task<ResponseModel<PaginatedDataModel<BrandDto>>> SearchAsync(BaseSearchCriteriaModel criteria);

        /// <summary>
        /// Save or update a brand.
        /// </summary>
        Task<ResponseModel<BrandDto>> SaveAsync(BrandDto dto);

        /// <summary>
        /// Delete a brand by ID.
        /// </summary>
        Task<ResponseModel<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Mark or unmark a brand as favorite.
        /// </summary>
        Task<ResponseModel<string>> MarkAsFavoriteAsync(Guid brandId);
    }
}