using BL.Contracts.IMapper;
using BL.Contracts.Service.Order;
using BL.Contracts.Service.Order.Fulfillment;
using BL.Contracts.Service.Order.Notifications;
using BL.Contracts.Service.Wallet.Customer;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order;
using Serilog;
using Shared.DTOs.Order.OrderEvents;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order;

/// <summary>
/// Handles order lifecycle events with integrated notifications
/// Coordinates between order status, shipment creation, fulfillment, and customer notifications
/// </summary>
public class OrderEventHandlerService : IOrderEventHandlerService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly IShipmentService _shipmentService;
    private readonly IFulfillmentService _fulfillmentService;
    private readonly ICustomerWalletService _walletService;
    private readonly IOrderNotificationService _notificationService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public OrderEventHandlerService(
        IOrderRepository orderRepository,
        IOrderPaymentRepository paymentRepository,
        IShipmentService shipmentService,
        IFulfillmentService fulfillmentService,
        ICustomerWalletService walletService,
        IOrderNotificationService notificationService,
        IBaseMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _shipmentService = shipmentService ?? throw new ArgumentNullException(nameof(shipmentService));
        _fulfillmentService = fulfillmentService ?? throw new ArgumentNullException(nameof(fulfillmentService));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles order payment confirmation event
    /// Updates order status, creates shipments, initiates fulfillment, and sends notifications
    /// </summary>
    public async Task<InvoiceActivationResult> OnOrderPaid(
        OrderPaidEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await FindOrderAsync(orderEvent.OrderId, orderEvent.InvoiceId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order not found - OrderId: {orderEvent.OrderId}, InvoiceId: {orderEvent.InvoiceId}");
            }

            // Check if already processed
            if (order.PaymentStatus == PaymentStatus.Completed)
            {
                return new InvoiceActivationResult { IsActivated = true };
            }

            // Update order payment status
            order.PaymentStatus = PaymentStatus.Completed;
            order.PaidAt = DateTime.UtcNow;
            order.OrderStatus = OrderProgressStatus.Processing;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            // Update payment record
            var payment = await _paymentRepository.GetOrderPaymentAsync(order.Id, cancellationToken);
            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;
                payment.UpdatedDateUtc = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);
            }

            // Send payment confirmation notification
            await _notificationService.NotifyOrderPaidAsync(order.Id, cancellationToken);

            // Create shipments from order
            var shipments = await _shipmentService.SplitOrderIntoShipmentsAsync(order.Id);

            // Process fulfillment for each shipment
            foreach (var shipment in shipments)
            {
                await ProcessShipmentFulfillmentAsync(shipment.Id, order.Id, cancellationToken);
            }

            // Send order processing notification
            await _notificationService.NotifyOrderProcessingAsync(order.Id, cancellationToken);

            return new InvoiceActivationResult { IsActivated = true };
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Failed to process OnOrderPaid - OrderId: {OrderId}, InvoiceId: {InvoiceId}",
                orderEvent.OrderId,
                orderEvent.InvoiceId);

            // Mark order as payment failed and notify customer
            if (orderEvent.OrderId.HasValue)
            {
                await MarkOrderAsPaymentFailed(orderEvent.OrderId.Value, ex.Message, cancellationToken);
                await _notificationService.NotifyPaymentFailedAsync(
                    orderEvent.OrderId.Value,
                    ex.Message,
                    cancellationToken);
            }

            return new InvoiceActivationResult
            {
                IsActivated = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Handles order completion event
    /// Processes cash on delivery payments and sends delivery confirmation
    /// </summary>
    public async Task<bool> OnOrderCompleted(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(orderEvent.OrderId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            order.OrderStatus = OrderProgressStatus.Completed;
            order.OrderDeliveryDate = DateTime.UtcNow;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            // Send delivery confirmation notification
            await _notificationService.NotifyOrderDeliveredAsync(order.Id, cancellationToken);

            // Process cash on delivery payment
            if (order.PaymentStatus != PaymentStatus.Completed)
            {
                await ProcessCashOnDeliveryPayment(order, cancellationToken);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process OnOrderCompleted - OrderId: {OrderId}", orderEvent.OrderId);
            return false;
        }
    }

    /// <summary>
    /// Handles order cancellation event
    /// Releases inventory, processes refunds, and notifies customer
    /// </summary>
    public async Task<bool> OnOrderCanceled(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(orderEvent.OrderId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            // Update order status
            order.OrderStatus = OrderProgressStatus.Cancelled;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);

            // Release inventory for all shipments
            var shipments = await _shipmentService.GetOrderShipmentsAsync(order.Id);

            foreach (var shipment in shipments)
            {
                await ReleaseShipmentInventoryAsync(shipment.Id, cancellationToken);
            }

            // Process refund if payment was completed
            if (order.PaymentStatus == PaymentStatus.Completed)
            {
                await ProcessOrderRefundAsync(order, cancellationToken);

                // Send cancellation and refund notification
                await _notificationService.NotifyOrderCancelledAsync(
                    order.Id,
                    "Order cancelled by customer request",
                    cancellationToken);

                await _notificationService.NotifyOrderRefundedAsync(
                    order.Id,
                    order.Price,
                    cancellationToken);
            }
            else
            {
                // Send cancellation notification only
                await _notificationService.NotifyOrderCancelledAsync(
                    order.Id,
                    "Order cancelled before payment",
                    cancellationToken);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process OnOrderCanceled - OrderId: {OrderId}", orderEvent.OrderId);
            return false;
        }
    }

    /// <summary>
    /// Handles order refund request
    /// Validates refund eligibility, processes refund, and notifies customer
    /// </summary>
    public async Task<bool> OnOrderRefund(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(orderEvent.OrderId, cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            // Validate refund eligibility
            ValidateRefundEligibility(order);

            // Update order status
            order.OrderStatus = OrderProgressStatus.Returned;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order);

            // Process refund
            await ProcessOrderRefundAsync(order, cancellationToken);

            // Send refund notification
            await _notificationService.NotifyOrderRefundedAsync(
                order.Id,
                order.Price,
                cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process OnOrderRefund - OrderId: {OrderId}", orderEvent.OrderId);
            return false;
        }
    }

    /// <summary>
    /// Handles order pending event
    /// </summary>
    public async Task<bool> OnOrderPending(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Pending,
            cancellationToken);

        if (result)
        {
            // Send payment reminder if order is pending payment
            var order = await _orderRepository.FindByIdAsync(orderEvent.OrderId, cancellationToken);
            if (order?.PaymentStatus == PaymentStatus.Pending)
            {
                await _notificationService.NotifyPaymentReminderAsync(order.Id, cancellationToken);
            }
        }

        return result;
    }

    /// <summary>
    /// Handles order accepted/confirmed event
    /// </summary>
    public async Task<bool> OnOrderAccepted(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Confirmed,
            cancellationToken);

        if (result)
        {
            await _notificationService.NotifyOrderConfirmedAsync(orderEvent.OrderId, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Handles order rejected event
    /// </summary>
    public async Task<bool> OnOrderRejected(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Cancelled,
            cancellationToken);

        if (result)
        {
            await _notificationService.NotifyOrderCancelledAsync(
                orderEvent.OrderId,
                "Order rejected by merchant",
                cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Handles order in progress event
    /// </summary>
    public async Task<bool> OnOrderInProgress(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Processing,
            cancellationToken);

        if (result)
        {
            await _notificationService.NotifyOrderProcessingAsync(orderEvent.OrderId, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Handles order shipping event
    /// </summary>
    public async Task<bool> OnOrderShipping(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Shipped,
            cancellationToken);

        if (result)
        {
            // Get first shipment for tracking info
            var shipments = await _shipmentService.GetOrderShipmentsAsync(orderEvent.OrderId);
            var firstShipment = shipments.FirstOrDefault();

            await _notificationService.NotifyOrderShippedAsync(
                orderEvent.OrderId,
                firstShipment?.Id,
                cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Handles order not active event
    /// </summary>
    public async Task<bool> OnOrderNotActive(
        OrderEvent orderEvent,
        CancellationToken cancellationToken = default)
    {
        return await UpdateOrderStatusAsync(
            orderEvent.OrderId,
            OrderProgressStatus.Pending,
            cancellationToken);
    }

    #region Private Helper Methods

    private async Task<TbOrder?> FindOrderAsync(
        Guid? orderId,
        string? invoiceId,
        CancellationToken cancellationToken)
    {
        if (orderId.HasValue && orderId != Guid.Empty)
        {
            return await _orderRepository.FindByIdAsync(orderId.Value, cancellationToken);
        }

        if (!string.IsNullOrEmpty(invoiceId))
        {
            return await _orderRepository.GetByInvoiceIdAsync(invoiceId, cancellationToken);
        }

        return null;
    }

    private async Task ProcessShipmentFulfillmentAsync(
        Guid shipmentId,
        Guid orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);

            if (shipment?.WarehouseId == null)
            {
                return;
            }

            var fulfillmentType = await _fulfillmentService.DetermineFulfillmentTypeAsync(
                shipment.WarehouseId.Value,
                cancellationToken);

            if (fulfillmentType == FulfillmentType.Marketplace)
            {
                await _fulfillmentService.ProcessFulfillmentByMarketplaceShipmentAsync(shipmentId, cancellationToken);
            }
            else
            {
                await _fulfillmentService.ProcessFulfillmentBySellerShipmentAsync(shipmentId, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process fulfillment for shipment {ShipmentId}", shipmentId);
        }
    }

    private async Task ReleaseShipmentInventoryAsync(
        Guid shipmentId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _fulfillmentService.ReleaseInventoryAsync(shipmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to release inventory for shipment {ShipmentId}", shipmentId);
        }
    }

    private async Task ProcessCashOnDeliveryPayment(
        TbOrder order,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetOrderPaymentAsync(order.Id, cancellationToken);

        if (payment?.PaymentMethodType == PaymentMethodType.CashOnDelivery)
        {
            payment.PaymentStatus = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;
            payment.UpdatedDateUtc = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);

            order.PaymentStatus = PaymentStatus.Completed;
            order.PaidAt = DateTime.UtcNow;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            // Send payment confirmation
            await _notificationService.NotifyOrderPaidAsync(order.Id, cancellationToken);
        }
    }

    private async Task ProcessOrderRefundAsync(
        TbOrder order,
        CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetOrderPaymentsAsync(order.Id, cancellationToken);

        foreach (var payment in payments)
        {
            if (payment.PaymentStatus != PaymentStatus.Completed)
            {
                continue;
            }

            try
            {
                if (payment.PaymentMethodType == PaymentMethodType.Wallet ||
                    payment.PaymentMethodType == PaymentMethodType.WalletAndCard)
                {
                    await _walletService.ProcessRefundAsync(
                        order.UserId,
                        payment.Amount,
                        order.Id);
                }
                else if (payment.PaymentMethodType == PaymentMethodType.Card)
                {
                    payment.PaymentStatus = PaymentStatus.Refunded;
                    payment.RefundedAt = DateTime.UtcNow;
                    payment.RefundAmount = payment.Amount;
                    payment.UpdatedDateUtc = DateTime.UtcNow;
                    await _paymentRepository.UpdateAsync(payment);
                }

                payment.PaymentStatus = PaymentStatus.Refunded;
                payment.RefundedAt = DateTime.UtcNow;
                payment.RefundAmount = payment.Amount;
                payment.UpdatedDateUtc = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to refund payment {PaymentId} for order {OrderId}",
                    payment.Id, order.Id);
            }
        }

        order.PaymentStatus = PaymentStatus.Refunded;
        order.UpdatedDateUtc = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(order);
    }

    private void ValidateRefundEligibility(TbOrder order)
    {
        if (order.OrderStatus != OrderProgressStatus.Completed)
        {
            throw new InvalidOperationException(
                $"Order {order.Id} must be completed before refund. Current status: {order.OrderStatus}");
        }

        if (!order.OrderDeliveryDate.HasValue)
        {
            throw new InvalidOperationException(
                $"Order {order.Id} has no delivery date");
        }

        var daysSinceDelivery = (DateTime.UtcNow - order.OrderDeliveryDate.Value).Days;
        const int maxRefundDays = 15;

        if (daysSinceDelivery > maxRefundDays)
        {
            throw new InvalidOperationException(
                $"Refund period expired for order {order.Id}. " +
                $"Delivered {daysSinceDelivery} days ago (limit: {maxRefundDays} days)");
        }
    }

    /// <summary>
    /// Marks order as payment failed
    /// Updates both TbOrder and TbOrderPayment with failure information
    /// UPDATED: Now updates payment record with FailureReason
    /// </summary>
    private async Task MarkOrderAsPaymentFailed(
        Guid orderId,
        string failureReason,
        CancellationToken cancellationToken)
    {
        try
        {
            // 1. Update Order status
            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order != null)
            {
                order.OrderStatus = OrderProgressStatus.PaymentFailed;
                order.PaymentStatus = PaymentStatus.Failed;
                order.UpdatedDateUtc = DateTime.UtcNow;
                await _orderRepository.UpdateAsync(order);
            }

            // 2. Update Payment record with failure information
            var payment = await _paymentRepository.GetOrderPaymentAsync(orderId, cancellationToken);

            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Failed;

                // Respect MaxLength(500) constraint on FailureReason
                payment.FailureReason = failureReason?.Length > 500
                    ? failureReason.Substring(0, 500)
                    : failureReason;

                payment.UpdatedDateUtc = DateTime.UtcNow;

                await _paymentRepository.UpdateAsync(payment);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to mark order {OrderId} as payment failed", orderId);
        }
    }

    private async Task<bool> UpdateOrderStatusAsync(
        Guid orderId,
        OrderProgressStatus newStatus,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return false;
            }

            order.OrderStatus = newStatus;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update order {OrderId} status to {Status}", orderId, newStatus);
            return false;
        }
    }

    #endregion
}