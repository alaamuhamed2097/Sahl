using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Location;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Dashboard.Services.Location
{
    public class StateService : IStateService
    {
        private readonly IApiService _apiService;

        public StateService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all States with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<StateDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<StateDto>>($"{ApiEndpoints.State.Get}");

            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<StateDto>>
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

        /// <summary>
        /// Get State by ID.
        /// </summary>
        public async Task<ResponseModel<StateDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<StateDto>($"{ApiEndpoints.State.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<StateDto>
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

        /// <summary>
        /// Save or update a State.
        /// </summary>
        public async Task<ResponseModel<StateDto>> SaveAsync(StateDto state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            try
            {
                return await _apiService.PostAsync<StateDto, StateDto>($"{ApiEndpoints.State.Save}", state);
            }
            catch (Exception ex)
            {
                return new ResponseModel<StateDto>
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }
        /// <summary>
        /// Delete a State by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.State.Delete}", id);
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
