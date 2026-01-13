using BL.Contracts.Service.Base;
using Common.Enumerations.User;
using Common.Enumerations.VendorStatus;
using Common.Filters;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.Service.Vendor;

public interface IVendorManagementService : IBaseService<TbVendor, VendorDto>
{
    Task<PagedResult<VendorDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel);
    Task<TbVendor> GetByUserIdAsync(string userId);
    Task<bool> UpdateVendorStatusAsync(Guid vendorId, VendorStatus status);
    Task<bool> UpdateUserStatusAsync(Guid vendorId, UserStateType status);
    Task<ServiceResult<VendorUpdateResponseDto>> UpdateVendorAsync(VendorUpdateRequestDto request, string updaterId);
}
