namespace Shared.DTOs.Order.Payment.PaymentProcessing
{
    /// <summary>
    /// DTO for order purchase request
    /// </summary>
    public class OrderPurchaseDto
    {
        public Guid DeliveryAddressId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? CouponCode { get; set; }
        public string? CustomerNotes { get; set; }
    }
}
