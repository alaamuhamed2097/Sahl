using Common.Enumerations.Order;

namespace Shared.DTOs.Order.OrderProcessing
{
    /// <summary>
    /// Request DTO for changing order status
    /// Used by Admin Dashboard (Details.razor)
    /// </summary>
    public class ChangeOrderStatusRequest
    {
        public Guid OrderId { get; set; }

        /// <summary>
        /// New order status
        /// </summary>
        public OrderProgressStatus NewStatus { get; set; }

        /// <summary>
        /// Optional notes about the status change
        /// </summary>
        public string? Notes { get; set; }
    }
}