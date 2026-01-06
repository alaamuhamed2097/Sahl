using BL.Contracts.IMapper;
using BL.Contracts.Service.Setting;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.Setting;
using Shared.DTOs.Setting;

namespace BL.Services.Setting;

public class SettingService : BaseService<TbGeneralSettings, GeneralSettingsDto>, ISettingService
{
    private readonly ITableRepository<TbGeneralSettings> _settingRepository;
    private readonly IBaseMapper _mapper;

    public SettingService(
        ITableRepository<TbGeneralSettings> settingRepository,
        IBaseMapper mapper) : base(settingRepository, mapper)
    {
        _settingRepository = settingRepository;
        _mapper = mapper;
    }

    public async Task<GeneralSettingsDto> GetSettingsAsync()
    {
        var setting = (await _settingRepository.GetAllAsync())
            .FirstOrDefault();

        var result = _mapper.MapModel<TbGeneralSettings, GeneralSettingsDto>(setting);

        return result;
    }

    public async Task<bool> UpdateSettingsAsync(GeneralSettingsDto dto, Guid userId)
    {
        // Update existing setting
        var mappingResult = _mapper.MapModel<GeneralSettingsDto, TbGeneralSettings>(dto);

        var result = await _settingRepository.UpdateAsync(mappingResult, userId);
        return result.Success;
    }
}