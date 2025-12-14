using Asp.Versioning;
using BL.Services.Loyalty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Loyalty;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Loyalty
{
    /// <summary>
    /// Controller for Loyalty and Points management
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class LoyaltyController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<LoyaltyController> _logger;

        public LoyaltyController(ILoyaltyService loyaltyService, ILogger<LoyaltyController> logger)
        {
            _loyaltyService = loyaltyService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all loyalty tiers.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("tiers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTiers()
        {
            try
            {
                var tiers = await _loyaltyService.GetAllLoyaltyTiersAsync();
                return Ok(new ResponseModel<List<LoyaltyTierDto>>
                {
                    Success = true,
                    Data = tiers,
                    Message = "Loyalty tiers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loyalty tiers");
                return StatusCode(500, new ResponseModel<List<LoyaltyTierDto>>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer loyalty by customer ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerLoyalty(Guid customerId)
        {
            try
            {
                var loyalty = await _loyaltyService.GetCustomerLoyaltyAsync(customerId);
                if (loyalty == null)
                    return NotFound(new ResponseModel<object> { Success = false, Message = "Customer loyalty not found" });

                return Ok(new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = true,
                    Data = loyalty,
                    Message = "Customer loyalty retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer loyalty");
                return StatusCode(500, new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets my loyalty information (Current customer).
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpGet("customer/my-loyalty")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyLoyalty()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                    return Unauthorized(new ResponseModel<object> { Success = false, Message = "Invalid user credentials" });

                var loyalty = await _loyaltyService.GetCustomerLoyaltyAsync(userId);
                loyalty ??= await _loyaltyService.CreateCustomerLoyaltyAsync(userId);

                return Ok(new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = true,
                    Data = loyalty,
                    Message = "Loyalty information retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting my loyalty");
                return StatusCode(500, new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer points balance.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("customer/{customerId}/balance")]
        public async Task<IActionResult> GetPointsBalance(Guid customerId)
        {
            try
            {
                var balance = await _loyaltyService.GetCustomerPointsBalanceAsync(customerId);
                return Ok(new ResponseModel<decimal>
                {
                    Success = true,
                    Data = balance,
                    Message = "Balance retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting points balance");
                return StatusCode(500, new ResponseModel<decimal>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets tier distribution (Admin only).
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("analytics/tier-distribution")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTierDistribution()
        {
            try
            {
                var distribution = await _loyaltyService.GetTierDistributionAsync();
                return Ok(new ResponseModel<Dictionary<string, int>>
                {
                    Success = true,
                    Data = distribution,
                    Message = "Tier distribution retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tier distribution");
                return StatusCode(500, new ResponseModel<Dictionary<string, int>>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
