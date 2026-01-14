using Shared.DTOs.Setting;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Setting
{
    public interface IDevelopmentSettingsService
    {
        Task<ResponseModel<bool>> CheckIsMultiVendorEnabledAsync();
        Task<ResponseModel<DevelopmentSettingsDto>> GetDevelopmentSettingsAsync();
    }
}