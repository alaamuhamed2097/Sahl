using Shared.DTOs.VendorDashboard;

namespace BL.Contracts.Service.VendorDashboard;

/// <summary>
/// Service interface for Vendor Dashboard analytics and KPI metrics
/// </summary>
public interface IVendorDashboardService
{
    /// <summary>
    /// Gets comprehensive dashboard summary with all KPIs for a vendor
    /// </summary>
    /// <param name="userId">The vendor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Complete dashboard summary with all metrics</returns>
    Task<VendorDashboardSummaryDto> GetDashboardSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets daily sales metrics for the current day
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Daily sales metrics</returns>
    Task<DailySalesDto> GetDailySalesAsync(
        Guid vendorId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets new/pending orders count and status breakdown
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New orders metrics</returns>
    Task<NewOrdersDto> GetNewOrdersAsync(
        Guid vendorId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets best-selling products for the vendor
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="limit">Number of top products to return (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of best-selling products</returns>
    Task<IEnumerable<BestSellingProductDto>> GetBestSellingProductsAsync(
        Guid vendorId,
        int limit = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets latest customer reviews for the vendor
    /// </summary>
    /// <param name="vendorId">The vendor ID</param>
    /// <param name="limit">Number of latest reviews to return (default: 5)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of latest reviews</returns>
    Task<IEnumerable<LatestReviewDto>> GetLatestReviewsAsync(
        Guid vendorId,
        int limit = 5,
        CancellationToken cancellationToken = default);
}
