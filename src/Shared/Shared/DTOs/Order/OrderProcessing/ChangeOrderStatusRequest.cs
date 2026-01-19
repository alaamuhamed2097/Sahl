using Common.Enumerations.Order;

namespace Shared.DTOs.Order.OrderProcessing
{
    /// <summary>
    /// Request DTO for changing order status
    /// Used by Admin Dashboard (Details.razor)
    /// </summary>
    public class ChangeOrderStatusRequest
    {
        /// <summary>
        /// New order status
        /// </summary>
        public OrderProgressStatus NewStatus { get; set; }

        /// <summary>
        /// Optional notes about the status change
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Admin user ID making the change
        /// </summary>
        public string? AdminUserId { get; set; }
    }
}