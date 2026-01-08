using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.Catalog.Unit;
using Shared.DTOs.Catalog.Unit;

namespace BL.Contracts.Service.Catalog.Unit;

public interface IUnitService : IBaseService<TbUnit, UnitDto>
{
    Task<PagedResult<UnitDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
}
