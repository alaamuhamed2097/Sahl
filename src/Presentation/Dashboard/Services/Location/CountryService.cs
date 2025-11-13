using Dashboard.Constants;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Services.Location
{

    public class CountryService : ICountryService
    {
        private readonly IApiService _apiService;

        public CountryService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all Country with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CountryDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CountryDto>>($"{ApiEndpoints.Country.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<CountryDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get country by ID.
        /// </summary>
        public async Task<ResponseModel<CountryDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CountryDto>($"{ApiEndpoints.Country.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CountryDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a country.
        /// </summary>
        public async Task<ResponseModel<CountryDto>> SaveAsync(CountryDto country)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));

            try
            {
                return await _apiService.PostAsync<CountryDto, CountryDto>($"{ApiEndpoints.Country.Save}", country);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CountryDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a country by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.Country.Delete}", id);
                if (result.Success)
                {
                    return new ResponseModel<bool>
                    {
                        Success = true,
                        Message = result.Message
                    };
                }
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception)
            {
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }
    }
}
