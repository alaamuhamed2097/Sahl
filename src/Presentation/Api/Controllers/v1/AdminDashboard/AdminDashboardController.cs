using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.AdminDashboard;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.AdminDashboard;
using Shared.GeneralModels;

namespace Api.Controllers.v1.AdminDashboard;

/// <summary>
/// Controller for Admin Dashboard analytics and KPI metrics
/// Provides comprehensive system-wide insights and performance metrics
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin/dashboard")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminDashboardController : BaseController
{
    private readonly IAdminDashboardService _dashboardService;
    private readonly ILogger<AdminDashboardController> _logger;

    public AdminDashboardController(
        IAdminDashboardService dashboardService,
        ILogger<AdminDashboardController> logger)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get comprehensive admin dashboard summary with all KPIs
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns aggregated metrics including:
    /// - User statistics (total users, vendors, customers)
    /// - Product and category information
    /// - Order statistics (total, pending, completed)
    /// - Revenue metrics
    /// - Top-selling products
    /// - Top-performing vendors
    /// - Performance change percentages
    /// 
    /// Requires Admin role
    /// </remarks>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ResponseModel<AdminDashboardSummaryDto>>> GetDashboardSummary()
    {
        try
        {
            _logger.LogInformation("Fetching admin dashboard summary");
            var result = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching admin dashboard summary");
            return BadRequest(new ResponseModel<AdminDashboardSummaryDto>
            {
                Success = false,
                Message = "Error fetching dashboard summary",
                Errors = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get dashboard summary for a specific period
    /// </summary>
    /// <param name="period">Period identifier (ThisMonth, ThisWeek, LastMonth, etc.)</param>
    /// <remarks>
    /// Supported periods:
    /// - ThisMonth
    /// - ThisWeek
    /// - LastMonth
    /// - LastWeek
    /// - ThisYear
    /// </remarks>
    [HttpGet("summary/{period}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ResponseModel<AdminDashboardSummaryDto>>> GetDashboardSummaryByPeriod(string period)
    {
        try
        {
            _logger.LogInformation("Fetching admin dashboard summary for period: {Period}", period);
            var result = await _dashboardService.GetDashboardSummaryByPeriodAsync(period);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching admin dashboard summary for period: {Period}", period);
            return BadRequest(new ResponseModel<AdminDashboardSummaryDto>
            {
                Success = false,
                Message = "Error fetching dashboard summary",
                Errors = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get top-selling products
    /// </summary>
    /// <param name="limit">Number of products to retrieve (default: 10)</param>
    [HttpGet("top-products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ResponseModel<IEnumerable<TopProductDto>>>> GetTopProducts([FromQuery] int limit = 10)
    {
        try
        {
            _logger.LogInformation("Fetching top products with limit: {Limit}", limit);
            var result = await _dashboardService.GetTopProductsAsync(limit);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching top products");
            return BadRequest(new ResponseModel<IEnumerable<TopProductDto>>
            {
                Success = false,
                Message = "Error fetching top products",
                Errors = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get top-performing vendors
    /// </summary>
    /// <param name="limit">Number of vendors to retrieve (default: 10)</param>
    [HttpGet("top-vendors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ResponseModel<IEnumerable<VendorPerformanceDto>>>> GetTopVendors([FromQuery] int limit = 10)
    {
        try
        {
            _logger.LogInformation("Fetching top vendors with limit: {Limit}", limit);
            var result = await _dashboardService.GetTopVendorsAsync(limit);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching top vendors");
            return BadRequest(new ResponseModel<IEnumerable<VendorPerformanceDto>>
            {
                Success = false,
                Message = "Error fetching top vendors",
                Errors = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get system statistics for a specific date range
    /// </summary>
    /// <param name="startDate">Start date (yyyy-MM-dd)</param>
    /// <param name="endDate">End date (yyyy-MM-dd)</param>
    [HttpGet("statistics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ResponseModel<AdminDashboardSummaryDto>>> GetStatisticsForDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate >= endDate)
            {
                return BadRequest(new ResponseModel<AdminDashboardSummaryDto>
                {
                    Success = false,
                    Message = "Start date must be before end date",
                    Errors = new[] { "Invalid date range" }
                });
            }

            _logger.LogInformation("Fetching statistics for date range: {StartDate} to {EndDate}", startDate, endDate);
            var result = await _dashboardService.GetStatisticsForDateRangeAsync(startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching statistics for date range");
            return BadRequest(new ResponseModel<AdminDashboardSummaryDto>
            {
                Success = false,
                Message = "Error fetching statistics",
                Errors = new[] { ex.Message }
            });
        }
    }
}
