using BL.Contracts.IMapper;
using BL.Contracts.Service.Location;
using BL.Service.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domins.Entities.Location;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Location
{
    public class CityService : BaseService<TbCity, CityDto>, ICityService
    {
        private readonly ITableRepository<TbCity> _baseRepository;
        private readonly IBaseMapper _mapper;

        public CityService(ITableRepository<TbCity> baseRepository, IBaseMapper mapper)
            : base(baseRepository, mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public PaginatedDataModel<CityDto> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbCity, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.CurrentState == 1 &&
                             (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                             x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm));
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbCity>, IOrderedQueryable<TbCity>> orderBy = null;
            
            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "titlear" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        "titleen" => isDescending ? query.OrderByDescending(x => x.TitleEn) : query.OrderBy(x => x.TitleEn),
                        "title" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
                        _ => query.OrderBy(x => x.TitleAr) // Default sorting
                    };
                };
            }

            var entitiesList = _baseRepository.GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy);

            var dtoList = _mapper.MapList<TbCity, CityDto>(entitiesList.Items);

            return new PaginatedDataModel<CityDto>(dtoList, entitiesList.TotalRecords);
        }
    }
}
