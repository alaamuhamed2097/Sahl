using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.Order.Shipping;
using Shared.DTOs.ECommerce;

namespace BL.Contracts.Service.ShippingCompny;

public interface IShippingCompanyService : IBaseService<TbShippingCompany, ShippingCompanyDto>
{
    Task<PagedResult<ShippingCompanyDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
    Task<PagedResult<ShippingCompanyDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
    Task<bool> Save(ShippingCompanyDto dto, Guid userId);
}
