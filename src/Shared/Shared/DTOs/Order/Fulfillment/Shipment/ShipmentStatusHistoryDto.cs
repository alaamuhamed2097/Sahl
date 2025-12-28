namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    public class ShipmentStatusHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime StatusDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}
