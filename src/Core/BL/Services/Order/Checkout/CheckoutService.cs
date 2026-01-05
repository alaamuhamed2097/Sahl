using BL.Contracts.IMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.Fulfillment;
using BL.Contracts.Service.Setting;
using Common.Enumerations.Order;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Offer;
using Serilog;
using Shared.DTOs.ECommerce.Offer;
using Shared.DTOs.Order.Checkout;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;

namespace BL.Services.Order.Checkout;

/// <summary>
/// Service responsible for orchestrating the checkout process.
/// Coordinates between cart, address, shipping calculation, and coupon validation.
/// Note: For post-order operations, use IShipmentService, IDeliveryService, and IFulfillmentService
/// </summary>
public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly ICustomerAddressService _addressService;
    private readonly IShippingCalculationService _shippingService;
    private readonly ICouponCodeRepository _couponRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly ISystemSettingsService _systemSettings;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public CheckoutService(
        ICartService cartService,
        ICustomerAddressService addressService,
        IShippingCalculationService shippingService,
        ICouponCodeRepository couponRepository,
        IOfferRepository offerRepository,
        ISystemSettingsService systemSettings,
        IBaseMapper mapper,
        ILogger logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
        _shippingService = shippingService ?? throw new ArgumentNullException(nameof(shippingService));
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _systemSettings = systemSettings ?? throw new ArgumentNullException(nameof(systemSettings));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CheckoutSummaryDto> PrepareCheckoutAsync(
        string customerId,
        PrepareCheckoutRequest request)
    {
        // Step 1: Get cart summary and validate it's not empty
        var cart = await _cartService.GetCartSummaryAsync(customerId);
        if (!cart.Items.Any())
        {
            throw new InvalidOperationException("Cart is empty");
        }

        // Step 2: Validate and retrieve delivery address
        var address = await _addressService.GetAddressForOrderAsync(
            request.DeliveryAddressId,
            customerId
        );

        if (address == null)
        {
            throw new InvalidOperationException("Invalid delivery address");
        }

        // Step 3: Get all offers for cart items
        var offerCombinationPricingIds = cart.Items
            .Select(i => i.OfferCombinationPricingId)
            .Distinct()
            .ToList();

        var offersEntities = await _offerRepository
            .GetOffersByCombinationPricingIdsAsync(offerCombinationPricingIds);

        if (!offersEntities.Any())
        {
            throw new InvalidOperationException("No offers found for the given combination pricing IDs");
        }

        // Map offers to DTOs for easier access
        var offerDtos = _mapper.MapList<TbOffer, OfferDto>(offersEntities);
        var offerDictionary = offerDtos.ToDictionary(o => o.Id);

        // Step 4: Calculate shipping costs using dedicated shipping service
        var cartItemsForShipping = cart.Items.Select(i =>
        {
            // Find the matching offer for this cart item
            var offer = offerDictionary.TryGetValue(i.OfferCombinationPricingId, out var foundOffer)
                ? foundOffer
                : throw new InvalidOperationException($"Offer not found for cart item {i.ItemId}");

            return new CartItemForShipping
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                SellerName = i.SellerName,
                Offer = offer,
                Quantity = i.Quantity,
                SubTotal = i.SubTotal
            };
        }).ToList();

        var shippingResult = await _shippingService.CalculateShippingAsync(
            cartItemsForShipping,
            request.DeliveryAddressId,
            address.CityId
        );

        // Step 5: Calculate tax from system settings
        var subtotal = cart.SubTotal;
        var taxRate = await _systemSettings.GetTaxRateAsync();
        var taxAmount = (subtotal + shippingResult.TotalShippingCost) * (taxRate / 100);

        // Step 6: Apply coupon discount if provided
        decimal discountAmount = 0;
        CouponInfoDto? couponInfo = null;
        Guid? couponId = null;

        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var couponResult = await ApplyCouponAsync(
                request.CouponCode,
                customerId,
                subtotal
            );

            if (couponResult.IsValid)
            {
                discountAmount = couponResult.DiscountAmount;
                couponId = couponResult.CouponId;
                couponInfo = new CouponInfoDto
                {
                    Code = request.CouponCode,
                    DiscountAmount = discountAmount,
                    DiscountPercentage = couponResult.DiscountPercentage,
                    DiscountType = couponResult.DiscountTypeName
                };
            }
            else
            {
                _logger.Warning(
                    "Invalid coupon code {CouponCode} for customer {CustomerId}: {Error}",
                    request.CouponCode,
                    customerId,
                    couponResult.ErrorMessage
                );
            }
        }

        // Step 7: Calculate final grand total
        var grandTotal = subtotal + shippingResult.TotalShippingCost + taxAmount - discountAmount;

        // Step 8: Build checkout summary response
        var summary = new CheckoutSummaryDto
        {
            Items = cart.Items.Select(i => new CheckoutItemDto
            {
                ItemId = i.ItemId,
                ItemName = i.ItemName,
                SellerName = i.SellerName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                SubTotal = i.SubTotal,
                IsAvailable = i.IsAvailable
            }).ToList(),
            ShipmentPreviews = shippingResult.ShipmentPreviews,
            DeliveryAddress = address,
            PriceBreakdown = new PriceBreakdownDto
            {
                Subtotal = subtotal,
                ShippingCost = shippingResult.TotalShippingCost,
                TaxAmount = taxAmount,
                DiscountAmount = discountAmount,
                GrandTotal = grandTotal
            },
            CouponInfo = couponInfo,
            CouponId = couponId
        };

        return summary;
    }

    public async Task<CheckoutSummaryDto> PreviewShipmentsAsync(string customerId)
    {
        // Get customer's default delivery address for preview
        var defaultAddress = await _addressService.GetDefaultAddressAsync(customerId);
        if (defaultAddress == null)
        {
            throw new InvalidOperationException("No default delivery address found");
        }

        // Use default address to generate preview
        var request = new PrepareCheckoutRequest
        {
            DeliveryAddressId = defaultAddress.Id
        };

        return await PrepareCheckoutAsync(customerId, request);
    }

    public async Task ValidateCheckoutAsync(string customerId, Guid deliveryAddressId)
    {
        // Validation 1: Ensure cart is not empty
        var itemCount = await _cartService.GetCartItemCountAsync(customerId);
        if (itemCount == 0)
        {
            throw new InvalidOperationException("Cart is empty");
        }

        // Validation 2: Verify customer owns the delivery address
        var address = await _addressService.GetAddressByIdAsync(deliveryAddressId, customerId);
        if (address == null)
        {
            throw new UnauthorizedAccessException("Invalid delivery address");
        }

        // Validation 3: Check all cart items are still available for purchase
        var cart = await _cartService.GetCartSummaryAsync(customerId);
        var unavailableItems = cart.Items.Where(i => !i.IsAvailable).ToList();

        if (unavailableItems.Any())
        {
            var itemNames = string.Join(", ", unavailableItems.Select(i => i.ItemName));
            throw new InvalidOperationException(
                $"The following items are no longer available: {itemNames}"
            );
        }

        _logger.Information("Checkout validation passed for customer {CustomerId}", customerId);
    }

    #region Private Helper Methods

    /// <summary>
    /// Validates and applies a coupon code to the order.
    /// Checks coupon status, expiry, usage limits, and calculates discount amount.
    /// </summary>
    private async Task<CouponValidationResult> ApplyCouponAsync(
        string couponCode,
        string customerId,
        decimal orderSubtotal)
    {
        var coupon = await _couponRepository.GetByCodeAsync(couponCode);

        if (coupon == null)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = "Coupon code not found"
            };
        }

        // Check if coupon is active and not deleted
        if (!coupon.IsActive || coupon.IsDeleted)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = "Coupon code is not active"
            };
        }

        // Check if coupon has expired
        if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < DateTime.UtcNow)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = "Coupon code has expired"
            };
        }

        // Check if usage limit has been reached
        if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = "Coupon usage limit reached"
            };
        }

        // Check if order meets minimum amount requirement
        if (coupon.MinimumOrderAmount.HasValue && orderSubtotal < coupon.MinimumOrderAmount.Value)
        {
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Minimum order amount of {coupon.MinimumOrderAmount:C} required"
            };
        }

        // Calculate discount based on coupon type
        decimal discountAmount = 0;
        decimal discountPercentage = 0;
        string discountTypeName = string.Empty;

        if (coupon.DiscountType == DiscountType.Percentage)
        {
            // Percentage-based discount
            discountPercentage = coupon.DiscountValue;
            discountAmount = (orderSubtotal * coupon.DiscountValue) / 100;
            discountTypeName = "Percentage";

            // Apply maximum discount cap if specified
            if (coupon.MaxDiscountAmount.HasValue && discountAmount > coupon.MaxDiscountAmount.Value)
            {
                discountAmount = coupon.MaxDiscountAmount.Value;
            }
        }
        else if (coupon.DiscountType == DiscountType.FixedAmount)
        {
            // Fixed amount discount
            discountAmount = coupon.DiscountValue;
            discountPercentage = (discountAmount / orderSubtotal) * 100;
            discountTypeName = "Fixed Amount";
        }

        return new CouponValidationResult
        {
            IsValid = true,
            CouponId = coupon.Id,
            DiscountAmount = discountAmount,
            DiscountPercentage = discountPercentage,
            DiscountTypeName = discountTypeName
        };
    }

    #endregion
}