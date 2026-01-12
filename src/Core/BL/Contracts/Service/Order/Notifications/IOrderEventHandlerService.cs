using Shared.DTOs.Order.OrderEvents;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Contracts.Service.Order.Notifications;

/// <summary>
/// Interface for handling order lifecycle events
/// </summary>
public interface IOrderEventHandlerService
{
    /// <summary>
    /// Handles order payment confirmation
    /// Creates shipments and initiates fulfillment
    /// </summary>
    Task<InvoiceActivationResult> OnOrderPaid(
        OrderPaidEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order completion
    /// Processes cash on delivery payments
    /// </summary>
    Task<bool> OnOrderCompleted(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order cancellation
    /// Releases inventory and processes refunds
    /// </summary>
    Task<bool> OnOrderCanceled(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order refund request
    /// Validates eligibility and processes refund
    /// </summary>
    Task<bool> OnOrderRefund(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order pending status
    /// </summary>
    Task<bool> OnOrderPending(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order accepted/confirmed status
    /// </summary>
    Task<bool> OnOrderAccepted(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order rejected status
    /// </summary>
    Task<bool> OnOrderRejected(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order in progress status
    /// </summary>
    Task<bool> OnOrderInProgress(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order shipping status
    /// </summary>
    Task<bool> OnOrderShipping(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Handles order not active status
    /// </summary>
    Task<bool> OnOrderNotActive(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default);
}