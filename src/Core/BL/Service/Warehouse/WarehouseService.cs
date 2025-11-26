using BL.Contracts.IMapper;
using BL.Contracts.Service.Warehouse;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Warehouse;
using Resources;
using Serilog;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Warehouse
{
    public class WarehouseService : BaseService<TbWarehouse, WarehouseDto>, IWarehouseService
    {
        private readonly ITableRepository<TbWarehouse> _warehouseRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public WarehouseService(
            ITableRepository<TbWarehouse> warehouseRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(warehouseRepository, mapper)
        {
            _warehouseRepository = warehouseRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllAsync()
        {
            var warehouses = await _warehouseRepository
                .GetAsync(x => x.CurrentState == 1);

            return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses).ToList();
        }

        public async Task<IEnumerable<WarehouseDto>> GetActiveWarehousesAsync()
        {
            var warehouses = await _warehouseRepository
                .GetAsync(x => x.CurrentState == 1 && x.IsActive);

            return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses).ToList();
        }

        public async Task<WarehouseDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var warehouse = await _warehouseRepository.FindByIdAsync(id);
            if (warehouse == null) return null;

            return _mapper.MapModel<TbWarehouse, WarehouseDto>(warehouse);
        }

        public async Task<PaginatedDataModel<WarehouseDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbWarehouse, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
                    x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                    x.Address != null && x.Address.ToLower().Contains(searchTerm)
                );
            }

            var warehouses = await _warehouseRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc));

            var itemsDto = _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses.Items).ToList();

            return new PaginatedDataModel<WarehouseDto>(itemsDto, warehouses.TotalRecords);
        }

        public async Task<bool> SaveAsync(WarehouseDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<WarehouseDto, TbWarehouse>(dto);
            var result = await _warehouseRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _warehouseRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId)
        {
            var warehouse = await _warehouseRepository.FindByIdAsync(id);
            if (warehouse == null)
                throw new KeyNotFoundException($"Warehouse with ID {id} not found");

            warehouse.IsActive = !warehouse.IsActive;
            var result = await _warehouseRepository.UpdateAsync(warehouse, userId);
            return result.Success;
        }
    }
}
