using Dashboard.Constants;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.Catalog.Unit;
using Shared.GeneralModels;

namespace Dashboard.Services.Catalog.Item
{
    public class UnitService : IUnitService
    {
        private readonly IApiService _apiService;

        public UnitService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all units with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<UnitDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<UnitDto>>($"{ApiEndpoints.Unit.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<UnitDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get unit by ID.
        /// </summary>
        public async Task<ResponseModel<UnitDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<UnitDto>($"{ApiEndpoints.Unit.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UnitDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a unit.
        /// </summary>
        public async Task<ResponseModel<UnitDto>> SaveAsync(UnitDto unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            try
            {
                return await _apiService.PostAsync<UnitDto, UnitDto>($"{ApiEndpoints.Unit.Save}", unit); ;
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UnitDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a unit by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.Unit.Delete}", id);
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
