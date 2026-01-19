using AutoMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Order;
using Common.Enumerations.Shipping;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.ResponseOrderDetail;
using Shared.GeneralModels;

namespace BL.Services.Order.OrderProcessing;

/// <summary>
/// Service for managing orders (CRUD operations)
/// REFACTORED: Uses repositories directly, no UnitOfWork, no Info logging
/// Note: Order creation is handled by OrderCreationService
/// </summary>
public class OrderManagementService : IOrderManagementService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITableRepository<TbOrderDetail> _orderDetailRepository;
    private readonly ITableRepository<TbOrderShipment> _shipmentRepository;
    private readonly ITableRepository<TbItem> _itemRepository;
    private readonly ITableRepository<TbVendor> _vendorRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OrderManagementService(
        IOrderRepository orderRepository,
        ITableRepository<TbOrderDetail> orderDetailRepository,
        ITableRepository<TbOrderShipment> shipmentRepository,
        ITableRepository<TbItem> itemRepository,
        ITableRepository<TbVendor> vendorRepository,
        ICartService cartService,
        IMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
        _shipmentRepository = shipmentRepository ?? throw new ArgumentNullException(nameof(shipmentRepository));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // ============================================
    // READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get order by ID
    /// </summary>
    public async Task<OrderDto?> GetOrderByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get order by order number
    /// </summary>
    public async Task<OrderDto?> GetOrderByNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ArgumentException("Order number cannot be empty", nameof(orderNumber));
            }

            var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, cancellationToken);

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order by number {OrderNumber}", orderNumber);
            throw;
        }
    }

    /// <summary>
    /// Get order with full details including shipments
    /// </summary>
    public async Task<OrderDto?> GetOrderWithShipmentsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId, cancellationToken);

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order with shipments {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get all orders (for admin/dashboard)
    /// </summary>
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var orders = await _orderRepository.GetAsync(
                o => !o.IsDeleted,
                cancellationToken);

            if (!orders.Any())
            {
                return Enumerable.Empty<OrderDto>();
            }

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting all orders");
            throw;
        }
    }

    /// <summary>
    /// Get customer orders with pagination
    /// Returns one DTO per order detail (for display in list)
    /// </summary>
    public async Task<List<OrderListItemDto>> GetCustomerOrdersAsync(
        string customerId,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));
            }

            // Validate pagination
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Get orders with pagination
            var orders = await _orderRepository.GetByCustomerIdAsync(
                customerId,
                pageNumber,
                pageSize,
                cancellationToken);

            if (!orders.Any())
            {
                return new List<OrderListItemDto>();
            }

            var orderIds = orders.Select(o => o.Id).ToList();

            // Load order details
            var orderDetails = await _orderDetailRepository.GetAsync(
                od => orderIds.Contains(od.OrderId) && !od.IsDeleted,
                cancellationToken);

            // Load items
            var itemIds = orderDetails.Select(od => od.ItemId).Distinct().ToList();
            var items = await _itemRepository.GetAsync(
                i => itemIds.Contains(i.Id) && !i.IsDeleted,
                cancellationToken);
            var itemDict = items.ToDictionary(i => i.Id);

            // Load vendors
            var vendorIds = orderDetails.Select(od => od.VendorId).Distinct().ToList();
            var vendors = await _vendorRepository.GetAsync(
                v => vendorIds.Contains(v.Id) && !v.IsDeleted,
                cancellationToken);
            var vendorDict = vendors.ToDictionary(v => v.Id);

            // Load latest shipments
            var shipments = await _shipmentRepository.GetAsync(
                s => orderIds.Contains(s.OrderId) && !s.IsDeleted,
                cancellationToken);
            var latestShipments = shipments
                .GroupBy(s => s.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(s => s.CreatedDateUtc).First());

            // Build result
            var result = new List<OrderListItemDto>();

            foreach (var order in orders)
            {
                var orderDetailsForOrder = orderDetails.Where(od => od.OrderId == order.Id);

                // Get latest shipment status for this order
                var shipmentStatus = latestShipments.ContainsKey(order.Id)
                    ? latestShipments[order.Id].ShipmentStatus
                    : ShipmentStatus.Pending;

                foreach (var orderDetail in orderDetailsForOrder)
                {
                    var item = itemDict.ContainsKey(orderDetail.ItemId)
                        ? itemDict[orderDetail.ItemId]
                        : null;

                    var vendor = vendorDict.ContainsKey(orderDetail.VendorId)
                        ? vendorDict[orderDetail.VendorId]
                        : null;

                    result.Add(new OrderListItemDto
                    {
                        Id = order.Id,
                        OrderNumber = order.Number,
                        SellerName = vendor?.StoreName ?? "",
                        ItemImageUrl = item?.ThumbnailImage ?? "",
                        ItemNameAr = item?.TitleAr ?? "",
                        ItemNameEn = item?.TitleEn ?? "",
                        QuantityItem = orderDetail.Quantity,
                        Price = orderDetail.UnitPrice,
                        Total = order.Price,
                        OrderStatus = order.OrderStatus.ToString(),
                        PaymentStatus = order.PaymentStatus.ToString(),
                        ShipmentStatus = shipmentStatus,
                        CreatedDate = order.CreatedDateUtc
                    });
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting customer orders for {CustomerId}", customerId);
            throw;
        }
    }

    /// <summary>
    /// Get order items list by order ID
    /// </summary>
    public async Task<List<ResponseOrderItemDetailsDto>> GetListByOrderIdAsync(
        Guid orderId,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            // Get order
            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null || order.OrderStatus == OrderProgressStatus.Cancelled)
            {
                return new List<ResponseOrderItemDetailsDto>();
            }

            // Security check
            if (!string.IsNullOrEmpty(userId) && order.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't have permission to view this order");
            }

            // Get order details
            var orderDetails = await _orderDetailRepository.GetAsync(
                od => od.OrderId == orderId && !od.IsDeleted,
                cancellationToken);

            if (!orderDetails.Any())
            {
                return new List<ResponseOrderItemDetailsDto>();
            }

            // Load items
            var itemIds = orderDetails.Select(od => od.ItemId).Distinct().ToList();
            var items = await _itemRepository.GetAsync(
                i => itemIds.Contains(i.Id) && !i.IsDeleted,
                cancellationToken);
            var itemDict = items.ToDictionary(i => i.Id);

            // Load vendors
            var vendorIds = orderDetails.Select(od => od.VendorId).Distinct().ToList();
            var vendors = await _vendorRepository.GetAsync(
                v => vendorIds.Contains(v.Id) && !v.IsDeleted,
                cancellationToken);
            var vendorDict = vendors.ToDictionary(v => v.Id);

            // Load latest shipment
            var shipments = await _shipmentRepository.GetAsync(
                s => s.OrderId == orderId && !s.IsDeleted,
                cancellationToken);
            var latestShipment = shipments
                .OrderByDescending(s => s.CreatedDateUtc)
                .FirstOrDefault();

            // Build result
            var result = new List<ResponseOrderItemDetailsDto>();

            foreach (var orderDetail in orderDetails)
            {
                var item = itemDict.ContainsKey(orderDetail.ItemId)
                    ? itemDict[orderDetail.ItemId]
                    : null;

                var vendor = vendorDict.ContainsKey(orderDetail.VendorId)
                    ? vendorDict[orderDetail.VendorId]
                    : null;

                result.Add(new ResponseOrderItemDetailsDto
                {
                    OrderDetailId = orderDetail.Id,
                    ItemId = orderDetail.ItemId,
                    ItemName = item?.TitleEn ?? "",
                    ItemImageUrl = item?.ThumbnailImage ?? "",
                    VendorId = orderDetail.VendorId,
                    VendorStoreName = vendor?.StoreName ?? "",
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.UnitPrice,
                    SubTotal = orderDetail.SubTotal,
                    ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order items list for {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get detailed order information by order details ID
    /// </summary>
    public async Task<ResponseOrderDetailsDto?> GetOrderDetailsByIdAsync(
        Guid orderDetailsId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderDetailsId == Guid.Empty)
            {
                throw new ArgumentException("Order details ID cannot be empty", nameof(orderDetailsId));
            }

            // Get order detail
            var orderDetail = await _orderDetailRepository.FindByIdAsync(orderDetailsId, cancellationToken);

            if (orderDetail == null)
            {
                return null;
            }

            // Security check - verify user owns this order
            if (!string.IsNullOrEmpty(userId))
            {
                var order = await _orderRepository.FindByIdAsync(orderDetail.OrderId, cancellationToken);

                if (order == null)
                {
                    return null;
                }

                if (order.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You don't have permission to view this order detail");
                }
            }

            // Load item
            var item = await _itemRepository.FindByIdAsync(orderDetail.ItemId, cancellationToken);

            // Load vendor
            var vendor = await _vendorRepository.FindByIdAsync(orderDetail.VendorId, cancellationToken);

            // Load latest shipment
            var shipments = await _shipmentRepository.GetAsync(
                s => s.OrderId == orderDetail.OrderId && !s.IsDeleted,
                cancellationToken);
            var latestShipment = shipments
                .OrderByDescending(s => s.CreatedDateUtc)
                .FirstOrDefault();

            // Build result
            return new ResponseOrderDetailsDto
            {
                OrderDetailId = orderDetail.Id,
                ItemId = orderDetail.ItemId,
                ItemName = item?.TitleEn ?? "",
                ItemImageUrl = item?.ThumbnailImage ?? "",
                VendorId = orderDetail.VendorId,
                VendorStoreName = vendor?.StoreName ?? "",
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                SubTotal = orderDetail.SubTotal,
                DiscountAmount = orderDetail.DiscountAmount,
                TaxAmount = orderDetail.TaxAmount,
                ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order details {OrderDetailsId}", orderDetailsId);
            throw;
        }
    }

    // ============================================
    // WRITE OPERATIONS
    // ============================================

    /// <summary>
    /// Cancel order before shipping
    /// Can only cancel orders that are not yet shipped/delivered/cancelled
    /// </summary>
    public async Task<bool> CancelOrderAsync(
        Guid orderId,
        string reason,
        string? adminNotes = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Cancellation reason cannot be empty", nameof(reason));
            }

            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null || order.IsDeleted)
            {
                return false;
            }

            // Check if order can be cancelled
            if (order.OrderStatus == OrderProgressStatus.Shipped ||
                order.OrderStatus == OrderProgressStatus.Delivered ||
                order.OrderStatus == OrderProgressStatus.Cancelled)
            {
                return false;
            }

            // Update order status
            order.OrderStatus = OrderProgressStatus.Cancelled;
            order.UpdatedDateUtc = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error cancelling order {OrderId}", orderId);
            throw;
        }
    }

    // ============================================
    // VALIDATION & STATUS OPERATIONS
    // ============================================

    /// <summary>
    /// Get order completion status
    /// </summary>
    public async Task<OrderCompletionStatusDto> GetOrderCompletionStatusAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return new OrderCompletionStatusDto
                {
                    OrderId = orderId,
                    IsComplete = false
                };
            }

            bool isComplete = order.OrderStatus == OrderProgressStatus.Completed ||
                             order.OrderStatus == OrderProgressStatus.Delivered;

            return new OrderCompletionStatusDto
            {
                OrderId = orderId,
                OrderNumber = order.Number,
                OrderStatus = order.OrderStatus,
                IsComplete = isComplete
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order completion status {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Validate order data
    /// </summary>
    public async Task<bool> ValidateOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            }

            var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return false;
            }

            bool isValid = !string.IsNullOrWhiteSpace(order.Number) &&
                          !string.IsNullOrWhiteSpace(order.UserId) &&
                          order.Price > 0 &&
                          order.CreatedDateUtc <= DateTime.UtcNow;

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error validating order {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Search orders with pagination and filtering (for admin dashboard)
    /// </summary>
    public async Task<(List<OrderDto> Items, int TotalRecords)> SearchOrdersAsync(
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "CreatedDateUtc",
        string sortDirection = "desc",
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Delegate to repository for data access
            var (orders, totalCount) = await _orderRepository.SearchAsync(
                searchTerm,
                pageNumber,
                pageSize,
                sortBy,
                sortDirection,
                cancellationToken);

            if (!orders.Any())
            {
                return (new List<OrderDto>(), totalCount);
            }

            // Map to DTOs
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return (orderDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error searching orders with term: {SearchTerm}, page: {PageNumber}", searchTerm, pageNumber);
            throw;
        }
    }

    /// <summary>
    /// Save/Update order (used by Dashboard - Details.razor)
    /// </summary>
    public async Task<ResponseModel<bool>> SaveAsync(
        OrderDto orderDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderDto == null || orderDto.Id == Guid.Empty)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid order data",
                    Data = false
                };
            }

            var order = await _orderRepository.FindByIdAsync(orderDto.Id, cancellationToken);

            if (order == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order not found",
                    Data = false
                };
            }

            // Update allowed fields (only delivery date for now)
            if (orderDto.OrderDeliveryDate.HasValue)
            {
                order.OrderDeliveryDate = orderDto.OrderDeliveryDate;
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
            _logger.Error(ex, "Error saving order {OrderId}", orderDto?.Id);
            return new ResponseModel<bool>
            {
                Success = false,
                Message = $"Failed to update order: {ex.Message}",
                Data = false
            };
        }
    }

    /// <summary>
    /// Change order status (Admin Dashboard - Details.razor)
    /// Uses OrderStatusTransitionService for event handling
    /// </summary>
    public async Task<ResponseModel<bool>> ChangeOrderStatusAsync(
        OrderDto orderDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (orderDto == null || orderDto.Id == Guid.Empty)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid order data",
                    Data = false
                };
            }

            var order = await _orderRepository.FindByIdAsync(orderDto.Id, cancellationToken);

            if (order == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Order not found",
                    Data = false
                };
            }

            // Use the OrderProgressStatus directly from the DTO
            var newStatus = orderDto.CurrentState;

            // Validate status transition
            if (!IsValidStatusTransition(order.OrderStatus, newStatus))
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = $"Cannot change status from {order.OrderStatus} to {newStatus}",
                    Data = false
                };
            }

            // Update status
            var oldStatus = order.OrderStatus;
            order.OrderStatus = newStatus;
            order.UpdatedDateUtc = DateTime.UtcNow;

            // Additional logic based on status
            switch (newStatus)
            {
                case OrderProgressStatus.Delivered:
                    // Set delivery date if not already set
                    if (!order.OrderDeliveryDate.HasValue)
                    {
                        order.OrderDeliveryDate = DateTime.UtcNow;
                    }
                    break;

                case OrderProgressStatus.Cancelled:
                case OrderProgressStatus.PaymentFailed:
                    // Mark payment as failed/refunded if exists
                    // This could trigger refund processing
                    break;
            }

            await _orderRepository.UpdateAsync(order, Guid.Empty, cancellationToken);

            _logger.Information(
                "Order {OrderId} status changed from {OldStatus} to {NewStatus}",
                order.Id, oldStatus, newStatus);

            return new ResponseModel<bool>
            {
                Success = true,
                Message = $"Order status changed to {newStatus} successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error changing order status for order {OrderId}", orderDto?.Id);
            return new ResponseModel<bool>
            {
                Success = false,
                Message = $"Failed to change order status: {ex.Message}",
                Data = false
            };
        }
    }

    /// <summary>
    /// Validate if status transition is allowed
    /// Based on the actual OrderProgressStatus enum values
    /// </summary>
    private bool IsValidStatusTransition(OrderProgressStatus currentStatus, OrderProgressStatus newStatus)
    {
        // If same status, allow (idempotent)
        if (currentStatus == newStatus)
        {
            return true;
        }

        // Define valid transitions based on actual enum
        var validTransitions = new Dictionary<OrderProgressStatus, List<OrderProgressStatus>>
        {
            [OrderProgressStatus.Pending] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Confirmed,
                OrderProgressStatus.Cancelled
            },
            [OrderProgressStatus.Confirmed] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Processing,
                OrderProgressStatus.Cancelled
            },
            [OrderProgressStatus.Processing] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Shipped,
                OrderProgressStatus.Cancelled
            },
            [OrderProgressStatus.Shipped] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Delivered,
                OrderProgressStatus.Returned
            },
            [OrderProgressStatus.Delivered] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Completed,
                OrderProgressStatus.Returned // Allow returns after delivery
            },
            [OrderProgressStatus.Completed] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Returned // Allow returns after completion
            },
            [OrderProgressStatus.Cancelled] = new List<OrderProgressStatus>(), // Final state
            [OrderProgressStatus.PaymentFailed] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Pending // Allow retry
            },
            [OrderProgressStatus.RefundRequested] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Refunded,
                OrderProgressStatus.Cancelled
            },
            [OrderProgressStatus.Refunded] = new List<OrderProgressStatus>(), // Final state
            [OrderProgressStatus.Returned] = new List<OrderProgressStatus>
            {
                OrderProgressStatus.Refunded // Can lead to refund
            }
        };

        // Check if transition is valid
        return validTransitions.ContainsKey(currentStatus) &&
               validTransitions[currentStatus].Contains(newStatus);
    }
}