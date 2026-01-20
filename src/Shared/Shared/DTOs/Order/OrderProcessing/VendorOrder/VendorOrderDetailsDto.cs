using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.OrderProcessing.VendorDashboardOrder;

namespace Shared.DTOs.Order.OrderProcessing.VendorOrder;

/// <summary>
/// DTO for Vendor Order Details view
/// Shows detailed information about an order including only vendor's items
/// </summary>
public class VendorOrderDetailsDto
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
    /// Customer full name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Customer phone number
    /// </summary>
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// Current order status
    /// </summary>
    public OrderProgressStatus OrderStatus { get; set; }

    /// <summary>
    /// Payment status for vendor's portion
    /// </summary>
    public PaymentStatus VendorPaymentStatus { get; set; }

    /// <summary>
    /// Delivery address information
    /// </summary>
    public DeliveryAddressDto DeliveryAddress { get; set; } = null!;

    /// <summary>
    /// List of items belonging to this vendor
    /// </summary>
    public List<VendorOrderItemDto> VendorItems { get; set; } = new();

    /// <summary>
    /// Total amount for all vendor's items in this order
    /// </summary>
    public decimal VendorTotalAmount { get; set; }
}