using Common.Enumerations.Order;
using Common.Filters;

namespace Shared.DTOs.Order.Payment.Refund;

/// <summary>
/// Search criteria for refund requests
/// </summary>
public class RefundSearchCriteria : BaseSearchCriteriaModel
{
    public RefundStatus? Status { get; set; }
    public string? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
