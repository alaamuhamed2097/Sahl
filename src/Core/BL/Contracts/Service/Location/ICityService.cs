using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.Location;
using Shared.DTOs.Location;

namespace BL.Contracts.Service.Location;

public interface ICityService : IBaseService<TbCity, CityDto>
{
    Task<PagedResult<CityDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
    Task<IEnumerable<CityDto>> GetByStateIdAsync(Guid stateId);
}
