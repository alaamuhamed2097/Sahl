using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.ResultModels;
using Domains.Entities.ECommerceSystem.Cart;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    /// <summary>
    /// Implementation of cart repository support
    /// </summary>
    public class CartRepository : TableRepository<TbShoppingCart>, ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly DbSet<TbShoppingCartItem> _cartItems;

        public CartRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
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
            Guid itemId,
            Guid offerId,
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
                _logger.Information($"Starting AddItemToCart transaction for customer: {customerId}, Item: {itemId}, Offer: {offerId}");

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
                        ci.ItemId == itemId &&
                        ci.OfferId == offerId &&
                        ci.CurrentState == (int)Common.Enumerations.EntityState.Active,
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

                    _logger.Information($"Updated existing cart item {existingItem.Id}, new quantity: {existingItem.Quantity}");
                }
                else
                {
                    // Create new cart item
                    var newCartItem = new TbShoppingCartItem
                    {
                        ShoppingCartId = cart.Id,
                        ItemId = itemId,
                        OfferId = offerId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        CreatedDateUtc = DateTime.UtcNow,
                        CreatedBy = customerIdGuid,
                        CurrentState = (int)Common.Enumerations.EntityState.Active
                    };

                    await _cartItems.AddAsync(newCartItem, cancellationToken);
                    transactionResult.AffectedItemIds.Add(newCartItem.Id);

                    _logger.Information($"Added new cart item for item {itemId}, quantity: {quantity}");
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

                _logger.Information($"Successfully added item to cart. Cart ID: {cart.Id}, Total Items: {transactionResult.TotalItems}");

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
                        ci.CurrentState == (int)Common.Enumerations.EntityState.Active &&
                        ci.ShoppingCart.UserId == customerId &&
                        ci.ShoppingCart.CurrentState == (int)Common.Enumerations.EntityState.Active,
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

                _logger.Information($"Successfully updated cart item {cartItemId}. New quantity: {newQuantity}");

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
                _logger.Information($"Starting RemoveItemFromCart transaction for cart item: {cartItemId}, Customer: {customerId}");

                // Step 1: Get cart item with cart validation
                var cartItem = await _cartItems
                    .Include(ci => ci.ShoppingCart)
                    .FirstOrDefaultAsync(ci =>
                        ci.Id == cartItemId &&
                        ci.CurrentState == (int)Common.Enumerations.EntityState.Active &&
                        ci.ShoppingCart.UserId == customerId &&
                        ci.ShoppingCart.CurrentState == (int)Common.Enumerations.EntityState.Active,
                        cancellationToken);

                if (cartItem == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    transactionResult.Success = false;
                    return transactionResult;
                }

                var cartId = cartItem.ShoppingCartId;

                // Step 2: Soft delete the cart item
                cartItem.CurrentState = (int)Common.Enumerations.EntityState.Deleted;
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

                _logger.Information($"Successfully removed cart item {cartItemId} from cart {cartId}");

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
                _logger.Information($"Starting ClearCart transaction for customer: {customerId}");

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
                        ci.CurrentState == (int)Common.Enumerations.EntityState.Active)
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
                    item.CurrentState = (int)Common.Enumerations.EntityState.Deleted;
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

                _logger.Information($"Successfully cleared {cartItems.Count} items from cart {cart.Id}");

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
                    .AsNoTracking()
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Item)
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Offer)
                    .ThenInclude(o => o.User)
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Offer)
                    .ThenInclude(o => o.OfferCombinationPricings)
                    .FirstOrDefaultAsync(c =>
                        c.UserId == customerId &&
                        c.CurrentState == (int)Common.Enumerations.EntityState.Active,
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
                _logger.Information($"Starting MergeCarts transaction from {sourceCustomerId} to {targetCustomerId}");

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
                foreach (var sourceItem in sourceCart.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                {
                    // Check if item exists in target cart
                    var existingTargetItem = await _cartItems
                        .FirstOrDefaultAsync(ci =>
                            ci.ShoppingCartId == targetCart.Id &&
                            ci.ItemId == sourceItem.ItemId &&
                            ci.OfferId == sourceItem.OfferId &&
                            ci.CurrentState == (int)Common.Enumerations.EntityState.Active,
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
                        // Create new item in target cart
                        var newCartItem = new TbShoppingCartItem
                        {
                            ShoppingCartId = targetCart.Id,
                            ItemId = sourceItem.ItemId,
                            OfferId = sourceItem.OfferId,
                            Quantity = sourceItem.Quantity,
                            UnitPrice = sourceItem.UnitPrice,
                            CreatedDateUtc = DateTime.UtcNow,
                            CreatedBy = targetCustomerIdGuid,
                            CurrentState = (int)Common.Enumerations.EntityState.Active
                        };
                        await _cartItems.AddAsync(newCartItem, cancellationToken);
                        mergedItemIds.Add(newCartItem.Id);
                    }

                    // Soft delete source item
                    sourceItem.CurrentState = (int)Common.Enumerations.EntityState.Deleted;
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

                _logger.Information($"Successfully merged {sourceCart.Items.Count} items from cart {sourceCart.Id} to cart {targetCart.Id}");

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
                        c.CurrentState == (int)Common.Enumerations.EntityState.Active,
                        cancellationToken);

                if (cart != null)
                    return cart;

                // Create new cart
                cart = new TbShoppingCart
                {
                    UserId = customerId,
                    IsActive = true,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = customerIdGuid,
                    CurrentState = (int)Common.Enumerations.EntityState.Active
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
            try
            {
                var cart = await _dbContext.Set<TbShoppingCart>()
                    .AsNoTracking()
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Item)
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Offer)
                    .ThenInclude(o => o.User)
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                    .ThenInclude(i => i.Offer)
                    .ThenInclude(o => o.OfferCombinationPricings)
                    .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);

                return cart ?? new TbShoppingCart { Id = Guid.Empty };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting cart {cartId} with items");
                return new TbShoppingCart { Id = Guid.Empty };
            }
        }

        /// <summary>
        /// Calculate cart total from items
        /// </summary>
        private decimal CalculateCartTotal(TbShoppingCart cart)
        {
            if (cart?.Items?.Any() != true)
                return 0m;

            decimal total = 0;
            foreach (var item in cart.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
            {
                var price = item.Offer?.OfferCombinationPricings?.FirstOrDefault()?.Price ?? item.UnitPrice;
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
                    .Include(c => c.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active));

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                query = query.Where(e => e.CurrentState == (int)Common.Enumerations.EntityState.Active);

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