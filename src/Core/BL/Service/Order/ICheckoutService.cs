using Shared.DTOs.ECommerce.Checkout;

namespace BL.Services.Order
{
    public interface ICheckoutService
    {
        Task<CheckoutSummaryDto> PrepareCheckoutAsync(string customerId, PrepareCheckoutRequest request);
        Task<CheckoutSummaryDto> PreviewShipmentsAsync(string customerId);
        Task ValidateCheckoutAsync(string customerId, Guid deliveryAddressId);
    }
}
