using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Customer;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Customer;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Customer
{
    /// <summary>
    /// Repository for Wishlist operations
    /// All operations include customer validation for security
    /// </summary>
    public class CustomerRepository : TableRepository<TbCustomer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
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