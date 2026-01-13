using Common.Filters;
using DAL.Models;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Warehouse;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseDto>> GetAllAsync();
    Task<IEnumerable<WarehouseDto>> GetActiveWarehousesAsync();
    Task<WarehouseDto> GetMarketWarehousesAsync();
    Task<WarehouseDto?> GetByIdAsync(Guid id);
    Task<PagedResult<WarehouseDto>> SearchAsync(WarehouseSearchCriteriaModel criteria);
    Task<PagedResult<WarehouseDto>> SearchVendorAsync(WarehouseSearchCriteriaModel criteria);
    Task<bool> SaveAsync(WarehouseDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);

	Task<IEnumerable<VendorDto>> GetVendorsAsync();
	Task<bool> IsMultiVendorEnabledAsync();
    Task<IEnumerable<VendorWithUserDto>> GetVendorUsersAsync();
}
