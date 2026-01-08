using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Warehouse
{
    public interface IWarehouseService
    {
        Task<ResponseModel<IEnumerable<WarehouseDto>>> GetAllAsync();
        Task<ResponseModel<IEnumerable<WarehouseDto>>> GetActiveWarehousesAsync();
        Task<ResponseModel<WarehouseDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<PaginatedDataModel<WarehouseDto>>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<ResponseModel<WarehouseDto>> SaveAsync(WarehouseDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> ToggleStatusAsync(Guid id);
    }
}
