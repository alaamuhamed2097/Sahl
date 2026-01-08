using Common.Enumerations.Shipping;

namespace BL.Contracts.Service.Order;

/// <summary>
/// Interface for order lifecycle notifications
/// </summary>
public interface IOrderNotificationService
{
    /// <summary>
    /// Send notification when order is created
    /// </summary>
    Task NotifyOrderCreatedAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order payment is confirmed
    /// </summary>
    Task NotifyOrderPaidAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is confirmed/accepted
    /// </summary>
    Task NotifyOrderConfirmedAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is being processed
    /// </summary>
    Task NotifyOrderProcessingAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is shipped
    /// </summary>
    Task NotifyOrderShippedAsync(Guid orderId, Guid? shipmentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is out for delivery
    /// </summary>
    Task NotifyOrderOutForDeliveryAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is delivered
    /// </summary>
    Task NotifyOrderDeliveredAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when order is cancelled
    /// </summary>
    Task NotifyOrderCancelledAsync(Guid orderId, string cancellationReason = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when refund is processed
    /// </summary>
    Task NotifyOrderRefundedAsync(Guid orderId, decimal refundAmount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when payment fails
    /// </summary>
    Task NotifyPaymentFailedAsync(Guid orderId, string failureReason = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Send reminder for pending payment
    /// </summary>
    Task NotifyPaymentReminderAsync(Guid orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send notification when shipment status changes
    /// </summary>
    Task NotifyShipmentStatusChangedAsync(Guid shipmentId, ShipmentStatus newStatus, CancellationToken cancellationToken = default);
}