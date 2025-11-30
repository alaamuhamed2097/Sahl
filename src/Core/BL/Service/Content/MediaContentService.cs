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
    public class MediaContentService : BaseService<TbMediaContent, MediaContentDto>, IMediaContentService
    {
        private readonly ITableRepository<TbMediaContent> _mediaContentRepository;
        private readonly ITableRepository<TbContentArea> _contentAreaRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public MediaContentService(
            ITableRepository<TbMediaContent> mediaContentRepository,
            ITableRepository<TbContentArea> contentAreaRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(mediaContentRepository, mapper)
        {
            _mediaContentRepository = mediaContentRepository;
            _contentAreaRepository = contentAreaRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MediaContentDto>> GetAllAsync()
        {
            var media = await _mediaContentRepository
                .GetAsync(x => x.CurrentState == 1, orderBy: q => q.OrderBy(x => x.DisplayOrder));

            return _mapper.MapList<TbMediaContent, MediaContentDto>(media).ToList();
        }

        public async Task<IEnumerable<MediaContentDto>> GetByContentAreaIdAsync(Guid contentAreaId)
        {
            var media = await _mediaContentRepository
                .GetAsync(x => x.ContentAreaId == contentAreaId && x.CurrentState == 1,
                    orderBy: q => q.OrderBy(x => x.DisplayOrder));

            return _mapper.MapList<TbMediaContent, MediaContentDto>(media).ToList();
        }

        public async Task<IEnumerable<MediaContentDto>> GetActiveMediaByAreaCodeAsync(string areaCode)
        {
            var area = await _contentAreaRepository
                .FindAsync(x => x.AreaCode == areaCode && x.CurrentState == 1 && x.IsActive);

            if (area == null)
                return Enumerable.Empty<MediaContentDto>();

            var now = DateTime.UtcNow;
            var media = await _mediaContentRepository
                .GetAsync(x => x.ContentAreaId == area.Id
                    && x.CurrentState == 1
                    && x.IsActive
                    && (!x.StartDate.HasValue || x.StartDate.Value <= now)
                    && (!x.EndDate.HasValue || x.EndDate.Value >= now),
                    orderBy: q => q.OrderBy(x => x.DisplayOrder));

            return _mapper.MapList<TbMediaContent, MediaContentDto>(media).ToList();
        }

        public async Task<MediaContentDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var media = await _mediaContentRepository.FindByIdAsync(id);
            if (media == null) return null;

            return _mapper.MapModel<TbMediaContent, MediaContentDto>(media);
        }

        public async Task<PaginatedDataModel<MediaContentDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbMediaContent, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
                    x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                    x.MediaType != null && x.MediaType.ToLower().Contains(searchTerm)
                );
            }

            var media = await _mediaContentRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderBy(x => x.DisplayOrder));

            var itemsDto = _mapper.MapList<TbMediaContent, MediaContentDto>(media.Items).ToList();

            return new PaginatedDataModel<MediaContentDto>(itemsDto, media.TotalRecords);
        }

        public async Task<bool> SaveAsync(MediaContentDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<MediaContentDto, TbMediaContent>(dto);
            var result = await _mediaContentRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _mediaContentRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId)
        {
            var media = await _mediaContentRepository.FindByIdAsync(id);
            if (media == null)
                throw new KeyNotFoundException($"Media content with ID {id} not found");

            media.IsActive = !media.IsActive;
            var result = await _mediaContentRepository.UpdateAsync(media, userId);
            return result.Success;
        }

        public async Task<bool> UpdateDisplayOrderAsync(Guid id, int displayOrder, Guid userId)
        {
            var media = await _mediaContentRepository.FindByIdAsync(id);
            if (media == null)
                throw new KeyNotFoundException($"Media content with ID {id} not found");

            media.DisplayOrder = displayOrder;
            var result = await _mediaContentRepository.UpdateAsync(media, userId);
            return result.Success;
        }
    }
}
