using Common.Enumerations.Fulfillment;

namespace BL.Contracts.Service.Order.Fulfillment;

public interface IFulfillmentService
{
    Task ProcessFBAShipmentAsync(Guid shipmentId);
    Task ProcessFBMShipmentAsync(Guid shipmentId);
    Task<bool> ReserveInventoryAsync(Guid shipmentId);
    Task<bool> ReleaseInventoryAsync(Guid shipmentId);
    Task<FulfillmentType> DetermineFulfillmentTypeAsync(Guid warehouseId);
}
