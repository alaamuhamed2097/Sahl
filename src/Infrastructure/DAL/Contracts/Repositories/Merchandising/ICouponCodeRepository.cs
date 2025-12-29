using Domains.Entities.Merchandising.CouponCode;

namespace DAL.Contracts.Repositories.Merchandising
{
    /// <summary>
    /// Repository interface for coupon code operations
    /// </summary>
    public interface ICouponCodeRepository : ITableRepository<TbCouponCode>
    {
        // Basic CRUD Operations
        Task<TbCouponCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TbCouponCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<IEnumerable<TbCouponCode>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TbCouponCode> AddAsync(TbCouponCode couponCode, CancellationToken cancellationToken = default);
        Task<TbCouponCode> UpdateAsync(TbCouponCode couponCode, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // Query Operations
        Task<IEnumerable<TbCouponCode>> GetActiveCouponsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TbCouponCode>> GetCouponsByVendorAsync(Guid vendorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TbCouponCode>> GetCouponsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null, CancellationToken cancellationToken = default);

        // Usage Tracking
        Task<int> GetUserUsageCountAsync(Guid couponId, string userId, CancellationToken cancellationToken = default);
        Task IncrementUsageCountAsync(Guid couponId, CancellationToken cancellationToken = default);
        Task<bool> IsValidForUserAsync(Guid couponId, string userId, CancellationToken cancellationToken = default);
    }
}