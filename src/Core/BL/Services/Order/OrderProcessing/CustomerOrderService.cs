using AutoMapper;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Order;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using DAL.Models;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.CustomerOrder;
using Shared.DTOs.Order.Payment;
using Shared.GeneralModels;

namespace BL.Services.Order.OrderProcessing;

/// <summary>
/// FINAL CustomerOrderService
/// - ShipmentStatus.PendingProcessing (not Pending)
/// - No InvoiceId references (use OrderNumber)
/// </summary>
public class CustomerOrderService : ICustomerOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CustomerOrderService(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AdvancedPagedResult<CustomerOrderListDto>> GetCustomerOrdersListAsync(
        string customerId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (orders, totalCount) = await _orderRepository.GetCustomerOrdersWithPaginationAsync(
                customerId,
                pageNumber,
                pageSize,
                cancellationToken);

            var orderDtos = orders.Select(order => new CustomerOrderListDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.CreatedDateUtc,
                TotalAmount = order.Price,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                // FIXED: PendingProcessing instead of Pending
                ShipmentStatus = order.TbOrderShipments
                    .OrderByDescending(s => s.CreatedDateUtc)
                    .FirstOrDefault()?.ShipmentStatus ?? ShipmentStatus.PendingProcessing,
                TotalItems = order.OrderDetails.Sum(od => od.Quantity),
                ItemsSummary = order.OrderDetails
                    .Take(3)
                    .Select(od => new OrderItemSummaryDto
                    {
                        ItemName = od.Item?.TitleEn ?? "",
                        ThumbnailImage = od.Item?.ThumbnailImage ?? "",
                        Quantity = od.Quantity
                    }).ToList(),
                CanCancel = CanCancelOrder(order.OrderStatus),
                IsWithinRefundPeriod = IsWithinRefundPeriod(order.CreatedDateUtc)
            }).ToList();

            return new AdvancedPagedResult<CustomerOrderListDto>
            {
                Items = orderDtos,
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting customer orders for {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CustomerOrderDetailsDto?> GetCustomerOrderDetailsAsync(
        Guid orderId,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

            if (order == null || order.UserId != customerId)
            {
                return null;
            }

            return new CustomerOrderDetailsDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.CreatedDateUtc,
                DeliveryDate = order.OrderDeliveryDate,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                SubTotal = order.OrderDetails.Sum(od => od.SubTotal),
                ShippingAmount = order.ShippingAmount,
                TaxAmount = order.OrderDetails.Sum(od => od.TaxAmount),
                DiscountAmount = order.OrderDetails.Sum(od => od.DiscountAmount),
                TotalAmount = order.Price,
                DeliveryAddress = new DeliveryAddressDto
                {
                    Address = order.CustomerAddress?.Address ?? "",
                    CityNameAr = order.CustomerAddress?.City?.TitleAr ?? "",
                    CityNameEn = order.CustomerAddress?.City?.TitleEn ?? "",
                    StateNameAr = order.CustomerAddress?.City?.State?.TitleAr ?? "",
                    StateNameEn = order.CustomerAddress?.City?.State?.TitleEn ?? "",
                    PhoneCode = order.CustomerAddress?.PhoneCode ?? "",
                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? "",
                    RecipientName = order.CustomerAddress?.RecipientName ?? ""
                },
                PaymentInfo = new PaymentInfoDto
                {
                    Status = order.PaymentStatus,
                    // Use PaymentMethodType enum which is always populated
                    PaymentMethod = order.OrderPayments.FirstOrDefault()?.PaymentMethodType.ToString() ?? "Not Specified",
                    TransactionId = order.Number,
                    PaymentDate = order.PaidAt,
                    Amount = order.Price
                },
                Items = order.OrderDetails.Select(od => new CustomerOrderItemDto
                {
                    OrderDetailId = od.Id,
                    ItemId = od.ItemId,
                    ItemName = od.Item?.TitleEn ?? "",
                    ItemImage = od.Item?.ThumbnailImage ?? "",
                    VendorName = od.Vendor?.StoreName ?? "",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    SubTotal = od.SubTotal,
                    DiscountAmount = od.DiscountAmount,
                    TaxAmount = od.TaxAmount,
                    ShipmentStatus = GetItemShipmentStatus(od.Id, order.TbOrderShipments)
                }).ToList(),
                Shipments = order.TbOrderShipments.Select(s => new ShipmentInfoDto
                {
                    ShipmentId = s.Id,
                    ShipmentNumber = s.Number ?? "",
                    Status = s.ShipmentStatus,
                    TrackingNumber = s.TrackingNumber,
                    EstimatedDeliveryDate = s.EstimatedDeliveryDate,
                    ActualDeliveryDate = s.ActualDeliveryDate,
                    Items = s.Items.Select(si => new ShipmentItemDto
                    {
                        Id = si.Id,
                        ShipmentId = si.ShipmentId,
                        ItemId = si.ItemId,
                        ItemName = si.Item?.TitleEn ?? "",
                        TitleAr = si.Item?.TitleAr ?? "",
                        TitleEn = si.Item?.TitleEn ?? "",
                        ItemImage = si.Item?.ThumbnailImage,
                        ItemCombinationId = si.ItemCombinationId,
                        Quantity = si.Quantity,
                        UnitPrice = si.UnitPrice,
                        SubTotal = si.SubTotal
                    }).ToList()
                }).ToList(),
                CanCancel = CanCancelOrder(order.OrderStatus),
                CanRequestRefund = CanRequestRefund(order.OrderStatus, order.CreatedDateUtc),
                IsWithinRefundPeriod = IsWithinRefundPeriod(order.CreatedDateUtc)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting customer order details {OrderId}", orderId);
            throw;
        }
    }

    public async Task<ResponseModel<bool>> CancelOrderAsync(
        Guid orderId,
        string customerId,
        string? reason,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            if (order.UserId != customerId)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Unauthorized to cancel this order"
                };
            }

            if (!CanCancelOrder(order.OrderStatus))
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order cannot be cancelled at this stage"
                };
            }

            order.OrderStatus = OrderProgressStatus.Cancelled;
            order.Notes = string.IsNullOrEmpty(order.Notes)
                ? $"Cancelled by customer. Reason: {reason}"
                : $"{order.Notes}\nCancelled by customer. Reason: {reason}";
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

            return new ResponseModel<bool>
            {
                Success = true,
                Message = "Order cancelled successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error cancelling order {OrderId}", orderId);
            return new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    // HELPER METHODS

    private ShipmentStatus GetItemShipmentStatus(
        Guid orderDetailId,
        ICollection<TbOrderShipment> shipments)
    {
        // FIXED: PendingProcessing instead of Pending
        return shipments
            .Where(s => s.Items.Any(item => item.OrderDetailId == orderDetailId))
            .OrderByDescending(s => s.CreatedDateUtc)
            .FirstOrDefault()?.ShipmentStatus ?? ShipmentStatus.PendingProcessing;
    }

    private bool CanCancelOrder(OrderProgressStatus status)
    {
        return status == OrderProgressStatus.Pending ||
               status == OrderProgressStatus.Confirmed;
    }

    private bool CanRequestRefund(OrderProgressStatus status, DateTime orderDate)
    {
        return (status == OrderProgressStatus.Delivered ||
                status == OrderProgressStatus.Completed) &&
               IsWithinRefundPeriod(orderDate);
    }

    private bool IsWithinRefundPeriod(DateTime orderDate)
    {
        return DateTime.UtcNow.Subtract(orderDate).Days <= 15;
    }
}