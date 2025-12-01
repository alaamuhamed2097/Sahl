using BL.Contracts.Service.Pricing;
using DAL.Contracts.UnitOfWork;
using DAL.Contracts.Repositories;
using Domains.Entities.Pricing;
using Shared.DTOs.Pricing;
using BL.Contracts.IMapper;

namespace BL.Service.Pricing
{
    public class PricingService : IPricingService
    {
        private readonly ITableRepository<TbPricingSystemSetting> _repo;
        private readonly IBaseMapper _mapper;

        public PricingService(ITableRepository<TbPricingSystemSetting> repo, IBaseMapper mapper)
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
}
