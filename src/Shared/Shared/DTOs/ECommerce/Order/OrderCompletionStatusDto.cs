using Common.Enumerations.Order;
using Shared.DTOs.Base;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Order completion status
    /// </summary>
    public class OrderCompletionStatusDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public int TotalShipments { get; set; }
        public int DeliveredShipments { get; set; }
        public int PendingShipments { get; set; }
        public OrderProgressStatus OrderStatus { get; set; }
        public bool IsComplete { get; set; }
        public decimal CompletionPercentage { get; set; }
        public List<ShipmentStatusSummaryDto> ShipmentStatuses { get; set; } = new();
    }
}
