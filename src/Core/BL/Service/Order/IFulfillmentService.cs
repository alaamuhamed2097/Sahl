using Shared.DTOs.ECommerce.Shipment;

namespace BL.Services.Order
{
    public interface IFulfillmentService
    {
        Task ProcessFBAShipmentAsync(Guid shipmentId);
        Task ProcessFBMShipmentAsync(Guid shipmentId);
        Task<bool> ReserveInventoryAsync(Guid shipmentId);
        Task<bool> ReleaseInventoryAsync(Guid shipmentId);
        Task<string> DetermineFulfillmentTypeAsync(Guid warehouseId);
    }
}
