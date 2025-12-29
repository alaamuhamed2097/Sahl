namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    public class AssignTrackingNumberRequest
    {
        public Guid ShipmentId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
