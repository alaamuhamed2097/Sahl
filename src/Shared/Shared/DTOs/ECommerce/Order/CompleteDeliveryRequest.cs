using Common.Enumerations.Shipping;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Stage 8: Complete Delivery Request
    /// </summary>
    public class CompleteDeliveryRequest
    {
        public Guid ShipmentId { get; set; }
        public string? ReceivedBy { get; set; }
        public string? Notes { get; set; }
    }
}
