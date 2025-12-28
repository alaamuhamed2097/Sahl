using Shared.DTOs.Setting;

namespace BL.Contracts.Service.Setting;

public interface ISettingService
{
    Task<SettingDto> GetSettingsAsync(string clientIp = "0", bool applyConversion = true);
    Task<string> GetMainBannerPathAsync();
    Task<decimal> GetShippingAmountAsync(string clientIp = "0", bool applyConversion = true);
    Task<bool> UpdateSettingsAsync(SettingDto dto, Guid userId);
}