using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Contracts.Service.Order.Fulfillment;

public interface IDeliveryService
{
    Task<bool> CompleteDeliveryAsync(Guid shipmentId);
    Task<bool> ConfirmOrderCompletionAsync(Guid orderId);
    Task<bool> InitiateReturnAsync(Guid shipmentId, string reason);
    Task<bool> ProcessReturnAsync(Guid shipmentId, bool approved);
    Task<ShipmentDto> GetShipmentDeliveryInfoAsync(Guid shipmentId);
}
