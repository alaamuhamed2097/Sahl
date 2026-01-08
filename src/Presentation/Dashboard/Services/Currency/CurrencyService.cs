using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.Currency;
using Dashboard.Contracts.General;
using Dashboard.Models.pagintion;
using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace Dashboard.Services.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IApiService _apiService;

        public CurrencyService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CurrencyDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CurrencyDto>>(ApiEndpoints.Currency.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<CurrencyDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Gets a currency by ID
        /// </summary>
        public async Task<ResponseModel<CurrencyDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CurrencyDto>($"{ApiEndpoints.Currency.Get}/{id}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<CurrencyDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Saves a currency (create or update)
        /// </summary>
        public async Task<ResponseModel<CurrencyDto>> SaveAsync(CurrencyDto item)
        {
            try
            {
                return await _apiService.PostAsync<CurrencyDto, CurrencyDto>(ApiEndpoints.Currency.Save, item);
            }
            catch (Exception ex)
            {
                return new ResponseModel<CurrencyDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Deletes a currency
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>(ApiEndpoints.Currency.Delete, id);

                return result;
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Sets a currency as the base currency
        /// </summary>
        public async Task<ResponseModel<bool>> SetBaseCurrencyAsync(Guid currencyId)
        {
            try
            {
                return await _apiService.PostAsync<object, bool>($"{ApiEndpoints.Currency.SetBase}/{currencyId}", new { });
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Updates exchange rates for all currencies
        /// </summary>
        public async Task<ResponseModel<bool>> UpdateExchangeRatesAsync()
        {
            try
            {
                return await _apiService.PostAsync<object, bool>(ApiEndpoints.Currency.UpdateRates, new { });
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Gets the base currency
        /// </summary>
        public async Task<ResponseModel<CurrencyDto>> GetBaseCurrencyAsync()
        {
            try
            {
                return await _apiService.GetAsync<CurrencyDto>(ApiEndpoints.Currency.GetBase);
            }
            catch (Exception ex)
            {
                return new ResponseModel<CurrencyDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Gets all active currencies
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CurrencyDto>>> GetActiveCurrenciesAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CurrencyDto>>(ApiEndpoints.Currency.GetActive);
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<CurrencyDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Searches currencies with pagination
        /// </summary>
        public async Task<ResponseModel<PaginatedDataModel<CurrencyDto>>> SearchAsync(BaseSearchCriteriaModel criteria)
        {
            try
            {
                var queryString = $"?PageNumber={criteria.PageNumber}&PageSize={criteria.PageSize}";
                if (!string.IsNullOrEmpty(criteria.SearchTerm))
                {
                    queryString += $"&SearchTerm={Uri.EscapeDataString(criteria.SearchTerm)}";
                }

                return await _apiService.GetAsync<PaginatedDataModel<CurrencyDto>>($"{ApiEndpoints.Currency.Search}{queryString}");
            }
            catch (Exception ex)
            {
                return new ResponseModel<PaginatedDataModel<CurrencyDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}