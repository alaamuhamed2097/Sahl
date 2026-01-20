using Common.Enumerations.Order;

namespace Common.Filters;

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
