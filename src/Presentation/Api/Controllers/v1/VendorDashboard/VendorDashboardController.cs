using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.VendorDashboard;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.VendorDashboard;
using Shared.GeneralModels;

namespace Api.Controllers.v1.VendorDashboard;

/// <summary>
/// Controller for Vendor Dashboard analytics and KPI metrics
/// Provides near real-time insights into vendor performance
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/vendor/dashboard")]
[Authorize(Roles = nameof(UserRole.Vendor))]
public class VendorDashboardController : BaseController
{
    private readonly IVendorDashboardService _dashboardService;
    private readonly ILogger<VendorDashboardController> _logger;

    public VendorDashboardController(
        IVendorDashboardService dashboardService,
        ILogger<VendorDashboardController> logger)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get comprehensive vendor dashboard summary with all KPIs
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns aggregated metrics including:
    /// - Daily sales
    /// - New/pending orders
    /// - Best-selling products
    /// - Latest customer reviews
    /// - Overall vendor rating
    /// 
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDashboardSummary()
    {
        try
        {
            if (GuidUserId == Guid.Empty)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }

            var summary = await _dashboardService.GetDashboardSummaryAsync(GuidUserId);

            return Ok(new ResponseModel<VendorDashboardSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = summary
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetDashboardSummary");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard summary for vendor");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving the dashboard summary"
            });
        }
    }

    /// <summary>
    /// Get daily sales metrics for current day
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns total sales, order count, and percentage change from previous day
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("daily-sales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDailySales()
    {
        try
        {
            if (GuidUserId == Guid.Empty)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }

            var dailySales = await _dashboardService.GetDailySalesAsync(GuidUserId);

            return Ok(new ResponseModel<DailySalesDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = dailySales
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetDailySales");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving daily sales metrics");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving daily sales"
            });
        }
    }

    /// <summary>
    /// Get new/pending orders count and status breakdown
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns total new orders and breakdown by status (Pending, Processing, Ready for shipment)
    /// Includes percentage change from previous period
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("new-orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNewOrders()
    {
        try
        {
            if (GuidUserId == Guid.Empty)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }

            var newOrders = await _dashboardService.GetNewOrdersAsync(GuidUserId);

            return Ok(new ResponseModel<NewOrdersDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = newOrders
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetNewOrders");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving new orders metrics");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving new orders"
            });
        }
    }

    /// <summary>
    /// Get best-selling products for the vendor
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns top products by quantity sold
    /// Parameters:
    /// - limit: Number of products to return (default: 10)
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("best-selling-products")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBestSellingProducts([FromQuery] int limit = 10)
    {
        try
        {
            if (GuidUserId == Guid.Empty)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }

            if (limit < 1 || limit > 100)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Limit must be between 1 and 100"
                });
            }

            var products = await _dashboardService.GetBestSellingProductsAsync(GuidUserId, limit);

            return Ok(new ResponseModel<IEnumerable<BestSellingProductDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = products
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetBestSellingProducts");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving best-selling products");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving best-selling products"
            });
        }
    }

    /// <summary>
    /// Get latest customer reviews for the vendor
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns latest reviews from both vendor reviews and item/product reviews
    /// Parameters:
    /// - limit: Number of reviews to return (default: 5)
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("latest-reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLatestReviews([FromQuery] int limit = 5)
    {
        try
        {
            if (GuidUserId == Guid.Empty)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid user ID"
                });
            }

            if (limit < 1 || limit > 50)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Limit must be between 1 and 50"
                });
            }

            var reviews = await _dashboardService.GetLatestReviewsAsync(GuidUserId, limit);

            return Ok(new ResponseModel<IEnumerable<LatestReviewDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = reviews
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetLatestReviews");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest reviews");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving latest reviews"
            });
        }
    }
}
