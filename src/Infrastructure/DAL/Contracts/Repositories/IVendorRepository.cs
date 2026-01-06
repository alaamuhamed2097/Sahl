using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;

namespace DAL.Contracts.Repositories
{
    public interface IVendorRepository : ITableRepository<TbVendor>
    {
        Task<(bool Success, IEnumerable<string> Errors)> RegisterVendorWithUserAsync(ApplicationUser user, string password, TbVendor vendor);
    }
}
