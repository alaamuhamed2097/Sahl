using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.Location;
using Shared.DTOs.Location;

namespace BL.Contracts.Service.Location;

public interface IStateService : IBaseService<TbState, StateDto>
{
    Task<PagedResult<StateDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
}
