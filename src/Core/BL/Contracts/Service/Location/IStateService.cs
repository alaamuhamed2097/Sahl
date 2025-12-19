using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Location;
using Shared.DTOs.Location;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Location
{
    public interface IStateService : IBaseService<TbState, StateDto>
    {
        Task<PagedResult<StateDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}
