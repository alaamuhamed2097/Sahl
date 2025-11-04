using BL.Contracts.Service.Base;
using DAL.Models;
using Domins.Entities.Location;
using Shared.DTOs.Location;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Location
{
    public interface IStateService : IBaseService<TbState, StateDto>
    {
        PaginatedDataModel<StateDto> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}
