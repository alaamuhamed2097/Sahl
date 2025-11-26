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

namespace Api.Controllers.Loyalty
{
    /// <summary>
    /// Controller for Loyalty and Points management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoyaltyController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;
        private readonly ILogger<LoyaltyController> _logger;

        public LoyaltyController(
            ILoyaltyService loyaltyService,
            ILogger<LoyaltyController> logger)
        {
            _loyaltyService = loyaltyService;
            _logger = logger;
        }

        #region Loyalty Tier Endpoints

        /// <summary>
        /// Gets all loyalty tiers
        /// </summary>
        [HttpGet("tiers")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<LoyaltyTierDto>>), StatusCodes.Status200OK)]
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
                    Message = "An error occurred while retrieving loyalty tiers",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets active loyalty tiers
        /// </summary>
        [HttpGet("tiers/active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<LoyaltyTierDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveTiers()
        {
            try
            {
                var tiers = await _loyaltyService.GetActiveLoyaltyTiersAsync();
                return Ok(new ResponseModel<List<LoyaltyTierDto>>
                {
                    Success = true,
                    Data = tiers,
                    Message = "Active loyalty tiers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active loyalty tiers");
                return StatusCode(500, new ResponseModel<List<LoyaltyTierDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving active tiers",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets a loyalty tier by ID
        /// </summary>
        [HttpGet("tiers/{id}")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyTierDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTierById(Guid id)
        {
            try
            {
                var tier = await _loyaltyService.GetLoyaltyTierByIdAsync(id);
                if (tier == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Loyalty tier not found"
                    });
                }

                return Ok(new ResponseModel<LoyaltyTierDto>
                {
                    Success = true,
                    Data = tier,
                    Message = "Loyalty tier retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loyalty tier {Id}", id);
                return StatusCode(500, new ResponseModel<LoyaltyTierDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Creates a new loyalty tier (Admin only)
        /// </summary>
        [HttpPost("tiers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyTierDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTier([FromBody] LoyaltyTierCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var tier = await _loyaltyService.CreateLoyaltyTierAsync(dto);
                return CreatedAtAction(
                    nameof(GetTierById),
                    new { id = tier.Id },
                    new ResponseModel<LoyaltyTierDto>
                    {
                        Success = true,
                        Data = tier,
                        Message = "Loyalty tier created successfully"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating loyalty tier");
                return StatusCode(500, new ResponseModel<LoyaltyTierDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Updates a loyalty tier (Admin only)
        /// </summary>
        [HttpPut("tiers/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyTierDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTier(Guid id, [FromBody] LoyaltyTierUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "ID mismatch"
                    });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var tier = await _loyaltyService.UpdateLoyaltyTierAsync(dto);
                return Ok(new ResponseModel<LoyaltyTierDto>
                {
                    Success = true,
                    Data = tier,
                    Message = "Loyalty tier updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating loyalty tier {Id}", id);
                return StatusCode(500, new ResponseModel<LoyaltyTierDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Deletes a loyalty tier (Admin only)
        /// </summary>
        [HttpDelete("tiers/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTier(Guid id)
        {
            try
            {
                var result = await _loyaltyService.DeleteLoyaltyTierAsync(id);
                if (!result)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Loyalty tier not found"
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Loyalty tier deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting loyalty tier {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Activates a loyalty tier (Admin only)
        /// </summary>
        [HttpPost("tiers/{id}/activate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateTier(Guid id)
        {
            try
            {
                var result = await _loyaltyService.ActivateLoyaltyTierAsync(id);
                if (!result)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Loyalty tier not found"
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Loyalty tier activated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating loyalty tier {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while activating the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Deactivates a loyalty tier (Admin only)
        /// </summary>
        [HttpPost("tiers/{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateTier(Guid id)
        {
            try
            {
                var result = await _loyaltyService.DeactivateLoyaltyTierAsync(id);
                if (!result)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Loyalty tier not found"
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Loyalty tier deactivated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating loyalty tier {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deactivating the tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Customer Loyalty Endpoints

        /// <summary>
        /// Gets customer loyalty by customer ID
        /// </summary>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ResponseModel<CustomerLoyaltyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerLoyalty(Guid customerId)
        {
            try
            {
                var loyalty = await _loyaltyService.GetCustomerLoyaltyAsync(customerId);
                if (loyalty == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Customer loyalty not found"
                    });
                }

                return Ok(new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = true,
                    Data = loyalty,
                    Message = "Customer loyalty retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer loyalty {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<CustomerLoyaltyDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving customer loyalty",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets my loyalty information (Current customer)
        /// </summary>
        [HttpGet("customer/my-loyalty")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(ResponseModel<CustomerLoyaltyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyLoyalty()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Unauthorized(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Invalid user credentials"
                    });
                }

                var loyalty = await _loyaltyService.GetCustomerLoyaltyAsync(userId);
                if (loyalty == null)
                {
                    // Auto-create loyalty if doesn't exist
                    loyalty = await _loyaltyService.CreateCustomerLoyaltyAsync(userId);
                }

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
                    Message = "An error occurred while retrieving your loyalty information",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customers by tier (Admin only)
        /// </summary>
        [HttpGet("tiers/{tierId}/customers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerLoyaltyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomersByTier(Guid tierId)
        {
            try
            {
                var customers = await _loyaltyService.GetCustomerLoyaltiesByTierAsync(tierId);
                return Ok(new ResponseModel<List<CustomerLoyaltyDto>>
                {
                    Success = true,
                    Data = customers,
                    Message = "Customers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers by tier {TierId}", tierId);
                return StatusCode(500, new ResponseModel<List<CustomerLoyaltyDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving customers",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets next tier for customer
        /// </summary>
        [HttpGet("customer/{customerId}/next-tier")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyTierDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNextTier(Guid customerId)
        {
            try
            {
                var nextTier = await _loyaltyService.GetNextTierForCustomerAsync(customerId);
                return Ok(new ResponseModel<LoyaltyTierDto>
                {
                    Success = true,
                    Data = nextTier,
                    Message = nextTier != null ? "Next tier retrieved successfully" : "Customer is at the highest tier"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next tier for customer {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<LoyaltyTierDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving next tier",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets points to next tier
        /// </summary>
        [HttpGet("customer/{customerId}/points-to-next-tier")]
        [ProducesResponseType(typeof(ResponseModel<decimal>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPointsToNextTier(Guid customerId)
        {
            try
            {
                var points = await _loyaltyService.CalculatePointsToNextTierAsync(customerId);
                return Ok(new ResponseModel<decimal>
                {
                    Success = true,
                    Data = points,
                    Message = "Points calculated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating points to next tier for customer {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<decimal>
                {
                    Success = false,
                    Message = "An error occurred while calculating points",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Points Management Endpoints

        /// <summary>
        /// Adds points to customer (Admin only)
        /// </summary>
        [HttpPost("points/add")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyPointsTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPoints([FromBody] LoyaltyPointsTransactionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var transaction = await _loyaltyService.AddPointsAsync(dto);
                return Ok(new ResponseModel<LoyaltyPointsTransactionDto>
                {
                    Success = true,
                    Data = transaction,
                    Message = "Points added successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding points");
                return StatusCode(500, new ResponseModel<LoyaltyPointsTransactionDto>
                {
                    Success = false,
                    Message = "An error occurred while adding points",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Redeems points (Customer)
        /// </summary>
        [HttpPost("points/redeem")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(ResponseModel<LoyaltyPointsTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RedeemPoints([FromBody] RedeemPointsRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return Unauthorized(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Invalid user credentials"
                    });
                }

                var transaction = await _loyaltyService.RedeemPointsAsync(userId, dto.Points, dto.Description);
                return Ok(new ResponseModel<LoyaltyPointsTransactionDto>
                {
                    Success = true,
                    Data = transaction,
                    Message = "Points redeemed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error redeeming points");
                return StatusCode(500, new ResponseModel<LoyaltyPointsTransactionDto>
                {
                    Success = false,
                    Message = "An error occurred while redeeming points",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer points balance
        /// </summary>
        [HttpGet("customer/{customerId}/balance")]
        [ProducesResponseType(typeof(ResponseModel<decimal>), StatusCodes.Status200OK)]
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
                _logger.LogError(ex, "Error getting points balance for customer {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<decimal>
                {
                    Success = false,
                    Message = "An error occurred while retrieving balance",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer transactions
        /// </summary>
        [HttpGet("customer/{customerId}/transactions")]
        [ProducesResponseType(typeof(ResponseModel<List<LoyaltyPointsTransactionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerTransactions(
            Guid customerId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var transactions = await _loyaltyService.GetCustomerTransactionsAsync(customerId, pageNumber, pageSize);
                return Ok(new ResponseModel<List<LoyaltyPointsTransactionDto>>
                {
                    Success = true,
                    Data = transactions,
                    Message = "Transactions retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions for customer {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<List<LoyaltyPointsTransactionDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving transactions",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Searches loyalty transactions (Admin only)
        /// </summary>
        [HttpPost("transactions/search")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<List<LoyaltyPointsTransactionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchTransactions([FromBody] LoyaltyPointsTransactionSearchRequest request)
        {
            try
            {
                var transactions = await _loyaltyService.SearchTransactionsAsync(request);
                return Ok(new ResponseModel<List<LoyaltyPointsTransactionDto>>
                {
                    Success = true,
                    Data = transactions,
                    Message = "Transactions retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching transactions");
                return StatusCode(500, new ResponseModel<List<LoyaltyPointsTransactionDto>>
                {
                    Success = false,
                    Message = "An error occurred while searching transactions",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Analytics Endpoints

        /// <summary>
        /// Gets tier distribution (Admin only)
        /// </summary>
        [HttpGet("analytics/tier-distribution")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<Dictionary<string, int>>), StatusCodes.Status200OK)]
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
                    Message = "An error occurred while retrieving tier distribution",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets points activity report (Admin only)
        /// </summary>
        [HttpGet("analytics/points-activity")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<Dictionary<DateTime, decimal>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPointsActivity(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            try
            {
                var activity = await _loyaltyService.GetPointsActivityReportAsync(fromDate, toDate);
                return Ok(new ResponseModel<Dictionary<DateTime, decimal>>
                {
                    Success = true,
                    Data = activity,
                    Message = "Points activity retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting points activity");
                return StatusCode(500, new ResponseModel<Dictionary<DateTime, decimal>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving points activity",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets top loyalty customers (Admin only)
        /// </summary>
        [HttpGet("analytics/top-customers")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerLoyaltyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopCustomers([FromQuery] int count = 10)
        {
            try
            {
                var customers = await _loyaltyService.GetTopLoyaltyCustomersAsync(count);
                return Ok(new ResponseModel<List<CustomerLoyaltyDto>>
                {
                    Success = true,
                    Data = customers,
                    Message = "Top customers retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top customers");
                return StatusCode(500, new ResponseModel<List<CustomerLoyaltyDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving top customers",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion
    }

    // Helper DTO for redeem points request
    public class RedeemPointsRequestDto
    {
        public decimal Points { get; set; }
        public string Description { get; set; }
    }
}
