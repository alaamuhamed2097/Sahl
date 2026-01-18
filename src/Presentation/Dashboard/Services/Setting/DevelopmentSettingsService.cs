using Common.Enumerations.Settings;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Setting;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Services.Setting
{
    public class DevelopmentSettingsService : IDevelopmentSettingsService
    {
        private readonly IApiService _apiService;
        public DevelopmentSettingsService(IApiService apiService)
        {
            _apiService = apiService;
        }


        public async Task<ResponseModel<DevelopmentSettingsDto>> GetDevelopmentSettingsAsync()
        {
            try
            {
                return await _apiService.GetAsync<DevelopmentSettingsDto>(ApiEndpoints.DevelopmentSettings.Get);
            }
            catch (Exception ex)
            {
                return new ResponseModel<DevelopmentSettingsDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> CheckIsMultiVendorEnabledAsync()
        {
            try
            {
                return await _apiService.GetAsync<bool>(ApiEndpoints.DevelopmentSettings.IsMultiVendorEnabled);
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
    }
}