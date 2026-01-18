using Shared.DTOs.Warehouse;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Warehouse
{
    public interface IVendorWarehouseService
    {
        Task<ResponseModel<WarehouseDto>> GetMarketWarehouseAsync(string userId);
    }
}