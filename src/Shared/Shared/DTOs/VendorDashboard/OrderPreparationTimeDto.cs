namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for Order Preparation Time metric
/// </summary>
public class OrderPreparationTimeDto
{
    /// <summary>
    /// Average time in hours to prepare orders
    /// </summary>
    public decimal AveragePreparationTimeHours { get; set; }

    /// <summary>
    /// Total orders counted in this period
    /// </summary>
    public int TotalOrdersAnalyzed { get; set; }

    /// <summary>
    /// Fastest preparation time in hours
    /// </summary>
    public decimal MinimumPreparationTimeHours { get; set; }

    /// <summary>
    /// Slowest preparation time in hours
    /// </summary>
    public decimal MaximumPreparationTimeHours { get; set; }

    /// <summary>
    /// Orders prepared within SLA (24 hours)
    /// </summary>
    public int OrdersWithinSLA { get; set; }

    /// <summary>
    /// Percentage of orders within SLA
    /// </summary>
    public decimal SLACompliancePercentage { get; set; }

    /// <summary>
    /// Orders delayed (beyond SLA)
    /// </summary>
    public int DelayedOrders { get; set; }

    /// <summary>
    /// Percentage change from previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }

    /// <summary>
    /// Time period this metric covers
    /// </summary>
    public string Period { get; set; } = "Current Month";
}
