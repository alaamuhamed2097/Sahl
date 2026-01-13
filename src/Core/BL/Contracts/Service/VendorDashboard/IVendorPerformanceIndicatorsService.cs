using Shared.DTOs.VendorDashboard;

namespace BL.Contracts.Service.VendorDashboard;

/// <summary>
/// Service interface for Vendor Performance Indicators (KPIs) calculation
/// </summary>
public interface IVendorPerformanceIndicatorsService
{
    /// <summary>
    /// Gets all performance indicators for a vendor
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="period">Time period for analysis (default: "CurrentMonth")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comprehensive performance indicators</returns>
    Task<VendorPerformanceIndicatorsDto> GetAllPerformanceIndicatorsAsync(
        Guid vendorId,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets conversion rate metric
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Conversion rate metric</returns>
    Task<ConversionRateDto> GetConversionRateAsync(
        Guid vendorId,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets average order value metric
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Average order value metric</returns>
    Task<AverageOrderValueDto> GetAverageOrderValueAsync(
        Guid vendorId,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets return rate metric
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return rate metric</returns>
    Task<ReturnRateDto> GetReturnRateAsync(
        Guid vendorId,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets order preparation time metric
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order preparation time metric</returns>
    Task<OrderPreparationTimeDto> GetOrderPreparationTimeAsync(
        Guid vendorId,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets buy box win rate metric
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="topProductsLimit">Number of top products to return (default: 10)</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Buy box win rate metric</returns>
    Task<BuyBoxWinRateDto> GetBuyBoxWinRateAsync(
        Guid vendorId,
        int topProductsLimit = 10,
        string period = "CurrentMonth",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical trend for a specific KPI
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="kpiType">Type of KPI (ConversionRate, AOV, ReturnRate, etc.)</param>
    /// <param name="months">Number of months to retrieve history for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Historical trend data</returns>
    Task<KPITrendDto> GetKPITrendAsync(
        Guid vendorId,
        string kpiType,
        int months = 6,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO for historical KPI trends
/// </summary>
public class KPITrendDto
{
    /// <summary>
    /// KPI type name
    /// </summary>
    public string KPIType { get; set; } = string.Empty;

    /// <summary>
    /// Current value
    /// </summary>
    public decimal CurrentValue { get; set; }

    /// <summary>
    /// Monthly historical values
    /// </summary>
    public List<KPIMonthlyValueDto> MonthlyValues { get; set; } = new List<KPIMonthlyValueDto>();

    /// <summary>
    /// Average value across all months
    /// </summary>
    public decimal AverageValue { get; set; }

    /// <summary>
    /// Highest value in the period
    /// </summary>
    public decimal MaxValue { get; set; }

    /// <summary>
    /// Lowest value in the period
    /// </summary>
    public decimal MinValue { get; set; }

    /// <summary>
    /// Overall trend direction
    /// </summary>
    public string Trend { get; set; } = "Stable"; // Improving, Declining, Stable
}

/// <summary>
/// Monthly KPI value
/// </summary>
public class KPIMonthlyValueDto
{
    /// <summary>
    /// Month in YYYY-MM format
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// KPI value for the month
    /// </summary>
    public decimal Value { get; set; }
}
