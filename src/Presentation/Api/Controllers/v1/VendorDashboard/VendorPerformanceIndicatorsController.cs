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
/// Controller for Vendor Performance Indicators (KPIs)
/// Provides detailed analytics on vendor performance metrics
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/vendor/performance-indicators")]
[Authorize(Roles = nameof(UserRole.Vendor))]
public class VendorPerformanceIndicatorsController : BaseController
{
    private readonly IVendorPerformanceIndicatorsService _performanceService;
    private readonly ILogger<VendorPerformanceIndicatorsController> _logger;

    public VendorPerformanceIndicatorsController(
        IVendorPerformanceIndicatorsService performanceService,
        ILogger<VendorPerformanceIndicatorsController> logger)
    {
        _performanceService = performanceService ?? throw new ArgumentNullException(nameof(performanceService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all performance indicators for vendor
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns comprehensive KPI metrics including:
    /// - Conversion Rate
    /// - Average Order Value
    /// - Return Rate
    /// - Order Preparation Time
    /// - Buy Box Win Rate
    /// - Overall Health Score
    /// 
    /// Requires Vendor role
    /// </remarks>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPerformanceIndicators([FromQuery] string period = "CurrentMonth")
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

            if (!IsValidPeriod(period))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid period. Valid values: CurrentMonth, CurrentWeek, CurrentYear, Last30Days"
                });
            }

            var indicators = await _performanceService.GetAllPerformanceIndicatorsAsync(GuidUserId, period);

            return Ok(new ResponseModel<VendorPerformanceIndicatorsDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = indicators
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument in GetAllPerformanceIndicators");
            return BadRequest(new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all performance indicators");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving performance indicators"
            });
        }
    }

    /// <summary>
    /// Get Conversion Rate metric
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Measures product views to orders conversion
    /// Helps identify how many visitors become customers
    /// </remarks>
    [HttpGet("conversion-rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetConversionRate([FromQuery] string period = "CurrentMonth")
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

            var conversionRate = await _performanceService.GetConversionRateAsync(GuidUserId, period);

            return Ok(new ResponseModel<ConversionRateDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = conversionRate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conversion rate");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving conversion rate"
            });
        }
    }

    /// <summary>
    /// Get Average Order Value metric
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Measures average spending per order
    /// Includes min/max order values for analysis
    /// </remarks>
    [HttpGet("average-order-value")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAverageOrderValue([FromQuery] string period = "CurrentMonth")
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

            var aov = await _performanceService.GetAverageOrderValueAsync(GuidUserId, period);

            return Ok(new ResponseModel<AverageOrderValueDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = aov
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving average order value");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving average order value"
            });
        }
    }

    /// <summary>
    /// Get Return Rate metric
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Measures percentage of returned orders
    /// Includes refund request and processing information
    /// </remarks>
    [HttpGet("return-rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReturnRate([FromQuery] string period = "CurrentMonth")
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

            var returnRate = await _performanceService.GetReturnRateAsync(GuidUserId, period);

            return Ok(new ResponseModel<ReturnRateDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = returnRate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving return rate");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving return rate"
            });
        }
    }

    /// <summary>
    /// Get Order Preparation Time metric
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Measures time taken to prepare orders
    /// Includes SLA compliance percentage
    /// </remarks>
    [HttpGet("preparation-time")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderPreparationTime([FromQuery] string period = "CurrentMonth")
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

            var prepTime = await _performanceService.GetOrderPreparationTimeAsync(GuidUserId, period);

            return Ok(new ResponseModel<OrderPreparationTimeDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = prepTime
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order preparation time");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving order preparation time"
            });
        }
    }

    /// <summary>
    /// Get Buy Box Win Rate metric
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Measures percentage of products where vendor won the buy box
    /// Includes competition analysis and top products
    /// </remarks>
    [HttpGet("buybox-win-rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBuyBoxWinRate([FromQuery] int topProducts = 10, [FromQuery] string period = "CurrentMonth")
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

            if (topProducts < 1 || topProducts > 50)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "topProducts must be between 1 and 50"
                });
            }

            var buyBoxRate = await _performanceService.GetBuyBoxWinRateAsync(GuidUserId, topProducts, period);

            return Ok(new ResponseModel<BuyBoxWinRateDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = buyBoxRate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving buy box win rate");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving buy box win rate"
            });
        }
    }

    /// <summary>
    /// Get KPI trend over time
    /// </summary>
    /// <remarks>
    /// API Version: 1.0+
    /// Returns historical trend data for specific KPI
    /// Parameters:
    /// - kpiType: ConversionRate, AOV, ReturnRate, PreparationTime, BuyBoxWinRate
    /// - months: Number of months to retrieve (1-24)
    /// </remarks>
    [HttpGet("trend/{kpiType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetKPITrend(string kpiType, [FromQuery] int months = 6)
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

            if (!IsValidKPIType(kpiType))
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid KPI type. Valid values: ConversionRate, AOV, ReturnRate, PreparationTime, BuyBoxWinRate"
                });
            }

            if (months < 1 || months > 24)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Months must be between 1 and 24"
                });
            }

            var trend = await _performanceService.GetKPITrendAsync(GuidUserId, kpiType, months);

            return Ok(new ResponseModel<KPITrendDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = trend
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving KPI trend for {KPIType}", kpiType);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
            {
                Success = false,
                Message = "An error occurred while retrieving KPI trend"
            });
        }
    }

    // Helper methods
    private bool IsValidPeriod(string period)
    {
        return period switch
        {
            "CurrentMonth" or "CurrentWeek" or "CurrentYear" or "Last30Days" => true,
            _ => false
        };
    }

    private bool IsValidKPIType(string kpiType)
    {
        return kpiType switch
        {
            "ConversionRate" or "AOV" or "ReturnRate" or "PreparationTime" or "BuyBoxWinRate" => true,
            _ => false
        };
    }
}
