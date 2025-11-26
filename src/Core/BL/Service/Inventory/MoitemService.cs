using BL.Contracts.IMapper;
using BL.Contracts.Service.Inventory;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Inventory;
using Resources;
using Serilog;
using Shared.DTOs.Inventory;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Inventory
{
    public class MoitemService : BaseService<TbMoitem, MoitemDto>, IMoitemService
    {
        private readonly ITableRepository<TbMoitem> _moitemRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public MoitemService(
            ITableRepository<TbMoitem> moitemRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(moitemRepository, mapper)
        {
            _moitemRepository = moitemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MoitemDto>> GetAllAsync()
        {
            var moitems = await _moitemRepository
                .GetAsync(x => x.CurrentState == 1, orderBy: q => q.OrderByDescending(x => x.DocumentDate));

            return _mapper.MapList<TbMoitem, MoitemDto>(moitems).ToList();
        }

        public async Task<MoitemDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var moitem = await _moitemRepository.FindByIdAsync(id);
            if (moitem == null) return null;

            return _mapper.MapModel<TbMoitem, MoitemDto>(moitem);
        }

        public async Task<MoitemDto?> GetByDocumentNumberAsync(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentNullException(nameof(documentNumber));

            var moitem = _moitemRepository
                .Get(x => x.DocumentNumber == documentNumber && x.CurrentState == 1)
                .FirstOrDefault();

            if (moitem == null) return null;

            return _mapper.MapModel<TbMoitem, MoitemDto>(moitem);
        }

        public async Task<PaginatedDataModel<MoitemDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbMoitem, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.DocumentNumber != null && x.DocumentNumber.ToLower().Contains(searchTerm) ||
                    x.MovementType != null && x.MovementType.ToLower().Contains(searchTerm) ||
                    x.Notes != null && x.Notes.ToLower().Contains(searchTerm)
                );
            }

            var moitems = await _moitemRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.DocumentDate));

            var itemsDto = _mapper.MapList<TbMoitem, MoitemDto>(moitems.Items).ToList();

            return new PaginatedDataModel<MoitemDto>(itemsDto, moitems.TotalRecords);
        }

        public async Task<bool> SaveAsync(MoitemDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<MoitemDto, TbMoitem>(dto);
            var result = await _moitemRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _moitemRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<string> GenerateDocumentNumberAsync()
        {
            var lastDocument = _moitemRepository
                .Get(x => x.CurrentState == 1)
                .OrderByDescending(x => x.CreatedDateUtc)
                .FirstOrDefault();

            if (lastDocument == null)
                return $"MOI-{DateTime.UtcNow:yyyyMMdd}-0001";

            var lastNumber = lastDocument.DocumentNumber.Split('-').LastOrDefault();
            if (int.TryParse(lastNumber, out int number))
            {
                return $"MOI-{DateTime.UtcNow:yyyyMMdd}-{(number + 1):D4}";
            }

            return $"MOI-{DateTime.UtcNow:yyyyMMdd}-0001";
        }
    }
}
