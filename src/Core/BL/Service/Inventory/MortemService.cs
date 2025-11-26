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
    public class MortemService : BaseService<TbMortem, MortemDto>, IMortemService
    {
        private readonly ITableRepository<TbMortem> _mortemRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public MortemService(
            ITableRepository<TbMortem> mortemRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(mortemRepository, mapper)
        {
            _mortemRepository = mortemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MortemDto>> GetAllAsync()
        {
            var mortems = await _mortemRepository
                .GetAsync(x => x.CurrentState == 1, orderBy: q => q.OrderByDescending(x => x.DocumentDate));

            return _mapper.MapList<TbMortem, MortemDto>(mortems).ToList();
        }

        public async Task<MortemDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var mortem = await _mortemRepository.FindByIdAsync(id);
            if (mortem == null) return null;

            return _mapper.MapModel<TbMortem, MortemDto>(mortem);
        }

        public async Task<MortemDto?> GetByDocumentNumberAsync(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentNullException(nameof(documentNumber));

            var mortem = _mortemRepository
                .Get(x => x.DocumentNumber == documentNumber && x.CurrentState == 1)
                .FirstOrDefault();

            if (mortem == null) return null;

            return _mapper.MapModel<TbMortem, MortemDto>(mortem);
        }

        public async Task<PaginatedDataModel<MortemDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbMortem, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.DocumentNumber != null && x.DocumentNumber.ToLower().Contains(searchTerm) ||
                    x.Reason != null && x.Reason.ToLower().Contains(searchTerm)
                );
            }

            var mortems = await _mortemRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.DocumentDate));

            var itemsDto = _mapper.MapList<TbMortem, MortemDto>(mortems.Items).ToList();

            return new PaginatedDataModel<MortemDto>(itemsDto, mortems.TotalRecords);
        }

        public async Task<bool> SaveAsync(MortemDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<MortemDto, TbMortem>(dto);
            var result = await _mortemRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _mortemRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, int status, Guid userId)
        {
            var mortem = await _mortemRepository.FindByIdAsync(id);
            if (mortem == null)
                throw new KeyNotFoundException($"Return document with ID {id} not found");

            mortem.Status = status;
            var result = await _mortemRepository.UpdateAsync(mortem, userId);
            return result.Success;
        }

        public async Task<string> GenerateDocumentNumberAsync()
        {
            var lastDocument = _mortemRepository
                .Get(x => x.CurrentState == 1)
                .OrderByDescending(x => x.CreatedDateUtc)
                .FirstOrDefault();

            if (lastDocument == null)
                return $"MOR-{DateTime.UtcNow:yyyyMMdd}-0001";

            var lastNumber = lastDocument.DocumentNumber.Split('-').LastOrDefault();
            if (int.TryParse(lastNumber, out int number))
            {
                return $"MOR-{DateTime.UtcNow:yyyyMMdd}-{(number + 1):D4}";
            }

            return $"MOR-{DateTime.UtcNow:yyyyMMdd}-0001";
        }
    }
}
