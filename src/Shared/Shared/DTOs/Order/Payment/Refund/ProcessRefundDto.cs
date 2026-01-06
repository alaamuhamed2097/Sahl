using Common.Enumerations.Order;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// DTO for processing a refund request (admin action)
/// </summary>
public class ProcessRefundDto
{
    public RefundStatus RefundStatus { get; set; }
    public decimal RefundAmount { get; set; }
    public string? AdminNotes { get; set; }
    public string? RejectionReason { get; set; }
}
