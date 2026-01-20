using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;
using Shared.DTOs.Order.Payment;
using Shared.DTOs.User.Customer;

namespace Shared.DTOs.Order.OrderProcessing.AdminOrder;

/// <summary>
/// DTO for Admin Order Details view
/// Contains complete order information for administrative purposes
/// </summary>
public class AdminOrderDetailsDto
{
    /// <summary>
    /// Order unique identifier
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Human-readable order number
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// When the order was created
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Expected or actual delivery date
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Current order status
    /// </summary>
    public OrderProgressStatus OrderStatus { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    public AdminCustomerInfoDto Customer { get; set; } = null!;

    /// <summary>
    /// Delivery address information
    /// </summary>
    public DeliveryAddressDto DeliveryAddress { get; set; } = null!;

    /// <summary>
    /// List of all items in the order
    /// </summary>
    public List<AdminOrderItemDto> Items { get; set; } = new();

    /// <summary>
    /// List of all shipments for this order
    /// </summary>
    public List<ShipmentInfoDto> Shipments { get; set; } = new();

    /// <summary>
    /// Payment information
    /// </summary>
    public PaymentInfoDto PaymentInfo { get; set; } = null!;

    /// <summary>
    /// Coupon/discount information if applied
    /// </summary>
    public CouponInfoDto? Coupon { get; set; }

    /// <summary>
    /// Subtotal (before discounts, shipping, and tax)
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    /// Shipping charges
    /// </summary>
    public decimal ShippingAmount { get; set; }

    /// <summary>
    /// Tax amount
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Final total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Order notes and comments
    /// </summary>
    public string? OrderNotes { get; set; }
}