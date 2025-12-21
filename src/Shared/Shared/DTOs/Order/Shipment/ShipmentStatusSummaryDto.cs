using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.Shipment
{
    /// <summary>
    /// Shipment status summary
    /// </summary>
    public class ShipmentStatusSummaryDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public ShipmentStatus Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int ItemCount { get; set; }
    }
}
