using Shared.DTOs.ECommerce.Shipment;

namespace BL.Contracts.Service.Order
{
    public interface IShipmentService
    {
        Task<List<ShipmentDto>> SplitOrderIntoShipmentsAsync(Guid orderId);
        Task<ShipmentDto> GetShipmentByIdAsync(Guid shipmentId);
        Task<ShipmentDto> GetShipmentByNumberAsync(string shipmentNumber);
        Task<List<ShipmentDto>> GetOrderShipmentsAsync(Guid orderId);
        Task<ShipmentDto> UpdateShipmentStatusAsync(Guid shipmentId, string newStatus, string? location = null, string? notes = null);
        Task<ShipmentDto> AssignTrackingNumberAsync(Guid shipmentId, string trackingNumber, DateTime? estimatedDeliveryDate = null);
        Task<ShipmentTrackingDto> TrackShipmentAsync(string trackingNumber);
        Task<List<ShipmentDto>> GetVendorShipmentsAsync(Guid vendorId, int pageNumber = 1, int pageSize = 10);
    }
}
