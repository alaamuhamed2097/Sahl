using BL.Contracts.Service.Base;
using Common.Filters;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;

namespace BL.Contracts.Service.Vendor;

public interface IVendorManagementService : IBaseService<TbVendor, VendorDto>
{
    Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
    Task<TbVendor> GetByUserIdAsync(string userId);
    Guid GetMarketStoreVendorId();
    //Task<PaginatedDataModel<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);

}
