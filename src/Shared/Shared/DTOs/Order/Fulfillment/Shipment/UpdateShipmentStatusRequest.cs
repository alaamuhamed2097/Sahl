namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    public class UpdateShipmentStatusRequest
    {
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
        public string NewStatus { get; set; } = null!;
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}
