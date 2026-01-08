using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using Domains.Entities.ECommerceSystem.Customer;
using Serilog;

namespace DAL.Repositories.Customer
{
    /// <summary>
    /// Repository for Wishlist operations
    /// All operations include customer validation for security
    /// </summary>
    public class CustomerRepository : TableRepository<TbCustomer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
        }
        /// <summary>
        /// Get customer by user id
        /// </summary>
        /// <param name="userId">ApplicationUser ID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TbCustomer> GetCustomerByUserIdAsync(string userId)
        {
            // Validate input
            if (!string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required.", nameof(userId));

            // Get customer by user id
            var customer = await FindAsync(
                w => w.UserId == userId && !w.IsDeleted
            );
            return customer;
        }
    }
}