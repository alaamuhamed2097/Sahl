using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;

namespace DAL.Contracts.Repositories
{
    public interface IVendorManagementRepository : ITableRepository<TbVendor>
    {
        Task<(bool Success, IEnumerable<string> Errors)> RegisterVendorWithUserAsync(ApplicationUser user, string password, TbVendor vendor);
        Task<(bool Success, IEnumerable<string> Errors)> UpdateVendorWithUserAsync(ApplicationUser user, TbVendor vendor, string? oldFrontImagePath = null, string? oldBackImagePath = null);
        Task<PagedResult<TbVendor>> GetPageAsync(BaseSearchCriteriaModel criteriaModel, CancellationToken cancellationToken);
        Task<TbVendor> FindByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken);
        Task<bool> UpdateUserStateAsync(Guid vendorId, UserStateType newType, CancellationToken cancellationToken);
    }
}
