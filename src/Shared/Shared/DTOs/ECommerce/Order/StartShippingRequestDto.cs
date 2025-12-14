namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 7: Start Shipping Request
    /// </summary>
    public class StartShippingRequest
    {
        public Guid ShipmentId { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string? Notes { get; set; }
    }
}
