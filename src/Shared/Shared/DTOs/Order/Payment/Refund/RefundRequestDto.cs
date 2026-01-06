using Common.Enumerations.Order;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// DTO for refund request details
/// </summary>
public class RefundRequestDto
{
    public string Number { get; set; } = null!;

    // Order References
    public Guid OrderDetailId { get; set; }

    // Customer and Vendor References
    public Guid CustomerId { get; set; }
    public Guid DeliveryAddressId { get; set; }
    public Guid VendorId { get; set; }

    // Refund Reason
    public RefundReason RefundReason { get; set; }
    public string? RefundReasonDetails { get; set; }
    public string? RejectionReason { get; set; } // Why request was denied
    public RefundStatus RefundStatus { get; set; }
    public decimal ReturnShippingCost { get; set; } = 0m;
    public decimal RefundAmount { get; set; } = 0m;
    public int RequestedItemsCount { get; set; }
    public int ApprovedItemsCount { get; set; }
    public string? RefundTransactionId { get; set; }
    public string? ReturnTrackingNumber { get; set; }
    public DateTime RequestDateUTC { get; set; }
    [JsonIgnore]
    public string RequestDateLocalFormatted =>
        TimeZoneInfo.ConvertTimeFromUtc(RequestDateUTC, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");
    public DateTime? ApprovedDateUTC { get; set; }
    public DateTime? ReturnedDateUTC { get; set; }
    public DateTime? RefundedDateUTC { get; set; }
    public string? AdminNotes { get; set; }
    public string? AdminUserId { get; set; }
}
