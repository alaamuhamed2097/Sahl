using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Merchandising.CouponCode;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Merchandising
{
    /// <summary>
    /// Repository implementation for coupon code operations - CORRECTED VERSION
    /// </summary>
    public class CouponCodeRepository : TableRepository<TbCouponCode>, ICouponCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponCodeRepository(ApplicationDbContext context, ICurrentUserService currentUserService, ILogger logger)
            : base(context, currentUserService, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Basic CRUD Operations

        public async Task<TbCouponCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.TbCouponCodes
                .Include(c => c.Vendor)
                .Include(c => c.CouponScopes)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        }

        public async Task<TbCouponCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _context.TbCouponCodes
                .Include(c => c.Vendor)
                .Include(c => c.CouponScopes)
                .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<TbCouponCode>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TbCouponCodes
                .Include(c => c.Vendor)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<TbCouponCode> AddAsync(TbCouponCode couponCode, CancellationToken cancellationToken = default)
        {
            if (couponCode == null)
                throw new ArgumentNullException(nameof(couponCode));

            await _context.TbCouponCodes.AddAsync(couponCode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return couponCode;
        }

        public async Task<TbCouponCode> UpdateAsync(TbCouponCode couponCode, CancellationToken cancellationToken = default)
        {
            if (couponCode == null)
                throw new ArgumentNullException(nameof(couponCode));

            _context.TbCouponCodes.Update(couponCode);
            await _context.SaveChangesAsync(cancellationToken);
            return couponCode;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var coupon = await _context.TbCouponCodes
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

            if (coupon == null)
                return false;

            // Soft delete
            coupon.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateWithScopesAsync(TbCouponCode coupon, List<TbCouponCodeScope> scopes, Guid userId, CancellationToken cancellationToken = default)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                // 1. Update Coupon Basic Info
                coupon.UpdatedBy = userId;
                coupon.UpdatedDateUtc = DateTime.UtcNow;
                _context.TbCouponCodes.Update(coupon);

                // 2. Remove Old Scopes
                var existingScopes = await _context.Set<TbCouponCodeScope>()
                    .Where(s => s.CouponCodeId == coupon.Id)
                    .ToListAsync(cancellationToken);
                
                if (existingScopes.Any())
                {
                    _context.Set<TbCouponCodeScope>().RemoveRange(existingScopes);
                }

                // 3. Add New Scopes
                if (scopes != null && scopes.Any())
                {
                    foreach (var scope in scopes)
                    {
                        scope.CouponCodeId = coupon.Id;
                        if (scope.Id == Guid.Empty) scope.Id = Guid.NewGuid();
                        if (scope.CreatedDateUtc == default) scope.CreatedDateUtc = DateTime.UtcNow;
                        if (scope.CreatedBy == Guid.Empty) scope.CreatedBy = userId;

                        await _context.Set<TbCouponCodeScope>().AddAsync(scope, cancellationToken);
                    }
                }

                // 4. Save and Commit
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<bool> AddWithScopesAsync(TbCouponCode coupon, List<TbCouponCodeScope> scopes, Guid userId, CancellationToken cancellationToken = default)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                // 1. Prepare Coupon
                if (coupon.Id == Guid.Empty) coupon.Id = Guid.NewGuid();
                coupon.CreatedBy = userId;
                coupon.CreatedDateUtc = DateTime.UtcNow;
                
                // 2. Add Coupon
                await _context.TbCouponCodes.AddAsync(coupon, cancellationToken);

                // 3. Add Scopes
                if (scopes != null && scopes.Any())
                {
                    foreach (var scope in scopes)
                    {
                        scope.CouponCodeId = coupon.Id;
                        if (scope.Id == Guid.Empty) scope.Id = Guid.NewGuid();
                        scope.CreatedDateUtc = DateTime.UtcNow;
                        scope.CreatedBy = userId;

                        await _context.Set<TbCouponCodeScope>().AddAsync(scope, cancellationToken);
                    }
                }

                // 4. Save and Commit
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        #endregion

        #region Query Operations

        public async Task<IEnumerable<TbCouponCode>> GetActiveCouponsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _context.TbCouponCodes
                .Where(c => !c.IsDeleted &&
                           c.IsActive &&
                           (c.StartDate <= now) &&
                           (!c.ExpiryDate.HasValue || c.ExpiryDate.Value >= now) &&
                           (!c.UsageLimit.HasValue || c.UsageCount < c.UsageLimit.Value))
                .Include(c => c.Vendor)
                .Include(c => c.CouponScopes)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbCouponCode>> GetCouponsByVendorAsync(Guid vendorId, CancellationToken cancellationToken = default)
        {
            return await _context.TbCouponCodes
                .Where(c => !c.IsDeleted && c.VendorId == vendorId)
                .Include(c => c.Vendor)
                .Include(c => c.CouponScopes)
                .OrderByDescending(c => c.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TbCouponCode>> GetCouponsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.TbCouponCodes
                .Where(c => !c.IsDeleted &&
                           c.IsActive &&
                           c.CouponScopes.Any(s =>
                               s.ScopeType == Common.Enumerations.Order.CouponCodeScopeType.Category &&
                               s.ScopeId == categoryId))
                .Include(c => c.Vendor)
                .Include(c => c.CouponScopes)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.TbCouponCodes
                .Where(c => !c.IsDeleted && c.Code == code);

            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return !await query.AnyAsync(cancellationToken);
        }

        #endregion

        #region Usage Tracking

        public async Task<int> GetUserUsageCountAsync(Guid couponId, string userId, CancellationToken cancellationToken = default)
        {
            return await _context.TbOrders
                .Where(o => !o.IsDeleted && o.CouponId == couponId && o.UserId == userId)
                .CountAsync(cancellationToken);
        }

        public async Task IncrementUsageCountAsync(Guid couponId, CancellationToken cancellationToken = default)
        {
            var coupon = await _context.TbCouponCodes
                .FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted, cancellationToken);

            if (coupon != null)
            {
                coupon.UsageCount++;
                coupon.UpdatedDateUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> IsValidForUserAsync(Guid couponId, string userId, CancellationToken cancellationToken = default)
        {
            var coupon = await GetByIdAsync(couponId, cancellationToken);
            if (coupon == null || !coupon.IsActive || coupon.IsDeleted)
                return false;

            var now = DateTime.UtcNow;

            // Check date validity
            if (coupon.StartDate > now)
                return false;

            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < now)
                return false;

            // Check usage limit
            if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
                return false;

            // Check per-user limit
            if (coupon.UsageLimitPerUser.HasValue)
            {
                var userUsageCount = await GetUserUsageCountAsync(couponId, userId, cancellationToken);
                if (userUsageCount >= coupon.UsageLimitPerUser.Value)
                    return false;
            }

            // Check first order only
            if (coupon.IsFirstOrderOnly)
            {
                var hasOrders = await _context.TbOrders
                    .AnyAsync(o => !o.IsDeleted && o.UserId == userId, cancellationToken);
                if (hasOrders)
                    return false;
            }

            return true;
        }

        #endregion
    }
}