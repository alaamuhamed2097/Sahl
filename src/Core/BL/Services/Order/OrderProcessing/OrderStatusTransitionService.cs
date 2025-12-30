using BL.Contracts.Service.Order;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Order;
using Shared.DTOs.Order.OrderEvents;

namespace BL.Services.Order.OrderProcessing;

public class OrderStatusTransitionService : IOrderStatusTransitionService
{
    private readonly IOrderEventHandlerService _orderEventHandler;

    public OrderStatusTransitionService(IOrderEventHandlerService orderEventHandler)
    {
        _orderEventHandler = orderEventHandler;
    }

    public async Task<bool> HandleStatusChangeAsync(Guid orderId, OrderStatus newStatus, string userId)
    {
        OrderEvent orderEvent = new() { OrderId = orderId, UserId = userId };
        switch (newStatus)
        {
            case OrderStatus.NotActive:
                return await _orderEventHandler.OnOrderNotActive(orderEvent);

            case OrderStatus.Pending:
                return await _orderEventHandler.OnOrderPending(orderEvent);

            case OrderStatus.Accepted:
                return await _orderEventHandler.OnOrderAccepted(orderEvent);

            case OrderStatus.Rejected:
                return await _orderEventHandler.OnOrderRejected(orderEvent);

            case OrderStatus.InProgress:
                return await _orderEventHandler.OnOrderInProgress(orderEvent);

            case OrderStatus.Shipping:
                return await _orderEventHandler.OnOrderShipping(orderEvent);

            case OrderStatus.Delivered:
                return await _orderEventHandler.OnOrderCompleted(orderEvent);

            case OrderStatus.Canceled:
                return await _orderEventHandler.OnOrderCanceled(orderEvent);

            case OrderStatus.Returned:
                return await _orderEventHandler.OnOrderRefund(orderEvent);

            default:
                throw new ArgumentOutOfRangeException(nameof(newStatus), $"Unsupported status: {newStatus}");
        }
    }
}
