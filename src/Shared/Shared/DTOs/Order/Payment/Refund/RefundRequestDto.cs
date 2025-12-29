using Common.Enumerations.Order;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// DTO for refund request details
/// </summary>
public class RefundRequestDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string RefundReason { get; set; } = string.Empty;
    public string? RefundReasonDetails { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal RefundAmount { get; set; }
    public RefundStatus RefundStatus { get; set; }
    public string? CustomerNotes { get; set; }
    public string? AdminUserId { get; set; }
    public string? AdminNotes { get; set; }
    public string? RefundTransactionId { get; set; }
    public string? RefundFailureReason { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime? RefundCompletedDate { get; set; }
}
