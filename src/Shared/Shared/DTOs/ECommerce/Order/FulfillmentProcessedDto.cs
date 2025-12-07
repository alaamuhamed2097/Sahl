using Common.Enumerations.Shipping;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 6: Fulfillment Result
    /// </summary>
    public class FulfillmentProcessedDto : BaseDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public ShipmentStatus NewStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string Message { get; set; } = null!;
    }
}
