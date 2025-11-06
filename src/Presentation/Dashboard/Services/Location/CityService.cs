using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Location;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Services.Location
{

    public class CityService : ICityService
    {
        private readonly IApiService _apiService;

        public CityService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all City with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CityDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CityDto>>($"{ApiEndpoints.City.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<CityDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get City by ID.
        /// </summary>
        public async Task<ResponseModel<CityDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CityDto>($"{ApiEndpoints.City.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CityDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a City.
        /// </summary>
        public async Task<ResponseModel<CityDto>> SaveAsync(CityDto City)
        {
            if (City == null) throw new ArgumentNullException(nameof(City));

            try
            {
                return await _apiService.PostAsync<CityDto, CityDto>($"{ApiEndpoints.City.Save}", City);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CityDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a City by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.City.Delete}", id);
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
            catch (Exception ex)
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
