namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for Return Rate metric
/// </summary>
public class ReturnRateDto
{
    /// <summary>
    /// Total number of orders delivered
    /// </summary>
    public int TotalDeliveredOrders { get; set; }

    /// <summary>
    /// Number of orders that were returned
    /// </summary>
    public int TotalReturnedOrders { get; set; }

    /// <summary>
    /// Return rate percentage (Returned / Delivered * 100)
    /// </summary>
    public decimal ReturnRatePercentage { get; set; }

    /// <summary>
    /// Number of refund requests
    /// </summary>
    public int RefundRequests { get; set; }

    /// <summary>
    /// Number of refunds processed
    /// </summary>
    public int RefundsProcessed { get; set; }

    /// <summary>
    /// Total refund amount
    /// </summary>
    public decimal TotalRefundAmount { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string CurrencyCode { get; set; } = "USD";

    /// <summary>
    /// Percentage change from previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }

    /// <summary>
    /// Time period this metric covers
    /// </summary>
    public string Period { get; set; } = "Current Month";
}
