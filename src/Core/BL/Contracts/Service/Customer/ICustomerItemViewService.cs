using Common.Filters;
using DAL.Models;
using Shared.DTOs.Customer;

namespace BL.Contracts.Service.Customer
{
    public interface ICustomerItemViewService
    {
        Task<bool> AddCustomerItemViewAsync(CustomerItemViewDto customerItemViewDto, Guid creatorId);
        Task<PagedResult<CustomerItemViewDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
    }
}