namespace Common.Enumerations.Notification;

/// <summary>
/// Notification types for order lifecycle events
/// These should be added to the existing NotificationType enum
/// </summary>
public enum OrderNotificationType
{
    // ============================================
    // ORDER LIFECYCLE NOTIFICATIONS
    // ============================================

    /// <summary>
    /// Sent when order is successfully created
    /// Channels: Email, SMS, SignalR
    /// </summary>
    OrderCreated = 100,

    /// <summary>
    /// Sent when order payment is confirmed
    /// Channels: Email, SignalR
    /// </summary>
    PaymentConfirmed = 101,

    /// <summary>
    /// Sent when payment fails
    /// Channels: Email, SignalR
    /// </summary>
    PaymentFailed = 102,

    /// <summary>
    /// Reminder for pending payment
    /// Channels: Email, SignalR
    /// </summary>
    PaymentReminder = 103,

    /// <summary>
    /// Sent when order is confirmed by merchant
    /// Channels: Email, SMS, SignalR
    /// </summary>
    OrderConfirmed = 104,

    /// <summary>
    /// Sent when order is being prepared/processed
    /// Channels: SignalR
    /// </summary>
    OrderProcessing = 105,

    /// <summary>
    /// Sent when order is shipped
    /// Channels: Email, SMS, SignalR
    /// </summary>
    OrderShipped = 106,

    /// <summary>
    /// Sent when order is out for delivery
    /// Channels: SMS, SignalR
    /// </summary>
    OrderOutForDelivery = 107,

    /// <summary>
    /// Sent when order is delivered
    /// Channels: Email, SMS, SignalR
    /// </summary>
    OrderDelivered = 108,

    /// <summary>
    /// Sent when order is cancelled
    /// Channels: Email, SMS, SignalR
    /// </summary>
    OrderCancelled = 109,

    /// <summary>
    /// Sent when refund is processed
    /// Channels: Email, SignalR
    /// </summary>
    RefundProcessed = 110,

    /// <summary>
    /// Sent when shipment status changes
    /// Channels: SignalR
    /// </summary>
    ShipmentStatusChanged = 111,

    // ============================================
    // DELIVERY NOTIFICATIONS
    // ============================================

    /// <summary>
    /// Delivery attempt failed
    /// Channels: SMS, SignalR
    /// </summary>
    DeliveryAttemptFailed = 120,

    /// <summary>
    /// Package delayed notification
    /// Channels: Email, SMS, SignalR
    /// </summary>
    DeliveryDelayed = 121,

    /// <summary>
    /// Delivery rescheduled
    /// Channels: SMS, SignalR
    /// </summary>
    DeliveryRescheduled = 122,

    // ============================================
    // REVIEW & FEEDBACK NOTIFICATIONS
    // ============================================

    /// <summary>
    /// Request for order review after delivery
    /// Channels: Email, SignalR
    /// </summary>
    ReviewRequest = 130,

    /// <summary>
    /// Thank you for review
    /// Channels: Email, SignalR
    /// </summary>
    ReviewThankYou = 131
}
