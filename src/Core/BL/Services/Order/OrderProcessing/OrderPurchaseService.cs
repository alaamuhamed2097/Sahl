using AutoMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.CouponCode;
using Domains.Entities.Order;
using Serilog;
using Shared.DTOs.Order.Checkout;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.OrderProcessing;

public class OrderPurchaseService : IOrderPurchaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;
    private readonly ICheckoutService _checkoutService;
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OrderPurchaseService(
        IUnitOfWork unitOfWork,
        ICartService cartService,
        ICheckoutService checkoutService,
        IPaymentService paymentService,
        IMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaymentTransactionResult> PurchaseAsync(
        OrderPurchaseDto purchaseDto,
        string customerId)
    {
        try
        {
            _logger.Information(
                "Starting purchase process for customer {CustomerId}",
                customerId
            );

            await _checkoutService.ValidateCheckoutAsync(
                customerId,
                purchaseDto.DeliveryAddressId
            );

            var checkoutRequest = new PrepareCheckoutRequest
            {
                DeliveryAddressId = purchaseDto.DeliveryAddressId,
                CouponCode = purchaseDto.CouponCode
            };

            var checkoutSummary = await _checkoutService.PrepareCheckoutAsync(
                customerId,
                checkoutRequest
            );

            var order = await CreateOrderFromCheckoutAsync(
                customerId,
                purchaseDto,
                checkoutSummary
            );

            _logger.Information(
                "Order {OrderId} created with total {Total}",
                order.Id,
                checkoutSummary.PriceBreakdown.GrandTotal
            );

            // FIXED: Use correct IPaymentService signature
            var paymentRequest = new PaymentProcessRequest
            {
                OrderId = order.Id,
                Amount = checkoutSummary.PriceBreakdown.GrandTotal,
                PaymentMethodId = purchaseDto.PaymentMethodId
            };

            var paymentResult = await _paymentService.ProcessPaymentAsync(paymentRequest);

            if (!paymentResult.Success)
            {
                _logger.Warning(
                    "Payment failed for order {OrderId}: {Error}",
                    order.Id,
                    paymentResult.Message
                );

                order.OrderStatus = Common.Enumerations.Order.OrderProgressStatus.PaymentFailed;
                order.UpdatedDateUtc = DateTime.UtcNow;
                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                await orderRepo.UpdateAsync(order, Guid.Empty);

                return new PaymentTransactionResult
                {
                    IsSuccess = false,
                    ErrorMessage = paymentResult.Message,
                    OrderId = order.Id
                };
            }

            await _cartService.ClearCartAsync(customerId);

            _logger.Information(
                "Purchase completed successfully for order {OrderId}",
                order.Id
            );

            return new PaymentTransactionResult
            {
                IsSuccess = true,
                OrderId = order.Id,
                TransactionId = paymentResult.TransactionId,
                PaymentUrl = paymentResult.PaymentUrl,
                Amount = checkoutSummary.PriceBreakdown.GrandTotal
            };
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error processing purchase for customer {CustomerId}",
                customerId
            );

            return new PaymentTransactionResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<TbOrder> CreateOrderFromCheckoutAsync(
        string customerId,
        OrderPurchaseDto purchaseDto,
        CheckoutSummaryDto checkoutSummary)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var orderDetailRepo = _unitOfWork.TableRepository<TbOrderDetail>();

            var orderNumber = await GenerateOrderNumberAsync();

            var order = new TbOrder
            {
                Id = Guid.NewGuid(),
                Number = orderNumber,
                UserId = customerId,
                DeliveryAddressId = purchaseDto.DeliveryAddressId,
                Price = checkoutSummary.PriceBreakdown.GrandTotal,
                ShippingAmount = checkoutSummary.PriceBreakdown.ShippingCost,
                TaxAmount = checkoutSummary.PriceBreakdown.TaxAmount,
                CouponId = checkoutSummary.CouponId,
                PaymentStatus = PaymentStatus.Pending,
                OrderStatus = Common.Enumerations.Order.OrderProgressStatus.Pending,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty
            };

            await orderRepo.CreateAsync(order, Guid.Empty);

            var cart = await _cartService.GetCartSummaryAsync(customerId);

            foreach (var cartItem in cart.Items)
            {
                var orderDetail = new TbOrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ItemId = cartItem.ItemId,
                    OfferCombinationPricingId = cartItem.OfferCombinationPricingId,
                    VendorId = cartItem.VendorId,
                    WarehouseId = cartItem.WarehouseId ?? Guid.Empty,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice,
                    SubTotal = cartItem.SubTotal,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                };

                await orderDetailRepo.CreateAsync(orderDetail, Guid.Empty);
            }

            if (checkoutSummary.CouponId.HasValue)
            {
                var couponRepo = _unitOfWork.TableRepository<TbCouponCode>();
                var coupon = await couponRepo.FindByIdAsync(checkoutSummary.CouponId.Value);

                if (coupon != null)
                {
                    coupon.UsageCount++;
                    coupon.UpdatedDateUtc = DateTime.UtcNow;
                    await couponRepo.UpdateAsync(coupon, Guid.Empty);
                }
            }

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Order {OrderNumber} created with {ItemCount} items",
                orderNumber,
                cart.Items.Count
            );

            return order;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating order from checkout");
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var date = DateTime.UtcNow;
        var datePrefix = date.ToString("yyyyMMdd");

        var orderRepo = _unitOfWork.TableRepository<TbOrder>();
        var todayCount = await orderRepo.CountAsync(o => o.CreatedDateUtc.Date == date.Date);

        return $"ORD-{datePrefix}-{(todayCount + 1).ToString("D6")}";
    }
}