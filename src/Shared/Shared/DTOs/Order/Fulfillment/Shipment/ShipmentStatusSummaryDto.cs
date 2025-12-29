namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    public class ShipmentStatusSummaryDto
    {
        public int TotalShipments { get; set; }
        public int PendingShipments { get; set; }
        public int ProcessingShipments { get; set; }
        public int InTransitShipments { get; set; }
        public int DeliveredShipments { get; set; }
        public int CancelledShipments { get; set; }
        public int FailedShipments { get; set; }
        public Dictionary<string, int> ShipmentsByStatus { get; set; }
    }
}