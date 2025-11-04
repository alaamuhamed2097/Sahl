using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Setting
{
    public interface ISettingService
    {
        Task<ResponseModel<SettingDto>> GetSettingsAsync();
        Task<ResponseModel<string>> GetMainBannerPathAsync();
        Task<ResponseModel<string>> UpdateSettingsAsync(SettingDto dto);
    }
}