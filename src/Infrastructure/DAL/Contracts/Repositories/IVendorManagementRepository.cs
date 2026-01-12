using Common.Filters;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;

namespace DAL.Contracts.Repositories
{
    public interface IVendorManagementRepository : ITableRepository<TbVendor>
    {
        Task<(bool Success, IEnumerable<string> Errors)> RegisterVendorWithUserAsync(ApplicationUser user, string password, TbVendor vendor);
        Task<PagedResult<TbVendor>> GetPageAsync(BaseSearchCriteriaModel criteriaModel, CancellationToken cancellationToken);
    }
}
