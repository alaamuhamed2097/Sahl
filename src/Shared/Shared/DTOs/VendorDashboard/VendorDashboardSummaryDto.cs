namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// Main DTO for the Vendor Dashboard Summary containing all KPI metrics
/// </summary>
public class VendorDashboardSummaryDto
{
    /// <summary>
    /// Vendor ID for which the dashboard data is generated
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Daily sales metrics
    /// </summary>
    public DailySalesDto? DailySales { get; set; }

    /// <summary>
    /// New orders metrics
    /// </summary>
    public NewOrdersDto? NewOrders { get; set; }

    /// <summary>
    /// List of best-selling products
    /// </summary>
    public IEnumerable<BestSellingProductDto> BestSellingProducts { get; set; } = new List<BestSellingProductDto>();

    /// <summary>
    /// List of latest customer reviews
    /// </summary>
    public IEnumerable<LatestReviewDto> LatestReviews { get; set; } = new List<LatestReviewDto>();

    /// <summary>
    /// Overall vendor rating/satisfaction
    /// </summary>
    public decimal? OverallRating { get; set; }

    /// <summary>
    /// Total number of products sold (lifetime or period-based)
    /// </summary>
    public int TotalProductsSold { get; set; }

    /// <summary>
    /// Total number of reviews received
    /// </summary>
    public int TotalReviews { get; set; }

    /// <summary>
    /// Timestamp when the dashboard data was generated
    /// </summary>
    public DateTime GeneratedAt { get; set; }
}
