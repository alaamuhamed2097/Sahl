namespace Shared.DTOs.Order.Checkout
{
    /// <summary>
    /// Request DTO for preparing checkout
    /// </summary>
    public class PrepareCheckoutRequest
    {
        public Guid DeliveryAddressId { get; set; }
        public string? CouponCode { get; set; }
    }
}
