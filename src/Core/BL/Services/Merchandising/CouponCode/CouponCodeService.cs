using AutoMapper;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Merchandising.CouponCode;
using Common.Enumerations.Order;
using Common.Filters;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Merchandising.CouponCode;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.CouponCode;

namespace BL.Services.Merchandising.CouponCode
{
    /// <summary>
    /// Service implementation for coupon code business logic - STANDALONE
    /// </summary>
    public class CouponCodeService : ICouponCodeService
    {
        private readonly ICouponCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;

        public CouponCodeService(
            ICouponCodeRepository repository,
            IUnitOfWork unitOfWork,
            IBaseMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region CRUD Operations

        public async Task<CouponCodeDto?> GetByIdAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            if (coupon == null) return null;

            var dto = _mapper.MapModel<TbCouponCode, CouponCodeDto>(coupon);

            // Manual mapping for ScopeItems if needed
            if (coupon.CouponScopes != null && coupon.CouponScopes.Any())
            {
                dto.ScopeItems = _mapper.MapList<TbCouponCodeScope, CouponScopeDto>(coupon.CouponScopes).ToList();
            }

            return dto;
        }

        public async Task<IEnumerable<CouponCodeDto>> GetAllAsync()
        {
            var coupons = await _repository.GetAllAsync();
            return _mapper.MapList<TbCouponCode, CouponCodeDto>(coupons);
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
                Items = _mapper.MapList<TbCouponCode, CouponCodeDto>(items).ToList(),
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

                coupon = _mapper.MapModel<CouponCodeDto, TbCouponCode>(dto);
                coupon.Code = dto.Code.ToUpper().Trim();
                // Ensure ID is generated for scopes linking
                coupon.Id = Guid.NewGuid(); 

                var newScopes = new List<TbCouponCodeScope>();
                if (dto.ScopeItems != null)
                {
                    foreach (var scope in dto.ScopeItems)
                    {
                        newScopes.Add(new TbCouponCodeScope
                        {
                            Id = Guid.NewGuid(),
                            CouponCodeId = coupon.Id,
                            ScopeType = scope.ScopeType,
                            ScopeId = scope.ScopeId
                        });
                    }
                }

                var success = await _repository.AddWithScopesAsync(coupon, newScopes, userId);

                if (!success)
                    throw new InvalidOperationException("فشل حفظ الكوبون");

                // Reload to get complete entity with scopes
                coupon = await _repository.GetByIdAsync(coupon.Id);
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
                coupon.UsageLimitPerUser = dto.UsageLimitPerUser;
                coupon.IsActive = dto.IsActive;
                coupon.IsFirstOrderOnly = dto.IsFirstOrderOnly;
                coupon.DiscountType = dto.DiscountType;
                coupon.DiscountValue = dto.DiscountValue;
                coupon.MaxDiscountAmount = dto.MaxDiscountAmount;
                coupon.MinimumOrderAmount = dto.MinimumOrderAmount;
                
                // Update Coupon Type & Vendor info if changed
                coupon.PromoType = dto.PromoType;
                coupon.VendorId = dto.VendorId;
                coupon.PlatformSharePercentage = dto.PlatformSharePercentage;
                //coupon.IsVisible = dto.IsVisible;

                var newScopes = new List<TbCouponCodeScope>();
                if (dto.ScopeItems != null)
                {
                    foreach (var scope in dto.ScopeItems)
                    {
                        newScopes.Add(new TbCouponCodeScope
                        {
                            Id = Guid.NewGuid(),
                            CouponCodeId = coupon.Id,
                            ScopeType = scope.ScopeType,
                            ScopeId = scope.ScopeId
                        });
                    }
                }

                // Use transactional update method
                var updateResult = await _repository.UpdateWithScopesAsync(coupon, newScopes, userId);

                if (!updateResult)
                    throw new InvalidOperationException("فشل تحديث الكوبون");

                // Reload to get updated entity
                coupon = await _repository.GetByIdAsync(dto.Id);
            }

            var resultDto = _mapper.MapModel<TbCouponCode, CouponCodeDto>(coupon);

            // Manual mapping for ScopeItems
            if (coupon.CouponScopes != null && coupon.CouponScopes.Any())
            {
                resultDto.ScopeItems = _mapper.MapList<TbCouponCodeScope, CouponScopeDto>(coupon.CouponScopes).ToList();
            }

            return resultDto;
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
            if (coupon == null) return null;

            var dto = _mapper.MapModel<TbCouponCode, CouponCodeDto>(coupon);

            // Manual mapping for ScopeItems
            if (coupon.CouponScopes != null && coupon.CouponScopes.Any())
            {
                dto.ScopeItems = _mapper.MapList<TbCouponCodeScope, CouponScopeDto>(coupon.CouponScopes).ToList();
            }

            return dto;
        }

        public async Task<IEnumerable<CouponCodeDto>> GetActiveCouponsAsync()
        {
            var coupons = await _repository.GetActiveCouponsAsync();
            return _mapper.MapList<TbCouponCode, CouponCodeDto>(coupons);
        }

        public async Task<IEnumerable<CouponCodeDto>> GetVendorCouponsAsync(Guid vendorId)
        {
            var coupons = await _repository.GetCouponsByVendorAsync(vendorId);
            return _mapper.MapList<TbCouponCode, CouponCodeDto>(coupons);
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