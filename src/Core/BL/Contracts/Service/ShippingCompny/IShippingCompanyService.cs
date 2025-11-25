using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.Shipping;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.ShippingCompny
{
    public interface IShippingCompanyService : IBaseService<TbShippingCompany, ShippingCompanyDto>
    {
        PaginatedDataModel<ShippingCompanyDto> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<bool> Save(ShippingCompanyDto dto, Guid userId);
    }
}
