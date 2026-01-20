//using AutoMapper;
//using BL.Contracts.Service.Order.OrderProcessing;
//using Common.Enumerations.Order;
//using Common.Enumerations.Shipping;
//using DAL.Contracts.Repositories.Order;
//using DAL.Models;
//using Domains.Entities.Order;
//using Domains.Entities.Order.Shipping;
//using Serilog;
//using Shared.DTOs.Order.Checkout.Address;
//using Shared.DTOs.Order.CouponCode;
//using Shared.DTOs.Order.Fulfillment.Shipment;
//using Shared.DTOs.Order.OrderProcessing;
//using Shared.DTOs.Order.OrderProcessing.AdminOrder;
//using Shared.DTOs.Order.OrderProcessing.CustomerOrder;
//using Shared.DTOs.Order.OrderProcessing.VendorDashboardOrder;
//using Shared.DTOs.Order.OrderProcessing.VendorOrder;
//using Shared.DTOs.Order.Payment;
//using Shared.DTOs.User.Customer;
//using Shared.GeneralModels;

//namespace BL.Services.Order.OrderProcessing;

///// <summary>
///// Service for managing orders - FIXED VERSION
///// 
///// FIXES APPLIED:
///// 1. Fixed GetItemShipmentStatus method
/////    - TbOrderShipment doesn't have OrderDetailId directly
/////    - Must navigate through TbOrderShipment.Items collection
/////    - Items is ICollection<TbOrderShipmentItem> where each has OrderDetailId
///// 
///// 2. Fixed MapToAdminOrderItem method
/////    - TbOrderDetail doesn't have Warehouse navigation property
/////    - Only has WarehouseId field
/////    - Removed reference to od.Warehouse?.NameEn
///// </summary>
//public class OrderManagementService : IOrderManagementService
//{
//    private readonly IOrderRepository _orderRepository;
//    private readonly IMapper _mapper;
//    private readonly ILogger _logger;

//    public OrderManagementService(
//        IOrderRepository orderRepository,
//        IMapper mapper,
//        ILogger logger)
//    {
//        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
//        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    }

//    // ============================================
//    // CUSTOMER OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Get customer orders list with pagination
//    /// </summary>
//    public async Task<AdvancedPagedResult<CustomerOrderListDto>> GetCustomerOrdersListAsync(
//        string customerId,
//        int pageNumber = 1,
//        int pageSize = 10,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var (orders, totalCount) = await _orderRepository.GetCustomerOrdersWithPaginationAsync(
//                customerId,
//                pageNumber,
//                pageSize,
//                cancellationToken);

//            var orderDtos = orders.Select(order => new CustomerOrderListDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                TotalAmount = order.Price,
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                ShipmentStatus = order.TbOrderShipments
//                    .OrderByDescending(s => s.CreatedDateUtc)
//                    .FirstOrDefault()?.ShipmentStatus ?? ShipmentStatus.Pending,
//                TotalItems = order.OrderDetails.Sum(od => od.Quantity),
//                ItemsSummary = order.OrderDetails
//                    .Take(3)
//                    .Select(od => new OrderItemSummaryDto
//                    {
//                        ItemName = od.Item?.TitleEn ?? "",
//                        ThumbnailImage = od.Item?.ThumbnailImage ?? "",
//                        Quantity = od.Quantity
//                    }).ToList(),
//                CanCancel = CanCancelOrder(order.OrderStatus),
//                IsWithinRefundPeriod = IsWithinRefundPeriod(order.CreatedDateUtc)
//            }).ToList();

//            return new AdvancedPagedResult<CustomerOrderListDto>
//            {
//                Items = orderDtos,
//                TotalRecords = totalCount,
//                PageNumber = pageNumber,
//                PageSize = pageSize
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting customer orders for {CustomerId}", customerId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Get customer order full details
//    /// </summary>
//    public async Task<CustomerOrderDetailsDto?> GetCustomerOrderDetailsAsync(
//        Guid orderId,
//        string customerId,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

//            if (order == null || order.UserId != customerId)
//            {
//                return null;
//            }

//            return new CustomerOrderDetailsDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                DeliveryDate = order.OrderDeliveryDate,
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                SubTotal = order.OrderDetails.Sum(od => od.SubTotal),
//                ShippingAmount = order.ShippingAmount,
//                TaxAmount = order.OrderDetails.Sum(od => od.TaxAmount),
//                DiscountAmount = order.OrderDetails.Sum(od => od.DiscountAmount),
//                TotalAmount = order.Price,
//                DeliveryAddress = new DeliveryAddressDto
//                {
//                    Address = order.CustomerAddress?.Address ?? "",
//                    CityNameAr = order.CustomerAddress?.City?.TitleAr ?? "",
//                    CityNameEn = order.CustomerAddress?.City?.TitleEn ?? "",
//                    StateNameAr = order.CustomerAddress?.City?.State?.TitleAr ?? "",
//                    StateNameEn = order.CustomerAddress?.City?.State?.TitleEn ?? "",
//                    PhoneCode = order.CustomerAddress?.PhoneCode ?? "",
//                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? "",
//                    RecipientName = order.CustomerAddress?.RecipientName ?? ""
//                },
//                PaymentInfo = new PaymentInfoDto
//                {
//                    Status = order.PaymentStatus,
//                    PaymentMethod = order.OrderPayments.FirstOrDefault()?.PaymentMethod.ToString() ?? "",
//                    TransactionId = order.InvoiceId,
//                    PaymentDate = order.PaidAt,
//                    Amount = order.Price
//                },
//                Items = order.OrderDetails.Select(od => new CustomerOrderItemDto
//                {
//                    OrderDetailId = od.Id,
//                    ItemId = od.ItemId,
//                    ItemName = od.Item?.TitleEn ?? "",
//                    ItemImage = od.Item?.ThumbnailImage ?? "",
//                    VendorName = od.Vendor?.StoreName ?? "",
//                    Quantity = od.Quantity,
//                    UnitPrice = od.UnitPrice,
//                    SubTotal = od.SubTotal,
//                    DiscountAmount = od.DiscountAmount,
//                    TaxAmount = od.TaxAmount,
//                    ShipmentStatus = GetItemShipmentStatus(od.Id, order.TbOrderShipments)
//                }).ToList(),
//                Shipments = order.TbOrderShipments.Select(s => new ShipmentInfoDto
//                {
//                    ShipmentId = s.Id,
//                    ShipmentNumber = s.Number ?? "",
//                    Status = s.ShipmentStatus,
//                    TrackingNumber = s.TrackingNumber,
//                    EstimatedDeliveryDate = s.EstimatedDeliveryDate,
//                    ActualDeliveryDate = s.ActualDeliveryDate
//                }).ToList(),
//                CanCancel = CanCancelOrder(order.OrderStatus),
//                CanRequestRefund = CanRequestRefund(order.OrderStatus, order.CreatedDateUtc),
//                IsWithinRefundPeriod = IsWithinRefundPeriod(order.CreatedDateUtc)
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting customer order details {OrderId}", orderId);
//            throw;
//        }
//    }

//    // ============================================
//    // VENDOR OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Get vendor orders with pagination and search
//    /// Shows only orders containing vendor's items
//    /// </summary>
//    public async Task<AdvancedPagedResult<VendorOrderListDto>> GetVendorOrdersAsync(
//        string vendorId,
//        string? searchTerm,
//        int pageNumber,
//        int pageSize,
//        string? sortBy,
//        string? sortDirection,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var (orders, totalCount) = await _orderRepository.GetVendorOrdersWithPaginationAsync(
//                vendorId,
//                searchTerm,
//                pageNumber,
//                pageSize,
//                sortBy,
//                sortDirection,
//                cancellationToken);

//            var orderDtos = orders.Select(order => new VendorOrderListDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
//                TotalAmount = order.OrderDetails.Sum(od => od.SubTotal),
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                ItemsCount = order.OrderDetails.Sum(od => od.Quantity)
//            }).ToList();

//            return new AdvancedPagedResult<VendorOrderListDto>
//            {
//                Items = orderDtos,
//                TotalRecords = totalCount,
//                PageNumber = pageNumber,
//                PageSize = pageSize
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting vendor orders for {VendorId}", vendorId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Get vendor order details
//    /// Shows only items belonging to this vendor
//    /// </summary>
//    public async Task<VendorOrderDetailsDto?> GetVendorOrderDetailsAsync(
//        Guid orderId,
//        string vendorId,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (!Guid.TryParse(vendorId, out var vendorGuid))
//            {
//                return null;
//            }

//            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

//            if (order == null)
//            {
//                return null;
//            }

//            // Filter order details to only show vendor's items
//            var vendorItems = order.OrderDetails
//                .Where(od => od.VendorId == vendorGuid)
//                .ToList();

//            if (!vendorItems.Any())
//            {
//                return null;
//            }

//            return new VendorOrderDetailsDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
//                CustomerPhone = order.CustomerAddress?.PhoneNumber ?? "",
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                DeliveryAddress = new DeliveryAddressDto
//                {
//                    Address = order.CustomerAddress?.Address ?? "",
//                    CityNameEn = order.CustomerAddress?.City?.TitleEn ?? "",
//                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? "",
//                    RecipientName = order.CustomerAddress?.RecipientName ?? ""
//                },
//                Items = vendorItems.Select(od => new VendorOrderItemDto
//                {
//                    OrderDetailId = od.Id,
//                    ItemId = od.ItemId,
//                    ItemName = od.Item?.TitleEn ?? "",
//                    ItemImage = od.Item?.ThumbnailImage ?? "",
//                    Quantity = od.Quantity,
//                    UnitPrice = od.UnitPrice,
//                    SubTotal = od.SubTotal,
//                    ShipmentStatus = GetItemShipmentStatus(od.Id, order.TbOrderShipments)
//                }).ToList(),
//                VendorTotal = vendorItems.Sum(od => od.SubTotal)
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting vendor order details {OrderId}", orderId);
//            throw;
//        }
//    }

//    // ============================================
//    // ADMIN OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Get admin orders with pagination and search
//    /// </summary>
//    public async Task<AdvancedPagedResult<AdminOrderListDto>> GetAdminOrdersAsync(
//        string? searchTerm,
//        int pageNumber,
//        int pageSize,
//        string? sortBy,
//        string? sortDirection,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var (orders, totalCount) = await _orderRepository.SearchOrdersAsync(
//                searchTerm,
//                pageNumber,
//                pageSize,
//                sortBy,
//                sortDirection,
//                cancellationToken);

//            var orderDtos = orders.Select(order => new AdminOrderListDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                CustomerName = $"{order.User?.FirstName} {order.User?.LastName}",
//                CustomerEmail = order.User?.Email ?? "",
//                TotalAmount = order.Price,
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                ItemsCount = order.OrderDetails.Sum(od => od.Quantity)
//            }).ToList();

//            return new AdvancedPagedResult<AdminOrderListDto>
//            {
//                Items = orderDtos,
//                TotalRecords = totalCount,
//                PageNumber = pageNumber,
//                PageSize = pageSize
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting admin orders");
//            throw;
//        }
//    }

//    /// <summary>
//    /// Get admin order details with full information
//    /// </summary>
//    public async Task<AdminOrderDetailsDto?> GetAdminOrderDetailsAsync(
//        Guid orderId,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var order = await _orderRepository.GetOrderWithFullDetailsAsync(orderId, cancellationToken);

//            if (order == null)
//            {
//                return null;
//            }

//            return new AdminOrderDetailsDto
//            {
//                OrderId = order.Id,
//                OrderNumber = order.Number,
//                OrderDate = order.CreatedDateUtc,
//                DeliveryDate = order.OrderDeliveryDate,
//                OrderStatus = order.OrderStatus,
//                PaymentStatus = order.PaymentStatus,
//                Customer = new CustomerInfoDto
//                {
//                    UserId = order.UserId,
//                    FullName = $"{order.User?.FirstName} {order.User?.LastName}",
//                    Email = order.User?.Email ?? "",
//                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? ""
//                },
//                DeliveryAddress = new DeliveryAddressDto
//                {
//                    Address = order.CustomerAddress?.Address ?? "",
//                    CityNameAr = order.CustomerAddress?.City?.TitleAr ?? "",
//                    CityNameEn = order.CustomerAddress?.City?.TitleEn ?? "",
//                    StateNameAr = order.CustomerAddress?.City?.State?.TitleAr ?? "",
//                    StateNameEn = order.CustomerAddress?.City?.State?.TitleEn ?? "",
//                    PhoneCode = order.CustomerAddress?.PhoneCode ?? "",
//                    PhoneNumber = order.CustomerAddress?.PhoneNumber ?? "",
//                    RecipientName = order.CustomerAddress?.RecipientName ?? ""
//                },
//                Items = order.OrderDetails.Select(od => MapToAdminOrderItem(od, order.TbOrderShipments)).ToList(),
//                Shipments = order.TbOrderShipments.Select(s => new ShipmentInfoDto
//                {
//                    ShipmentId = s.Id,
//                    ShipmentNumber = s.Number ?? "",
//                    Status = s.ShipmentStatus,
//                    TrackingNumber = s.TrackingNumber,
//                    EstimatedDeliveryDate = s.EstimatedDeliveryDate,
//                    ActualDeliveryDate = s.ActualDeliveryDate
//                }).ToList(),
//                PaymentInfo = new PaymentInfoDto
//                {
//                    Status = order.PaymentStatus,
//                    PaymentMethod = order.OrderPayments.FirstOrDefault()?.PaymentMethod.ToString() ?? "",
//                    TransactionId = order.InvoiceId,
//                    PaymentDate = order.PaidAt,
//                    Amount = order.Price
//                },
//                Coupon = order.Coupon != null ? new CouponInfoDto
//                {
//                    Code = order.Coupon.Code ?? "",
//                    DiscountAmount = order.DiscountAmount
//                } : null,
//                SubTotal = order.SubTotal,
//                ShippingAmount = order.ShippingAmount,
//                TaxAmount = order.TaxAmount,
//                DiscountAmount = order.DiscountAmount,
//                TotalAmount = order.Price,
//                Notes = order.Notes
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error getting admin order details {OrderId}", orderId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Cancel order (Customer or Admin)
//    /// </summary>
//    public async Task<ResponseModel<bool>> CancelOrderAsync(
//        Guid orderId,
//        string? reason,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

//            if (order == null)
//            {
//                return new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = "Order not found"
//                };
//            }

//            if (!CanCancelOrder(order.OrderStatus))
//            {
//                return new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = "Order cannot be cancelled at this stage"
//                };
//            }

//            order.OrderStatus = OrderProgressStatus.Cancelled;
//            order.UpdatedDateUtc = DateTime.UtcNow;

//            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

//            return new ResponseModel<bool>
//            {
//                Success = true,
//                Message = "Order cancelled successfully",
//                Data = true
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error cancelling order {OrderId}", orderId);
//            return new ResponseModel<bool>
//            {
//                Success = false,
//                Message = ex.Message
//            };
//        }
//    }

//    /// <summary>
//    /// Update order (Admin)
//    /// </summary>
//    public async Task<ResponseModel<bool>> UpdateOrderAsync(
//        UpdateOrderRequest request,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var order = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

//            if (order == null)
//            {
//                return new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = "Order not found"
//                };
//            }

//            if (request.OrderDeliveryDate.HasValue)
//            {
//                order.OrderDeliveryDate = request.OrderDeliveryDate;
//            }

//            order.UpdatedDateUtc = DateTime.UtcNow;
//            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

//            return new ResponseModel<bool>
//            {
//                Success = true,
//                Message = "Order updated successfully",
//                Data = true
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error updating order {OrderId}", request.OrderId);
//            return new ResponseModel<bool>
//            {
//                Success = false,
//                Message = ex.Message
//            };
//        }
//    }

//    /// <summary>
//    /// Change order status (Admin)
//    /// </summary>
//    public async Task<ResponseModel<bool>> ChangeOrderStatusAsync(
//        Guid orderId,
//        OrderProgressStatus newStatus,
//        string? notes,
//        string adminUserId,
//        CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

//            if (order == null)
//            {
//                return new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = "Order not found"
//                };
//            }

//            if (!IsValidStatusTransition(order.OrderStatus, newStatus))
//            {
//                return new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = $"Invalid status transition from {order.OrderStatus} to {newStatus}"
//                };
//            }

//            order.OrderStatus = newStatus;
//            order.UpdatedDateUtc = DateTime.UtcNow;

//            if (newStatus == OrderProgressStatus.Delivered && !order.OrderDeliveryDate.HasValue)
//            {
//                order.OrderDeliveryDate = DateTime.UtcNow;
//            }

//            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

//            return new ResponseModel<bool>
//            {
//                Success = true,
//                Message = $"Order status changed to {newStatus}",
//                Data = true
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error changing order status {OrderId}", orderId);
//            return new ResponseModel<bool>
//            {
//                Success = false,
//                Message = ex.Message
//            };
//        }
//    }

//    // ============================================
//    // HELPER METHODS - FIXED
//    // ============================================

//    /// <summary>
//    /// Map OrderDetail to AdminOrderItemDto
//    /// 
//    /// FIXED: Removed reference to od.Warehouse
//    /// - TbOrderDetail doesn't have Warehouse navigation property
//    /// - Only has WarehouseId (Guid) field
//    /// - If you need warehouse name, you must:
//    ///   1. Add Warehouse navigation property to TbOrderDetail
//    ///   2. Include it in the query
//    ///   3. Or load warehouses separately and join in memory
//    /// </summary>
//    private AdminOrderItemDto MapToAdminOrderItem(
//        TbOrderDetail od,
//        ICollection<TbOrderShipment> shipments)
//    {
//        return new AdminOrderItemDto
//        {
//            OrderDetailId = od.Id,
//            ItemId = od.ItemId,
//            ItemName = od.Item?.TitleEn ?? "",
//            ItemImage = od.Item?.ThumbnailImage ?? "",
//            VendorId = od.VendorId,
//            VendorName = od.Vendor?.StoreName ?? "",
//            Quantity = od.Quantity,
//            UnitPrice = od.UnitPrice,
//            SubTotal = od.SubTotal,
//            DiscountAmount = od.DiscountAmount,
//            TaxAmount = od.TaxAmount,
//            ShipmentStatus = GetItemShipmentStatus(od.Id, shipments),
//            WarehouseId = od.WarehouseId
//            // REMOVED: WarehouseName = od.Warehouse?.NameEn
//            // REASON: TbOrderDetail doesn't have Warehouse navigation property
//            // SOLUTION: Add the navigation property or load warehouses separately
//        };
//    }

//    /// <summary>
//    /// Get shipment status for a specific order detail item
//    /// 
//    /// FIXED: Correct navigation through TbOrderShipment.Items
//    /// 
//    /// EXPLANATION:
//    /// - TbOrderShipment does NOT have a direct OrderDetailId property
//    /// - Instead, it has an Items collection (ICollection<TbOrderShipmentItem>)
//    /// - Each TbOrderShipmentItem has an OrderDetailId
//    /// - So we must navigate: shipment → Items → find item with matching OrderDetailId
//    /// 
//    /// PREVIOUS INCORRECT CODE:
//    /// shipments.Where(s => s.OrderDetailId == orderDetailId)
//    /// 
//    /// CORRECTED CODE:
//    /// shipments.Where(s => s.Items.Any(item => item.OrderDetailId == orderDetailId))
//    /// </summary>
//    private ShipmentStatus GetItemShipmentStatus(
//        Guid orderDetailId,
//        ICollection<TbOrderShipment> shipments)
//    {
//        return shipments
//            .Where(s => s.Items.Any(item => item.OrderDetailId == orderDetailId))
//            .OrderByDescending(s => s.CreatedDateUtc)
//            .FirstOrDefault()?.ShipmentStatus ?? ShipmentStatus.Pending;
//    }

//    private bool CanCancelOrder(OrderProgressStatus status)
//    {
//        return status == OrderProgressStatus.Pending ||
//               status == OrderProgressStatus.Confirmed;
//    }

//    private bool CanRequestRefund(OrderProgressStatus status, DateTime orderDate)
//    {
//        return (status == OrderProgressStatus.Delivered ||
//                status == OrderProgressStatus.Completed) &&
//               IsWithinRefundPeriod(orderDate);
//    }

//    private bool IsWithinRefundPeriod(DateTime orderDate)
//    {
//        return DateTime.UtcNow.Subtract(orderDate).Days <= 15;
//    }

//    private bool IsValidStatusTransition(OrderProgressStatus current, OrderProgressStatus target)
//    {
//        // Same status - allow
//        if (current == target) return true;

//        // Define valid transitions
//        var validTransitions = new Dictionary<OrderProgressStatus, List<OrderProgressStatus>>
//        {
//            [OrderProgressStatus.Pending] = new() { OrderProgressStatus.Confirmed, OrderProgressStatus.Cancelled },
//            [OrderProgressStatus.Confirmed] = new() { OrderProgressStatus.Processing, OrderProgressStatus.Cancelled },
//            [OrderProgressStatus.Processing] = new() { OrderProgressStatus.Shipped, OrderProgressStatus.Cancelled },
//            [OrderProgressStatus.Shipped] = new() { OrderProgressStatus.Delivered, OrderProgressStatus.Returned },
//            [OrderProgressStatus.Delivered] = new() { OrderProgressStatus.Completed, OrderProgressStatus.Returned },
//            [OrderProgressStatus.Completed] = new() { OrderProgressStatus.Returned },
//            [OrderProgressStatus.Cancelled] = new(),
//            [OrderProgressStatus.Returned] = new() { OrderProgressStatus.Refunded },
//            [OrderProgressStatus.Refunded] = new()
//        };

//        return validTransitions.ContainsKey(current) &&
//               validTransitions[current].Contains(target);
//    }
//}
