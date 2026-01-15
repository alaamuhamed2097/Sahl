using Dashboard.Models.pagintion;
using Shared.DTOs.AdminDashboard;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Dashboard
{
    public interface IAdminDashboardService
    {
        Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryAsync();
        Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryByPeriodAsync(string period);
        Task<ResponseModel<IEnumerable<TopProductDto>>> GetTopProductsAsync(int limit = 10);
        Task<ResponseModel<IEnumerable<VendorPerformanceDto>>> GetTopVendorsAsync(int limit = 10);
        Task<ResponseModel<AdminDashboardSummaryDto>> GetStatisticsForDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
