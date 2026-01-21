using AutoMapper;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories.Order;
using DAL.Models;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.OrderProcessing.VendorDashboardOrder;
using Shared.DTOs.Order.OrderProcessing.VendorOrder;

namespace BL.Services.Order.OrderProcessing;
/// <summary>
/// Service for Vendor-specific order operations
/// Handles: View vendor orders, update shipment status, manage fulfillment
/// </summary>
public class VendorOrderService : IVendorOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public VendorOrderService(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get vendor orders with pagination and search
    /// Shows only orders containing vendor's items
    /// </summary>
    public async Task<AdvancedPagedResult<VendorOrderListDto>> GetVendorOrdersAsync(
        string vendorId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (orders, totalCount) = await _orderRepository.GetVendorOrdersWithPaginationAsync(
                vendorId,
                searchTerm,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection,
                cancellationToken);

            var orderDtos = orders.Select(order => new VendorOrderListDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.CreatedDateUtc,
                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
                VendorTotal = order.OrderDetails.Sum(od => od.SubTotal), // ✅ Changed from TotalAmount
                OrderStatus = order.OrderStatus,
                VendorPaymentStatus = order.PaymentStatus, // ✅ Changed from PaymentStatus
                VendorItemsCount = order.OrderDetails.Sum(od => od.Quantity) // ✅ Changed from ItemsCount
            }).ToList();

            return new AdvancedPagedResult<VendorOrderListDto>
            {
                Items = orderDtos,
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting vendor orders for {VendorId}", vendorId);
            throw;
        }
    }

    /// <summary>
    /// Get vendor order details
    /// Shows only items belonging to this vendor
    /// </summary>
    public async Task<VendorOrderDetailsDto?> GetVendorOrderDetailsAsync(
        Guid orderId,
        string vendorId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!Guid.TryParse(vendorId, out var vendorGuid))
            {
                return null;
            }

            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

            if (order == null)
            {
                return null;
            }

            // Filter order details to only show vendor's items
            var vendorItems = order.OrderDetails
                .Where(od => od.VendorId == vendorGuid)
                .ToList();

            if (!vendorItems.Any())
            {
                return null;
            }

            return new VendorOrderDetailsDto
            {
                OrderId = order.Id,
                OrderNumber = order.Number,
                OrderDate = order.CreatedDateUtc,
                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
                CustomerPhone = order.CustomerAddress?.PhoneNumber ?? "",
                OrderStatus = order.OrderStatus,
                VendorPaymentStatus = order.PaymentStatus, // ✅ Changed from PaymentStatus
                DeliveryAddress = new DeliveryAddressDto
                {
                    Address = order.CustomerAddress?.Address ?? "",
                    CityNameEn = order.CustomerAddress?.City?.TitleEn ?? "",
                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? "",
                    RecipientName = order.CustomerAddress?.RecipientName ?? ""
                },
                VendorItems = vendorItems.Select(od => new VendorOrderItemDto // ✅ Changed from Items
                {
                    OrderDetailId = od.Id,
                    ItemId = od.ItemId,
                    ItemName = od.Item?.TitleEn ?? "",
                    ItemImage = od.Item?.ThumbnailImage ?? "",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    SubTotal = od.SubTotal,
                    ShipmentStatus = GetItemShipmentStatus(od.Id, order.TbOrderShipments)
                }).ToList(),
                VendorTotalAmount = vendorItems.Sum(od => od.SubTotal) // ✅ Changed from VendorTotal
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting vendor order details {OrderId}", orderId);
            throw;
        }
    }

    // ============================================
    // HELPER METHODS
    // ============================================

    private ShipmentStatus GetItemShipmentStatus(
        Guid orderDetailId,
        ICollection<TbOrderShipment> shipments)
    {
        return shipments
            .Where(s => s.Items.Any(item => item.OrderDetailId == orderDetailId))
            .OrderByDescending(s => s.CreatedDateUtc)
            .FirstOrDefault()?.ShipmentStatus ?? ShipmentStatus.PendingProcessing;
    }
}