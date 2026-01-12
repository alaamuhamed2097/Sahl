using Common.Filters;
using DAL.Models;
using Shared.DTOs.Warehouse;

namespace BL.Contracts.Service.Warehouse;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseDto>> GetAllAsync();
    Task<IEnumerable<WarehouseDto>> GetActiveWarehousesAsync();
    Task<WarehouseDto> GetMarketWarehousesAsync();
    Task<WarehouseDto?> GetByIdAsync(Guid id);
    Task<PagedResult<WarehouseDto>> SearchAsync(BaseSearchCriteriaModel criteria);
    Task<bool> SaveAsync(WarehouseDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);
}
