using Common.Enumerations.Order;
using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.OrderProcessing.AdminOrder;

/// <summary>
/// DTO for Admin Order List view
/// Shows summary information for all orders in the system
/// </summary>
public class AdminOrderListDto
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
    /// Customer email address
    /// </summary>
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// Customer phone code
    /// </summary>
    public string CustomerPhoneCode { get; set; } = string.Empty;


    /// <summary>
    /// Customer phone number with country code
    /// </summary>
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// Total order amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Current order status
    /// </summary>
    public OrderProgressStatus OrderStatus { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Total number of items in the order
    /// </summary>
    public int TotalItemsCount { get; set; }
}