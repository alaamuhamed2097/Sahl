namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// DTO for creating a refund request
/// </summary>
public class CreateRefundRequestDto
{
    public Guid OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReasonDetails { get; set; }
    public string? CustomerNotes { get; set; }
}

