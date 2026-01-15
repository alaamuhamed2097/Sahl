namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for Conversion Rate metric
/// </summary>
public class ConversionRateDto
{
    /// <summary>
    /// Total number of product views
    /// </summary>
    public long TotalProductViews { get; set; }

    /// <summary>
    /// Total number of successful orders/purchases
    /// </summary>
    public long TotalOrders { get; set; }

    /// <summary>
    /// Conversion rate percentage (Orders / Views * 100)
    /// </summary>
    public decimal ConversionRatePercentage { get; set; }

    /// <summary>
    /// Percentage change from previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }

    /// <summary>
    /// Time period this metric covers
    /// </summary>
    public string Period { get; set; } = "Current Month";
}
