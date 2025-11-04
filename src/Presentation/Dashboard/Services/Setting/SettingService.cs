using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Setting;
using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Services.Setting
{
    public class SettingService : ISettingService
    {
        private readonly IApiService _apiService;

        public SettingService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get application settings.
        /// </summary>
        public async Task<ResponseModel<SettingDto>> GetSettingsAsync()
        {
            try
            {
                return await _apiService.GetAsync<SettingDto>(ApiEndpoints.Setting.Get);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<SettingDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<string>> GetMainBannerPathAsync()
        {
            try
            {
                return await _apiService.GetAsync<string>(ApiEndpoints.Setting.MainBanner);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Update application settings.
        /// </summary>
        public async Task<ResponseModel<string>> UpdateSettingsAsync(SettingDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _apiService.PostAsync<SettingDto, string>(ApiEndpoints.Setting.Update, dto);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}