using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment.PaymentProcessing;

/// <summary>
/// Request DTO for creating an order
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Delivery address ID (MANDATORY)
    /// </summary>
    public Guid? DeliveryAddressId { get; set; }

    /// <summary>
    /// Payment method type (MANDATORY)
    /// </summary>
    public PaymentMethodType PaymentMethod { get; set; }

    /// <summary>
    /// Payment method ID (required for Card and WalletAndCard)
    /// </summary>
    public Guid? PaymentMethodId { get; set; }

    /// <summary>
    /// Coupon code (optional)
    /// </summary>
    public string? CouponCode { get; set; }

    /// <summary>
    /// Order notes (optional)
    /// </summary>
    public string? Notes { get; set; }
}
