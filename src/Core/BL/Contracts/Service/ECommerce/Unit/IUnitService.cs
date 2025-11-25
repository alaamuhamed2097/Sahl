using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Catalog.Unit;
using Shared.DTOs.ECommerce.Unit;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.ECommerce.Unit
{
    public interface IUnitService : IBaseService<TbUnit, UnitDto>
    {
        PaginatedDataModel<UnitDto> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}
