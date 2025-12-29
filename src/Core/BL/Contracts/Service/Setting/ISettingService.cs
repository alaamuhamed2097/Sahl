using BL.Contracts.Service.Base;
using Domains.Entities.Setting;
using Shared.DTOs.Setting;

namespace BL.Contracts.Service.Setting;

public interface ISettingService : IBaseService<TbSetting, SettingDto>
{
    Task<SettingDto> GetSettingsAsync(string clientIp = "0", bool applyConversion = true);
    Task<string> GetMainBannerPathAsync();
    Task<decimal> GetShippingAmountAsync(string clientIp = "0", bool applyConversion = true);
    Task<bool> UpdateSettingsAsync(SettingDto dto, Guid userId);
}