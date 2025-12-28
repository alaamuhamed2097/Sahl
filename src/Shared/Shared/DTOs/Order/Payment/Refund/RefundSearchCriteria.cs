using Common.Enumerations.Order;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// Search criteria for refund requests
/// </summary>
public class RefundSearchCriteria
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public RefundStatus? Status { get; set; }
    public string? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
