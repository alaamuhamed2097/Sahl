namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// Comprehensive DTO for all Vendor Performance Indicators (KPIs)
/// </summary>
public class VendorPerformanceIndicatorsDto
{
    /// <summary>
    /// Vendor ID these metrics belong to
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Conversion Rate metric
    /// </summary>
    public ConversionRateDto ConversionRate { get; set; } = new();

    /// <summary>
    /// Average Order Value metric
    /// </summary>
    public AverageOrderValueDto AverageOrderValue { get; set; } = new();

    /// <summary>
    /// Return Rate metric
    /// </summary>
    public ReturnRateDto ReturnRate { get; set; } = new();

    /// <summary>
    /// Order Preparation Time metric
    /// </summary>
    public OrderPreparationTimeDto OrderPreparationTime { get; set; } = new();

    /// <summary>
    /// Buy Box Win Rate metric
    /// </summary>
    public BuyBoxWinRateDto BuyBoxWinRate { get; set; } = new();

    /// <summary>
    /// Overall vendor health score (0-100)
    /// Based on all performance indicators
    /// </summary>
    public decimal OverallHealthScore { get; set; }

    /// <summary>
    /// Health status badge
    /// </summary>
    public string HealthStatus { get; set; } = "Good"; // Good, Fair, Poor

    /// <summary>
    /// Timestamp when metrics were calculated
    /// </summary>
    public DateTime CalculatedAt { get; set; }

    /// <summary>
    /// Time period covered by these metrics
    /// </summary>
    public string ReportPeriod { get; set; } = "Current Month";
}
