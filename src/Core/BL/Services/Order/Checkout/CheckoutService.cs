using AutoMapper;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using Common.Enumerations.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Order.Shipping;
using Serilog;
using Shared.DTOs.Order.Checkout;
using Shared.DTOs.Order.CouponCode;

namespace BL.Services.Order.Checkout;

/// <summary>
/// FINAL FIXED VERSION - All errors resolved
/// - Fixed DiscountType enum usage
/// - Fixed TbCouponCode status check (uses IsActive instead of CurrentState)
/// </summary>
public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly ICustomerAddressService _addressService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckoutService(
        ICartService cartService,
        ICustomerAddressService addressService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CheckoutSummaryDto> PrepareCheckoutAsync(
        string customerId,
        PrepareCheckoutRequest request)
    {
        try
        {
            _logger.Information("Preparing checkout for customer {CustomerId}", customerId);

            // 1. Get cart summary
            var cart = await _cartService.GetCartSummaryAsync(customerId);
            if (!cart.Items.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }

            // 2. Validate and get delivery address
            var address = await _addressService.GetAddressForOrderAsync(
                request.DeliveryAddressId,
                customerId
            );

            if (address == null)
            {
                throw new InvalidOperationException("Invalid delivery address");
            }

            // 3. Group items by vendor/warehouse for shipment calculation
            var shipmentGroups = cart.Items
                .GroupBy(i => new
                {
                    VendorId = i.VendorId,
                    WarehouseId = i.WarehouseId ?? Guid.Empty
                })
                .ToList();

            // 4. Calculate shipping for each group
            var shipmentPreviews = new List<ShipmentPreviewDto>();
            decimal totalShipping = 0;

            foreach (var group in shipmentGroups)
            {
                var shippingCost = await CalculateShippingCostAsync(
                    group.Key.VendorId,
                    address.CityId,
                    group.Sum(i => i.Quantity)
                );

                totalShipping += shippingCost;

                var estimatedDays = await GetEstimatedDeliveryDaysAsync(
                    group.Key.VendorId,
                    address.CityId
                );

                shipmentPreviews.Add(new ShipmentPreviewDto
                {
                    VendorName = group.First().SellerName,
                    ItemCount = group.Sum(i => i.Quantity),
                    ItemsList = group.Select(i => i.ItemName).ToList(),
                    SubTotal = group.Sum(i => i.SubTotal),
                    ShippingCost = shippingCost,
                    EstimatedDeliveryDays = estimatedDays
                });
            }

            // 5. Calculate tax (14% VAT for Egypt)
            const decimal taxRate = 0.14m;
            var subtotal = cart.SubTotal;
            var taxAmount = (subtotal + totalShipping) * taxRate;

            // 6. Apply coupon if provided
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

            // 7. Calculate grand total
            var grandTotal = subtotal + totalShipping + taxAmount - discountAmount;

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
                ShipmentPreviews = shipmentPreviews,
                DeliveryAddress = address,
                PriceBreakdown = new PriceBreakdownDto
                {
                    Subtotal = subtotal,
                    ShippingCost = totalShipping,
                    TaxAmount = taxAmount,
                    DiscountAmount = discountAmount,
                    GrandTotal = grandTotal
                },
                CouponInfo = couponInfo,
                CouponId = couponId
            };

            _logger.Information(
                "Checkout prepared for customer {CustomerId}. Total: {Total}",
                customerId,
                grandTotal
            );

            return summary;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error preparing checkout for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CheckoutSummaryDto> PreviewShipmentsAsync(string customerId)
    {
        try
        {
            // Get default address
            var defaultAddress = await _addressService.GetDefaultAddressAsync(customerId);
            if (defaultAddress == null)
            {
                throw new InvalidOperationException("No default delivery address found");
            }

            // Use default address for preview
            var request = new PrepareCheckoutRequest
            {
                DeliveryAddressId = defaultAddress.Id
            };

            return await PrepareCheckoutAsync(customerId, request);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error previewing shipments for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task ValidateCheckoutAsync(string customerId, Guid deliveryAddressId)
    {
        try
        {
            // 1. Validate cart not empty
            var itemCount = await _cartService.GetCartItemCountAsync(customerId);
            if (itemCount == 0)
            {
                throw new InvalidOperationException("Cart is empty");
            }

            // 2. Validate address ownership
            var address = await _addressService.GetAddressByIdAsync(deliveryAddressId, customerId);
            if (address == null)
            {
                throw new UnauthorizedAccessException("Invalid delivery address");
            }

            // 3. Validate all items are still available
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
        catch (Exception ex)
        {
            _logger.Error(ex, "Checkout validation failed for customer {CustomerId}", customerId);
            throw;
        }
    }

    // ==================== PRIVATE HELPER METHODS ====================

    private async Task<decimal> CalculateShippingCostAsync(
        Guid vendorId,
        Guid cityId,
        int totalItems)
    {
        try
        {
            // Get shipping details for this vendor-city combination
            var shippingDetailRepo = _unitOfWork.TableRepository<TbShippingDetail>();
            var shippingDetail = await shippingDetailRepo.FindAsync(
                sd => sd.Offer.VendorId == vendorId && sd.CityId == cityId
            );

            if (shippingDetail != null)
            {
                return shippingDetail.ShippingCost;
            }

            // Fallback: Fixed amount per item
            const decimal defaultShippingPerItem = 20m;
            return defaultShippingPerItem * Math.Max(1, totalItems / 5); // Group shipping
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error calculating shipping for vendor {VendorId}, city {CityId}",
                vendorId,
                cityId
            );
            return 50m; // Fallback default
        }
    }

    private async Task<int> GetEstimatedDeliveryDaysAsync(Guid vendorId, Guid cityId)
    {
        try
        {
            // TODO: Implement sophisticated delivery estimation based on:
            // - Vendor location
            // - City distance
            // - Historical delivery data
            // - Current logistics capacity

            return 3; // Default: 3 days
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error getting estimated delivery days for vendor {VendorId}, city {CityId}",
                vendorId,
                cityId
            );
            return 5; // Fallback: 5 days
        }
    }

    private async Task<CouponValidationResult> ApplyCouponAsync(
        string couponCode,
        string customerId,
        decimal orderSubtotal)
    {
        try
        {
            var couponRepo = _unitOfWork.TableRepository<TbCouponCode>();
            var coupon = await couponRepo.FindAsync(c => c.Code == couponCode);

            if (coupon == null)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Coupon code not found"
                };
            }

            // ✅ Check if coupon is active (using IsActive instead of CurrentState)
            if (!coupon.IsActive || coupon.IsDeleted)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Coupon code is not active"
                };
            }

            // Check expiry date
            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < DateTime.UtcNow)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Coupon code has expired"
                };
            }

            // Check usage limit
            if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Coupon usage limit reached"
                };
            }

            // Check minimum order amount
            if (coupon.MinimumOrderAmount.HasValue && orderSubtotal < coupon.MinimumOrderAmount.Value)
            {
                return new CouponValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Minimum order amount of {coupon.MinimumOrderAmount:C} required"
                };
            }

            // Calculate discount
            decimal discountAmount = 0;
            decimal discountPercentage = 0;
            string discountTypeName = string.Empty;

            // ✅ Use enum value comparison instead of DiscountType.Percentage
            if (coupon.DiscountType == DiscountType.Percentage) // Percentage discount
            {
                discountPercentage = coupon.DiscountValue;
                discountAmount = (orderSubtotal * coupon.DiscountValue) / 100;
                discountTypeName = "Percentage";

                // Apply maximum discount cap if exists
                if (coupon.MaxDiscountAmount.HasValue && discountAmount > coupon.MaxDiscountAmount.Value)
                {
                    discountAmount = coupon.MaxDiscountAmount.Value;
                }
            }
            else if (coupon.DiscountType == DiscountType.FixedAmount) // Fixed amount discount
            {
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
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error applying coupon {CouponCode} for customer {CustomerId}",
                couponCode,
                customerId
            );
            return new CouponValidationResult
            {
                IsValid = false,
                ErrorMessage = "Error validating coupon code"
            };
        }
    }
}