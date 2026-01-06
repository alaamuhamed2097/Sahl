using BL.Contracts.IMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Shipping;
using Domains.Entities.Warehouse;
using Serilog;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.Cart;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.ResponseOrderDetail;

namespace BL.Services.Order.OrderProcessing;

/// <summary>
/// Handles order creation, retrieval, cancellation, and validation.
/// FIXED: Now correctly handles CartItemDto.OfferId as OfferCombinationPricingId
/// </summary>
public class OrderMangmentService : IOrderMangmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;


    public OrderMangmentService(
        IUnitOfWork unitOfWork,
        ICartService cartService,
        IBaseMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OrderCreatedResponseDto> CreateOrderFromCartAsync(string customerId, CreateOrderFromCartRequest request)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("Customer ID is required.", nameof(customerId));
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        try
        {
            var cartSummary = await _cartService.GetCartSummaryAsync(customerId);
            if (cartSummary?.Items == null || !cartSummary.Items.Any())
                throw new InvalidOperationException("Cannot create order: cart is empty.");

            //validate DeliveryAddress
            var deliveryAddressRepo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var deliveryAddress = await deliveryAddressRepo
                .FindAsync(a => a.Id == request.DeliveryAddressId
                && a.UserId == customerId && !a.IsDeleted);
            if (deliveryAddress == null)
                throw new NotFoundException("Delivery address not found.", _logger);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customerIdGuid = Guid.Parse(customerId);

                // Step 1: Validate and reserve stock (with offer data loaded)
                var pricingWithOffers = await ReserveStockForCartItems(customerId, cartSummary.Items);

                // Step 2: Create order header
                var order = CreateOrderHeader(customerId, request, cartSummary);
                await _unitOfWork.TableRepository<TbOrder>().CreateAsync(order, customerIdGuid);

                // Step 3: Create order details
                var orderDetails = await CreateOrderDetails(order.Id, cartSummary.Items, customerIdGuid, pricingWithOffers);
                await _unitOfWork.TableRepository<TbOrderDetail>().AddRangeAsync(orderDetails, customerIdGuid);

                // Step 4: Create payment record
                var defaultCurrency = await _unitOfWork.TableRepository<TbCurrency>()
                    .FindAsync(c => c.IsBaseCurrency && !c.IsDeleted);

                if (defaultCurrency == null)
                {
                    _logger.Error("Default currency not found while creating order for customer {CustomerId}", customerId);
                    throw new NotFoundException("Default currency not found.", _logger);
                }

                var payment = CreateOrderPayment(order.Id, order.Price, request.PaymentMethodId, defaultCurrency.Id);
                await _unitOfWork.TableRepository<TbOrderPayment>().CreateAsync(payment, customerIdGuid);

                // Step 5: Create shipment drafts (grouped by vendor/warehouse)
                var shipments = await CreateOrderShipmentsFromDetails(order.Id, orderDetails, pricingWithOffers, customerIdGuid);
                if (shipments.Count > 0)
                {
                    await _unitOfWork.TableRepository<TbOrderShipment>().AddRangeAsync(shipments, customerIdGuid);
                }

                // Step 6: Clear cart (after transaction commit to avoid partial clears on rollback)
                await transaction.CommitAsync();

                /// check if payment on delivery
                await _cartService.ClearCartAsync(customerId);

                return new OrderCreatedResponseDto
                {
                    OrderId = order.Id,
                    OrderNumber = order.Number,
                    TotalAmount = order.Price,
                    SubTotal = order.Price - order.ShippingAmount - order.TaxAmount,
                    ShippingAmount = order.ShippingAmount,
                    TaxAmount = order.TaxAmount,
                    OrderStatus = order.OrderStatus,
                    PaymentStatus = order.PaymentStatus,
                    Message = "Order created successfully"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to create order for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
    {
        try
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));

            var order = await _unitOfWork.TableRepository<TbOrder>().FindByIdAsync(orderId);
            return order == null ? null : _mapper.MapModel<TbOrder, OrderDto>(order);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in GetOrderByIdAsync for order ID {OrderId}", orderId);
            throw;
        }
    }

	public async Task<List<ResponseOrderItemDetailsDto>> GetListByOrderIdAsync(
	Guid orderId,
	string? userId = null)
	{
		try
		{
			if (orderId == Guid.Empty)
				throw new ArgumentException("Order ID cannot be empty", nameof(orderId));

			var order = await _unitOfWork.TableRepository<TbOrder>().GetAsync(o => o.Id == orderId && o.OrderStatus != OrderProgressStatus.Cancelled);

			if (order == null)
				return new List<ResponseOrderItemDetailsDto>();

			// Security check
			//if (!string.IsNullOrEmpty(userId) && order. != userId)
			//	throw new UnauthorizedAccessException("You don't have permission to view this order");

			var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();

			// Get orders details by ID
			var ordersDetails = await orderDetailsRepo.GetAsync(od => od.OrderId == orderId && !od.IsDeleted);
			
			if (!ordersDetails.Any())
				return new List<ResponseOrderItemDetailsDto>();

			// Get all item IDs
			var itemIds = ordersDetails.Select(od => od.ItemId).Distinct().ToList();

			// Load Items
			var itemRepo = _unitOfWork.TableRepository<TbItem>();
			var itemsPage = await itemRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 100,
				filter: i => itemIds.Contains(i.Id) && !i.IsDeleted
			);
			var items = itemsPage.Items?.ToList() ?? new List<TbItem>();

			
			// Get all vendor IDs
			var vendorIds = ordersDetails.Select(od => od.VendorId).Distinct().ToList();

			// Load Vendors
			var vendorRepo = _unitOfWork.TableRepository<TbVendor>();
			var vendorsPage = await vendorRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 100,
				filter: v => vendorIds.Contains(v.Id) && !v.IsDeleted
			);
			var vendors = vendorsPage.Items?.ToList() ?? new List<TbVendor>();

			// Load latest shipment for this order
			var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
			var shipmentsPage = await shipmentRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 10,
				filter: s => s.OrderId == orderId && !s.IsDeleted,
				orderBy: q => q.OrderByDescending(s => s.CreatedDateUtc)
			);
			var latestShipment = shipmentsPage.Items?.FirstOrDefault();

			// Build DTOs
			var result = new List<ResponseOrderItemDetailsDto>();

			foreach (var orderDetail in ordersDetails)
			{
				var item = items.FirstOrDefault(i => i.Id == orderDetail.ItemId);
				//var itemImage = itemImages.FirstOrDefault(img => img.ItemId == orderDetail.ItemId);
				var vendor = vendors.FirstOrDefault(v => v.Id == orderDetail.VendorId);

				result.Add(new ResponseOrderItemDetailsDto
				{
					OrderDetailId = orderDetail.Id,
					ItemId = orderDetail.ItemId,
					ItemName = item?.TitleEn ?? "",
					ItemImageUrl = item?.ThumbnailImage ?? "",
					//ItemType = item?.?.ToString() ?? "",
					VendorId = orderDetail.VendorId,
					VendorNameAr = vendor?.NameAr ?? "",
					VendorNameEn = vendor?.NameEn ?? "",
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
			_logger.Error(ex, "Error in GetListByOrderIdAsync for order ID {OrderId}", orderId);
			throw;
		}
	}

	public async Task<OrderDto?> GetOrderByNumberAsync(string orderNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentException("Order number cannot be empty", nameof(orderNumber));

            var repo = _unitOfWork.TableRepository<TbOrder>();
            var order = await repo.FindAsync(o => o.Number == orderNumber && !o.IsDeleted);
            return order == null ? null : _mapper.MapModel<TbOrder, OrderDto>(order);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in GetOrderByNumberAsync for order number {OrderNumber}", orderNumber);
            throw;
        }
    }

	//public async Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10)
	//{
	//    try
	//    {
	//        if (string.IsNullOrWhiteSpace(customerId))
	//            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
	//        if (pageNumber < 1) pageNumber = 1;
	//        if (pageSize < 1 || pageSize > 100) pageSize = 10;

	//        var repo = _unitOfWork.TableRepository<TbOrder>();
	//        var paged = await repo.GetPageAsync(
	//            pageNumber,
	//            pageSize,
	//            filter: o => o.UserId == customerId && !o.IsDeleted,
	//            orderBy: q => q.OrderByDescending(o => o.CreatedDateUtc)
	//        );

	//        return paged.Items.Select(o => new OrderListItemDto
	//        {
	//            Id = o.Id,
	//            OrderNumber = o.Number,
	//            Total = o.Price,
	//            OrderStatus = o.OrderStatus.ToString(),
	//            PaymentStatus = o.PaymentStatus.ToString(),
	//            CreatedDate = o.CreatedDateUtc
	//        }).ToList();
	//    }
	//    catch (Exception ex)
	//    {
	//        _logger.Error(ex, "Error in GetCustomerOrdersAsync for customer {CustomerId}", customerId);
	//        throw;
	//    }
	//}

	//   public async Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10)
	//{
	//	try
	//	{
	//		if (string.IsNullOrWhiteSpace(customerId))
	//			throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
	//		if (pageNumber < 1) pageNumber = 1;
	//		if (pageSize < 1 || pageSize > 100) pageSize = 10;

	//		var repo = _unitOfWork.TableRepository<TbOrder>();

	//		var paged = await repo.GetPageAsync(
	//			pageNumber,
	//			pageSize,
	//			filter: o => o.UserId == customerId && !o.IsDeleted,
	//			orderBy: q => q.OrderByDescending(o => o.CreatedDateUtc)

	//		);

	//		var result = new List<OrderListItemDto>();

	//		foreach (var order in paged.Items)
	//		{
	//			foreach (var orderItem in order. ?? Enumerable.Empty<TbOrderItem>())
	//			{
	//				var dto = new OrderListItemDto
	//				{
	//					Id = order.Id,
	//					OrderNumber = order.Number,


	//					VindorNameAr = orderItem.Item?.Vendor?.NameAr,
	//					VindorNameEn = orderItem.Item?.Vendor?.NameEn,

	//					ItemImageUrl = orderItem.Item?.ItemImages?.FirstOrDefault()?.ImageUrl,
	//					ItemName = orderItem.Item?.Name,
	//					ItemType = orderItem.Item?.ItemType.ToString(), 
	//					QuantityItem = orderItem.Quantity,
	//					Price = orderItem.UnitPrice, 


	//					Total = order.Price, 
	//					OrderStatus = order.OrderStatus.ToString(),
	//					PaymentStatus = order.PaymentStatus.ToString(),
	//					ShipmentStatus = orderItem.ShipmentStatus, 
	//					CreatedDate = order.CreatedDateUtc
	//				};

	//				result.Add(dto);
	//			}
	//		}

	//		return result;
	//	}
	//	catch (Exception ex)
	//	{
	//		_logger.Error(ex, "Error in GetCustomerOrdersAsync for customer {CustomerId}", customerId);
	//		throw;
	//	}
	//}

	public async Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(customerId))
				throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
			if (pageNumber < 1) pageNumber = 1;
			if (pageSize < 1 || pageSize > 100) pageSize = 10;

			var orderRepo = _unitOfWork.TableRepository<TbOrder>();

			
			var pagedOrders = await orderRepo.GetPageAsync(
				pageNumber,
				pageSize,
				filter: o => o.UserId == customerId && !o.IsDeleted && o.OrderStatus != OrderProgressStatus.Cancelled,
				orderBy: q => q.OrderByDescending(o => o.CreatedDateUtc)
			);

			if (pagedOrders.Items == null || !pagedOrders.Items.Any())
				return new List<OrderListItemDto>();

			var orderIds = pagedOrders.Items.Select(o => o.Id).ToList();

			
			var orderDetailRepo = _unitOfWork.TableRepository<TbOrderDetail>();
			var orderDetailsPage = await orderDetailRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000, 
				filter: od => orderIds.Contains(od.OrderId) && !od.IsDeleted
			);
			var orderDetails = orderDetailsPage.Items;

			
			var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
			var shipmentsPage = await shipmentRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: s => orderIds.Contains(s.OrderId) && !s.IsDeleted
			);
			var shipments = shipmentsPage.Items;

			var result = new List<OrderListItemDto>();

			foreach (var order in pagedOrders.Items)
			{
				var orderShipments = shipments?.Where(s => s.OrderId == order.Id);
				var latestOrderShipment = orderShipments?
					.OrderByDescending(s => s.CreatedDateUtc)
					.FirstOrDefault();

				var orderDetailsForOrder = orderDetails?.Where(od => od.OrderId == order.Id);

				if (orderDetailsForOrder == null || !orderDetailsForOrder.Any())
					continue;

				foreach (var orderDetail in orderDetailsForOrder)
				{
					var itemShipmentStatus = latestOrderShipment?.ShipmentStatus ?? ShipmentStatus.Pending;

					var dto = new OrderListItemDto
					{
						Id = order.Id,
						OrderNumber = order.Number,
						//VindorNameAr = orderDetail.Vendor?.NameAr,
						//VindorNameEn = orderDetail.Vendor?.NameEn,
						//ItemImageUrl = orderDetail.Item?.ItemImages?.FirstOrDefault(),
                        //ItemName = orderDetail.Item?.ItemCombinations,
                        //ItemType = orderDetail.Item?.ItemType?.ToString(),
                        QuantityItem = orderDetail.Quantity,
						Price = orderDetail.UnitPrice,
						Total = order.Price,
						OrderStatus = order.OrderStatus.ToString(),
						PaymentStatus = order.PaymentStatus.ToString(),
						ShipmentStatus = itemShipmentStatus,
						CreatedDate = order.CreatedDateUtc
					};

					result.Add(dto);
				}
			}

			return result;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error in GetCustomerOrdersAsync for customer {CustomerId}", customerId);
			throw;
		}
	}
	public async Task<OrderDto?> GetOrderWithShipmentsAsync(Guid orderId)
    {
        return await GetOrderByIdAsync(orderId);
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, string reason, string? adminNotes = null)
    {
        try
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Cancellation reason cannot be empty", nameof(reason));

            var order = await _unitOfWork.TableRepository<TbOrder>().FindByIdAsync(orderId);
            if (order == null || order.IsDeleted)
                return false;

            if (order.OrderStatus == OrderProgressStatus.Shipped ||
                order.OrderStatus == OrderProgressStatus.Delivered ||
                order.OrderStatus == OrderProgressStatus.Cancelled)
            {
                return false;
            }

            order.OrderStatus = OrderProgressStatus.Cancelled;
            order.UpdatedDateUtc = DateTime.UtcNow;

            var updaterId = order.CreatedBy != Guid.Empty ? order.CreatedBy : Guid.Empty;
            var result = await _unitOfWork.TableRepository<TbOrder>().UpdateAsync(order, updaterId);
            return result.Success;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in CancelOrderAsync for order ID {OrderId} with reason: {Reason}", orderId, reason);
            throw;
        }
    }

    public async Task<OrderCompletionStatusDto> GetOrderCompletionStatusAsync(Guid orderId)
    {
        try
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));

            var order = await _unitOfWork.TableRepository<TbOrder>().FindByIdAsync(orderId);
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
            _logger.Error(ex, "Error in GetOrderCompletionStatusAsync for order ID {OrderId}", orderId);
            throw;
        }
    }

    public async Task<bool> ValidateOrderAsync(Guid orderId)
    {
        try
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));

            var order = await _unitOfWork.TableRepository<TbOrder>().FindByIdAsync(orderId);
            if (order == null)
                return false;

            bool isValid = !string.IsNullOrWhiteSpace(order.Number) &&
                          !string.IsNullOrWhiteSpace(order.UserId) &&
                          order.Price > 0 &&
                          order.CreatedDateUtc <= DateTime.UtcNow;

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in ValidateOrderAsync for order ID {OrderId}", orderId);
            throw;
        }
    }

    // ==================== PRIVATE HELPERS ====================

    /// <summary>
    /// Reserves stock for cart items with batch loading to avoid N+1 queries.
    /// Returns a mapping of pricing ID to offer ID for use in shipment creation.
    /// </summary>
    private async Task<Dictionary<Guid, Guid>> ReserveStockForCartItems(string customerId, List<CartItemDto> cartItems)
    {
        var pricingToOfferMap = new Dictionary<Guid, Guid>();
        var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();

        // Batch load all pricings upfront to avoid N+1 queries
        var pricingIds = cartItems.Select(i => i.OfferCombinationPricingId).ToList();
        var pricings = await pricingRepo.GetAsync(
            p => pricingIds.Contains(p.Id) && !p.IsDeleted);

        var pricingDict = pricings.ToDictionary(p => p.Id);
        var customerIdGuid = Guid.Parse(customerId);

        foreach (var item in cartItems)
        {
            var offerCombinationPricingId = item.OfferCombinationPricingId;

            if (!pricingDict.TryGetValue(offerCombinationPricingId, out var pricing))
            {
                throw new InvalidOperationException(
                    $"Pricing record {offerCombinationPricingId} not found for item {item.ItemId}");
            }

            if (pricing.AvailableQuantity < item.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for {item.ItemName}. " +
                    $"Available: {pricing.AvailableQuantity}, Requested: {item.Quantity}");
            }

            // Reserve stock
            pricing.ReservedQuantity += item.Quantity;
            pricing.AvailableQuantity -= item.Quantity;
            pricing.LastStockUpdate = DateTime.UtcNow;

            await pricingRepo.UpdateAsync(pricing, customerIdGuid);

            // Map pricing ID to offer ID for later use
            pricingToOfferMap[offerCombinationPricingId] = pricing.OfferId;
        }

        return pricingToOfferMap;
    }

    private TbOrder CreateOrderHeader(string customerId, CreateOrderFromCartRequest request, CartSummaryDto cartSummary)
    {
        return new TbOrder
        {
            Id = Guid.NewGuid(),
            Number = GenerateOrderNumber(),
            Price = cartSummary.TotalEstimate,
            DeliveryAddressId = request.DeliveryAddressId,
            PaymentStatus = PaymentStatus.Pending,
            OrderStatus = OrderProgressStatus.Pending,
            UserId = customerId,
            ShippingAmount = cartSummary.ShippingEstimate,
            TaxAmount = cartSummary.TaxEstimate,
            IsDeleted = false,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = Guid.Parse(customerId)
        };
    }

    /// <summary>
    /// Creates order details from cart items using pre-loaded pricing and offer data.
    /// This avoids N+1 queries by reusing data from the reservation step.
    /// </summary>
    private async Task<List<TbOrderDetail>> CreateOrderDetails(
        Guid orderId,
        List<CartItemDto> cartItems,
        Guid userId,
        Dictionary<Guid, Guid> pricingToOfferMap)
    {
        var details = new List<TbOrderDetail>();
        var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();
        var offerRepo = _unitOfWork.TableRepository<TbOffer>();

        // Batch load all pricings to get vendor IDs
        var pricingIds = cartItems.Select(i => i.OfferCombinationPricingId).ToList();
        var pricings = await pricingRepo.GetAsync(
            p => pricingIds.Contains(p.Id) && !p.IsDeleted);

        var pricingDict = pricings.ToDictionary(p => p.Id);

        // Batch load all offers to avoid N+1 queries
        var offerIds = pricingToOfferMap.Values.Distinct().ToList();
        var offers = await offerRepo.GetAsync(
            o => offerIds.Contains(o.Id) && !o.IsDeleted);

        var offerDict = offers.ToDictionary(o => o.Id);

        foreach (var item in cartItems)
        {
            if (!pricingDict.TryGetValue(item.OfferCombinationPricingId, out var pricing))
            {
                throw new InvalidOperationException($"Pricing not found for cart item {item.ItemId}");
            }

            if (!pricingToOfferMap.TryGetValue(item.OfferCombinationPricingId, out var offerId))
            {
                throw new InvalidOperationException($"Offer ID not found for pricing {item.OfferCombinationPricingId}");
            }

            if (!offerDict.TryGetValue(offerId, out var offer))
            {
                throw new InvalidOperationException($"Offer not found for pricing {item.OfferCombinationPricingId}");
            }

            var vendorId = offer.VendorId;

            details.Add(new TbOrderDetail
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ItemId = item.ItemId,
                OfferCombinationPricingId = item.OfferCombinationPricingId,
                VendorId = vendorId,
                WarehouseId = Guid.Empty,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.Quantity * item.UnitPrice,
                DiscountAmount = 0,
                TaxAmount = 0,
                IsDeleted = false,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = userId
            });
        }

        return details;
    }

    private TbOrderPayment CreateOrderPayment(Guid orderId, decimal amount, Guid paymentMethodId, Guid currencyId)
    {
        return new TbOrderPayment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            PaymentMethodId = paymentMethodId,
            CurrencyId = currencyId,
            PaymentStatus = PaymentStatus.Pending,
            Amount = amount,
            IsDeleted = false,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = Guid.Empty
        };
    }

    /// <summary>
    /// Creates shipment drafts grouped by vendor and warehouse from order details.
    /// Each unique vendor/warehouse combination gets its own shipment.
    /// Calculates estimated delivery dates from offer handling times and shipping details.
    /// Properly handles multiple offers in a single order.
    /// 
    /// Warehouse Assignment Logic:
    /// - If FulfillmentType is Marketplace/FBS: Use platform default warehouse
    /// - If FulfillmentType is Seller/FBM: Use vendor's default warehouse
    /// </summary>
    private async Task<List<TbOrderShipment>> CreateOrderShipmentsFromDetails(
Guid orderId,
List<TbOrderDetail> orderDetails,
Dictionary<Guid, Guid> pricingToOfferMap,
Guid userId)
    {
        var shipments = new List<TbOrderShipment>();
        var offerRepo = _unitOfWork.TableRepository<TbOffer>();
        var shippingDetailRepo = _unitOfWork.TableRepository<TbShippingDetail>();
        var warehouseRepo = _unitOfWork.TableRepository<TbWarehouse>();
        var vendorRepo = _unitOfWork.TableRepository<TbVendor>();

        // Batch load all offers
        var offerIds = pricingToOfferMap.Values.Distinct().ToList();
        var offers = await offerRepo.GetAsync(
            o => offerIds.Contains(o.Id) && !o.IsDeleted);
        var offerDict = offers.ToDictionary(o => o.Id);

        // Batch load warehouses
        var vendorIds = orderDetails.Select(od => od.VendorId).Distinct().ToList();
        var warehouses = await warehouseRepo.GetAsync(
            w => !w.IsDeleted &&
                 (w.IsDefaultPlatformWarehouse ||
                  (w.VendorId.HasValue && vendorIds.Contains(w.VendorId.Value))));

        // Get platform default warehouse
        var platformDefaultWarehouse = warehouses.FirstOrDefault(w => w.IsDefaultPlatformWarehouse);
        if (platformDefaultWarehouse == null)
        {
            throw new InvalidOperationException("Platform default warehouse not found.");
        }

        // Batch load shipping details
        var shippingDetails = await shippingDetailRepo.GetAsync(
            sd => offerIds.Contains(sd.OfferId) && !sd.IsDeleted);
        var shippingDetailDict = shippingDetails
            .GroupBy(sd => sd.OfferId)
            .ToDictionary(g => g.Key, g => g.FirstOrDefault());

        // ✅ Group by VendorId AND FulfillmentType
        var shipmentGroups = orderDetails
            .Select(od => new
            {
                OrderDetail = od,
                OfferId = pricingToOfferMap[od.OfferCombinationPricingId],
                Offer = offerDict[pricingToOfferMap[od.OfferCombinationPricingId]]
            })
            .GroupBy(x => new
            {
                x.OrderDetail.VendorId,
                x.Offer.FulfillmentType  // ✅ Group by FulfillmentType
            })
            .ToList();

        foreach (var group in shipmentGroups)
        {
            var shipmentSubTotal = group.Sum(x => x.OrderDetail.SubTotal);
            var shipmentId = Guid.NewGuid();
            var fulfillmentType = group.Key.FulfillmentType;

            // ✅ Warehouse Assignment Logic
            Guid warehouseId;

            if (fulfillmentType == FulfillmentType.Marketplace)
            {
                // ✅ FBM = Fulfilled By Marketplace → Use Platform Warehouse
                warehouseId = platformDefaultWarehouse.Id;
            }
            else // FBS = Fulfilled By Seller
            {
                // ✅ FBS = Fulfilled By Seller → Use Vendor Warehouse
                var vendorWarehouse = warehouses.FirstOrDefault(
                    w => w.VendorId == group.Key.VendorId &&
                         !w.IsDeleted);

                if (vendorWarehouse == null)
                {
                    _logger.Warning(
                        "No default warehouse found for vendor {VendorId}. Falling back to platform warehouse.",
                        group.Key.VendorId);

                    // Fallback to platform warehouse if vendor has no warehouse
                    warehouseId = platformDefaultWarehouse.Id;
                }
                else
                {
                    warehouseId = vendorWarehouse.Id;
                }
            }

            // Calculate estimated delivery date
            DateTime? estimatedDeliveryDate = null;
            int maxEstimatedDays = 0;

            // Get all offer IDs in this shipment group
            var groupOfferIds = group.Select(x => x.OfferId).Distinct().ToList();

            foreach (var offerId in groupOfferIds)
            {
                if (offerDict.TryGetValue(offerId, out var offer))
                {
                    int totalEstimatedDays = offer.EstimatedDeliveryDays;

                    if (shippingDetailDict.TryGetValue(offerId, out var shippingDetail) &&
                        shippingDetail != null)
                    {
                        totalEstimatedDays += shippingDetail.MaximumEstimatedDays;
                    }

                    maxEstimatedDays = Math.Max(maxEstimatedDays, totalEstimatedDays);
                }
            }

            if (maxEstimatedDays > 0)
            {
                estimatedDeliveryDate = DateTime.UtcNow.AddDays(maxEstimatedDays);
            }

            var shipment = new TbOrderShipment
            {
                Id = shipmentId,
                ShipmentNumber = GenerateShipmentNumber(shipmentId),
                OrderId = orderId,
                VendorId = group.Key.VendorId,
                WarehouseId = warehouseId,  // ✅ Correctly assigned
                FulfillmentType = fulfillmentType,
                ShippingCompanyId = null,  // Assigned later
                ShipmentStatus = ShipmentStatus.Pending,
                TrackingNumber = null,
                EstimatedDeliveryDate = estimatedDeliveryDate,
                ActualDeliveryDate = null,
                ShippingCost = 0,
                SubTotal = shipmentSubTotal,
                TotalAmount = shipmentSubTotal,
                Notes = null,
                IsDeleted = false,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = userId
            };

            shipments.Add(shipment);
        }

        return shipments;
    }

    /// <summary>
    /// Generates a unique shipment number.
    /// </summary>
    private string GenerateShipmentNumber(Guid shipmentId)
    {
        var timestamp = DateTime.UtcNow.ToString("yyMMddHH");
        var guidPart = shipmentId.ToString("N").Substring(0, 8).ToUpper();
        return $"SHP-{timestamp}-{guidPart}";
    }

    private string GenerateOrderNumber()
    {
        var random = new Random();
        var part1 = random.Next(100, 999).ToString();
        var part2 = Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        var part3 = Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        return $"{part1}-{part2}-{part3}";
    }
	public async Task<ResponseOrderDetailsDto?> GetOrderDetailsByIdAsync(
	Guid orderDetailsId,
	string userId,
	CancellationToken cancellationToken = default)
	{
		try
		{
			var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();

			// Get order detail by ID
			var orderDetail = await orderDetailsRepo.FindByIdAsync(orderDetailsId);

			if (orderDetail == null)
				return null;

			// Security check - verify user owns this order
			if (!string.IsNullOrEmpty(userId))
			{
				var orderRepo = _unitOfWork.TableRepository<TbOrder>();
				var order = await orderRepo.FindByIdAsync(orderDetail.OrderId);

				if (order == null)
					return null;

				if (order.UserId != userId)
					throw new UnauthorizedAccessException("You don't have permission to view this order detail");
			}
			var ItemRepo = _unitOfWork.TableRepository<TbItem>();
			var Item = await ItemRepo.FindByIdAsync(orderDetail.ItemId);

			// Load Item
			var CombinationPriceRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();
			var CombinationPrice = await CombinationPriceRepo.FindByIdAsync(orderDetail.OfferCombinationPricingId);

			var CombinationRepo = _unitOfWork.TableRepository<TbItemCombination>();
			var Combination = await CombinationRepo.FindByIdAsync(CombinationPrice.ItemCombinationId);

			
			// Load Item Image
			//TbItemImage? itemImage = null;
			//if (Combination != null)
			//{
			//	var itemImageRepo = _unitOfWork.TableRepository<TbItemImage>();
			//	var itemImagesPage = await itemImageRepo.GetPageAsync(
			//		pageNumber: 1,
			//		pageSize: 1,
			//		filter: img => img.ItemId == item.Id && !img.IsDeleted
			//	);
			//	itemImage = itemImagesPage.Items?.FirstOrDefault();
			//}

			// Load Vendor
			var vendorRepo = _unitOfWork.TableRepository<TbVendor>();
			var vendor = await vendorRepo.FindByIdAsync(orderDetail.VendorId);

			// Load Shipment for this order
			var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
			//var shipmentsPage = await shipmentRepo.FindByIdAsync(orderDetail.Id)
			//var latestShipment = shipmentsPage.Items?.FirstOrDefault();

			// Build DTO
			var orderDetailsDto = new ResponseOrderDetailsDto
			{
				OrderDetailId = orderDetail.Id,
				ItemId = orderDetail.ItemId,
				ItemName = Item.TitleEn,
				ItemImageUrl = Item.ThumbnailImage ?? "",
				//ItemType = Item.t ?? "",
				VendorId = orderDetail.VendorId,
				VendorNameAr = vendor?.NameAr ?? "",
				VendorNameEn = vendor?.NameEn ?? "",
				Quantity = orderDetail.Quantity,
				UnitPrice = orderDetail.UnitPrice,
				SubTotal = orderDetail.SubTotal,
				DiscountAmount = orderDetail.DiscountAmount,
				TaxAmount = orderDetail.TaxAmount,
				//ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending
				ShipmentStatus =  ShipmentStatus.Pending
			};

			return orderDetailsDto;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(GetOrderDetailsByIdAsync)} for order detail {orderDetailsId}");
			throw;
		}
	}
	//public async Task<ResponseOrderDetailsDto?> GetOrderDetailsByIdAsync(
	//Guid orderDetailsId,
	//string userId,
	//CancellationToken cancellationToken = default)
	//{
	//	try
	//	{
	//		var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();

	//		//  Get order without includeProperties
	//		var orderDetails = await orderDetailsRepo.FindByIdAsync(orderDetailsId
	//		);

	//		if (orderDetails == null)
	//			return null;

	//		////  Load OrderDetails separately
	//		//var orderDetailRepo = _unitOfWork.TableRepository<TbOrderDetail>();
	//		//var orderDetailsPage = await orderDetailRepo.GetPageAsync(
	//		//	pageNumber: 1,
	//		//	pageSize: 100,
	//		//	filter: od => od.OrderId == orderDetailsId && !od.IsDeleted
	//		//);
	//		//var orderDetailsList = orderDetails.Item?.ToList() ?? new List<TbOrderDetail>();

	//		////  Load Item IDs
	//		//var itemIds = orderDetails.Select(od => od.ItemId).ToList();

	//		//  Load Items
	//		var itemRepo = _unitOfWork.TableRepository<TbItem>();

	//		var item = await itemRepo.FindByIdAsync(orderDetails.ItemId
	//		);
	//		var items = item.?.ToList() ?? new List<TbItem>();

	//		//  Load ItemImages
	//		var itemImageRepo = _unitOfWork.TableRepository<TbItemImage>();
	//		var itemImagesPage = await itemImageRepo.GetPageAsync(
	//			pageNumber: 1,
	//			pageSize: 100,
	//			filter: img => itemIds.Contains(img.ItemId) && !img.IsDeleted
	//		);
	//		var itemImages = itemImagesPage.Items?.ToList() ?? new List<TbItemImage>();

	//		//  Load Vendors
	//		var vendorIds = orderDetails.Select(od => od.VendorId).Distinct().ToList();
	//		var vendorRepo = _unitOfWork.TableRepository<TbVendor>();
	//		var vendorsPage = await vendorRepo.GetPageAsync(
	//			pageNumber: 1,
	//			pageSize: 100,
	//			filter: v => vendorIds.Contains(v.Id) && !v.IsDeleted
	//		);
	//		var vendors = vendorsPage.Items?.ToList() ?? new List<TbVendor>();

	//		//  Load Customer Address
	//		ResponseOrderAddressDto? addressDto = null;
	//		if (order.DeliveryAddressId != Guid.Empty)
	//		{
	//			var addressRepo = _unitOfWork.TableRepository<TbCustomerAddress>();
	//			var addressPage = await addressRepo.GetPageAsync(
	//				pageNumber: 1,
	//				pageSize: 1,
	//				filter: a => a.Id == order.DeliveryAddressId && !a.IsDeleted
	//			);
	//			var address = addressPage.Items?.FirstOrDefault();

	//			if (address != null)
	//			{
	//				addressDto = new ResponseOrderAddressDto
	//				{
	//					AddressId = address.Id,
	//					FullAddress = address.Address,
	//					CityAr = address.City.TitleAr,
	//					CityEn = address.City.TitleEn,

	//					PhoneNumber = address.PhoneNumber
	//				};
	//			}
	//		}

	//		//  Load Shipments
	//		var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
	//		var shipmentsPage = await shipmentRepo.GetPageAsync(
	//			pageNumber: 1,
	//			pageSize: 10,
	//			filter: s => s.OrderId == orderDetailsId && !s.IsDeleted
	//		);
	//		var shipments = shipmentsPage.Items?.ToList() ?? new List<TbOrderShipment>();

	//		//  Get latest shipment
	//		var latestShipment = shipments
	//			.OrderByDescending(s => s.CreatedDateUtc)
	//			.FirstOrDefault();

	//		//  Get shipment tracking
	//		var shipmentTracking = latestShipment != null ? new ResponseShipmentTrackingDto
	//		{
	//			ShipmentId = latestShipment.Id,
	//			ShipmentNumber = latestShipment.TrackingNumber ?? "",
	//			CurrentStatus = latestShipment.ShipmentStatus,
	//			StatusHistory = GetShipmentStatusHistory(latestShipment)
	//		} : null;

	//		//  Build Items DTOs
	//		var itemDtos = new List<ResponseOrderItemDetailDto>();
	//		foreach (var od in orderDetails)
	//		{
	//			var item = items.FirstOrDefault(i => i.Id == od.ItemId);
	//			var vendor = vendors.FirstOrDefault(v => v.Id == od.VendorId);
	//			var firstImage = itemImages.FirstOrDefault(img => img.ItemId == od.ItemId);

	//			itemDtos.Add(new ResponseOrderItemDetailDto
	//			{
	//				OrderDetailId = od.Id,
	//				ItemId = od.ItemId,
	//				//ItemName = item?.Name ?? "",
	//				//ItemImageUrl = firstImage?.ImageUrl ?? "",
	//				//ItemType = item?.ItemType?.ToString() ?? "",
	//				VendorId = od.VendorId,
	//				VendorNameAr = vendor?.NameAr ?? "",
	//				VendorNameEn = vendor?.NameEn ?? "",
	//				Quantity = od.Quantity,
	//				UnitPrice = od.UnitPrice,
	//				SubTotal = od.SubTotal,
	//				DiscountAmount = od.DiscountAmount,
	//				TaxAmount = od.TaxAmount,
	//				ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending
	//			});
	//		}


	//		var orderDetailsDto = new ResponseOrderDetailsDto
	//		{
	//			OrderDetailId = order.Id,
	//			ItemId = orderDetails.FirstOrDefault()?.ItemId ?? Guid.Empty,
	//			//ItemName = items.FirstOrDefault()?.Name ?? "",
	//			//ItemImageUrl = itemImages.FirstOrDefault()?.ImageUrl ?? "",
	//			//ItemType = items.FirstOrDefault()?.ItemType?.ToString() ?? "",
	//			VendorId = orderDetails.FirstOrDefault()?.VendorId ?? Guid.Empty,
	//			VendorNameAr = vendors.FirstOrDefault()?.NameAr ?? "",
	//			VendorNameEn = vendors.FirstOrDefault()?.NameEn ?? "",
	//			Quantity = orderDetails.FirstOrDefault()?.Quantity ?? 0,
	//			UnitPrice = orderDetails.FirstOrDefault()?.UnitPrice ?? 0,
	//			SubTotal = orderDetails.FirstOrDefault()?.SubTotal ?? 0,
	//			DiscountAmount = orderDetails.FirstOrDefault()?.DiscountAmount ?? 0,
	//			TaxAmount = orderDetails.FirstOrDefault()?.TaxAmount ?? 0,
	//			ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending
	//		};

	//		return orderDetailsDto;
	//	}
	//	catch (Exception ex)
	//	{
	//		_logger.Error(ex, $"Error in {nameof(GetOrderDetailsByIdAsync)} for order {orderDetailsId}");
	//		throw;
	//	}
	//}

	public async Task<IEnumerable<OrderListItemDto>> GetCustomerOrdersAsync(
	string customerId,
	int pageNumber,
	int pageSize,
	CancellationToken cancellationToken = default)
	{
		try
		{
			var orderRepo = _unitOfWork.TableRepository<TbOrder>();

			// Get customer orders with pagination
			var ordersPage = await orderRepo.GetPageAsync(
				pageNumber: pageNumber,
				pageSize: pageSize,
				filter: o => o.UserId == customerId && !o.IsDeleted,
				orderBy: q => q.OrderByDescending(o => o.CreatedDateUtc)
			);

			var orders = ordersPage.Items?.ToList() ?? new List<TbOrder>();

			if (!orders.Any())
				return new List<OrderListItemDto>();

			// Get all order IDs
			var orderIds = orders.Select(o => o.Id).ToList();

			// Load OrderDetails for all orders
			var orderDetailRepo = _unitOfWork.TableRepository<TbOrderDetail>();
			var orderDetailsPage = await orderDetailRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: od => orderIds.Contains(od.OrderId) && !od.IsDeleted
			);
			var allOrderDetails = orderDetailsPage.Items?.ToList() ?? new List<TbOrderDetail>();

			// Get all item IDs
			var itemIds = allOrderDetails.Select(od => od.ItemId).Distinct().ToList();

			// Load Items
			var itemRepo = _unitOfWork.TableRepository<TbItem>();
			var itemsPage = await itemRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: i => itemIds.Contains(i.Id) && !i.IsDeleted
			);
			var items = itemsPage.Items?.ToList() ?? new List<TbItem>();

			// Load ItemImages
			var itemImageRepo = _unitOfWork.TableRepository<TbItemImage>();
			var itemImagesPage = await itemImageRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: img => itemIds.Contains(img.ItemId) && !img.IsDeleted
			);
			var itemImages = itemImagesPage.Items?.ToList() ?? new List<TbItemImage>();

			// Get all vendor IDs
			var vendorIds = allOrderDetails.Select(od => od.VendorId).Distinct().ToList();

			// Load Vendors
			var vendorRepo = _unitOfWork.TableRepository<TbVendor>();
			var vendorsPage = await vendorRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: v => vendorIds.Contains(v.Id) && !v.IsDeleted
			);
			var vendors = vendorsPage.Items?.ToList() ?? new List<TbVendor>();

			// Load latest shipments for all orders
			var shipmentRepo = _unitOfWork.TableRepository<TbOrderShipment>();
			var shipmentsPage = await shipmentRepo.GetPageAsync(
				pageNumber: 1,
				pageSize: 1000,
				filter: s => orderIds.Contains(s.OrderId) && !s.IsDeleted
			);
			var allShipments = shipmentsPage.Items?.ToList() ?? new List<TbOrderShipment>();

			// Build DTOs - one DTO per OrderDetail
			var orderDtos = new List<OrderListItemDto>();

			foreach (var order in orders)
			{
				// Get order details for this order
				var orderDetails = allOrderDetails.Where(od => od.OrderId == order.Id).ToList();

				// Get latest shipment for this order
				var latestShipment = allShipments
					.Where(s => s.OrderId == order.Id)
					.OrderByDescending(s => s.CreatedDateUtc)
					.FirstOrDefault();

				foreach (var orderDetail in orderDetails)
				{
					// Get item details
					var item = items.FirstOrDefault(i => i.Id == orderDetail.ItemId);
					var itemImage = item != null
						? itemImages.FirstOrDefault(img => img.ItemId == item.Id)
						: null;

					// Get vendor details
					var vendor = vendors.FirstOrDefault(v => v.Id == orderDetail.VendorId);

					orderDtos.Add(new OrderListItemDto
					{
						Id = order.Id,
						//OrderNumber = order.Number,
						//VindorNameAr = vendor?.NameAr ?? "",
						//VindorNameEn = vendor?.NameEn ?? "",
						//ItemImageUrl = itemImage?.ImageUrl ?? "",
						//ItemName = item?.Name ?? "",
						//ItemType = item?.ItemType?.ToString() ?? "",
						QuantityItem = orderDetail.Quantity,
						ShipmentStatus = latestShipment?.ShipmentStatus ?? ShipmentStatus.Pending,
						Price = orderDetail.UnitPrice,
						Total = orderDetail.SubTotal,
						//OrderStatus = GetOrderStatusArabic(order.OrderStatus),
						//PaymentStatus = GetPaymentStatusArabic(order.PaymentStatus),
						CreatedDate = order.CreatedDateUtc
					});
				}
			}

			return orderDtos;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Error in {nameof(GetCustomerOrdersAsync)} for customer {customerId}");
			throw;
		}
	}

	// ==================== SHIPMENT STATUS HELPERS ====================
	private List<ResponseShipmentStatusHistoryDto> GetShipmentStatusHistory(TbOrderShipment shipment)
	{
		var history = new List<ResponseShipmentStatusHistoryDto>
	{
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.Pending,
			
			StatusDate = shipment.CreatedDateUtc,
			IsCompleted = shipment.ShipmentStatus >= ShipmentStatus.Pending
		},
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.Processing,
			
			StatusDate = shipment.ShipmentStatus >= ShipmentStatus.Processing ? shipment.UpdatedDateUtc : null,
			IsCompleted = shipment.ShipmentStatus >= ShipmentStatus.Processing
		},
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.Shipped,
			
			StatusDate = shipment.ShipmentStatus >= ShipmentStatus.Shipped ? shipment.UpdatedDateUtc : null,
			IsCompleted = shipment.ShipmentStatus >= ShipmentStatus.Shipped
		},
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.InTransit,
			StatusDate = shipment.ShipmentStatus >= ShipmentStatus.InTransit ? shipment.UpdatedDateUtc : null,
			IsCompleted = shipment.ShipmentStatus >= ShipmentStatus.InTransit
		},
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.OutForDelivery,
			
			StatusDate = shipment.ShipmentStatus >= ShipmentStatus.OutForDelivery ? shipment.UpdatedDateUtc : null,
			IsCompleted = shipment.ShipmentStatus >= ShipmentStatus.OutForDelivery
		},
		new ResponseShipmentStatusHistoryDto
		{
			Status = ShipmentStatus.Delivered,
			StatusDate = shipment.ShipmentStatus == ShipmentStatus.Delivered ? shipment.UpdatedDateUtc : null,
			IsCompleted = shipment.ShipmentStatus == ShipmentStatus.Delivered
		}
	};

		if (shipment.ShipmentStatus == ShipmentStatus.Cancelled)
		{
			history.Add(new ResponseShipmentStatusHistoryDto
			{
				Status = ShipmentStatus.Cancelled,
				StatusDate = shipment.UpdatedDateUtc,
				IsCompleted = true
			});
		}
		else if (shipment.ShipmentStatus == ShipmentStatus.Returned)
		{
			history.Add(new ResponseShipmentStatusHistoryDto
			{
				Status = ShipmentStatus.Returned,
				StatusDate = shipment.UpdatedDateUtc,
				IsCompleted = true
			});
		}

		return history;
	}

	
}