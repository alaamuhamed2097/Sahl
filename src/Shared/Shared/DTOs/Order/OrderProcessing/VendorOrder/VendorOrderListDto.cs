using Common.Enumerations.Order;
using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.OrderProcessing.VendorOrder;

/// <summary>
/// DTO for Vendor Order List view
/// Shows summary information about orders containing vendor's items
/// </summary>
public class VendorOrderListDto
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
    /// Total amount for vendor's items in this order
    /// (Sum of vendor's order details subtotals)
    /// </summary>
    public decimal VendorTotal { get; set; }

    /// <summary>
    /// Current order status
    /// </summary>
    public OrderProgressStatus OrderStatus { get; set; }

    /// <summary>
    /// Payment status for vendor's portion
    /// </summary>
    public PaymentStatus VendorPaymentStatus { get; set; }

    /// <summary>
    /// Total quantity of vendor's items in this order
    /// </summary>
    public int VendorItemsCount { get; set; }
}