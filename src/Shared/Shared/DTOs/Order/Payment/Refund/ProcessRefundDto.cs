namespace Shared.DTOs.Order.Refund;

/// <summary>
/// DTO for processing a refund request (admin action)
/// </summary>
public class ProcessRefundDto
{
    public bool IsApproved { get; set; }
    public decimal RefundAmount { get; set; }
    public string? AdminNotes { get; set; }
}
