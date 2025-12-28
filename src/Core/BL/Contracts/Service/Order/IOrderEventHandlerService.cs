using Shared.DTOs.Order.OrderEvents;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.Events.Order;

namespace BL.Contracts.Service.Order;

/// <summary>
/// Interface for Order Event Handler Service
/// </summary>
public interface IOrderEventHandlerService
{
    /// <summary>
    /// Handles order paid event
    /// </summary>
    Task<InvoiceActivationResult> OnOrderPaid(OrderPaidEvent orderEvent);

    /// <summary>
    /// Handles order completed event
    /// </summary>
    Task<bool> OnOrderCompleted(OrderEvent orderEvent);

    /// <summary>
    /// Handles order cancelled event
    /// </summary>
    Task<bool> OnOrderCanceled(OrderEvent orderEvent);

    /// <summary>
    /// Handles order refund event
    /// </summary>
    Task<bool> OnOrderRefund(OrderEvent orderEvent);

    /// <summary>
    /// Handles order pending event
    /// </summary>
    Task<bool> OnOrderPending(OrderEvent orderEvent);

    /// <summary>
    /// Handles order accepted event
    /// </summary>
    Task<bool> OnOrderAccepted(OrderEvent orderEvent);

    /// <summary>
    /// Handles order rejected event
    /// </summary>
    Task<bool> OnOrderRejected(OrderEvent orderEvent);

    /// <summary>
    /// Handles order in progress event
    /// </summary>
    Task<bool> OnOrderInProgress(OrderEvent orderEvent);

    /// <summary>
    /// Handles order shipping event
    /// </summary>
    Task<bool> OnOrderShipping(OrderEvent orderEvent);

    /// <summary>
    /// Handles order not active event
    /// </summary>
    Task<bool> OnOrderNotActive(OrderEvent orderEvent);
}
