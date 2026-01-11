using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Currency;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Settings
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Get all currencies.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _currencyService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get currency by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _currencyService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Save currency.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CurrencyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _currencyService.SaveAsync(dto, GuidUserId);
            return Ok(result);
        }

        /// <summary>
        /// Delete currency.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _currencyService.DeleteAsync(id, GuidUserId);
            return Ok(result);
        }

        /// <summary>
        /// Search currencies with pagination.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            var result = await _currencyService.GetAllAsync();
            if (result.Success)
            {
                var filteredData = result.Data ?? new List<CurrencyDto>();

                if (!string.IsNullOrEmpty(criteria.SearchTerm))
                {
                    filteredData = filteredData.Where(c =>
                        c.Code.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        c.NameEn.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        c.NameAr.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                var totalRecords = filteredData.Count();
                var pagedData = filteredData
                    .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                    .Take(criteria.PageSize)
                    .ToList();

                var paginatedResult = new PagedResult<CurrencyDto>(pagedData, totalRecords);

                return Ok(new ResponseModel<PagedResult<CurrencyDto>>
                {
                    Success = true,
                    Data = paginatedResult
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Set base currency.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("set-base/{id}")]
        public async Task<IActionResult> SetBaseCurrency(Guid id)
        {
            var result = await _currencyService.SetBaseCurrencyAsync(id, GuidUserId);
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Base currency updated successfully" : "Failed to update base currency"
            });
        }

        /// <summary>
        /// Update exchange rates.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("update-rates")]
        public async Task<IActionResult> UpdateExchangeRates()
        {
            var result = await _currencyService.UpdateExchangeRatesAsync(GuidUserId);
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Exchange rates updated successfully" : "Failed to update exchange rates"
            });
        }

        /// <summary>
        /// Get base currency.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("base")]
        public async Task<IActionResult> GetBaseCurrency()
        {
            var result = await _currencyService.GetBaseCurrencyAsync();
            return Ok(new ResponseModel<CurrencyDto>
            {
                Success = result != null,
                Data = result,
                Message = result != null ? "Base currency retrieved successfully" : "No base currency found"
            });
        }

        /// <summary>
        /// Get active currencies.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCurrencies()
        {
            var result = await _currencyService.GetActiveCurrenciesAsync();
            return Ok(new ResponseModel<IEnumerable<CurrencyDto>>
            {
                Success = true,
                Data = result,
                Message = "Active currencies retrieved successfully"
            });
        }
    }
}
