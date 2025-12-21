using Common.Enumerations.Shipping;
using Shared.DTOs.Base;

namespace Shared.DTOs.Order.Shipment
{
    /// <summary>
    /// Stage 7: Shipment Tracking
    /// </summary>
    public class ShipmentTrackingDto : BaseDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public ShipmentStatus CurrentStatus { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public List<TrackingEventDto> Events { get; set; } = new();
    }
}
