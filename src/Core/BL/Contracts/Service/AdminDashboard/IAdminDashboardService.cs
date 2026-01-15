using Shared.DTOs.AdminDashboard;
using Shared.GeneralModels;

namespace BL.Contracts.Service.AdminDashboard;

/// <summary>
/// Service contract for Admin Dashboard operations
/// Provides comprehensive system analytics and KPI metrics
/// </summary>
public interface IAdminDashboardService
{
    /// <summary>
    /// Get comprehensive admin dashboard summary with all KPIs
    /// </summary>
    /// <returns>Admin dashboard summary DTO with all metrics</returns>
    Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryAsync();

    /// <summary>
    /// Get dashboard summary for a specific period
    /// </summary>
    /// <param name="period">Period identifier (e.g., "ThisMonth", "ThisWeek", "LastMonth")</param>
    /// <returns>Admin dashboard summary DTO filtered by period</returns>
    Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryByPeriodAsync(string period);

    /// <summary>
    /// Get top-selling products
    /// </summary>
    /// <param name="limit">Number of products to retrieve (default: 10)</param>
    /// <returns>List of top-selling products</returns>
    Task<ResponseModel<IEnumerable<TopProductDto>>> GetTopProductsAsync(int limit = 10);

    /// <summary>
    /// Get top-performing vendors
    /// </summary>
    /// <param name="limit">Number of vendors to retrieve (default: 10)</param>
    /// <returns>List of top-performing vendors</returns>
    Task<ResponseModel<IEnumerable<VendorPerformanceDto>>> GetTopVendorsAsync(int limit = 10);

    /// <summary>
    /// Get system statistics for a specific date range
    /// </summary>
    /// <param name="startDate">Start date for the statistics</param>
    /// <param name="endDate">End date for the statistics</param>
    /// <returns>Admin dashboard summary DTO for the date range</returns>
    Task<ResponseModel<AdminDashboardSummaryDto>> GetStatisticsForDateRangeAsync(DateTime startDate, DateTime endDate);
}
