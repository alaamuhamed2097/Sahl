using Shared.DTOs.Order.Checkout;

namespace BL.Contracts.Service.Order.Checkout;

/// <summary>
/// Stage 2: Checkout Service
/// Responsibilities:
/// - Validate cart items
/// - Check stock availability
/// - Calculate totals and shipping
/// - Preview shipments
/// - Apply coupons
/// </summary>
public interface ICheckoutService
{
    Task<CheckoutSummaryDto> PrepareCheckoutAsync(
        string customerId,
        PrepareCheckoutRequest request);
    Task<CheckoutSummaryDto> PreviewShipmentsAsync(string customerId);
    Task ValidateCheckoutAsync(string customerId, Guid? deliveryAddressId);
}
