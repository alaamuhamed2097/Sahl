using BL.Contracts.IMapper;
using BL.Contracts.Service.Pricing;
using DAL.Contracts.Repositories;
using Domains.Entities.Pricing;
using Shared.DTOs.Pricing;

namespace BL.Services.Setting.Pricing;

public class PricingSettingsService : IPricingSettingsService
{
    private readonly ITableRepository<TbPricingSystemSetting> _repo;
    private readonly IBaseMapper _mapper;

    public PricingSettingsService(ITableRepository<TbPricingSystemSetting> repo, IBaseMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PricingSystemSettingDto>> GetAllSystemsAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(i => _mapper.MapModel<TbPricingSystemSetting, PricingSystemSettingDto>(i));
    }
}
