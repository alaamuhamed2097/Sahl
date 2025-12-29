using DAL.Models;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Warehouse;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseDto>> GetAllAsync();
    Task<IEnumerable<WarehouseDto>> GetActiveWarehousesAsync();
    Task<WarehouseDto?> GetByIdAsync(Guid id);
    Task<PagedResult<WarehouseDto>> SearchAsync(BaseSearchCriteriaModel criteria);
    Task<bool> SaveAsync(WarehouseDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);
}
