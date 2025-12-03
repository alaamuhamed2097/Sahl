using Shared.DTOs.ECommerce.Order;

namespace BL.Services.Order
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string customerId, CreateOrderRequest request);
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<OrderDto> GetOrderByNumberAsync(string orderNumber);
        Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10);
        Task<bool> CancelOrderAsync(Guid orderId, string reason);
        Task<OrderDto> GetOrderWithShipmentsAsync(Guid orderId);
    }
}
