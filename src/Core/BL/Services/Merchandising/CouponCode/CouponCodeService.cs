using AutoMapper;
using BL.Contracts.Service.Merchandising.CouponCode;
using Common.Enumerations.Order;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Merchandising.CouponCode;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Services.Merchandising.CouponCode
{
    /// <summary>
    /// Service implementation for coupon code business logic - STANDALONE
    /// </summary>
    public class CouponCodeService : ICouponCodeService
    {
        private readonly ICouponCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponCodeService(
            ICouponCodeRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region CRUD Operations

        public async Task<CouponCodeDto?> GetByIdAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            return coupon == null ? null : _mapper.Map<CouponCodeDto>(coupon);
        }

        public async Task<IEnumerable<CouponCodeDto>> GetAllAsync()
        {
            var coupons = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponCodeDto>>(coupons);
        }

        public async Task<AdvancedPagedResult<CouponCodeDto>> GetPageAsync(BaseSearchCriteriaModel criteria)
        {
            var allCoupons = await _repository.GetAllAsync();

            var query = allCoupons.AsQueryable();

            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                query = query.Where(c =>
                    c.Code.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.TitleAr.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.TitleEn.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();

            var items = query
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToList();

            return new AdvancedPagedResult<CouponCodeDto>
            {
                Items = _mapper.Map<List<CouponCodeDto>>(items),
                TotalRecords = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)criteria.PageSize)
            };
        }

        public async Task<CouponCodeDto> SaveAsync(CouponCodeDto dto, Guid userId)
        {
            TbCouponCode coupon;

            if (dto.Id == Guid.Empty)
            {
                // Create new
                await ValidateCreateDto(dto);

                coupon = _mapper.Map<TbCouponCode>(dto);
                coupon.Code = dto.Code.ToUpper().Trim();

                var saveResult = await _repository.SaveAsync(coupon, userId);

                if (!saveResult.Success)
                    throw new InvalidOperationException("فشل حفظ الكوبون");

                // Add scopes if provided
                if (dto.ScopeItems != null && dto.ScopeItems.Any())
                {
                    coupon = await _repository.GetByIdAsync(saveResult.Id);

                    foreach (var scope in dto.ScopeItems)
                    {
                        var scopeEntity = new TbCouponCodeScope
                        {
                            Id = Guid.NewGuid(),
                            CouponCodeId = coupon.Id,
                            ScopeType = scope.ScopeType,
                            ScopeId = scope.ScopeId,
                            CreatedBy = userId,
                            CreatedDateUtc = DateTime.UtcNow
                        };

                        coupon.CouponScopes.Add(scopeEntity);
                    }

                    await _repository.UpdateAsync(coupon);
                }

                // Reload to get complete entity with scopes
                coupon = await _repository.GetByIdAsync(saveResult.Id);
            }
            else
            {
                // Update existing
                coupon = await _repository.GetByIdAsync(dto.Id)
                    ?? throw new KeyNotFoundException($"Coupon with ID {dto.Id} not found");

                // Update only allowed fields
                coupon.TitleAr = dto.TitleAr;
                coupon.TitleEn = dto.TitleEn;
                coupon.StartDate = dto.StartDate;
                coupon.ExpiryDate = dto.ExpiryDate;
                coupon.UsageLimit = dto.UsageLimit;
                coupon.IsActive = dto.IsActive;

                var updateResult = await _repository.UpdateAsync(coupon, userId);

                if (!updateResult.Success)
                    throw new InvalidOperationException("فشل تحديث الكوبون");

                // Reload to get updated entity
                coupon = await _repository.GetByIdAsync(dto.Id);
            }

            return _mapper.Map<CouponCodeDto>(coupon);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _repository.SoftDeleteAsync(id, userId);
        }

        #endregion

        #region Coupon-Specific Methods

        public async Task<CouponCodeDto?> GetByCodeAsync(string code)
        {
            var coupon = await _repository.GetByCodeAsync(code);
            return coupon == null ? null : _mapper.Map<CouponCodeDto>(coupon);
        }

        public async Task<IEnumerable<CouponCodeDto>> GetActiveCouponsAsync()
        {
            var coupons = await _repository.GetActiveCouponsAsync();
            return _mapper.Map<IEnumerable<CouponCodeDto>>(coupons);
        }

        public async Task<IEnumerable<CouponCodeDto>> GetVendorCouponsAsync(Guid vendorId)
        {
            var coupons = await _repository.GetCouponsByVendorAsync(vendorId);
            return _mapper.Map<IEnumerable<CouponCodeDto>>(coupons);
        }

        public async Task<CouponValidationResultDto> ValidateCouponCodeAsync(string couponCode, string userId)
        {
            var result = new CouponValidationResultDto
            {
                IsValid = false,
                DiscountAmount = 0
            };

            var coupon = await _repository.GetByCodeAsync(couponCode);
            if (coupon == null)
            {
                result.Message = "الكوبون غير موجود";
                return result;
            }

            // Check if active
            if (!coupon.IsActive || coupon.IsDeleted)
            {
                result.Message = "الكوبون غير مفعل";
                return result;
            }

            // Check date validity
            var now = DateTime.UtcNow;
            if (coupon.StartDate > now)
            {
                result.Message = "الكوبون لم يبدأ بعد";
                return result;
            }

            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < now)
            {
                result.Message = "الكوبون منتهي الصلاحية";
                return result;
            }

            // Check usage limits
            if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
            {
                result.Message = "تم استخدام الكوبون بالحد الأقصى";
                return result;
            }

            // Check per-user limit
            if (coupon.UsageLimitPerUser.HasValue)
            {
                var userUsageCount = await _repository.GetUserUsageCountAsync(coupon.Id, userId);
                if (userUsageCount >= coupon.UsageLimitPerUser.Value)
                {
                    result.Message = "لقد تجاوزت الحد الأقصى لاستخدام هذا الكوبون";
                    return result;
                }
            }

            // Check first order only
            if (coupon.IsFirstOrderOnly)
            {
                var isValid = await _repository.IsValidForUserAsync(coupon.Id, userId);
                if (!isValid)
                {
                    result.Message = "هذا الكوبون متاح للطلب الأول فقط";
                    return result;
                }
            }

            result.IsValid = true;
            result.CouponId = coupon.Id;
            result.DiscountType = coupon.DiscountType;
            result.DiscountValue = coupon.DiscountValue;
            result.MaxDiscountAmount = coupon.MaxDiscountAmount;
            result.MinimumOrderAmount = coupon.MinimumOrderAmount;
            result.Message = "الكوبون صالح";

            return result;
        }

        public async Task<bool> ApplyCouponToOrderAsync(Guid orderId, string couponCode)
        {
            var coupon = await _repository.GetByCodeAsync(couponCode);
            if (coupon == null)
                return false;

            await _repository.IncrementUsageCountAsync(coupon.Id);
            return true;
        }

        #endregion

        #region Private Helpers

        private async Task ValidateCreateDto(CouponCodeDto dto)
        {
            // Check unique code
            var isUnique = await _repository.IsCodeUniqueAsync(dto.Code);
            if (!isUnique)
                throw new InvalidOperationException("رمز الكوبون مستخدم بالفعل");

            // Validate percentage
            if (dto.DiscountType == DiscountType.Percentage && dto.DiscountValue > 100)
                throw new InvalidOperationException("النسبة المئوية لا يمكن أن تتجاوز 100%");

            // Validate dates
            if (dto.ExpiryDate.HasValue && dto.StartDate > dto.ExpiryDate)
                throw new InvalidOperationException("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

            // Validate co-funded
            if (dto.PlatformSharePercentage.HasValue && dto.PlatformSharePercentage.Value > 0)
            {
                if (!dto.VendorId.HasValue)
                    throw new InvalidOperationException("يجب تحديد البائع للكوبونات المشتركة");
            }

            // Validate scope based on promo type
            switch (dto.PromoType)
            {
                case CouponCodeType.CategoryBased:
                case CouponCodeType.VendorBased:
                    if (dto.ScopeItems == null || !dto.ScopeItems.Any())
                        throw new InvalidOperationException("يجب تحديد العناصر المؤهلة للخصم");
                    break;
            }
        }

        #endregion
    }
}