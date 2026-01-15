using Dashboard.Constants;
using Dashboard.Contracts.Dashboard;
using Dashboard.Contracts.General;
using Shared.DTOs.AdminDashboard;
using Shared.GeneralModels;

namespace Dashboard.Services.Dashboard
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IApiService _apiService;

        public AdminDashboardService(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        public async Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryAsync()
        {
            try
            {
                return await _apiService.GetAsync<AdminDashboardSummaryDto>(ApiEndpoints.AdminDashboard.Summary);
            }
            catch (Exception ex)
            {
                return new ResponseModel<AdminDashboardSummaryDto>
                {
                    Success = false,
                    Message = "Error retrieving dashboard summary",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ResponseModel<AdminDashboardSummaryDto>> GetDashboardSummaryByPeriodAsync(string period)
        {
            try
            {
                return await _apiService.GetAsync<AdminDashboardSummaryDto>(
                    ApiEndpoints.AdminDashboard.SummaryByPeriod(period));
            }
            catch (Exception ex)
            {
                return new ResponseModel<AdminDashboardSummaryDto>
                {
                    Success = false,
                    Message = "Error retrieving dashboard summary for period",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<TopProductDto>>> GetTopProductsAsync(int limit = 10)
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<TopProductDto>>(
                    $"{ApiEndpoints.AdminDashboard.TopProducts}?limit={limit}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<TopProductDto>>
                {
                    Success = false,
                    Message = "Error retrieving top products",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ResponseModel<IEnumerable<VendorPerformanceDto>>> GetTopVendorsAsync(int limit = 10)
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<VendorPerformanceDto>>(
                    $"{ApiEndpoints.AdminDashboard.TopVendors}?limit={limit}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<VendorPerformanceDto>>
                {
                    Success = false,
                    Message = "Error retrieving top vendors",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<ResponseModel<AdminDashboardSummaryDto>> GetStatisticsForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var startDateStr = startDate.ToString("yyyy-MM-dd");
                var endDateStr = endDate.ToString("yyyy-MM-dd");
                return await _apiService.GetAsync<AdminDashboardSummaryDto>(
                    $"{ApiEndpoints.AdminDashboard.Statistics}?startDate={startDateStr}&endDate={endDateStr}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<AdminDashboardSummaryDto>
                {
                    Success = false,
                    Message = "Error retrieving statistics",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
