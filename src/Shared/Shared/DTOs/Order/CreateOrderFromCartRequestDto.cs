namespace Shared.DTOs.Order
{
    /// <summary>
    /// Stage 3: Create Order Request
    /// </summary>
    public class CreateOrderFromCartRequest
    {
        public Guid DeliveryAddressId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? CouponCode { get; set; }
        public string? Notes { get; set; }
    }
}
