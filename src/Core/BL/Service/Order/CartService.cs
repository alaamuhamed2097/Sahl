using BL.Services.Order;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Cart;
using Serilog;
using Shared.DTOs.ECommerce.Cart;

namespace BL.Service.Order
{
    /// <summary>
    /// Cart Service - Handles shopping cart operations
    /// Uses specialized ICartRepository with transaction support for data persistence
    /// </summary>
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ILogger _logger;

        public CartService(
            ICartRepository cartRepository,
            IOfferRepository offerRepository,
            ILogger logger)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get cart summary for a customer
        /// </summary>
        public async Task<CartSummaryDto> GetCartSummaryAsync(string customerId)
        {
            try
            {
                _logger.Information($"Getting cart summary for user {customerId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("User ID cannot be empty", nameof(customerId));

                var cart = await _cartRepository.GetCartWithItemsAsync(customerId);

                if (cart == null || cart.Id == Guid.Empty)
                {
                    _logger.Information($"No cart found for user {customerId}, returning empty cart");
                    return new CartSummaryDto { CartId = Guid.Empty, Items = new() };
                }

                return await MapToCartSummaryDtoAsync(cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting cart summary for user {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Add item to cart with transactional support
        /// </summary>
        public async Task<CartSummaryDto> AddToCartAsync(string customerId, AddToCartRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("User ID cannot be empty", nameof(customerId));
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.ItemId == Guid.Empty)
                    throw new ArgumentException("Item ID cannot be empty", nameof(request.ItemId));
                if (request.OfferId == Guid.Empty)
                    throw new ArgumentException("Offer ID cannot be empty", nameof(request.OfferId));
                if (request.ItemCombinationId == Guid.Empty)
                    throw new ArgumentException("Item combination ID cannot be empty", nameof(request.ItemCombinationId));
                if (request.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero", nameof(request.Quantity));

                _logger.Information($"Adding item {request.ItemId} (combination: {request.ItemCombinationId}) to cart for user {customerId}");

                // Validate offer exists and has stock
                var hasStock = await _offerRepository.CheckOfferStockAsync(
                    request.OfferId,
                    request.ItemCombinationId,
                    request.Quantity);

                if (!hasStock)
                {
                    _logger.Warning($"Insufficient stock for offer {request.OfferId}, combination {request.ItemCombinationId}");
                    throw new InvalidOperationException("Insufficient stock for requested quantity");
                }

                // Fetch the unit price from the database
                var unitPrice = await GetOfferPriceAsync(request.OfferId, request.ItemCombinationId);
                if (unitPrice <= 0)
                {
                    _logger.Warning($"Invalid or missing price for offer {request.OfferId}, combination {request.ItemCombinationId}");
                    throw new InvalidOperationException($"Unable to retrieve valid price for offer {request.OfferId}");
                }

                // Use the transactional cart repository method
                var result = await _cartRepository.AddItemToCartAsync(
                    customerId,
                    request.ItemId,
                    request.OfferId,
                    request.Quantity,
                    unitPrice);

                if (!result.Success)
                {
                    _logger.Error($"Failed to add item to cart for user {customerId}");
                    throw new InvalidOperationException("Failed to add item to cart");
                }

                _logger.Information($"Successfully added item to cart. Total items: {result.TotalItems}, Cart total: {result.CartTotal}");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error adding item to cart for user {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Remove item from cart with transactional support
        /// </summary>
        public async Task<CartSummaryDto> RemoveFromCartAsync(string customerId, Guid cartItemId)
        {
            try
            {
                _logger.Information($"Removing cart item {cartItemId} for user {customerId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("User ID cannot be empty", nameof(customerId));
                if (cartItemId == Guid.Empty)
                    throw new ArgumentException("Cart item ID cannot be empty", nameof(cartItemId));

                // Use the transactional cart repository method
                var result = await _cartRepository.RemoveItemFromCartAsync(cartItemId, customerId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to remove cart item {cartItemId} for user {customerId}");
                    throw new InvalidOperationException("Cart item not found or cannot be removed");
                }

                _logger.Information($"Successfully removed cart item {cartItemId}. Remaining items: {result.TotalItems}");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error removing cart item {cartItemId} for user {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Update cart item quantity with transactional support
        /// </summary>
        public async Task<CartSummaryDto> UpdateCartItemAsync(string customerId, UpdateCartItemRequest request)
        {
            try
            {
                _logger.Information($"Updating cart item {request.CartItemId} for customer {customerId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.CartItemId == Guid.Empty)
                    throw new ArgumentException("Cart item ID cannot be empty", nameof(request.CartItemId));
                if (request.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero", nameof(request.Quantity));

                // Get the current cart item to validate stock
                var cart = await _cartRepository.GetCartWithItemsAsync(customerId);
                var cartItem = cart?.Items?.FirstOrDefault(i => i.Id == request.CartItemId);

                if (cartItem == null)
                {
                    throw new InvalidOperationException("Cart item not found");
                }

                // Get the item combination ID from the offer's pricing
                // Since TbShoppingCartItem doesn't have ItemCombinationId, we need to find it
                var offerPricings = await _offerRepository.GetOfferPricingCombinationsAsync(cartItem.OfferId);
                var itemCombinationId = offerPricings.FirstOrDefault()?.ItemCombinationId ?? Guid.Empty;

                if (itemCombinationId == Guid.Empty)
                {
                    throw new InvalidOperationException("Unable to determine item combination for cart item");
                }

                // Validate stock for new quantity
                var hasStock = await _offerRepository.CheckOfferStockAsync(
                    cartItem.OfferId,
                    itemCombinationId,
                    request.Quantity);

                if (!hasStock)
                {
                    _logger.Warning($"Insufficient stock for offer {cartItem.OfferId}, combination {itemCombinationId}");
                    throw new InvalidOperationException("Insufficient stock for requested quantity");
                }

                // Use the transactional cart repository method
                var result = await _cartRepository.UpdateCartItemAsync(
                    request.CartItemId,
                    customerId,
                    request.Quantity);

                if (!result.Success)
                {
                    _logger.Error($"Failed to update cart item {request.CartItemId} for user {customerId}");
                    throw new InvalidOperationException("Cart item not found or cannot be updated");
                }

                _logger.Information($"Successfully updated cart item {request.CartItemId} to quantity {request.Quantity}");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating cart item {request?.CartItemId} for user {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Clear entire cart with transactional support
        /// </summary>
        public async Task<CartSummaryDto> ClearCartAsync(string customerId)
        {
            try
            {
                _logger.Information($"Clearing cart for customer {customerId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

                // Use the transactional cart repository method
                var result = await _cartRepository.ClearCartAsync(customerId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to clear cart for user {customerId}");
                    throw new InvalidOperationException("Failed to clear cart");
                }

                _logger.Information($"Successfully cleared cart for customer {customerId}");

                return new CartSummaryDto
                {
                    CartId = result.Id,
                    Items = new(),
                    SubTotal = 0m,
                    ShippingEstimate = 0m,
                    TaxEstimate = 0m,
                    TotalEstimate = 0m,
                    ItemCount = 0
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error clearing cart for customer {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Get cart item count
        /// </summary>
        public async Task<int> GetCartItemCountAsync(string customerId)
        {
            try
            {
                _logger.Information($"Getting cart item count for customer {customerId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

                var cart = await _cartRepository.GetCartWithItemsAsync(customerId);

                if (cart == null || cart.Id == Guid.Empty)
                {
                    _logger.Information($"No cart found for customer {customerId}");
                    return 0;
                }

                var count = cart.Items?.Sum(i => i.Quantity) ?? 0;
                _logger.Information($"Cart item count for customer {customerId}: {count}");
                return count;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting cart item count for customer {customerId}");
                throw;
            }
        }

        /// <summary>
        /// Merge guest cart into user cart (useful for post-login scenarios)
        /// </summary>
        public async Task<CartSummaryDto> MergeGuestCartAsync(string guestId, string userId)
        {
            try
            {
                _logger.Information($"Merging guest cart {guestId} into user cart {userId}");

                if (string.IsNullOrWhiteSpace(guestId))
                    throw new ArgumentException("Guest ID cannot be empty", nameof(guestId));
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("User ID cannot be empty", nameof(userId));

                var result = await _cartRepository.MergeCartsAsync(guestId, userId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to merge guest cart {guestId} into user cart {userId}");
                    throw new InvalidOperationException("Failed to merge carts");
                }

                _logger.Information($"Successfully merged carts. Total items: {result.TotalItems}, Cart total: {result.CartTotal}");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error merging guest cart {guestId} into user cart {userId}");
                throw;
            }
        }

        /// <summary>
        /// Get offer price from database using the offer repository
        /// </summary>
        private async Task<decimal> GetOfferPriceAsync(Guid offerId, Guid itemCombinationId)
        {
            try
            {
                // Get all pricing combinations for the offer
                var pricingCombinations = await _offerRepository.GetOfferPricingCombinationsAsync(offerId);

                if (pricingCombinations == null || !pricingCombinations.Any())
                {
                    _logger.Warning($"No pricing combinations found for offer {offerId}");
                    return 0m;
                }

                // Find the specific pricing for this item combination
                var pricing = pricingCombinations.FirstOrDefault(p => p.ItemCombinationId == itemCombinationId);

                if (pricing == null)
                {
                    _logger.Warning($"No pricing found for offer {offerId}, combination {itemCombinationId}");
                    return 0m;
                }

                if (pricing.Price <= 0)
                {
                    _logger.Warning($"Invalid price {pricing.Price} for offer {offerId}, combination {itemCombinationId}");
                    return 0m;
                }

                _logger.Information($"Retrieved price {pricing.Price} for offer {offerId}, combination {itemCombinationId}");
                return pricing.Price;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error fetching price for offer {offerId}, combination {itemCombinationId}");
                return 0m;
            }
        }

        /// <summary>
        /// Map cart entity to CartSummaryDto with async price lookup
        /// </summary>
        private async Task<CartSummaryDto> MapToCartSummaryDtoAsync(TbShoppingCart cart)
        {
            if (cart == null || cart.Id == Guid.Empty)
                return new CartSummaryDto
                {
                    CartId = Guid.Empty,
                    Items = new(),
                    SubTotal = 0m,
                    ShippingEstimate = 0m,
                    TaxEstimate = 0m,
                    TotalEstimate = 0m,
                    ItemCount = 0
                };

            var items = new List<CartItemDto>();

            // Process each cart item
            if (cart.Items != null)
            {
                foreach (var ci in cart.Items.Where(i => i.CurrentState == (int)Common.Enumerations.EntityState.Active))
                {
                    // Get the current price from the offer
                    var currentPrice = ci.UnitPrice; // Default to stored price

                    // Try to get current price from offer
                    var offerPricings = await _offerRepository.GetOfferPricingCombinationsAsync(ci.OfferId);
                    if (offerPricings.Any())
                    {
                        // Use the first available pricing (or you could add logic to determine which one)
                        currentPrice = offerPricings.First().Price;
                    }

                    items.Add(new CartItemDto
                    {
                        Id = ci.Id,
                        ItemId = ci.ItemId,
                        ItemName = ci.Item?.TitleEn ?? "Unknown Item",
                        OfferId = ci.OfferId,
                        SellerName = ci.Offer?.User?.UserName ?? "Unknown Seller",
                        Quantity = ci.Quantity,
                        UnitPrice = currentPrice,
                        SubTotal = currentPrice * ci.Quantity,
                        IsAvailable = true
                    });
                }
            }

            var subTotal = items.Sum(i => i.SubTotal);
            var shippingEstimate = items.Any() ? 50m : 0m; // Only charge shipping if there are items
            var taxRate = 0.14m;
            var taxEstimate = subTotal * taxRate;

            return new CartSummaryDto
            {
                CartId = cart.Id,
                Items = items,
                SubTotal = subTotal,
                ShippingEstimate = shippingEstimate,
                TaxEstimate = taxEstimate,
                TotalEstimate = subTotal + shippingEstimate + taxEstimate,
                ItemCount = items.Sum(i => i.Quantity)
            };
        }
    }
}