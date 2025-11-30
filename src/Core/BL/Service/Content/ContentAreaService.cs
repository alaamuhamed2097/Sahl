using BL.Contracts.IMapper;
using BL.Contracts.Service.Content;
using BL.Extensions;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Content;
using Resources;
using Serilog;
using Shared.DTOs.Content;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Content
{
    public class ContentAreaService : BaseService<TbContentArea, ContentAreaDto>, IContentAreaService
    {
        private readonly ITableRepository<TbContentArea> _contentAreaRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public ContentAreaService(
            ITableRepository<TbContentArea> contentAreaRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(contentAreaRepository, mapper)
        {
            _contentAreaRepository = contentAreaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentAreaDto>> GetAllAsync()
        {
            var areas = await _contentAreaRepository
                .GetAsync(x => x.CurrentState == 1, orderBy: q => q.OrderBy(x => x.DisplayOrder));

            return _mapper.MapList<TbContentArea, ContentAreaDto>(areas).ToList();
        }

        public async Task<IEnumerable<ContentAreaDto>> GetActiveAreasAsync()
        {
            var areas = await _contentAreaRepository
                .GetAsync(x => x.CurrentState == 1 && x.IsActive, orderBy: q => q.OrderBy(x => x.DisplayOrder));

            return _mapper.MapList<TbContentArea, ContentAreaDto>(areas).ToList();
        }

        public async Task<ContentAreaDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var area = await _contentAreaRepository.FindByIdAsync(id);
            if (area == null) return null;

            return _mapper.MapModel<TbContentArea, ContentAreaDto>(area);
        }

        public async Task<ContentAreaDto?> GetByAreaCodeAsync(string areaCode)
        {
            if (string.IsNullOrWhiteSpace(areaCode))
                throw new ArgumentNullException(nameof(areaCode));

            var area = await _contentAreaRepository
                .FindAsync(x => x.AreaCode == areaCode && x.CurrentState == 1);

            if (area == null) return null;

            return _mapper.MapModel<TbContentArea, ContentAreaDto>(area);
        }

        public async Task<PaginatedDataModel<ContentAreaDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbContentArea, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
                    x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                    x.AreaCode != null && x.AreaCode.ToLower().Contains(searchTerm)
                );
            }

            var areas = await _contentAreaRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderBy(x => x.DisplayOrder));

            var itemsDto = _mapper.MapList<TbContentArea, ContentAreaDto>(areas.Items).ToList();

            return new PaginatedDataModel<ContentAreaDto>(itemsDto, areas.TotalRecords);
        }

        public async Task<bool> SaveAsync(ContentAreaDto dto, Guid userId)
        {
            // Check for duplicate AreaCode
            var existingArea = await _contentAreaRepository
                .FindAsync(x => x.AreaCode == dto.AreaCode && x.Id != dto.Id && x.CurrentState == 1);

            if (existingArea != null)
                throw new InvalidOperationException($"Area code '{dto.AreaCode}' already exists");

            var entity = _mapper.MapModel<ContentAreaDto, TbContentArea>(dto);
            var result = await _contentAreaRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _contentAreaRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId)
        {
            var area = await _contentAreaRepository.FindByIdAsync(id);
            if (area == null)
                throw new KeyNotFoundException($"Content area with ID {id} not found");

            area.IsActive = !area.IsActive;
            var result = await _contentAreaRepository.UpdateAsync(area, userId);
            return result.Success;
        }
    }
}
