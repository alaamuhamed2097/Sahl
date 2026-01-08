using Common.Filters;
using DAL.Models;
using Shared.DTOs.Brand;

namespace BL.Contracts.Service.Brand;

public interface IBrandService
{
    Task<IEnumerable<BrandDto>> GetAllAsync();
    Task<BrandDto?> GetByIdAsync(Guid id);
    Task<PagedResult<BrandDto>> SearchAsync(BaseSearchCriteriaModel criteria);
    Task<bool> SaveAsync(BrandDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}