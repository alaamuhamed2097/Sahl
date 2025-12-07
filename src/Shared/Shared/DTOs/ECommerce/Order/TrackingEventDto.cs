using Common.Enumerations.Shipping;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Tracking event for shipment
    /// </summary>
    public class TrackingEventDto
    {
        public ShipmentStatus Status { get; set; }
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
    }
}
