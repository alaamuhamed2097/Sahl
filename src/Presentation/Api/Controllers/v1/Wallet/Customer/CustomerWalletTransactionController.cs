using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Wallet.Customer;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Wallet.Customer;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Wallet.Customer
{
    
    [ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Admin)}")]
    public class CustomerWalletTransactionController : BaseController
    {
        private readonly ICustomerWalletTransactionService _walletTransactionService;

        public CustomerWalletTransactionController(ICustomerWalletTransactionService walletTransactionService, Serilog.ILogger logger)
        {
            _walletTransactionService = walletTransactionService;
        }

        /// <summary>
        /// Retrieves all wallets transactions.
        /// </summary>
        [HttpGet("All")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> GetAsync()
        {
            var items = await _walletTransactionService.GetAllTransactions(GuidUserId);
            if (items == null || !items.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<IEnumerable<CustomerWalletTransactionsDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = items
            });
        }

        [HttpPost("SearchWalletTransactions")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> SearchWalletTransactionsAsync([FromQuery] BaseSearchCriteriaModel criteria, [FromBody] Guid userId)
        {
            // Validate and set default pagination values if not provided
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _walletTransactionService.GetPage(criteria, userId);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<CustomerWalletTransactionsDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<CustomerWalletTransactionsDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetByUserIdAsync()
        {
            var items = await _walletTransactionService.GetAllTransactions(GuidUserId);
            if (items == null || !items.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<IEnumerable<CustomerWalletTransactionsDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = items
            });
        }

        /// <summary>
        /// Searches ranks with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("currentUser/search")]
        public async Task<IActionResult> SearchAsync([FromQuery] BaseSearchCriteriaModel criteria)
        {
            // Validate and set default pagination values if not provided
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _walletTransactionService.GetPage(criteria, GuidUserId);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<CustomerWalletTransactionsDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<CustomerWalletTransactionsDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }
    }
}
