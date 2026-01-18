using BL.Contracts.Service.Order.Notifications;
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

    public async Task<bool> HandleStatusChangeAsync(Guid orderId, OrderProgressStatus newStatus, string userId)
    {
        OrderEvent orderEvent = new() { OrderId = orderId, UserId = userId };

        switch (newStatus)
        {
            case OrderProgressStatus.Pending:
                return await _orderEventHandler.OnOrderPending(orderEvent);

            case OrderProgressStatus.Confirmed:
                return await _orderEventHandler.OnOrderAccepted(orderEvent);

            case OrderProgressStatus.Processing:
                return await _orderEventHandler.OnOrderInProgress(orderEvent);

            case OrderProgressStatus.Shipped:
                return await _orderEventHandler.OnOrderShipping(orderEvent);

            case OrderProgressStatus.Delivered:
            case OrderProgressStatus.Completed:
                return await _orderEventHandler.OnOrderCompleted(orderEvent);

            case OrderProgressStatus.Cancelled:
                return await _orderEventHandler.OnOrderCanceled(orderEvent);

            case OrderProgressStatus.Returned:
            case OrderProgressStatus.Refunded:
                return await _orderEventHandler.OnOrderRefund(orderEvent);

            case OrderProgressStatus.PaymentFailed:
                return await _orderEventHandler.OnOrderRejected(orderEvent);

            case OrderProgressStatus.RefundRequested:
                // Handle refund request - you may need a new handler method
                return await _orderEventHandler.OnOrderRefund(orderEvent);

            default:
                throw new ArgumentOutOfRangeException(nameof(newStatus), $"Unsupported status: {newStatus}");
        }
    }
}