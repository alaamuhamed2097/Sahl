using Common.Enumerations.Shipping;
using Shared.DTOs.Base;

namespace Shared.DTOs.Order.Shipment
{
    /// <summary>
    /// Stage 7: Shipping Started Response
    /// </summary>
    public class ShippingStartedDto : BaseDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public ShipmentStatus CurrentStatus { get; set; }
        public string Message { get; set; } = null!;
    }
}
