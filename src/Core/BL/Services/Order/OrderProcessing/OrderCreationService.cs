using AutoMapper;
using BL.Contracts.GeneralService;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order;
using Serilog;
using Shared.DTOs.Order.Checkout;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.OrderProcessing;

/// <summary>
/// Service for creating orders
/// REFACTORED: Uses IUnitOfWork for transactions, optimized with CheckoutItemDto, no Info logging
/// </summary>
public class OrderCreationService : IOrderCreationService
{
    private readonly ICheckoutService _checkoutService;
    private readonly IOrderPaymentProcessor _paymentProcessor;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICartService _cartService;
    private readonly IOrderRepository _orderRepository;
    private readonly ITableRepository<TbOrderDetail> _orderDetailRepository;
    private readonly ITableRepository<TbCouponCode> _couponRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OrderCreationService(
        ICheckoutService checkoutService,
        IOrderPaymentProcessor paymentProcessor,
        ICurrentUserService currentUserService,
        ICartService cartService,
        IOrderRepository orderRepository,
        ITableRepository<TbOrderDetail> orderDetailRepository,
        ITableRepository<TbCouponCode> couponRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger)
    {
        _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Create order from cart with payment processing
    /// Uses IUnitOfWork transaction to ensure data integrity
    /// </summary>
    public async Task<CreateOrderResult> CreateOrderAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Validate: Address is mandatory
            if (!request.DeliveryAddressId.HasValue)
            {
                return CreateOrderResult.CreateFailure("Delivery address is required");
            }

            // 2. Validate: Payment method is mandatory
            if (request.PaymentMethod == PaymentMethodType.CashOnDelivery)
            {
                // COD doesn't require payment method setup
            }
            else if (request.PaymentMethod != PaymentMethodType.Wallet && 
                     request.PaymentMethod != PaymentMethodType.Card && 
                     request.PaymentMethod != PaymentMethodType.WalletAndCard)
            {
                return CreateOrderResult.CreateFailure("Invalid payment method");
            }

            var userId = _currentUserService.GetCurrentUserId();

            // 3. Validate checkout (uses CheckoutService internally)
            await _checkoutService.ValidateCheckoutAsync(userId, request.DeliveryAddressId.Value);

            // 4. Prepare checkout summary (calculates all prices AND gets checkout items)
            var checkoutSummary = await _checkoutService.PrepareCheckoutAsync(
                userId,
                new PrepareCheckoutRequest
                {
                    DeliveryAddressId = request.DeliveryAddressId.Value,
                    CouponCode = request.CouponCode
                });

            // ✅ START TRANSACTION - All database operations must succeed together
            await _unitOfWork.BeginTransactionAsync(); // ✅ No CancellationToken parameter

            try
            {
                // 5. Create order entity
                var order = await CreateOrderEntityAsync(
                    userId,
                    request,
                    checkoutSummary,
                    cancellationToken);

                // 6. Create order details (uses checkout items from checkoutSummary - no duplicate query!)
                await CreateOrderDetailsAsync(
                    order.Id,
                    checkoutSummary.Items, // ✅ CheckoutItemDto from checkoutSummary
                    cancellationToken);

                // 7. Update coupon usage if applied
                if (checkoutSummary.CouponId.HasValue)
                {
                    await UpdateCouponUsageAsync(checkoutSummary.CouponId.Value, cancellationToken);
                }

                // ✅ COMMIT TRANSACTION - All or nothing!
                await _unitOfWork.CommitAsync(); // ✅ No CancellationToken parameter

                // 8. Process payment AFTER transaction (payment can fail without rolling back order)
                var paymentResult = await ProcessOrderPaymentAsync(
                    order,
                    request,
                    checkoutSummary.PriceBreakdown.GrandTotal,
                    cancellationToken);

                if (!paymentResult.Success)
                {
                    // Update order status to payment failed
                    order.OrderStatus = OrderProgressStatus.PaymentFailed;
                    order.PaymentStatus = PaymentStatus.Failed;
                    await _orderRepository.UpdateAsync(order, cancellationToken);

                    return CreateOrderResult.CreateFailure(
                        paymentResult.Message,
                        order.Id,
                        order.Number);
                }

                // 9. Clear cart after successful order creation
                await _cartService.ClearCartAsync(userId);

                // 10. Return success result
                return CreateOrderResult.CreateSuccess(
                    order.Id,
                    order.Number,
                    checkoutSummary.PriceBreakdown.GrandTotal,
                    paymentResult);
            }
            catch
            {
                // ✅ ROLLBACK TRANSACTION on any error
                await _unitOfWork.RollbackAsync(); // ✅ No CancellationToken parameter
                throw;
            }
        }
        catch (Exception ex)
        {
            var userId = _currentUserService.GetCurrentUserId();
            _logger.Error(ex, "Failed to create order for userId {UserId}", userId);
            return CreateOrderResult.CreateFailure("Failed to create order. Please try again.");
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Create order entity with all calculated values
    /// </summary>
    private async Task<TbOrder> CreateOrderEntityAsync(
        string customerId,
        CreateOrderRequest request,
        CheckoutSummaryDto checkoutSummary,
        CancellationToken cancellationToken)
    {
        var orderNumber = await GenerateOrderNumberAsync(cancellationToken);

        var order = new TbOrder
        {
            Id = Guid.NewGuid(),
            Number = orderNumber,
            UserId = customerId,
            DeliveryAddressId = request.DeliveryAddressId.Value,

            // Price breakdown
            SubTotal = checkoutSummary.PriceBreakdown.Subtotal,
            DiscountAmount = checkoutSummary.PriceBreakdown.DiscountAmount,
            ShippingAmount = checkoutSummary.PriceBreakdown.ShippingCost ?? 0m,
            TaxPercentage = checkoutSummary.PriceBreakdown.TaxPercentage,
            TaxAmount = checkoutSummary.PriceBreakdown.TaxAmount,
            Price = checkoutSummary.PriceBreakdown.GrandTotal,

            // Coupon
            CouponId = checkoutSummary.CouponId,

            // Notes
            Notes = request.Notes,

            // Status
            PaymentStatus = PaymentStatus.Pending,
            OrderStatus = OrderProgressStatus.Pending,

            // Audit
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = Guid.Empty,
            IsDeleted = false
        };

        await _orderRepository.CreateAsync(order, cancellationToken);

        return order;
    }

    /// <summary>
    /// Create order detail records from checkout items
    /// ✅ OPTIMIZED: Reuses checkout items from checkout (no duplicate database query)
    /// ✅ FIXED: Uses CheckoutItemDto instead of CartItemDto
    /// </summary>
    private async Task CreateOrderDetailsAsync(
        Guid orderId,
        List<CheckoutItemDto> checkoutItems, // ✅ CheckoutItemDto from checkoutSummary
        CancellationToken cancellationToken)
    {
        if (checkoutItems == null || !checkoutItems.Any())
        {
            throw new InvalidOperationException("Cannot create order without checkout items");
        }

        var orderDetails = checkoutItems.Select(item => new TbOrderDetail
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ItemId = item.ItemId,
            OfferCombinationPricingId = item.OfferCombinationPricingId,
            VendorId = item.VendorId,
            WarehouseId = item.WarehouseId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            SubTotal = item.SubTotal,
            DiscountAmount = item.DiscountAmount,
            TaxAmount = item.TaxAmount,
            CreatedDateUtc = DateTime.UtcNow,
            CreatedBy = Guid.Empty,
            IsDeleted = false
        }).ToList();

        // Create order details
        foreach (var detail in orderDetails)
        {
            await _orderDetailRepository.CreateAsync(detail, Guid.Empty, cancellationToken);
        }
    }

    /// <summary>
    /// Update coupon usage count
    /// </summary>
    private async Task UpdateCouponUsageAsync(
        Guid couponId,
        CancellationToken cancellationToken)
    {
        try
        {
            var coupon = await _couponRepository.FindByIdAsync(couponId, cancellationToken);

            if (coupon != null)
            {
                coupon.UsageCount++;
                coupon.UpdatedDateUtc = DateTime.UtcNow;
                await _couponRepository.UpdateAsync(coupon, Guid.Empty, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update coupon usage for {CouponId}", couponId);
            throw; // ✅ Throw to trigger transaction rollback
        }
    }

    /// <summary>
    /// Process payment based on payment method type
    /// Handles: CashOnDelivery, Wallet, Card, WalletAndCard
    /// </summary>
    private async Task<PaymentResult> ProcessOrderPaymentAsync(
        TbOrder order,
        CreateOrderRequest request,
        decimal totalAmount,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _currentUserService.GetCurrentUserId();

            return request.PaymentMethod switch
            {
                // 1. Cash on Delivery - No immediate payment
                PaymentMethodType.CashOnDelivery => await ProcessCashOnDeliveryAsync(
                    order,
                    totalAmount,
                    cancellationToken),

                // 2. Wallet - Deduct from wallet balance
                PaymentMethodType.Wallet => await ProcessWalletPaymentAsync(
                    order,
                    totalAmount,
                    userId,
                    cancellationToken),

                // 3. Card - Payment gateway
                PaymentMethodType.Card => await ProcessCardPaymentAsync(
                    order,
                    totalAmount,
                    userId,
                    cancellationToken),

                // 4. Mixed - Wallet first, then card
                PaymentMethodType.WalletAndCard => await ProcessMixedPaymentAsync(
                    order,
                    totalAmount,
                    userId,
                    cancellationToken),

                _ => PaymentResult.CreateFailure("Invalid payment method")
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process payment for order {OrderId}", order.Id);
            throw;
        }
    }

    /// <summary>
    /// Process Cash on Delivery payment
    /// </summary>
    private async Task<PaymentResult> ProcessCashOnDeliveryAsync(
        TbOrder order,
        decimal amount,
        CancellationToken cancellationToken)
    {
        return await _paymentProcessor.ProcessCashOnDeliveryAsync(
            order.Id,
            amount,
            cancellationToken);
    }

    /// <summary>
    /// Process Wallet payment
    /// </summary>
    private async Task<PaymentResult> ProcessWalletPaymentAsync(
        TbOrder order,
        decimal amount,
        string customerId,
        CancellationToken cancellationToken)
    {
        return await _paymentProcessor.ProcessWalletPaymentAsync(
            order.Id,
            amount,
            customerId,
            cancellationToken);
    }

    /// <summary>
    /// Process Card payment via gateway
    /// </summary>
    private async Task<PaymentResult> ProcessCardPaymentAsync(
        TbOrder order,
        decimal amount,
        string customerId,
        CancellationToken cancellationToken)
    {
        return await _paymentProcessor.ProcessCardPaymentAsync(
            order.Id,
            amount,
            customerId,
            cancellationToken);
    }

    /// <summary>
    /// Process Mixed payment (Wallet + Card)
    /// </summary>
    private async Task<PaymentResult> ProcessMixedPaymentAsync(
        TbOrder order,
        decimal amount,
        string customerId,
        CancellationToken cancellationToken)
    {
        return await _paymentProcessor.ProcessMixedPaymentAsync(
            order.Id,
            amount,
            customerId,
            cancellationToken);
    }

    /// <summary>
    /// Generate unique order number
    /// </summary>
    private async Task<string> GenerateOrderNumberAsync(
        CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var datePrefix = date.ToString("yyyyMMdd");
        var todayCount = await _orderRepository.CountTodayOrdersAsync(date.Date, cancellationToken);

        return $"ORD-{datePrefix}-{(todayCount + 1):D6}";
    }

    #endregion
}