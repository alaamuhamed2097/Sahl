using BL.Contracts.Service.Base;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.Vendor
{
    public interface IVendorService : IBaseService<TbVendor, VendorDto>
    {
        Task<PaginatedDataModel<VendorDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<PaginatedDataModel<VendorDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
        Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
        //Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);

    }
}
