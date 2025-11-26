using BL.Contracts.IMapper;
using BL.Contracts.Service.Inventory;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using Domains.Entities.Inventory;
using Serilog;
using Shared.DTOs.Inventory;

namespace BL.Service.Inventory
{
    public class MovitemsdetailService : BaseService<TbMovitemsdetail, MovitemsdetailDto>, IMovitemsdetailService
    {
        private readonly ITableRepository<TbMovitemsdetail> _movitemsdetailRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public MovitemsdetailService(
            ITableRepository<TbMovitemsdetail> movitemsdetailRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(movitemsdetailRepository, mapper)
        {
            _movitemsdetailRepository = movitemsdetailRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovitemsdetailDto>> GetByMoitemIdAsync(Guid moitemId)
        {
            var details = await _movitemsdetailRepository
                .GetAsync(x => x.MoitemId == moitemId && x.CurrentState == 1);

            return _mapper.MapList<TbMovitemsdetail, MovitemsdetailDto>(details).ToList();
        }

        public async Task<IEnumerable<MovitemsdetailDto>> GetByMortemIdAsync(Guid mortemId)
        {
            var details = await _movitemsdetailRepository
                .GetAsync(x => x.MortemId == mortemId && x.CurrentState == 1);

            return _mapper.MapList<TbMovitemsdetail, MovitemsdetailDto>(details).ToList();
        }

        public async Task<IEnumerable<MovitemsdetailDto>> GetByWarehouseIdAsync(Guid warehouseId)
        {
            var details = await _movitemsdetailRepository
                .GetAsync(x => x.WarehouseId == warehouseId && x.CurrentState == 1);

            return _mapper.MapList<TbMovitemsdetail, MovitemsdetailDto>(details).ToList();
        }

        public async Task<MovitemsdetailDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var detail = await _movitemsdetailRepository.FindByIdAsync(id);
            if (detail == null) return null;

            return _mapper.MapModel<TbMovitemsdetail, MovitemsdetailDto>(detail);
        }

        public async Task<bool> SaveAsync(MovitemsdetailDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<MovitemsdetailDto, TbMovitemsdetail>(dto);
            entity.TotalPrice = entity.Quantity * entity.UnitPrice;
            var result = await _movitemsdetailRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> SaveRangeAsync(IEnumerable<MovitemsdetailDto> dtos, Guid userId)
        {
            var entities = _mapper.MapList<MovitemsdetailDto, TbMovitemsdetail>(dtos).ToList();
            
            foreach (var entity in entities)
            {
                entity.TotalPrice = entity.Quantity * entity.UnitPrice;
            }

            return await _movitemsdetailRepository.AddRangeAsync(entities, userId);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _movitemsdetailRepository.UpdateCurrentStateAsync(id, userId, 0);
        }
    }
}
