using Shared.DTOs.ECommerce.Order;

namespace BL.Services.Order
{
    public interface IOrderService
    {
        // Stage 3: Order Creation
        Task<OrderCreatedResponseDto> CreateOrderFromCartAsync(string customerId, CreateOrderFromCartRequest request);
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<OrderDto> GetOrderByNumberAsync(string orderNumber);
        Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10);
        Task<OrderDto> GetOrderWithShipmentsAsync(Guid orderId);
        
        // Order Management
        Task<OrderCompletionStatusDto> GetOrderCompletionStatusAsync(Guid orderId);
        Task<bool> CancelOrderAsync(Guid orderId, string reason, string? adminNotes = null);
        Task<bool> ValidateOrderAsync(Guid orderId);
    }
}
