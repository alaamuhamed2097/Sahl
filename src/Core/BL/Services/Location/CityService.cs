using BL.Contracts.IMapper;
using BL.Contracts.Service.Location;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Location;
using Resources;
using Shared.DTOs.Location;
using System.Linq.Expressions;

namespace BL.Services.Location;

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

    public async Task<PagedResult<CityDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        // Base filter for active entities
        Expression<Func<TbCity, bool>> filter = x => !x.IsDeleted;

        // Apply search term if provided
        if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
        {
            string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
            filter = x => !x.IsDeleted &&
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

        var entitiesList = await _baseRepository.GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter,
            orderBy);

        var dtoList = _mapper.MapList<TbCity, CityDto>(entitiesList.Items);

        return new PagedResult<CityDto>(dtoList, entitiesList.TotalRecords);
    }

    public async Task<IEnumerable<CityDto>> GetByStateIdAsync(Guid stateId)
    {
        if (stateId == Guid.Empty)
            throw new ArgumentException("State ID cannot be empty.", nameof(stateId));

        var cities = await _baseRepository.GetAsync(x => !x.IsDeleted && x.StateId == stateId);
        return _mapper.MapList<TbCity, CityDto>(cities);
    }
}
