using BL.Contracts.Service.Base;
using Domains.Entities.Setting;
using Shared.DTOs.Setting;

namespace BL.Contracts.Service.Setting;

public interface ISettingService : IBaseService<TbGeneralSettings, GeneralSettingsDto>
{
    Task<GeneralSettingsDto> GetSettingsAsync();
    Task<bool> UpdateSettingsAsync(GeneralSettingsDto dto, Guid userId);
}