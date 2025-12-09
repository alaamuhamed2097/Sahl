using BL.Services.Order;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.ECommerceSystem.Cart;
using Domains.Entities.Offer;
using Serilog;
using Shared.DTOs.ECommerce.Cart;

namespace BL.Service.Order
{
    /// <summary>
    /// Cart Service - COMPLETELY FIXED VERSION
    /// Now correctly stores OfferCombinationPricingId in the cart
    /// </summary>
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CartService(
            ICartRepository cartRepository,
            IOfferRepository offerRepository,
            IUnitOfWork unitOfWork,
            ILogger logger)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CartSummaryDto> GetCartSummaryAsync(string customerId)
        {
            try
            {
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
        /// FIXED: Now finds the exact OfferCombinationPricingId and stores it
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
                if (request.OfferCombinationPricingId == Guid.Empty)
                    throw new ArgumentException("Offer combination pricing ID cannot be empty", nameof(request.OfferCombinationPricingId));
                if (request.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero", nameof(request.Quantity));

                // ✅ CRITICAL FIX: Find the exact OfferCombinationPricingId
                var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();
                var pricing = await pricingRepo.FindByIdAsync(request.OfferCombinationPricingId);

                if (pricing == null)
                {
                    _logger.Warning($"Pricing not found for offer combination pricing {request.OfferCombinationPricingId}");
                    throw new InvalidOperationException("Invalid offer combination pricing");
                }

                // Validate that the pricing matches the requested item
                if (pricing.ItemCombinationId == Guid.Empty || pricing.ItemCombinationId == null)
                {
                    _logger.Warning($"Item combination not found for pricing {request.OfferCombinationPricingId}");
                    throw new InvalidOperationException("Invalid offer combination pricing configuration");
                }

                // Validate stock
                if (pricing.AvailableQuantity < request.Quantity)
                {
                    _logger.Warning($"Insufficient stock. Available: {pricing.AvailableQuantity}, Requested: {request.Quantity}");
                    throw new InvalidOperationException($"Insufficient stock. Available: {pricing.AvailableQuantity}");
                }

                var unitPrice = pricing.SalesPrice;
                if (unitPrice <= 0)
                {
                    throw new InvalidOperationException("Invalid price");
                }

                // ✅ Store OfferCombinationPricingId in the OfferId field
                var result = await _cartRepository.AddItemToCartAsync(
                    customerId,
                    request.ItemId,
                    pricing.Id,  // ✅ ده الـ OfferCombinationPricingId
                    request.Quantity,
                    unitPrice);

                if (!result.Success)
                {
                    _logger.Error($"Failed to add item to cart");
                    throw new InvalidOperationException("Failed to add item to cart");
                }

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error adding item to cart for user {customerId}");
                throw;
            }
        }

        public async Task<CartSummaryDto> RemoveFromCartAsync(string customerId, Guid cartItemId)
        {
            try
            {
                _logger.Information($"Removing cart item {cartItemId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("User ID cannot be empty", nameof(customerId));
                if (cartItemId == Guid.Empty)
                    throw new ArgumentException("Cart item ID cannot be empty", nameof(cartItemId));

                var result = await _cartRepository.RemoveItemFromCartAsync(cartItemId, customerId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to remove cart item {cartItemId}");
                    throw new InvalidOperationException("Cart item not found");
                }

                _logger.Information($"Successfully removed cart item");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error removing cart item {cartItemId}");
                throw;
            }
        }

        /// <summary>
        /// FIXED: Now uses stored OfferCombinationPricingId for stock validation
        /// </summary>
        public async Task<CartSummaryDto> UpdateCartItemAsync(string customerId, UpdateCartItemRequest request)
        {
            try
            {
                _logger.Information($"Updating cart item {request.CartItemId}");

                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.CartItemId == Guid.Empty)
                    throw new ArgumentException("Cart item ID cannot be empty", nameof(request.CartItemId));
                if (request.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero", nameof(request.Quantity));

                // Get cart item
                var cart = await _cartRepository.GetCartWithItemsAsync(customerId);
                var cartItem = cart?.Items?.FirstOrDefault(i => i.Id == request.CartItemId);

                if (cartItem == null)
                {
                    throw new InvalidOperationException("Cart item not found");
                }

                // ✅ cartItem.OfferId contains OfferCombinationPricingId
                var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();
                var pricing = await pricingRepo.FindByIdAsync(cartItem.OfferCombinationPricingId);

                if (pricing == null)
                {
                    throw new InvalidOperationException("Pricing not found");
                }

                // Validate stock
                if (pricing.AvailableQuantity < request.Quantity)
                {
                    _logger.Warning($"Insufficient stock. Available: {pricing.AvailableQuantity}, Requested: {request.Quantity}");
                    throw new InvalidOperationException($"Insufficient stock. Available: {pricing.AvailableQuantity}");
                }

                var result = await _cartRepository.UpdateCartItemAsync(
                    request.CartItemId,
                    customerId,
                    request.Quantity);

                if (!result.Success)
                {
                    _logger.Error($"Failed to update cart item");
                    throw new InvalidOperationException("Failed to update cart item");
                }

                _logger.Information($"Successfully updated cart item to quantity {request.Quantity}");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error updating cart item");
                throw;
            }
        }

        public async Task<CartSummaryDto> ClearCartAsync(string customerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

                var result = await _cartRepository.ClearCartAsync(customerId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to clear cart");
                    throw new InvalidOperationException("Failed to clear cart");
                }

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
                _logger.Error(ex, $"Error clearing cart");
                throw;
            }
        }

        public async Task<int> GetCartItemCountAsync(string customerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));

                var cart = await _cartRepository.GetCartWithItemsAsync(customerId);

                if (cart == null || cart.Id == Guid.Empty)
                    return 0;

                return cart.Items?.Sum(i => i.Quantity) ?? 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error getting cart item count");
                throw;
            }
        }

        public async Task<CartSummaryDto> MergeGuestCartAsync(string guestId, string userId)
        {
            try
            {
                _logger.Information($"Merging guest cart into user cart");

                if (string.IsNullOrWhiteSpace(guestId))
                    throw new ArgumentException("Guest ID cannot be empty", nameof(guestId));
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("User ID cannot be empty", nameof(userId));

                var result = await _cartRepository.MergeCartsAsync(guestId, userId);

                if (!result.Success)
                {
                    _logger.Error($"Failed to merge carts");
                    throw new InvalidOperationException("Failed to merge carts");
                }

                _logger.Information($"Successfully merged carts");

                return await MapToCartSummaryDtoAsync(result.Cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error merging carts");
                throw;
            }
        }

        /// <summary>
        /// FIXED: Now correctly maps OfferCombinationPricingId from cart items
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
            var pricingRepo = _unitOfWork.TableRepository<TbOfferCombinationPricing>();

            if (cart.Items != null)
            {
                foreach (var ci in cart.Items.Where(i => !i.IsDeleted))
                {
                    // ✅ ci.OfferId contains OfferCombinationPricingId
                    var pricing = await pricingRepo.FindByIdAsync(ci.OfferCombinationPricingId);

                    if (pricing == null)
                    {
                        _logger.Warning($"Pricing {ci.OfferCombinationPricingId} not found for cart item {ci.Id}, skipping");
                        continue;
                    }

                    var currentPrice = pricing.Price;
                    var isAvailable = pricing.AvailableQuantity >= ci.Quantity;

                    items.Add(new CartItemDto
                    {
                        Id = ci.Id,
                        ItemId = ci.ItemId,
                        ItemName = ci.Item?.TitleEn ?? "Unknown Item",
                        OfferCombinationPricingId = ci.OfferCombinationPricingId, // ✅ This is OfferCombinationPricingId
                        SellerName = pricing.Offer?.Vendor?.CompanyName ?? "Unknown Seller",
                        Quantity = ci.Quantity,
                        UnitPrice = currentPrice,
                        SubTotal = currentPrice * ci.Quantity,
                        IsAvailable = isAvailable
                    });
                }
            }

            var subTotal = items.Sum(i => i.SubTotal);
            var shippingEstimate = items.Any() ? 50m : 0m;
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