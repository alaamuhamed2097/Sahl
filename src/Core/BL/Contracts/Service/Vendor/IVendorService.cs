using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Vendor
{
    public interface IVendorService : IBaseService<TbVendor, VendorDto>
    {
        Task<PagedResult<VendorDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<PagedResult<VendorDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
        Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
        //Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);

    }
}
