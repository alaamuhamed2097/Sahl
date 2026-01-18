using Shared.DTOs.Warehouse;

namespace BL.Contracts.Service.VendorWarehouse
{
    public interface IVendorWarehouseService
    {
        Task<WarehouseDto> GetMarketWarehousesAsync();
        Task<IEnumerable<WarehouseDto>> GetVendorAvailableWarehousesByUserIdAsync(string userId);
        Task<IEnumerable<WarehouseDto>> GetVendorAvailableWarehousesByVendorIdAsync(Guid userId);
    }
}