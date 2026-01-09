using BL.Contracts.IMapper;
using BL.Contracts.Services.Page;
using BL.Extensions;
using Common.Enumerations;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Page;
using Resources;
using Shared.DTOs.Page;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.Page
{
    public class PageService : IPageService
    {
        private readonly ITableRepository<TbPage> _pageRepository;
        private readonly IBaseMapper _mapper;

        public PageService(
            ITableRepository<TbPage> pageRepository,
            IBaseMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PageDto>> GetAllAsync()
        {
            var pages = await _pageRepository.GetAllAsync();

            return _mapper.MapList<TbPage, PageDto>(pages);
        }

        public async Task<PageDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var page = await _pageRepository
                .FindAsync(x => x.Id == id && !x.IsDeleted );

            return _mapper.MapModel<TbPage, PageDto>(page);
        }

        public async Task<PageDto> GetByTypeAsync(PageType pageType)
        {
            var page = await _pageRepository
                .FindAsync(x => x.PageType == pageType && !x.IsDeleted);

            return _mapper.MapModel<TbPage, PageDto>(page);
        }

        public async Task<PagedResult<PageDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<TbPage, bool>> filter = x => !x.IsDeleted;

            // Search term filter
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
                    (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                    (x.ContentEn != null && x.ContentEn.ToLower().Contains(searchTerm)) ||
                    (x.ContentAr != null && x.ContentAr.ToLower().Contains(searchTerm)) ||
                    (x.ShortDescriptionEn != null && x.ShortDescriptionEn.ToLower().Contains(searchTerm)) ||
                    (x.ShortDescriptionAr != null && x.ShortDescriptionAr.ToLower().Contains(searchTerm))
                );
            }

            var pages = await _pageRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter);

            var itemsDto = _mapper.MapList<TbPage, PageDto>(pages.Items);

            return new PagedResult<PageDto>(itemsDto, pages.TotalRecords);
        }

        public async Task<bool> SaveAsync(PageDto dto, Guid userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty)
                throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            var page = _mapper.MapModel<PageDto, TbPage>(dto);

            var result = await _pageRepository.SaveAsync(page, userId);

            return result.Success;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var success = await _pageRepository
                .UpdateCurrentStateAsync(id, userId);

            return success;
        }
    }
}