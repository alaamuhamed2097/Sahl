using Common.Filters;
using DAL.Models;
using Shared.DTOs.Vendor;

namespace BL.Contracts.Service.Vendor
{
    public interface IVendorService
    {
        Task<PagedResult<VendorDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<PagedResult<VendorDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
        Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
    }
}