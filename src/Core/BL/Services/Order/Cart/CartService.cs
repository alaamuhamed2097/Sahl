using BL.Contracts.Service.Order.Cart;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Offer;
using Domains.Entities.Order.Cart;
using Serilog;
using Shared.DTOs.Order.Cart;

namespace BL.Services.Order.Cart;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ITableRepository<TbOfferCombinationPricing> _combinationPricingRepository;
    private readonly ILogger _logger;

    public CartService(
        ICartRepository cartRepository,
        ITableRepository<TbOfferCombinationPricing> combinationPricingRepository,
        ILogger logger)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _combinationPricingRepository = combinationPricingRepository ?? throw new ArgumentNullException(nameof(combinationPricingRepository));
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
                throw new ArgumentException($"Quantity must be greater than zero", nameof(request.Quantity));

            var pricing = await _combinationPricingRepository.FindByIdAsync(request.OfferCombinationPricingId);

            if (pricing == null)
            {
                _logger.Error($"Pricing not found for offer combination pricing {request.OfferCombinationPricingId}");
                throw new KeyNotFoundException($"Invalid offer combination pricing {request.OfferCombinationPricingId}");
            }

            // Validate stock
            if (pricing.AvailableQuantity < request.Quantity)
            {
                _logger.Error($"Insufficient stock. Available: {pricing.AvailableQuantity}, Requested: {request.Quantity}");
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
                pricing.Id,
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

    public async Task<BulkAddToCartResult> AddMultipleToCartAsync(string customerId, BulkAddToCartRequest request)
    {
        var result = new BulkAddToCartResult
        {
            SuccessfulItems = new List<BulkAddItemResult>(),
            FailedItems = new List<BulkAddItemFailure>()
        };

        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("User ID cannot be empty", nameof(customerId));

            if (request == null || request.Items == null || !request.Items.Any())
                throw new ArgumentException("At least one item must be provided", nameof(request));

            // Validate all items first
            var validationResults = new List<(AddToCartRequest item, TbOfferCombinationPricing pricing, string error)>();

            foreach (var item in request.Items)
            {
                try
                {
                    // Basic validation
                    if (item.ItemId == Guid.Empty)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Item ID cannot be empty"
                        });
                        continue;
                    }

                    if (item.OfferCombinationPricingId == Guid.Empty)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Offer combination pricing ID cannot be empty"
                        });
                        continue;
                    }

                    if (item.Quantity <= 0)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Quantity must be greater than zero"
                        });
                        continue;
                    }

                    // Get and validate pricing
                    var pricing = await _combinationPricingRepository.FindByIdAsync(item.OfferCombinationPricingId);

                    if (pricing == null)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Invalid offer combination pricing"
                        });
                        continue;
                    }

                    // Validate stock
                    if (pricing.AvailableQuantity < item.Quantity)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = $"Insufficient stock. Available: {pricing.AvailableQuantity}"
                        });
                        continue;
                    }

                    // Validate price
                    if (pricing.SalesPrice <= 0)
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Invalid price"
                        });
                        continue;
                    }

                    // Item passed validation
                    validationResults.Add((item, pricing, null));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error validating item {item.ItemId}");
                    result.FailedItems.Add(new BulkAddItemFailure
                    {
                        ItemId = item.ItemId,
                        OfferCombinationPricingId = item.OfferCombinationPricingId,
                        Quantity = item.Quantity,
                        ErrorMessage = $"Validation error: {ex.Message}"
                    });
                }
            }

            // If no items passed validation, return early
            if (!validationResults.Any())
            {
                result.CartSummary = await GetCartSummaryAsync(customerId);
                return result;
            }

            // Add validated items to cart
            foreach (var (item, pricing, _) in validationResults)
            {
                try
                {
                    var addResult = await _cartRepository.AddItemToCartAsync(
                        customerId,
                        item.ItemId,
                        pricing.Id,
                        item.Quantity,
                        pricing.SalesPrice);

                    if (addResult.Success)
                    {
                        result.SuccessfulItems.Add(new BulkAddItemResult
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            UnitPrice = pricing.SalesPrice,
                            SubTotal = pricing.SalesPrice * item.Quantity
                        });
                    }
                    else
                    {
                        result.FailedItems.Add(new BulkAddItemFailure
                        {
                            ItemId = item.ItemId,
                            OfferCombinationPricingId = item.OfferCombinationPricingId,
                            Quantity = item.Quantity,
                            ErrorMessage = "Failed to add item to cart"
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error adding item {item.ItemId} to cart");
                    result.FailedItems.Add(new BulkAddItemFailure
                    {
                        ItemId = item.ItemId,
                        OfferCombinationPricingId = item.OfferCombinationPricingId,
                        Quantity = item.Quantity,
                        ErrorMessage = $"Error: {ex.Message}"
                    });
                }
            }

            // Get updated cart summary
            result.CartSummary = await GetCartSummaryAsync(customerId);
            result.TotalItemsAdded = result.SuccessfulItems.Count;
            result.TotalItemsFailed = result.FailedItems.Count;

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error in bulk add to cart for user {customerId}");
            throw;
        }
    }

    public async Task<CartSummaryDto> RemoveFromCartAsync(string customerId, Guid cartItemId)
    {
        try
        {
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

            var pricing = await _combinationPricingRepository.FindByIdAsync(cartItem.OfferCombinationPricingId);

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

        if (cart.Items != null)
        {
            foreach (var ci in cart.Items.Where(i => !i.IsDeleted))
            {
                var pricing = await _combinationPricingRepository.FindByIdAsync(ci.OfferCombinationPricingId);

                if (pricing == null)
                {
                    _logger.Error($"Pricing not found for offer combination pricing ID {ci.OfferCombinationPricingId}");
                    throw new InvalidOperationException($"Pricing not found for offer combination pricing ID {ci.OfferCombinationPricingId}");
                }

                var currentPrice = pricing.Price;
                var isAvailable = pricing.AvailableQuantity >= ci.Quantity;

                items.Add(new CartItemDto
                {
                    Id = ci.Id,
                    ItemId = ci.ItemId,
                    ItemName = ci.Item?.TitleEn ?? "Unknown Item",
                    OfferCombinationPricingId = ci.OfferCombinationPricingId, // ✅ This is OfferCombinationPricingId
                    SellerName = "Unknown Seller",
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