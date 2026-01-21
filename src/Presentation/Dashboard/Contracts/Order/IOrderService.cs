using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Order
{
    /// <summary>
    /// Interface for Order service - CLEAN VERSION
    /// Works directly with API DTOs without intermediate mapping
    /// This is proper business logic - one DTO system throughout
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get order by ID with full details
        /// Returns AdminOrderDetailsDto directly from API
        /// </summary>
        Task<ResponseModel<AdminOrderDetailsDto>> GetOrderByIdAsync(Guid orderId);

        /// <summary>
        /// Change order status
        /// </summary>
        Task<ResponseModel<bool>> ChangeOrderStatusAsync(ChangeOrderStatusRequest request);

        /// <summary>
        /// Update order details
        /// </summary>
        Task<ResponseModel<bool>> UpdateOrderAsync(UpdateOrderRequest request);

        /// <summary>
        /// Cancel order
        /// </summary>
        Task<ResponseModel<bool>> CancelOrderAsync(Guid orderId, string reason);

        /// <summary>
        /// Get today's orders count for dashboard statistics
        /// </summary>
        Task<ResponseModel<int>> GetTodayOrdersCountAsync();
    }
}