using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Customer;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Customer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Customer
{
    /// <summary>
    /// Repository for Wishlist operations
    /// All operations include customer validation for security
    /// </summary>
    public class WishlistRepository : TableRepository<TbWishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
        }

        public async Task<TbWishlist> GetOrCreateWishlistAsync(string customerId, Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            // Try to find existing wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist != null)
                return wishlist;

            // Create new wishlist
            var newWishlist = new TbWishlist
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await CreateAsync(newWishlist, createdBy);

            return newWishlist;
        }

        public async Task<TbWishlist?> GetWishlistByCustomerIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            return await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );
        }

        public async Task<TbWishlistItem?> GetWishlistItemAsync(string customerId, Guid itemCombinationId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || itemCombinationId == Guid.Empty)
                return null;

            // Get customer's wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist == null)
                return null;

            // Get item in customer's wishlist using DbContext directly
            return await _dbContext.Set<TbWishlistItem>()
                .AsNoTracking()
                .FirstOrDefaultAsync(wi =>
                    wi.WishlistId == wishlist.Id &&
                    wi.ItemCombinationId == itemCombinationId &&
                    !wi.IsDeleted
                );
        }

        public async Task<AdvancedPagedResult<TbWishlistItem>> GetWishlistItemsPagedAsync(
            string customerId,
            int page,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new AdvancedPagedResult<TbWishlistItem>
                {
                    Items = new List<TbWishlistItem>(),
                    TotalRecords = 0,
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalPages = 0
                };
            }

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Get customer's wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist == null)
            {
                return new AdvancedPagedResult<TbWishlistItem>
                {
                    Items = new List<TbWishlistItem>(),
                    TotalRecords = 0,
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalPages = 0
                };
            }

            // Get paginated items using DbContext
            var query = _dbContext.Set<TbWishlistItem>()
                .AsNoTracking()
                .Where(wi => wi.WishlistId == wishlist.Id && !wi.IsDeleted)
                .OrderByDescending(wi => wi.DateAdded);

            var totalRecords = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new AdvancedPagedResult<TbWishlistItem>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }

        public async Task<TbWishlistItem?> AddToWishlistAsync(
            string customerId,
            Guid itemCombinationId,
            Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(customerId) || itemCombinationId == Guid.Empty)
                throw new ArgumentException("Customer ID and Item Combination ID are required.");

            // Get or create wishlist
            var wishlist = await GetOrCreateWishlistAsync(customerId, createdBy);

            // Check if already exists using DbContext
            var existingItem = await _dbContext.Set<TbWishlistItem>()
                .AsNoTracking()
                .FirstOrDefaultAsync(wi =>
                    wi.WishlistId == wishlist.Id &&
                    wi.ItemCombinationId == itemCombinationId &&
                    !wi.IsDeleted
                );

            if (existingItem != null)
                return null; // Already exists

            // Create new wishlist item
            var wishlistItem = new TbWishlistItem
            {
                Id = Guid.NewGuid(),
                WishlistId = wishlist.Id,
                ItemCombinationId = itemCombinationId,
                DateAdded = DateTime.UtcNow,
                IsDeleted = false,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _dbContext.Set<TbWishlistItem>().AddAsync(wishlistItem);
            await _dbContext.SaveChangesAsync();

            return wishlistItem;
        }

        public async Task<bool> RemoveFromWishlistAsync(
            string customerId,
            Guid itemCombinationId,
            Guid updatedBy)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

            if (itemCombinationId == Guid.Empty)
                throw new ArgumentException("Item combination ID cannot be empty", nameof(itemCombinationId));

            // Get customer's wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist == null)
                throw new NotFoundException($"Wishlist not found for customer ID: {customerId}", _logger);

            // Get item in customer's wishlist
            var item = await _dbContext.Set<TbWishlistItem>()
                .FirstOrDefaultAsync(wi =>
                    wi.WishlistId == wishlist.Id &&
                    wi.ItemCombinationId == itemCombinationId &&
                    !wi.IsDeleted
                );

            if (item == null)
                throw new NotFoundException($"Item with combination ID {itemCombinationId} not found in wishlist", _logger);

            // Soft delete
            item.IsDeleted = true;
            item.UpdatedDateUtc = DateTime.UtcNow;
            item.UpdatedBy = updatedBy;

            _dbContext.Set<TbWishlistItem>().Update(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> ClearWishlistAsync(string customerId, Guid updatedBy)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return 0;

            // Get customer's wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist == null)
                return 0;

            // Get all items
            var items = await _dbContext.Set<TbWishlistItem>()
                .Where(wi => wi.WishlistId == wishlist.Id && !wi.IsDeleted)
                .ToListAsync();

            if (!items.Any())
                return 0;

            // Soft delete all items
            var utcNow = DateTime.UtcNow;
            foreach (var item in items)
            {
                item.IsDeleted = true;
                item.UpdatedDateUtc = utcNow;
                item.UpdatedBy = updatedBy;
            }

            _dbContext.Set<TbWishlistItem>().UpdateRange(items);
            await _dbContext.SaveChangesAsync();

            return items.Count;
        }

        public async Task<int> GetWishlistCountAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return 0;

            // Get customer's wishlist
            var wishlist = await FindAsync(
                w => w.CustomerId == customerId && !w.IsDeleted
            );

            if (wishlist == null)
                return 0;

            // Count items
            return await _dbContext.Set<TbWishlistItem>()
                .AsNoTracking()
                .CountAsync(wi => wi.WishlistId == wishlist.Id && !wi.IsDeleted);
        }

        public async Task<bool> IsInWishlistAsync(string customerId, Guid itemCombinationId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || itemCombinationId == Guid.Empty)
                return false;

            var item = await GetWishlistItemAsync(customerId, itemCombinationId);
            return item != null;
        }

        public async Task<bool> ValidateWishlistItemOwnershipAsync(
            string customerId,
            Guid wishlistItemId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || wishlistItemId == Guid.Empty)
                return false;

            // Get wishlist item
            var item = await _dbContext.Set<TbWishlistItem>()
                .AsNoTracking()
                .FirstOrDefaultAsync(wi => wi.Id == wishlistItemId && !wi.IsDeleted);

            if (item == null)
                return false;

            // Get wishlist and verify ownership
            var wishlist = await FindAsync(
                w => w.Id == item.WishlistId && !w.IsDeleted
            );

            return wishlist != null && wishlist.CustomerId == customerId;
        }
    }
}