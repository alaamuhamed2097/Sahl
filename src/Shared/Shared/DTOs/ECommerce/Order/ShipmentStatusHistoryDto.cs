using Common.Enumerations.Shipping;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Shipment status history entry
    /// </summary>
    public class ShipmentStatusHistoryDto
    {
        public Guid HistoryId { get; set; }
        public ShipmentStatus Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}
