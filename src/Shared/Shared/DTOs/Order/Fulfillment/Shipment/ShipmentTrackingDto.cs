using Shared.DTOs.Order.Checkout.Address;

namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    // <summary>
    /// Shipment tracking DTO
    /// </summary>
    public class ShipmentTrackingDto
    {
        public string ShipmentNumber { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public string? ShippingCompanyName { get; set; }
        public DeliveryAddressDto? DeliveryAddress { get; set; }
        public List<StatusHistoryDto> StatusHistory { get; set; } = new();
    }

}