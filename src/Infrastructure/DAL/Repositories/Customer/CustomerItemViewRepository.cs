using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Customer;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Customer
{
    /// <summary>
    /// Repository for Wishlist operations
    /// All operations include customer validation for security
    /// </summary>
    public class CustomerItemViewRepository : TableRepository<TbCustomerItemView>, ICustomerItemViewRepository
    {
        public CustomerItemViewRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger) : base(dbContext, currentUserService, logger)
        {
        }

        public async Task<IEnumerable<TbCustomerItemView>> GetAllCustomerViewsAsync(Guid customerId)
        {
            // Validate input
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            // Get all customer item views
            var customerItemViews = await GetAsync(
                w => w.CustomerId == customerId && !w.IsDeleted,
                orderBy: q => q.OrderByDescending(civ => civ.ViewedAt)
            );

            return customerItemViews;
        }
        public async Task<IEnumerable<TbCustomerItemView>> GetAllItemCombinationViewsAsync(Guid itemCombinationId)
        {
            // Validate input
            if (itemCombinationId == Guid.Empty)
                throw new ArgumentException("Combination ID is required.", nameof(itemCombinationId));

            // Get all customer item views
            var customerItemViews = await GetAsync(
                w => w.ItemCombinationId == itemCombinationId && !w.IsDeleted,
                orderBy: q => q.OrderByDescending(civ => civ.ViewedAt)
            );

            return customerItemViews;
        }
        public async Task<IEnumerable<TbCustomerItemView>> GetAllItemViewsAsync(Guid itemId)
        {
            // Validate input
            if (itemId == Guid.Empty)
                throw new ArgumentException("Item ID is required.", nameof(itemId));

            // Get all item combinations for the item
            var itemCombinationIds = await _dbContext.Set<TbItemCombination>()
                .AsNoTracking()
                .Where(ic => ic.ItemId == itemId && !ic.IsDeleted)
                .Select(ic => ic.Id)
                .ToListAsync();

            // Get all customer item views
            var customerItemViews = await GetAsync(
                w => itemCombinationIds.Contains(w.ItemCombinationId) && !w.IsDeleted,
                orderBy: q => q.OrderByDescending(civ => civ.ViewedAt)
            );

            return customerItemViews;
        }


        public async Task<int> GetItemCombinationViewsCountAsync(Guid itemCombinationId)
        {
            // Validate input
            if (itemCombinationId == Guid.Empty)
                throw new ArgumentException("Item Combination ID is required.", nameof(itemCombinationId));

            // Count items
            return await _dbContext.Set<TbCustomerItemView>()
                .AsNoTracking()
                .CountAsync(wi => wi.ItemCombinationId == itemCombinationId && !wi.IsDeleted);
        }
    }
}