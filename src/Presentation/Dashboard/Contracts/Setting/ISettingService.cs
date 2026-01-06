using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Setting
{
    public interface ISettingService
    {
        Task<ResponseModel<GeneralSettingsDto>> GetSettingsAsync();
        Task<ResponseModel<string>> GetMainBannerPathAsync();
        Task<ResponseModel<string>> UpdateSettingsAsync(GeneralSettingsDto dto);
    }
}