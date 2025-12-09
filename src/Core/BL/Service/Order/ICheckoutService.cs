using Shared.DTOs.ECommerce.Checkout;
using Shared.DTOs.ECommerce.Order;

namespace BL.Services.Order
{
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
        Task<Shared.DTOs.ECommerce.Checkout.CheckoutSummaryDto> PrepareCheckoutAsync(string customerId, PrepareCheckoutRequest request);
        Task<Shared.DTOs.ECommerce.Checkout.CheckoutSummaryDto> PreviewShipmentsAsync(string customerId);
        Task ValidateCheckoutAsync(string customerId, Guid deliveryAddressId);
    }
}
