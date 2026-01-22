using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Dashboard.Contracts.Warehouse
{
    public interface IWarehouseService
    {
        Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllAsync();
        Task<ResponseModel<IEnumerable<WarehouseDto>>> GetActiveWarehousesAsync();
        Task<ResponseModel<WarehouseDto>> GetByIdAsync(Guid id);
        Task<WarehouseDto?> GetMarketWarehouse();

		Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchAsync(WarehouseSearchCriteriaModel criteria);
        Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchVendorAsync(WarehouseSearchCriteriaModel criteria);
		Task<ResponseModel<IEnumerable<VendorWithUserDto>>> GetActiveVendorsAsync();
		Task<ResponseModel<WarehouseDto>> SaveAsync(WarehouseDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> ToggleStatusAsync(Guid id);
		Task<ResponseModel<IEnumerable<VendorDto>>> GetVendorsAsync();
		Task<ResponseModel<bool>> IsMultiVendorEnabledAsync();
	}
}
