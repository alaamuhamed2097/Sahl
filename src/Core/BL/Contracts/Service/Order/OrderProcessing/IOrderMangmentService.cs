using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.ResponseOrderDetail;

namespace BL.Contracts.Service.Order.OrderProcessing;

public interface IOrderMangmentService
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

	// Detailed Order Information
	Task<IEnumerable<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<ResponseOrderDetailsDto?> GetOrderDetailsByIdAsync(
        Guid orderId,
        string userId,
        CancellationToken cancellationToken = default);
    Task<List<ResponseOrderItemDetailsDto>> GetListByOrderIdAsync(
    Guid orderId,
    string? userId = null);
}
