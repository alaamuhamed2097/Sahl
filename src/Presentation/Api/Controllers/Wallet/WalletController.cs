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

namespace Api.Controllers.Wallet
{
    /// <summary>
    /// Controller for Wallet and Treasury management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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

        #region Customer Wallet Endpoints

        /// <summary>
        /// Gets all customer wallets (Admin only)
        /// </summary>
        [HttpGet("customer")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerWalletDto>>), StatusCodes.Status200OK)]
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
        /// Gets customer wallet by customer ID
        /// </summary>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ResponseModel<CustomerWalletDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerWallet(Guid customerId)
        {
            try
            {
                var wallet = await _walletService.GetCustomerWalletAsync(customerId);
                if (wallet == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Customer wallet not found"
                    });
                }

                return Ok(new ResponseModel<CustomerWalletDto>
                {
                    Success = true,
                    Data = wallet,
                    Message = "Customer wallet retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer wallet {CustomerId}", customerId);
                return StatusCode(500, new ResponseModel<CustomerWalletDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the wallet",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets my wallet (Current customer)
        /// </summary>
        [HttpGet("customer/my-wallet")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(ResponseModel<CustomerWalletDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyWallet()
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

                var wallet = await _walletService.GetCustomerWalletAsync(userId);
                if (wallet == null)
                {
                    // Auto-create wallet if doesn't exist
                    wallet = await _walletService.CreateCustomerWalletAsync(userId);
                }

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
                    Message = "An error occurred while retrieving your wallet",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Creates a customer wallet
        /// </summary>
        [HttpPost("customer/{customerId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<CustomerWalletDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCustomerWallet(Guid customerId)
        {
            try
            {
                var wallet = await _walletService.CreateCustomerWalletAsync(customerId);
                return CreatedAtAction(
                    nameof(GetCustomerWallet),
                    new { customerId },
                    new ResponseModel<CustomerWalletDto>
                    {
                        Success = true,
                        Data = wallet,
                        Message = "Customer wallet created successfully"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer wallet");
                return StatusCode(500, new ResponseModel<CustomerWalletDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the wallet",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets customer balance
        /// </summary>
        [HttpGet("customer/{customerId}/balance")]
        [ProducesResponseType(typeof(ResponseModel<decimal>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerBalance(Guid customerId)
        {
            try
            {
                var balance = await _walletService.GetCustomerBalanceAsync(customerId);
                return Ok(new ResponseModel<decimal>
                {
                    Success = true,
                    Data = balance,
                    Message = "Balance retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer balance");
                return StatusCode(500, new ResponseModel<decimal>
                {
                    Success = false,
                    Message = "An error occurred while retrieving balance",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Vendor Wallet Endpoints

        /// <summary>
        /// Gets all vendor wallets (Admin only)
        /// </summary>
        [HttpGet("vendor")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<List<VendorWalletDto>>), StatusCodes.Status200OK)]
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
                    Message = "An error occurred while retrieving vendor wallets",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets vendor wallet by vendor ID
        /// </summary>
        [HttpGet("vendor/{vendorId}")]
        [ProducesResponseType(typeof(ResponseModel<VendorWalletDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVendorWallet(Guid vendorId)
        {
            try
            {
                var wallet = await _walletService.GetVendorWalletAsync(vendorId);
                if (wallet == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Vendor wallet not found"
                    });
                }

                return Ok(new ResponseModel<VendorWalletDto>
                {
                    Success = true,
                    Data = wallet,
                    Message = "Vendor wallet retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vendor wallet {VendorId}", vendorId);
                return StatusCode(500, new ResponseModel<VendorWalletDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the wallet",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Creates a vendor wallet
        /// </summary>
        [HttpPost("vendor/{vendorId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<VendorWalletDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateVendorWallet(Guid vendorId)
        {
            try
            {
                var wallet = await _walletService.CreateVendorWalletAsync(vendorId);
                return CreatedAtAction(
                    nameof(GetVendorWallet),
                    new { vendorId },
                    new ResponseModel<VendorWalletDto>
                    {
                        Success = true,
                        Data = wallet,
                        Message = "Vendor wallet created successfully"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vendor wallet");
                return StatusCode(500, new ResponseModel<VendorWalletDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the wallet",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Transaction Endpoints

        /// <summary>
        /// Gets wallet transactions
        /// </summary>
        [HttpGet("transactions")]
        [ProducesResponseType(typeof(ResponseModel<List<WalletTransactionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] Guid? customerWalletId,
            [FromQuery] Guid? vendorWalletId)
        {
            try
            {
                var transactions = await _walletService.GetTransactionsAsync(customerWalletId, vendorWalletId);
                return Ok(new ResponseModel<List<WalletTransactionDto>>
                {
                    Success = true,
                    Data = transactions,
                    Message = "Transactions retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transactions");
                return StatusCode(500, new ResponseModel<List<WalletTransactionDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving transactions",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Searches wallet transactions
        /// </summary>
        [HttpPost("transactions/search")]
        [ProducesResponseType(typeof(ResponseModel<List<WalletTransactionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchTransactions([FromBody] WalletTransactionSearchRequest request)
        {
            try
            {
                var transactions = await _walletService.SearchTransactionsAsync(request);
                return Ok(new ResponseModel<List<WalletTransactionDto>>
                {
                    Success = true,
                    Data = transactions,
                    Message = "Transactions retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching transactions");
                return StatusCode(500, new ResponseModel<List<WalletTransactionDto>>
                {
                    Success = false,
                    Message = "An error occurred while searching transactions",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets a transaction by ID
        /// </summary>
        [HttpGet("transactions/{id}")]
        [ProducesResponseType(typeof(ResponseModel<WalletTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            try
            {
                var transaction = await _walletService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Transaction not found"
                    });
                }

                return Ok(new ResponseModel<WalletTransactionDto>
                {
                    Success = true,
                    Data = transaction,
                    Message = "Transaction retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transaction {Id}", id);
                return StatusCode(500, new ResponseModel<WalletTransactionDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the transaction",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Approves a pending transaction (Admin only)
        /// </summary>
        [HttpPost("transactions/{id}/approve")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ApproveTransaction(Guid id)
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

                var result = await _walletService.ApproveTransactionAsync(id, userId);
                if (!result)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Transaction not found"
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Transaction approved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving transaction {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while approving the transaction",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Rejects a pending transaction (Admin only)
        /// </summary>
        [HttpPost("transactions/{id}/reject")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RejectTransaction(Guid id, [FromBody] string reason)
        {
            try
            {
                var result = await _walletService.RejectTransactionAsync(id, reason);
                if (!result)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Transaction not found"
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Transaction rejected successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting transaction {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while rejecting the transaction",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Deposit & Withdrawal Endpoints

        /// <summary>
        /// Processes a deposit request
        /// </summary>
        [HttpPost("deposit")]
        [ProducesResponseType(typeof(ResponseModel<WalletTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessDeposit([FromBody] DepositRequestDto dto)
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

                var transaction = await _walletService.ProcessDepositAsync(dto);
                return Ok(new ResponseModel<WalletTransactionDto>
                {
                    Success = true,
                    Data = transaction,
                    Message = "Deposit request submitted successfully. Pending approval."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing deposit");
                return StatusCode(500, new ResponseModel<WalletTransactionDto>
                {
                    Success = false,
                    Message = "An error occurred while processing the deposit",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Processes a withdrawal request
        /// </summary>
        [HttpPost("withdrawal")]
        [ProducesResponseType(typeof(ResponseModel<WalletTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessWithdrawal([FromBody] WithdrawalRequestDto dto)
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

                var transaction = await _walletService.ProcessWithdrawalAsync(dto);
                return Ok(new ResponseModel<WalletTransactionDto>
                {
                    Success = true,
                    Data = transaction,
                    Message = "Withdrawal request submitted successfully. Pending approval."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing withdrawal");
                return StatusCode(500, new ResponseModel<WalletTransactionDto>
                {
                    Success = false,
                    Message = "An error occurred while processing the withdrawal",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Platform Treasury Endpoints

        /// <summary>
        /// Gets platform treasury information (Admin only)
        /// </summary>
        [HttpGet("treasury")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<PlatformTreasuryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlatformTreasury()
        {
            try
            {
                var treasury = await _walletService.GetPlatformTreasuryAsync();
                return Ok(new ResponseModel<PlatformTreasuryDto>
                {
                    Success = true,
                    Data = treasury,
                    Message = "Treasury information retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting platform treasury");
                return StatusCode(500, new ResponseModel<PlatformTreasuryDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving treasury information",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Updates platform treasury (Admin only)
        /// </summary>
        [HttpPost("treasury/update")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePlatformTreasury()
        {
            try
            {
                var result = await _walletService.UpdatePlatformTreasuryAsync();
                return Ok(new ResponseModel<object>
                {
                    Success = result,
                    Message = result ? "Treasury updated successfully" : "Failed to update treasury"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating platform treasury");
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while updating treasury",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Statistics Endpoints

        /// <summary>
        /// Gets wallet statistics (Admin only)
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<WalletStatisticsDto>), StatusCodes.Status200OK)]
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
                    Message = "An error occurred while retrieving statistics",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets transaction summary for a period (Admin only)
        /// </summary>
        [HttpGet("statistics/summary")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<Dictionary<DateTime, decimal>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactionSummary(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            try
            {
                var summary = await _walletService.GetTransactionSummaryAsync(fromDate, toDate);
                return Ok(new ResponseModel<Dictionary<DateTime, decimal>>
                {
                    Success = true,
                    Data = summary,
                    Message = "Transaction summary retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transaction summary");
                return StatusCode(500, new ResponseModel<Dictionary<DateTime, decimal>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving transaction summary",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion
    }
}
