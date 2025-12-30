using BL.Contracts.IMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.OrderProcessing;
using Common.Enumerations.Fulfillment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Shipping;
using Domains.Entities.Warehouse;
using Serilog;
using Shared.DTOs.Order.Cart;
using Shared.DTOs.Order.OrderProcessing;

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

    public async Task<List<OrderListItemDto>> GetCustomerOrdersAsync(string customerId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var repo = _unitOfWork.TableRepository<TbOrder>();
            var paged = await repo.GetPageAsync(
                pageNumber,
                pageSize,
                filter: o => o.UserId == customerId && !o.IsDeleted,
                orderBy: q => q.OrderByDescending(o => o.CreatedDateUtc)
            );

            return paged.Items.Select(o => new OrderListItemDto
            {
                Id = o.Id,
                Number = o.Number,
                Total = o.Price,
                OrderStatus = o.OrderStatus.ToString(),
                PaymentStatus = o.PaymentStatus.ToString(),
                CreatedDate = o.CreatedDateUtc
            }).ToList();
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
                    int totalEstimatedDays = offer.HandlingTimeInDays;

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
}