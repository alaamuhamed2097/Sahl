using BL.Contracts.GeneralService;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Order;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.DTOs.Order.Payment;
using Shared.DTOs.User.Customer;
using Shared.GeneralModels;

namespace BL.Services.Order.OrderProcessing;

/// <summary>
/// FINAL AdminOrderService
/// - ShipmentStatus.PendingProcessing (not Pending)
/// - No InvoiceId references (use OrderNumber)
/// </summary>
public class AdminOrderService : IAdminOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger _logger;

    public AdminOrderService(
        IOrderRepository orderRepository,
        IDateTimeService dateTimeService,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(List<AdminOrderListDto> Orders, int TotalCount)> SearchOrdersAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (orders, totalCount) = await _orderRepository.SearchOrdersAsync(
                searchTerm,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection,
                cancellationToken);

            var orderDtos = orders.Select(order => new AdminOrderListDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = _dateTimeService.ConvertToLocalTime(order.CreatedDateUtc),
                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
                CustomerEmail = order.User?.Email ?? "",
                CustomerPhoneCode = order.User?.PhoneCode ?? "",
                CustomerPhone = order.User?.PhoneNumber ?? "",
                TotalAmount = order.Price,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,
                TotalItemsCount = order.OrderDetails.Sum(od => od.Quantity)
            }).ToList();

            return (orderDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting admin orders");
            throw;
        }
    }

    public async Task<AdminOrderDetailsDto?> GetAdminOrderDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

            if (order == null)
            {
                return null;
            }

            return new AdminOrderDetailsDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = _dateTimeService.ConvertToLocalTime(order.CreatedDateUtc),
                DeliveryDate = _dateTimeService.ConvertToLocalTime(order.OrderDeliveryDate),
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,

                // ✅ Payment Summary fields
                WalletPaidAmount = order.WalletPaidAmount,
                CardPaidAmount = order.CardPaidAmount,
                CashPaidAmount = order.CashPaidAmount,
                TotalPaidAmount = order.TotalPaidAmount,

                Customer = new AdminCustomerInfoDto
                {
                    CustomerId = order.UserId,
                    CustomerFullName = $"{order.User?.FirstName} {order.User?.LastName}",
                    Email = order.User?.Email ?? "",
                    Phone = order.CustomerAddress?.PhoneNumber ?? "",
                    PhoneCode = order.CustomerAddress?.PhoneCode ?? ""
                },
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
                Items = order.OrderDetails.Select(od => MapToAdminOrderItem(od, order.TbOrderShipments)).ToList(),
                Shipments = order.TbOrderShipments.Select(s => new ShipmentInfoDto
                {
                    ShipmentId = s.Id,
                    ShipmentNumber = s.Number ?? "",
                    Status = s.ShipmentStatus,
                    TrackingNumber = s.TrackingNumber,
                    EstimatedDeliveryDate = _dateTimeService.ConvertToLocalTime(s.EstimatedDeliveryDate),
                    ActualDeliveryDate = _dateTimeService.ConvertToLocalTime(s.ActualDeliveryDate)
                }).ToList(),
                PaymentInfo = new PaymentInfoDto
                {
                    Status = order.PaymentStatus,
                    PaymentMethod = order.OrderPayments.FirstOrDefault()?.PaymentMethod.ToString() ?? "",
                    // ✅ FIXED: Use OrderNumber instead of InvoiceId
                    TransactionId = order.Number,
                    PaymentDate = _dateTimeService.ConvertToLocalTime(order.PaidAt),
                    Amount = order.Price
                },
                Coupon = order.Coupon != null ? new CouponInfoDto
                {
                    Code = order.Coupon.Code ?? "",
                    DiscountAmount = order.DiscountAmount
                } : null,
                SubTotal = order.SubTotal,
                ShippingAmount = order.ShippingAmount,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.Price,
                OrderNotes = order.Notes
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting admin order details {OrderId}", orderId);
            throw;
        }
    }

    public async Task<ResponseModel<bool>> ChangeOrderStatusAsync(
        Guid orderId,
        OrderProgressStatus newStatus,
        string? notes,
        string adminUserId,
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

            if (!IsValidStatusTransition(order.OrderStatus, newStatus))
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = $"Invalid status transition from {order.OrderStatus} to {newStatus}"
                };
            }

            order.OrderStatus = newStatus;
            order.UpdatedDateUtc = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(notes))
            {
                order.Notes = string.IsNullOrEmpty(order.Notes)
                    ? $"Admin update: {notes}"
                    : $"{order.Notes}\nAdmin update: {notes}";
            }

            if (newStatus == OrderProgressStatus.Delivered && !order.OrderDeliveryDate.HasValue)
            {
                order.OrderDeliveryDate = DateTime.UtcNow;
            }

            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

            return new ResponseModel<bool>
            {
                Success = true,
                Message = $"Order status changed to {newStatus}",
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error changing order status {OrderId}", orderId);
            return new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ResponseModel<bool>> UpdateOrderAsync(
        UpdateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            if (request.OrderDeliveryDate.HasValue)
            {
                order.OrderDeliveryDate = request.OrderDeliveryDate;
            }

            order.UpdatedDateUtc = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

            return new ResponseModel<bool>
            {
                Success = true,
                Message = "Order updated successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating order {OrderId}", request.OrderId);
            return new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _orderRepository.CountTodayOrdersAsync(date, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error counting today's orders");
            throw;
        }
    }

    // ============================================
    // HELPER METHODS
    // ============================================

    private AdminOrderItemDto MapToAdminOrderItem(
        TbOrderDetail od,
        ICollection<TbOrderShipment> shipments)
    {
        return new AdminOrderItemDto
        {
            OrderDetailId = od.Id,
            ItemId = od.ItemId,
            ItemName = od.Item?.TitleEn ?? "",
            ItemImage = od.Item?.ThumbnailImage ?? "",
            VendorId = od.VendorId,
            VendorName = od.Vendor?.StoreName ?? "",
            Quantity = od.Quantity,
            UnitPrice = od.UnitPrice,
            SubTotal = od.SubTotal,
            DiscountAmount = od.DiscountAmount,
            TaxAmount = od.TaxAmount,
            ShipmentStatus = GetItemShipmentStatus(od.Id, shipments),
            WarehouseId = od.WarehouseId
        };
    }

    private Common.Enumerations.Shipping.ShipmentStatus GetItemShipmentStatus(
        Guid orderDetailId,
        ICollection<TbOrderShipment> shipments)
    {
        // ✅ FIXED: PendingProcessing instead of Pending
        return shipments
            .Where(s => s.Items.Any(item => item.OrderDetailId == orderDetailId))
            .OrderByDescending(s => s.CreatedDateUtc)
            .FirstOrDefault()?.ShipmentStatus ?? Common.Enumerations.Shipping.ShipmentStatus.PendingProcessing;
    }

    private bool IsValidStatusTransition(OrderProgressStatus current, OrderProgressStatus target)
    {
        if (current == target) return true;

        var validTransitions = new Dictionary<OrderProgressStatus, List<OrderProgressStatus>>
        {
            [OrderProgressStatus.Pending] = new() { OrderProgressStatus.Confirmed, OrderProgressStatus.Cancelled },
            [OrderProgressStatus.Confirmed] = new() { OrderProgressStatus.Processing, OrderProgressStatus.Cancelled },
            [OrderProgressStatus.Processing] = new() { OrderProgressStatus.Shipped, OrderProgressStatus.Cancelled },
            [OrderProgressStatus.Shipped] = new() { OrderProgressStatus.Delivered, OrderProgressStatus.Returned },
            [OrderProgressStatus.Delivered] = new() { OrderProgressStatus.Completed, OrderProgressStatus.Returned },
            [OrderProgressStatus.Completed] = new() { OrderProgressStatus.Returned },
            [OrderProgressStatus.Cancelled] = new(),
            [OrderProgressStatus.Returned] = new() { OrderProgressStatus.Refunded },
            [OrderProgressStatus.Refunded] = new()
        };

        return validTransitions.ContainsKey(current) &&
               validTransitions[current].Contains(target);
    }
}