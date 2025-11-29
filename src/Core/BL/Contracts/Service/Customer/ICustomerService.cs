using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Shared.DTOs.Customer;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Customer
{
    public interface ICustomerService : IBaseService<TbCustomer, CustomerDto>
    {
        Task<PaginatedDataModel<CustomerDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<PaginatedDataModel<CustomerDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
    }
}
