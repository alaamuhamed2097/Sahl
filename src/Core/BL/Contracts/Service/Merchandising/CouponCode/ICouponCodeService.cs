using Common.Filters;
using DAL.Models;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.CouponCode;

namespace BL.Contracts.Service.Merchandising.CouponCode;

/// <summary>
/// Service interface for coupon code business logic
/// Inherits from IBaseService which provides: FindByIdAsync, GetAllAsync, SaveAsync, CreateAsync, UpdateAsync, DeleteAsync
/// </summary>
public interface ICouponCodeService
{
    // CRUD Operations
    Task<CouponCodeDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CouponCodeDto>> GetAllAsync();
    Task<AdvancedPagedResult<CouponCodeDto>> GetPageAsync(BaseSearchCriteriaModel criteria);
    Task<CouponCodeDto> SaveAsync(CouponCodeDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);

    // Coupon-specific methods
    Task<CouponCodeDto?> GetByCodeAsync(string code);
    Task<IEnumerable<CouponCodeDto>> GetActiveCouponsAsync();
    Task<IEnumerable<CouponCodeDto>> GetVendorCouponsAsync(Guid vendorId);
    Task<CouponValidationResultDto> ValidateCouponCodeAsync(string couponCode, string userId);
    Task<bool> ApplyCouponToOrderAsync(Guid orderId, string couponCode);
}