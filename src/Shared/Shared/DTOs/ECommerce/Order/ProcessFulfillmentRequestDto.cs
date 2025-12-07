namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 6: Fulfillment Processing
    /// </summary>
    public class ProcessFulfillmentRequest
    {
        public Guid ShipmentId { get; set; }
        public string? Notes { get; set; }
    }
}
