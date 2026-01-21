using Common.Enumerations.Order;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order.Payment.Refund;

public class UpdateRefundStatusDto
{
    [Required]
    public RefundStatus NewStatus { get; set; }
    public Guid RefundId { get; set; }
    public string? Notes { get; set; }
    public string? RejectionReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public int? ApprovedItemsCount { get; set; }
    public string? TrackingNumber { get; set; }
}

