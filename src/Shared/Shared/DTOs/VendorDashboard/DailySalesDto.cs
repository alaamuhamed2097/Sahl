namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for daily sales metrics in the vendor dashboard
/// </summary>
public class DailySalesDto
{
    /// <summary>
    /// Total sales amount for the day
    /// </summary>
    public decimal TotalSales { get; set; }

    /// <summary>
    /// Number of orders placed
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// Currency code (e.g., "USD", "EGP")
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Percentage change compared to previous day
    /// </summary>
    public decimal? PercentageChange { get; set; }
}
