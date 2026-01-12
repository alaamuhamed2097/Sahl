namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for Average Order Value metric
/// </summary>
public class AverageOrderValueDto
{
    /// <summary>
    /// Total revenue from all orders
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Total number of orders
    /// </summary>
    public int TotalOrders { get; set; }

    /// <summary>
    /// Average Order Value (Total Revenue / Total Orders)
    /// </summary>
    public decimal AverageOrderValue { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string CurrencyCode { get; set; } = "USD";

    /// <summary>
    /// Highest order value in the period
    /// </summary>
    public decimal MaxOrderValue { get; set; }

    /// <summary>
    /// Lowest order value in the period
    /// </summary>
    public decimal MinOrderValue { get; set; }

    /// <summary>
    /// Percentage change from previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }

    /// <summary>
    /// Time period this metric covers
    /// </summary>
    public string Period { get; set; } = "Current Month";
}
