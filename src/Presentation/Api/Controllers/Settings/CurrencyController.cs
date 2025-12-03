using Api.Controllers.Base;
using BL.Contracts.Service.Currency;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Currency;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService, Serilog.ILogger logger) :
            base(logger)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _currencyService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _currencyService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CurrencyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _currencyService.SaveAsync(dto, GuidUserId);
            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _currencyService.DeleteAsync(id, GuidUserId);
            return Ok(result);
        }

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

                var paginatedResult = new PaginatedDataModel<CurrencyDto>(pagedData, totalRecords);

                return Ok(new ResponseModel<PaginatedDataModel<CurrencyDto>>
                {
                    Success = true,
                    Data = paginatedResult
                });
            }

            return Ok(result);
        }

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