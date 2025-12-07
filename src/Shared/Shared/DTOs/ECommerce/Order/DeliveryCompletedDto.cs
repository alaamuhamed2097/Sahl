using Common.Enumerations.Shipping;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 8: Delivery Completed Response
    /// </summary>
    public class DeliveryCompletedDto : BaseDto
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public DateTime DeliveryDate { get; set; }
        public ShipmentStatus CurrentStatus { get; set; }
        public bool IsOrderComplete { get; set; }
        public string Message { get; set; } = null!;
    }
}
