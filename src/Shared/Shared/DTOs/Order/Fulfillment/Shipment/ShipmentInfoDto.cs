using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    public class ShipmentInfoDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public ShipmentStatus Status { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public List<Guid> ItemIds { get; set; } = new();
    }
}