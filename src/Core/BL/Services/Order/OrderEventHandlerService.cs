using BL.Contracts.Service.Order;
using BL.Contracts.Service.Order.Fulfillment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.OrderEvents;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order;

public class OrderEventHandlerService : IOrderEventHandlerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentService _shipmentService;
    private readonly IFulfillmentService _fulfillmentService;
    private readonly ILogger _logger;

    public OrderEventHandlerService(
        IUnitOfWork unitOfWork,
        IShipmentService shipmentService,
        IFulfillmentService fulfillmentService,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _shipmentService = shipmentService ?? throw new ArgumentNullException(nameof(shipmentService));
        _fulfillmentService = fulfillmentService ?? throw new ArgumentNullException(nameof(fulfillmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<InvoiceActivationResult> OnOrderPaid(OrderPaidEvent orderEvent)
    {
        Guid? orderId = null;

        try
        {
            _logger.Information(
                "Processing OnOrderPaid event for Order: {OrderId}, Invoice: {InvoiceId}",
                orderEvent.OrderId,
                orderEvent.InvoiceId
            );

            await _unitOfWork.BeginTransactionAsync();

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            TbOrder? order = null;

            if (orderEvent.OrderId.HasValue && orderEvent.OrderId != Guid.Empty)
            {
                order = await orderRepo.FindByIdAsync(orderEvent.OrderId.Value);
            }
            else if (!string.IsNullOrEmpty(orderEvent.InvoiceId))
            {
                var orders = await orderRepo.GetAsync(o => o.InvoiceId == orderEvent.InvoiceId);
                order = orders.FirstOrDefault();
            }

            if (order == null)
            {
                throw new InvalidOperationException(
                    $"Order not found - OrderId: {orderEvent.OrderId}, InvoiceId: {orderEvent.InvoiceId}"
                );
            }

            orderId = order.Id;

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                _logger.Warning("Order {OrderId} is already marked as paid", order.Id);
                return new InvoiceActivationResult { IsActivated = true };
            }

            order.PaymentStatus = PaymentStatus.Paid;
            order.PaymentDate = DateTime.UtcNow;
            order.OrderStatus = OrderProgressStatus.Processing;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await orderRepo.UpdateAsync(order, Guid.Empty);

            var shipments = await _shipmentService.SplitOrderIntoShipmentsAsync(order.Id);

            _logger.Information(
                "Order {OrderId} split into {Count} shipments",
                order.Id,
                shipments.Count
            );

            foreach (var shipment in shipments)
            {
                try
                {
                    var fulfillmentType = await _fulfillmentService.DetermineFulfillmentTypeAsync(
                        shipment.WarehouseId ?? Guid.Empty
                    );

                    if (fulfillmentType == Common.Enumerations.Fulfillment.FulfillmentType.Marketplace)
                    {
                        await _fulfillmentService.ProcessFBAShipmentAsync(shipment.Id);
                        _logger.Information(
                            "FBA shipment {ShipmentId} initiated for order {OrderId}",
                            shipment.Id,
                            order.Id
                        );
                    }
                    else
                    {
                        await _fulfillmentService.ProcessFBMShipmentAsync(shipment.Id);
                        _logger.Information(
                            "FBM shipment {ShipmentId} initiated for order {OrderId}",
                            shipment.Id,
                            order.Id
                        );
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(
                        ex,
                        "Error processing fulfillment for shipment {ShipmentId}",
                        shipment.Id
                    );
                }
            }

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "OnOrderPaid completed successfully for order {OrderId}",
                order.Id
            );

            return new InvoiceActivationResult { IsActivated = true };
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error processing OnOrderPaid for Order: {OrderId}, Invoice: {InvoiceId}",
                orderEvent.OrderId,
                orderEvent.InvoiceId
            );

            await _unitOfWork.RollbackAsync();

            if (orderId.HasValue)
            {
                try
                {
                    var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                    var order = await orderRepo.FindByIdAsync(orderId.Value);
                    if (order != null)
                    {
                        order.OrderStatus = OrderProgressStatus.PaymentFailed;
                        order.UpdatedDateUtc = DateTime.UtcNow;
                        await orderRepo.UpdateAsync(order, Guid.Empty);
                    }
                }
                catch (Exception innerEx)
                {
                    _logger.Error(innerEx, "Error updating order status after payment failure");
                }
            }

            return new InvoiceActivationResult
            {
                IsActivated = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<bool> OnOrderCompleted(OrderEvent orderEvent)
    {
        try
        {
            _logger.Information("Processing OnOrderCompleted for order {OrderId}", orderEvent.OrderId);

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderEvent.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            order.OrderStatus = OrderProgressStatus.Completed;
            order.OrderDeliveryDate = DateTime.UtcNow;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await orderRepo.UpdateAsync(order, Guid.Empty);

            // FIXED: Check payment status from TbOrderPayment instead of PaymentGatewayMethodType
            if (order.PaymentStatus != PaymentStatus.Paid)
            {
                // Check if this is a cash on delivery order
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = await paymentRepo.GetQueryable()
                    .Include(p => p.PaymentMethod)
                    .FirstOrDefaultAsync(p => p.OrderId == order.Id && !p.IsDeleted);

                if (payment != null && payment.PaymentMethod.MethodType == PaymentMethod.CashOnDelivery)
                {
                    await OnOrderPaid(new OrderPaidEvent
                    {
                        OrderId = order.Id,
                        InvoiceId = order.InvoiceId
                    });
                }
            }

            _logger.Information("Order {OrderId} marked as completed", orderEvent.OrderId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing OnOrderCompleted for order {OrderId}", orderEvent.OrderId);
            return false;
        }
    }

    public async Task<bool> OnOrderCanceled(OrderEvent orderEvent)
    {
        try
        {
            _logger.Information("Processing OnOrderCanceled for order {OrderId}", orderEvent.OrderId);

            await _unitOfWork.BeginTransactionAsync();

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderEvent.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            order.OrderStatus = OrderProgressStatus.Cancelled;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Empty);

            var shipments = await _shipmentService.GetOrderShipmentsAsync(order.Id);
            foreach (var shipment in shipments)
            {
                try
                {
                    await _fulfillmentService.ReleaseInventoryAsync(shipment.Id);
                    _logger.Information(
                        "Inventory released for shipment {ShipmentId}",
                        shipment.Id
                    );
                }
                catch (Exception ex)
                {
                    _logger.Error(
                        ex,
                        "Error releasing inventory for shipment {ShipmentId}",
                        shipment.Id
                    );
                }
            }

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                _logger.Information("Refund required for cancelled order {OrderId}", order.Id);
            }

            await _unitOfWork.CommitAsync();

            _logger.Information("Order {OrderId} cancelled successfully", orderEvent.OrderId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing OnOrderCanceled for order {OrderId}", orderEvent.OrderId);
            await _unitOfWork.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> OnOrderRefund(OrderEvent orderEvent)
    {
        try
        {
            _logger.Information("Processing OnOrderRefund for order {OrderId}", orderEvent.OrderId);

            await _unitOfWork.BeginTransactionAsync();

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderEvent.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException($"Order {orderEvent.OrderId} not found");
            }

            if (order.OrderStatus != OrderProgressStatus.Completed)
            {
                throw new InvalidOperationException(
                    $"Order {orderEvent.OrderId} must be completed before refund"
                );
            }

            if (!order.OrderDeliveryDate.HasValue)
            {
                throw new InvalidOperationException(
                    $"Order {orderEvent.OrderId} has no delivery date"
                );
            }

            var daysSinceDelivery = (DateTime.UtcNow - order.OrderDeliveryDate.Value).Days;
            if (daysSinceDelivery > 15)
            {
                throw new InvalidOperationException(
                    $"Refund period expired for order {orderEvent.OrderId}"
                );
            }

            order.OrderStatus = OrderProgressStatus.Returned;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Empty);

            await _unitOfWork.CommitAsync();

            _logger.Information("Refund processed for order {OrderId}", orderEvent.OrderId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing OnOrderRefund for order {OrderId}", orderEvent.OrderId);
            await _unitOfWork.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> OnOrderPending(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Pending, "order pending");
    }

    public async Task<bool> OnOrderAccepted(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Confirmed, "order accepted");
    }

    public async Task<bool> OnOrderRejected(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Cancelled, "order rejected");
    }

    public async Task<bool> OnOrderInProgress(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Processing, "order in progress");
    }

    public async Task<bool> OnOrderShipping(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Shipped, "order shipping");
    }

    public async Task<bool> OnOrderNotActive(OrderEvent orderEvent)
    {
        return await UpdateOrderStatus(orderEvent.OrderId, OrderProgressStatus.Pending, "order not active");
    }

    private async Task<bool> UpdateOrderStatus(
        Guid orderId,
        OrderProgressStatus newStatus,
        string logMessage)
    {
        try
        {
            _logger.Information("Processing {Message} for order {OrderId}", logMessage, orderId);

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderId);

            if (order == null)
            {
                _logger.Warning("Order {OrderId} not found", orderId);
                return false;
            }

            order.OrderStatus = newStatus;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await orderRepo.UpdateAsync(order, Guid.Empty);

            _logger.Information(
                "Order {OrderId} status updated to {Status}",
                orderId,
                newStatus
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error updating order {OrderId} status to {Status}",
                orderId,
                newStatus
            );
            return false;
        }
    }
}