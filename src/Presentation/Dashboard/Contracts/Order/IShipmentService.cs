using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Order
{
    public interface IShipmentService
    {
        Task<ResponseModel<List<ShipmentDto>>> GetOrderShipmentsAsync(Guid orderId);
        Task<ResponseModel<ShipmentTrackingDto>> GetShipmentTrackingAsync(string trackingNumber);
    }
}
