using Asp.Versioning;
using BL.Services.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Wallet;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Wallet
{
    /// <summary>
    /// Controller for Wallet and Treasury management
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(
            IWalletService walletService,
            ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all customer wallets (Admin only)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("customer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomerWallets()
        {
            try
            {
                var wallets = await _walletService.GetAllCustomerWalletsAsync();
                return Ok(new ResponseModel<List<CustomerWalletDto>>
                {
                    Success = true,
                    Data = wallets,
                    Message = "Customer wallets retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer wallets");
                return StatusCode(500, new ResponseModel<List<CustomerWalletDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving customer wallets",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer wallet by ID
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerWallet(Guid customerId)
        {
            try
            {
                var wallet = await _walletService.GetCustomerWalletAsync(customerId);
                if (wallet == null)
                    return NotFound(new ResponseModel<object> { Success = false, Message = "Customer wallet not found" });

                return Ok(new ResponseModel<CustomerWalletDto>
                {
                    Success = true,
                    Data = wallet,
                    Message = "Customer wallet retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer wallet");
                return StatusCode(500, new ResponseModel<CustomerWalletDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets my wallet (Current customer)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        [HttpGet("customer/my-wallet")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyWallet()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                    return Unauthorized(new ResponseModel<object> { Success = false, Message = "Invalid user credentials" });

                var wallet = await _walletService.GetCustomerWalletAsync(userId);
                wallet ??= await _walletService.CreateCustomerWalletAsync(userId);

                return Ok(new ResponseModel<CustomerWalletDto>
                {
                    Success = true,
                    Data = wallet,
                    Message = "Wallet retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting my wallet");
                return StatusCode(500, new ResponseModel<CustomerWalletDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets all vendor wallets (Admin only)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("vendor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVendorWallets()
        {
            try
            {
                var wallets = await _walletService.GetAllVendorWalletsAsync();
                return Ok(new ResponseModel<List<VendorWalletDto>>
                {
                    Success = true,
                    Data = wallets,
                    Message = "Vendor wallets retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vendor wallets");
                return StatusCode(500, new ResponseModel<List<VendorWalletDto>>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets wallet statistics (Admin only)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var stats = await _walletService.GetWalletStatisticsAsync();
                return Ok(new ResponseModel<WalletStatisticsDto>
                {
                    Success = true,
                    Data = stats,
                    Message = "Statistics retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wallet statistics");
                return StatusCode(500, new ResponseModel<WalletStatisticsDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
