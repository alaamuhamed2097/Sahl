using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.Service.Order;
using Common.Enumerations.Notification;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Services.Order.Notifications;

/// <summary>
/// Service responsible for sending notifications throughout the order lifecycle
/// Handles order creation, payment, shipping, delivery, and cancellation notifications
/// </summary>
public class OrderNotificationService : IOrderNotificationService
{
    private readonly INotificationService _notificationService;
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ILogger _logger;

    public OrderNotificationService(
        INotificationService notificationService,
        IOrderRepository orderRepository,
        IShipmentRepository shipmentRepository,
        ILogger logger)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _shipmentRepository = shipmentRepository ?? throw new ArgumentNullException(nameof(shipmentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Order Lifecycle Notifications

    /// <summary>
    /// Send notification when order is created
    /// </summary>
    public async Task NotifyOrderCreatedAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null)
            {
                _logger.Error("Order {OrderId} not found for notification", orderId);
                return;
            }

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "OrderDate", order.CreatedDateUtc.ToString("dd/MM/yyyy") },
                { "TotalAmount", order.Price.ToString("N2") },
                { "ItemCount", order.OrderDetails?.Count.ToString() ?? "0" },
                { "CustomerName", order.User?.FirstName ?? "" }
            };

            // Email notification
            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.OrderCreated,
                subject: $"Order Confirmation - {order.Number}",
                title: "Order Created Successfully",
                parameters: parameters);

            // SMS notification
            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderCreated,
                subject: "",
                title: $"Your order {order.Number} has been confirmed",
                parameters: parameters);

            // SignalR notification
            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderCreated,
                subject: "",
                title: "Order Created Successfully",
                parameters: parameters,
                imagePath: "/images/notifications/order-created.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order created notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order payment is confirmed
    /// </summary>
    public async Task NotifyOrderPaidAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "PaymentDate", order.PaidAt?.ToString("dd/MM/yyyy HH:mm") ?? "" },
                { "Amount", order.Price.ToString("N2") },
                { "PaymentMethod", GetPaymentMethodName(order.PaymentStatus) }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.PaymentConfirmed,
                subject: $"Payment Confirmed - Order {order.Number}",
                title: "Payment Received",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.PaymentConfirmed,
                subject: "",
                title: "Payment Confirmed",
                parameters: parameters,
                imagePath: "/images/notifications/payment-confirmed.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send payment confirmation notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is confirmed/accepted
    /// </summary>
    public async Task NotifyOrderConfirmedAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "EstimatedDelivery", DateTime.UtcNow.AddDays(3).ToString("dd/MM/yyyy") }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.OrderConfirmed,
                subject: $"Order Confirmed - {order.Number}",
                title: "Order Confirmed",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderConfirmed,
                subject: "",
                title: $"Your order {order.Number} is confirmed",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderConfirmed,
                subject: "",
                title: "Order Confirmed",
                parameters: parameters,
                imagePath: "/images/notifications/order-confirmed.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order confirmed notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is being processed
    /// </summary>
    public async Task NotifyOrderProcessingAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number }
            };

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderProcessing,
                subject: "",
                title: "Order is Being Prepared",
                parameters: parameters,
                imagePath: "/images/notifications/order-processing.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order processing notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is shipped
    /// </summary>
    public async Task NotifyOrderShippedAsync(
        Guid orderId,
        Guid? shipmentId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            string trackingNumber = "";
            string estimatedDelivery = "";

            if (shipmentId.HasValue)
            {
                var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                    shipmentId.Value,
                    cancellationToken);

                trackingNumber = shipment?.TrackingNumber ?? "";
                estimatedDelivery = shipment?.EstimatedDeliveryDate?.ToString("dd/MM/yyyy") ?? "";
            }

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "TrackingNumber", trackingNumber },
                { "EstimatedDelivery", estimatedDelivery }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.OrderShipped,
                subject: $"Order Shipped - {order.Number}",
                title: "Your Order is On the Way",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderShipped,
                subject: "",
                title: $"Order {order.Number} shipped. Track: {trackingNumber}",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderShipped,
                subject: "",
                title: "Order Shipped",
                parameters: parameters,
                imagePath: "/images/notifications/order-shipped.png",
                callToActionUrl: $"/orders/{order.Id}/tracking");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order shipped notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is out for delivery
    /// </summary>
    public async Task NotifyOrderOutForDeliveryAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "EstimatedArrival", DateTime.UtcNow.AddHours(2).ToString("HH:mm") }
            };

            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderOutForDelivery,
                subject: "",
                title: $"Order {order.Number} is out for delivery",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderOutForDelivery,
                subject: "",
                title: "Order Out for Delivery",
                parameters: parameters,
                imagePath: "/images/notifications/out-for-delivery.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send out for delivery notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is delivered
    /// </summary>
    public async Task NotifyOrderDeliveredAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "DeliveryDate", order.OrderDeliveryDate?.ToString("dd/MM/yyyy HH:mm") ?? "" }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.OrderDelivered,
                subject: $"Order Delivered - {order.Number}",
                title: "Order Delivered Successfully",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderDelivered,
                subject: "",
                title: $"Order {order.Number} has been delivered",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderDelivered,
                subject: "",
                title: "Order Delivered",
                parameters: parameters,
                imagePath: "/images/notifications/order-delivered.png",
                callToActionUrl: $"/orders/{order.Id}/review");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order delivered notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when order is cancelled
    /// </summary>
    public async Task NotifyOrderCancelledAsync(
        Guid orderId,
        string cancellationReason = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "CancellationReason", cancellationReason },
                { "RefundAmount", order.Price.ToString("N2") }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.OrderCancelled,
                subject: $"Order Cancelled - {order.Number}",
                title: "Order Cancelled",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.User?.PhoneNumber ?? "",
                channel: NotificationChannel.Sms,
                type: OrderNotificationType.OrderCancelled,
                subject: "",
                title: $"Order {order.Number} has been cancelled",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.OrderCancelled,
                subject: "",
                title: "Order Cancelled",
                parameters: parameters,
                imagePath: "/images/notifications/order-cancelled.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send order cancelled notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when refund is processed
    /// </summary>
    public async Task NotifyOrderRefundedAsync(
        Guid orderId,
        decimal refundAmount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "RefundAmount", refundAmount.ToString("N2") },
                { "ProcessingTime", "3-5 business days" }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.RefundProcessed,
                subject: $"Refund Processed - Order {order.Number}",
                title: "Refund Processed",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.RefundProcessed,
                subject: "",
                title: "Refund Processed",
                parameters: parameters,
                imagePath: "/images/notifications/refund-processed.png",
                callToActionUrl: $"/orders/{order.Id}");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send refund notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send notification when payment fails
    /// </summary>
    public async Task NotifyPaymentFailedAsync(
        Guid orderId,
        string failureReason = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "FailureReason", failureReason },
                { "Amount", order.Price.ToString("N2") }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.PaymentFailed,
                subject: $"Payment Failed - Order {order.Number}",
                title: "Payment Failed",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.PaymentFailed,
                subject: "",
                title: "Payment Failed",
                parameters: parameters,
                imagePath: "/images/notifications/payment-failed.png",
                callToActionUrl: $"/orders/{order.Id}/payment/retry");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send payment failed notification for order {OrderId}", orderId);
        }
    }

    /// <summary>
    /// Send reminder for pending payment
    /// </summary>
    public async Task NotifyPaymentReminderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null) return;

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", order.Number },
                { "Amount", order.Price.ToString("N2") },
                { "ExpiryTime", DateTime.UtcNow.AddHours(24).ToString("dd/MM/yyyy HH:mm") }
            };

            await SendNotificationAsync(
                recipient: order.User?.Email ?? "",
                channel: NotificationChannel.Email,
                type: OrderNotificationType.PaymentReminder,
                subject: $"Payment Reminder - Order {order.Number}",
                title: "Complete Your Payment",
                parameters: parameters);

            await SendNotificationAsync(
                recipient: order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.PaymentReminder,
                subject: "",
                title: "Payment Reminder",
                parameters: parameters,
                imagePath: "/images/notifications/payment-reminder.png",
                callToActionUrl: $"/orders/{order.Id}/payment");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send payment reminder for order {OrderId}", orderId);
        }
    }

    #endregion

    #region Shipment Notifications

    /// <summary>
    /// Send notification when shipment status changes
    /// </summary>
    public async Task NotifyShipmentStatusChangedAsync(
        Guid shipmentId,
        ShipmentStatus newStatus,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var shipment = await _shipmentRepository.GetShipmentWithDetailsAsync(
                shipmentId,
                cancellationToken);

            if (shipment?.Order == null) return;

            var statusMessage = GetShipmentStatusMessage(newStatus);

            var parameters = new Dictionary<string, string>
            {
                { "OrderNumber", shipment.Order.Number },
                { "TrackingNumber", shipment.TrackingNumber ?? "" },
                { "Status", statusMessage },
                { "UpdateTime", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm") }
            };

            await SendNotificationAsync(
                recipient: shipment.Order.UserId,
                channel: NotificationChannel.SignalR,
                type: OrderNotificationType.ShipmentStatusChanged,
                subject: "",
                title: $"Shipment {statusMessage}",
                parameters: parameters,
                imagePath: "/images/notifications/shipment-update.png",
                callToActionUrl: $"/orders/{shipment.OrderId}/tracking");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to send shipment status notification for shipment {ShipmentId}", shipmentId);
        }
    }

    #endregion

    #region Private Helper Methods

    private async Task SendNotificationAsync(
        string recipient,
        NotificationChannel channel,
        OrderNotificationType type,
        string subject,
        string title,
        Dictionary<string, string> parameters,
        string? imagePath = null,
        string? callToActionUrl = null)
    {
        if (string.IsNullOrEmpty(recipient))
        {
            return;
        }

        var request = new NotificationRequest
        {
            Recipient = recipient,
            Channel = channel,
            Type = (NotificationType)type, // Cast to NotificationType
            Subject = subject,
            Title = title,
            ImagePath = imagePath,
            CallToActionUrl = callToActionUrl,
            Parameters = parameters
        };

        await _notificationService.SendNotificationAsync(request);
    }

    private string GetPaymentMethodName(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => "Pending",
            PaymentStatus.Completed => "Completed",
            PaymentStatus.Failed => "Failed",
            PaymentStatus.Refunded => "Refunded",
            _ => "Unknown"
        };
    }

    private string GetShipmentStatusMessage(ShipmentStatus status)
    {
        return status switch
        {
            ShipmentStatus.Pending => "Pending",
            ShipmentStatus.Processing => "Being Prepared",
            ShipmentStatus.Shipped => "Shipped",
            ShipmentStatus.InTransit => "In Transit",
            ShipmentStatus.OutForDelivery => "Out for Delivery",
            ShipmentStatus.Delivered => "Delivered",
            ShipmentStatus.Cancelled => "Cancelled",
            ShipmentStatus.Returned => "Returned",
            _ => "Unknown"
        };
    }

    #endregion
}