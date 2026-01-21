using Common.Enumerations.Order;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// DTO for creating a refund request
/// </summary>
public class CreateRefundRequestDto
{
    public Guid OrderDetailId { get; set; }
    public Guid? DeliveryAddressId { get; set; }
    public int RequestedItemsCount { get; set; }
    public RefundReason Reason { get; set; } 
    public string? ReasonDetails { get; set; }
}

