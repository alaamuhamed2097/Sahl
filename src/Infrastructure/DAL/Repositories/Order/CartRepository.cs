using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using DAL.Exceptions;
using DAL.ResultModels;
using Domains.Entities.Offer;
using Domains.Entities.Order.Cart;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories.Order
{
    /// <summary>
    /// Implementation of cart repository support
    /// </summary>
    public class CartRepository : TableRepository<TbShoppingCart>, ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly DbSet<TbShoppingCartItem> _cartItems;

        public CartRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cartItems = _dbContext.Set<TbShoppingCartItem>();
        }

        /// <summary>
        /// Add item to cart with full transaction support
        /// </summary>
        public async Task<CartTransactionResult> AddItemToCartAsync(
            string customerId,
            Guid OfferCombinationPricingId,
            int quantity,
            decimal unitPrice,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new CartTransactionResult();
            var customerIdGuid = Guid.TryParse(customerId, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.ReadCommitted,
                cancellationToken);

            try
            {
                // Step 1: Get or create cart
                var cart = await GetOrCreateCartAsync(customerId, customerIdGuid, cancellationToken);

                if (cart.Id == Guid.Empty)
                {
                    // Cart creation failed
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 2: Check if item already exists in cart
                var existingItem = await _cartItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ci =>
                        ci.ShoppingCartId == cart.Id &&
                        ci.OfferCombinationPricingId == OfferCombinationPricingId &&
                        !ci.IsDeleted,
                        cancellationToken);

                if (existingItem != null)
                {
                    // Update existing item - need to attach it first since we used AsNoTracking
                    _dbContext.Attach(existingItem);
                    existingItem.Quantity += quantity;
                    existingItem.UpdatedDateUtc = DateTime.UtcNow;
                    existingItem.UpdatedBy = customerIdGuid;
                    existingItem.UnitPrice = unitPrice;

                    _cartItems.Update(existingItem);
                    transactionResult.AffectedItemIds.Add(existingItem.Id);
                }
                else
                {
                    var offerCombinationPricing = await _dbContext.Set<TbOfferCombinationPricing>()
                        .Include(ocp => ocp.ItemCombination)
                        .ThenInclude(ic => ic.Item)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ocp => ocp.Id == OfferCombinationPricingId, cancellationToken);

                    var item = offerCombinationPricing?.ItemCombination?.Item;
                    if (item == null)
                        throw new DataAccessException($"OfferCombinationPricing with ID {OfferCombinationPricingId} not found.", null, _logger);

                    // Create new cart item
                    var newCartItem = new TbShoppingCartItem
                    {
                        ShoppingCartId = cart.Id,
                        ItemId = item.Id,
                        OfferCombinationPricingId = OfferCombinationPricingId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        CreatedDateUtc = DateTime.UtcNow,
                        CreatedBy = customerIdGuid,
                        IsDeleted = false
                    };

                    await _cartItems.AddAsync(newCartItem, cancellationToken);
                    transactionResult.AffectedItemIds.Add(newCartItem.Id);
                }

                // Step 3: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 4: Refresh cart with items for calculation
                cart = await GetCartWithItemsAsync(cart.Id, cancellationToken);

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.Id = cart.Id;
                transactionResult.Cart = cart;
                transactionResult.TotalItems = cart.Items?.Sum(i => i.Quantity) ?? 0;
                transactionResult.CartTotal = CalculateCartTotal(cart);

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error adding item to cart for customer {customerId}");
                throw new DataAccessException($"Failed to add item to cart due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error adding item to cart for customer {customerId}");
                throw new DataAccessException($"Failed to add item to cart", ex, _logger);
            }
        }

        /// <summary>
        /// Update cart item quantity
        /// </summary>
        public async Task<CartTransactionResult> UpdateCartItemAsync(
            Guid cartItemId,
            string customerId,
            int newQuantity,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new CartTransactionResult();
            var customerIdGuid = Guid.TryParse(customerId, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.ReadCommitted,
                cancellationToken);

            try
            {
                _logger.Information($"Starting UpdateCartItem transaction for cart item: {cartItemId}, Customer: {customerId}");

                // Step 1: Get cart item with cart validation
                var cartItem = await _cartItems
                    .Include(ci => ci.ShoppingCart)
                    .FirstOrDefaultAsync(ci =>
                        ci.Id == cartItemId &&
                        !ci.IsDeleted &&
                        ci.ShoppingCart.UserId == customerId &&
                        !ci.ShoppingCart.IsDeleted,
                        cancellationToken);

                if (cartItem == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 2: Update quantity
                cartItem.Quantity = newQuantity;
                cartItem.UpdatedDateUtc = DateTime.UtcNow;
                cartItem.UpdatedBy = customerIdGuid;

                _cartItems.Update(cartItem);
                transactionResult.AffectedItemIds.Add(cartItemId);

                // Step 3: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 4: Refresh cart
                var cart = await GetCartWithItemsAsync(cartItem.ShoppingCartId, cancellationToken);

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.Id = cart.Id;
                transactionResult.Cart = cart;
                transactionResult.TotalItems = cart.Items?.Sum(i => i.Quantity) ?? 0;
                transactionResult.CartTotal = CalculateCartTotal(cart);

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error updating cart item {cartItemId} for customer {customerId}");
                throw new DataAccessException($"Failed to update cart item due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error updating cart item {cartItemId} for customer {customerId}");
                throw new DataAccessException($"Failed to update cart item", ex, _logger);
            }
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        public async Task<CartTransactionResult> RemoveItemFromCartAsync(
            Guid cartItemId,
            string customerId,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new CartTransactionResult();
            var customerIdGuid = Guid.TryParse(customerId, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.ReadCommitted,
                cancellationToken);

            try
            {
                // Step 1: Get cart item with cart validation
                var cartItem = await _cartItems
                    .Include(ci => ci.ShoppingCart)
                    .FirstOrDefaultAsync(ci =>
                        ci.Id == cartItemId &&
                        !ci.IsDeleted &&
                        ci.ShoppingCart.UserId == customerId &&
                        !ci.ShoppingCart.IsDeleted,
                        cancellationToken);

                if (cartItem == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                var cartId = cartItem.ShoppingCartId;

                // Step 2: Soft delete the cart item
                cartItem.IsDeleted = true;
                cartItem.UpdatedDateUtc = DateTime.UtcNow;
                cartItem.UpdatedBy = customerIdGuid;

                _cartItems.Update(cartItem);
                transactionResult.AffectedItemIds.Add(cartItemId);

                // Step 3: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 4: Refresh cart
                var cart = await GetCartWithItemsAsync(cartId, cancellationToken);

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.Id = cart.Id;
                transactionResult.Cart = cart;
                transactionResult.TotalItems = cart.Items?.Sum(i => i.Quantity) ?? 0;
                transactionResult.CartTotal = CalculateCartTotal(cart);

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error removing cart item {cartItemId} for customer {customerId}");
                throw new DataAccessException($"Failed to remove cart item due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error removing cart item {cartItemId} for customer {customerId}");
                throw new DataAccessException($"Failed to remove cart item", ex, _logger);
            }
        }

        /// <summary>
        /// Clear entire cart
        /// </summary>
        public async Task<CartTransactionResult> ClearCartAsync(
            string customerId,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new CartTransactionResult();
            var customerIdGuid = Guid.TryParse(customerId, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.ReadCommitted,
                cancellationToken);

            try
            {
                // Step 1: Get cart with items
                var cart = await GetCartWithItemsAsync(customerId, cancellationToken);

                if (cart == null || cart.Id == Guid.Empty)
                {
                    // No cart exists, return empty result
                    await transaction.CommitAsync(cancellationToken);
                    transactionResult.Success = true;
                    return transactionResult;
                }

                // Step 2: Get all active cart items
                var cartItems = await _cartItems
                    .Where(ci =>
                        ci.ShoppingCartId == cart.Id &&
                        !ci.IsDeleted)
                    .ToListAsync(cancellationToken);

                if (!cartItems.Any())
                {
                    // No items to clear
                    await transaction.CommitAsync(cancellationToken);
                    transactionResult.Success = true;
                    transactionResult.Id = cart.Id;
                    transactionResult.Cart = cart;
                    return transactionResult;
                }

                // Step 3: Soft delete all cart items
                var utcNow = DateTime.UtcNow;
                foreach (var item in cartItems)
                {
                    item.IsDeleted = true;
                    item.UpdatedDateUtc = utcNow;
                    item.UpdatedBy = customerIdGuid;
                    transactionResult.AffectedItemIds.Add(item.Id);
                }

                _cartItems.UpdateRange(cartItems);

                // Step 4: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Refresh cart (should now have no active items)
                cart = await GetCartWithItemsAsync(cart.Id, cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.Id = cart.Id;
                transactionResult.Cart = cart;
                transactionResult.TotalItems = 0;
                transactionResult.CartTotal = 0m;

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error clearing cart for customer {customerId}");
                throw new DataAccessException($"Failed to clear cart due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error clearing cart for customer {customerId}");
                throw new DataAccessException($"Failed to clear cart", ex, _logger);
            }
        }

        /// <summary>
        /// Get cart with all items and related data in single query
        /// </summary>
        public async Task<TbShoppingCart> GetCartWithItemsAsync(
            string customerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var cart = await _dbContext.Set<TbShoppingCart>()
                    .Include(c => c.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(i => i.Item)
                    .Include(c => c.Items.Where(i => !i.IsDeleted))
                        .ThenInclude(i => i.OfferCombinationPricing)
                            .ThenInclude(ocp => ocp.Offer)
                                .ThenInclude(o => o.Vendor)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.UserId == customerId && !c.IsDeleted,
                        cancellationToken);

                return cart ?? new TbShoppingCart { Id = Guid.Empty };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting cart with items for customer {customerId}");
                throw new DataAccessException($"Failed to retrieve cart with items", ex, _logger);
            }
        }

        /// <summary>
        /// Merge carts (useful for guest to logged-in user conversion)
        /// </summary>
        public async Task<CartTransactionResult> MergeCartsAsync(
            string sourceCustomerId,
            string targetCustomerId,
            CancellationToken cancellationToken = default)
        {
            var transactionResult = new CartTransactionResult();
            var targetCustomerIdGuid = Guid.TryParse(targetCustomerId, out var parsedId) ? parsedId : Guid.Empty;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                System.Data.IsolationLevel.Serializable,
                cancellationToken);

            try
            {
                // Step 1: Get source cart
                var sourceCart = await GetCartWithItemsAsync(sourceCustomerId, cancellationToken);
                if (sourceCart.Id == Guid.Empty || sourceCart.Items?.Any() != true)
                {
                    // No source cart or items to merge
                    await transaction.CommitAsync(cancellationToken);
                    transactionResult.Success = true;
                    return transactionResult;
                }

                // Step 2: Get or create target cart
                var targetCart = await GetOrCreateCartAsync(targetCustomerId, targetCustomerIdGuid, cancellationToken);
                if (targetCart.Id == Guid.Empty)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 3: Merge items
                var mergedItemIds = new List<Guid>();
                foreach (var sourceItem in sourceCart.Items.Where(i => !i.IsDeleted))
                {
                    // Check if item exists in target cart
                    var existingTargetItem = await _cartItems
                        .FirstOrDefaultAsync(ci =>
                            ci.ShoppingCartId == targetCart.Id &&
                            ci.OfferCombinationPricingId == sourceItem.OfferCombinationPricingId &&
                            !ci.IsDeleted,
                            cancellationToken);

                    if (existingTargetItem != null)
                    {
                        // Update quantity
                        existingTargetItem.Quantity += sourceItem.Quantity;
                        existingTargetItem.UpdatedDateUtc = DateTime.UtcNow;
                        existingTargetItem.UpdatedBy = targetCustomerIdGuid;
                        _cartItems.Update(existingTargetItem);
                        mergedItemIds.Add(existingTargetItem.Id);
                    }
                    else
                    {

                        var offerCombinationPricing = await _dbContext.Set<TbOfferCombinationPricing>()
                            .Include(ocp => ocp.ItemCombination)
                            .ThenInclude(ic => ic.Item)
                            .AsSplitQuery()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(ocp => ocp.Id == sourceItem.OfferCombinationPricingId, cancellationToken);

                        var item = offerCombinationPricing?.ItemCombination?.Item;
                        if (item == null)
                            throw new DataAccessException($"OfferCombinationPricing with ID {sourceItem.OfferCombinationPricingId} not found.", null, _logger);

                        // Create new item in target cart
                        var newCartItem = new TbShoppingCartItem
                        {
                            ShoppingCartId = targetCart.Id,
                            OfferCombinationPricingId = sourceItem.OfferCombinationPricingId,
                            ItemId = item.Id,
                            Quantity = sourceItem.Quantity,
                            UnitPrice = sourceItem.UnitPrice,
                            CreatedDateUtc = DateTime.UtcNow,
                            CreatedBy = targetCustomerIdGuid,
                            IsDeleted = false
                        };
                        await _cartItems.AddAsync(newCartItem, cancellationToken);
                        mergedItemIds.Add(newCartItem.Id);
                    }

                    // Soft delete source item
                    sourceItem.IsDeleted = true;
                    sourceItem.UpdatedDateUtc = DateTime.UtcNow;
                    sourceItem.UpdatedBy = targetCustomerIdGuid;
                    _cartItems.Update(sourceItem);
                }

                // Step 4: Save changes
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                // Step 5: Commit transaction
                await transaction.CommitAsync(cancellationToken);

                // Refresh target cart
                targetCart = await GetCartWithItemsAsync(targetCart.Id, cancellationToken);

                // Set result
                transactionResult.Success = true;
                transactionResult.Id = targetCart.Id;
                transactionResult.Cart = targetCart;
                transactionResult.TotalItems = targetCart.Items?.Sum(i => i.Quantity) ?? 0;
                transactionResult.CartTotal = CalculateCartTotal(targetCart);
                transactionResult.AffectedItemIds = mergedItemIds;

                return transactionResult;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(dbEx, $"Database error merging carts from {sourceCustomerId} to {targetCustomerId}");
                throw new DataAccessException($"Failed to merge carts due to database error", dbEx, _logger);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.Error(ex, $"Error merging carts from {sourceCustomerId} to {targetCustomerId}");
                throw new DataAccessException($"Failed to merge carts", ex, _logger);
            }
        }

        // ============================================
        // PRIVATE HELPER METHODS
        // ============================================

        /// <summary>
        /// Get or create cart for customer
        /// </summary>
        private async Task<TbShoppingCart> GetOrCreateCartAsync(
            string customerId,
            Guid customerIdGuid,
            CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _dbContext.Set<TbShoppingCart>()
                    .FirstOrDefaultAsync(c =>
                        c.UserId == customerId &&
                        !c.IsDeleted,
                        cancellationToken);

                if (cart != null)
                    return cart;

                // Create new cart
                cart = new TbShoppingCart
                {
                    UserId = customerId,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = customerIdGuid,
                    IsDeleted = false
                };

                await _dbContext.Set<TbShoppingCart>().AddAsync(cart, cancellationToken);
                var saveResult = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!saveResult)
                {
                    _logger.Error($"Failed to create cart for customer {customerId}");
                    return new TbShoppingCart { Id = Guid.Empty };
                }

                _logger.Information($"Created new cart {cart.Id} for customer {customerId}");
                return cart;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting or creating cart for customer {customerId}");
                return new TbShoppingCart { Id = Guid.Empty };
            }
        }

        /// <summary>
        /// Get cart by ID with all items
        /// </summary>
        private async Task<TbShoppingCart> GetCartWithItemsAsync(
            Guid cartId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TbShoppingCart>()
                               .AsNoTracking()
                .Include(c => c.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.Item)
                .Include(c => c.Items.Where(i => !i.IsDeleted))
                    .ThenInclude(i => i.OfferCombinationPricing)
                        .ThenInclude(ocp => ocp.Offer)
                            .ThenInclude(o => o.Vendor)
                                .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(c => c.Id == cartId && !c.IsDeleted, cancellationToken);
        }

        /// <summary>
        /// Calculate cart total from items
        /// </summary>
        private decimal CalculateCartTotal(TbShoppingCart cart)
        {
            if (cart?.Items?.Any() != true)
                return 0m;

            decimal total = 0;
            foreach (var item in cart.Items.Where(i => !i.IsDeleted))
            {
                var price = item.OfferCombinationPricing?.Offer?.OfferCombinationPricings?.FirstOrDefault()?.Price ?? item.UnitPrice;
                total += price * item.Quantity;
            }

            return total;
        }

        /// <summary>
        /// Override base GetAsync to include items by default
        /// </summary>
        public override async Task<IEnumerable<TbShoppingCart>> GetAsync(
            Expression<Func<TbShoppingCart, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<TbShoppingCart> query = _dbContext.Set<TbShoppingCart>()
                    .AsNoTracking()
                    .Include(c => c.Items.Where(i => !i.IsDeleted));

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                query = query.Where(e => !e.IsDeleted);

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAsync), $"Error occurred while filtering active cart entities", ex);
                return Enumerable.Empty<TbShoppingCart>();
            }
        }
    }
}