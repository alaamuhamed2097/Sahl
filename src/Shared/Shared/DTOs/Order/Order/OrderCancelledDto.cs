using Common.Enumerations.Order;
using Shared.DTOs.Base;

namespace Shared.DTOs.Order.Order
{
    /// <summary>
    /// Cancel Order Response
    /// </summary>
    public class OrderCancelledDto : BaseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public OrderProgressStatus NewStatus { get; set; }
        public bool WasRefunded { get; set; }
        public decimal? RefundAmount { get; set; }
        public string Message { get; set; } = null!;
    }
}
