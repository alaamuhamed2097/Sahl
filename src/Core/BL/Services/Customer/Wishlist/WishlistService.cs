using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.Order.Cart;
using Common.Enumerations.Offer;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Offer;
using Serilog;
using Shared.DTOs.Customer.Wishlist;
using Shared.DTOs.Order.Cart;

namespace BL.Services.Customer.Wishlist;

/// <summary>
/// Service for managing customer wishlists
/// Uses custom repository for customer-scoped operations
/// </summary>
public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly ITableRepository<TbItemCombination> _combinationRepository;
    private readonly ITableRepository<TbOfferCombinationPricing> _offerCombinationPricingRepository;
    private readonly ITableRepository<TbItem> _itemRepository;
    private readonly ITableRepository<TbOffer> _offerRepository;
    private readonly ICartService _cartService;
    private readonly ILogger _logger;

    public WishlistService(
        IWishlistRepository wishlistRepository,
        ITableRepository<TbItemCombination> combinationRepository,
        ITableRepository<TbOfferCombinationPricing> offerCombinationPricingRepository,
    ITableRepository<TbItem> itemRepository,
        ITableRepository<TbOffer> offerRepository,
        ICartService cartService,
        ILogger logger)
    {
        _wishlistRepository = wishlistRepository ?? throw new ArgumentNullException(nameof(wishlistRepository));
        _combinationRepository = combinationRepository;
        _offerCombinationPricingRepository = offerCombinationPricingRepository;
        _offerRepository = offerRepository;
        _itemRepository = itemRepository;
        _offerRepository = offerRepository;
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // ==================== POST /api/wishlist/add ====================

    public async Task<AddToWishlistResponse> AddToWishlistAsync(string customerId, AddToWishlistRequest request)
    {
        try
        {
            // 1. Validate customer
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            // 2. Check product (item combination) exists
            var combination = await _combinationRepository.FindAsync(
                c => c.Id == request.ItemCombinationId && !c.IsDeleted
            );

            if (combination == null)
                throw new NotFoundException("Item combination not found.", _logger);

            // 3. Add to wishlist using repository (handles customer validation)
            var customerIdGuid = Guid.Parse(customerId);
            var wishlistItem = await _wishlistRepository.AddToWishlistAsync(
                customerId,
                request.ItemCombinationId,
                customerIdGuid
            );

            if (wishlistItem == null)
            {
                // Already in wishlist
                var existingItem = await _wishlistRepository.GetWishlistItemAsync(
                    customerId,
                    request.ItemCombinationId
                );

                return new AddToWishlistResponse
                {
                    Success = true,
                    Message = "This item is already in your wishlist.",
                    WishlistItemId = existingItem?.Id,
                    WasAlreadyInWishlist = true
                };
            }

            return new AddToWishlistResponse
            {
                Success = true,
                Message = "Item added to wishlist successfully.",
                WishlistItemId = wishlistItem.Id,
                WasAlreadyInWishlist = false
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error adding item to wishlist for customer {CustomerId}", customerId);
            throw;
        }
    }

    // ==================== DELETE /api/wishlist/remove/{itemCombinationId} ====================

    public async Task<bool> RemoveFromWishlistAsync(string customerId, Guid itemCombinationId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            if (itemCombinationId == Guid.Empty)
                throw new ArgumentException("Item combination ID is required.", nameof(itemCombinationId));

            var customerIdGuid = Guid.Parse(customerId);

            // Remove using repository (handles customer validation)
            var result = await _wishlistRepository.RemoveFromWishlistAsync(
                customerId,
                itemCombinationId,
                customerIdGuid
            );

            if (!result)
            {
                _logger.Warning(
                    "Item combination {CombinationId} not found in wishlist for customer {CustomerId}",
                    itemCombinationId, customerId
                );
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error removing item {CombinationId} from wishlist for customer {CustomerId}",
                itemCombinationId, customerId
            );
            throw;
        }
    }

    // ==================== GET /api/wishlist ====================

    public async Task<AdvancedPagedResult<WishlistItemDto>> GetWishlistItemsAsync(
        string customerId,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            // Get paginated items using repository
            var pagedResult = await _wishlistRepository.GetWishlistItemsPagedAsync(
                customerId,
                page,
                pageSize
            );

            // Map to DTOs with full product details
            var itemDtos = new List<WishlistItemDto>();
            foreach (var item in pagedResult.Items)
            {
                var dto = await MapWishlistItemToDto(item);
                itemDtos.Add(dto);
            }

            return new AdvancedPagedResult<WishlistItemDto>
            {
                Items = itemDtos,
                TotalRecords = pagedResult.TotalRecords,
                PageSize = pagedResult.PageSize,
                PageNumber = pagedResult.PageNumber,
                TotalPages = pagedResult.TotalPages
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving wishlist items for customer {CustomerId}", customerId);
            throw;
        }
    }

    // ==================== DELETE /api/wishlist/clear ====================

    public async Task<bool> ClearWishlistAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            var customerIdGuid = Guid.Parse(customerId);

            // Clear using repository (handles customer validation)
            var removedCount = await _wishlistRepository.ClearWishlistAsync(
                customerId,
                customerIdGuid
            );

            if (removedCount == 0)
            {
                _logger.Warning("Wishlist not found or empty for customer {CustomerId}", customerId);
                return false;
            }

            _logger.Information(
                "Wishlist cleared for customer {CustomerId}. {Count} items removed.",
                customerId, removedCount
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error clearing wishlist for customer {CustomerId}", customerId);
            throw;
        }
    }

    // ==================== POST /api/wishlist/move-to-cart ====================

    public async Task<MoveToCartResponse> MoveToCartAsync(string customerId, MoveToCartRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            // 1. Verify item is in customer's wishlist
            var wishlistItem = await _wishlistRepository.GetWishlistItemAsync(
                customerId,
                request.ItemCombinationId
            );

            if (wishlistItem == null)
            {
                _logger.Warning(
                    "Item combination {CombinationId} not found in wishlist for customer {CustomerId}",
                    request.ItemCombinationId, customerId
                );

                return new MoveToCartResponse
                {
                    Success = false,
                    Message = "Item not found in your wishlist.",
                    AddedToCart = false,
                    RemovedFromWishlist = false,
                    ErrorReason = "Item not in wishlist"
                };
            }

            // 2. Get item combination
            var combination = await _combinationRepository.FindAsync(
                c => c.Id == request.ItemCombinationId && !c.IsDeleted
            );

            if (combination == null)
            {
                return new MoveToCartResponse
                {
                    Success = false,
                    Message = "Product not found.",
                    AddedToCart = false,
                    RemovedFromWishlist = false,
                    ErrorReason = "Product not found"
                };
            }

            // 3. Get BuyBox offer pricing for combination
            var buyBoxOffer = await GetBuyBoxOfferForCombinationAsync(request.ItemCombinationId);

            if (buyBoxOffer == null || !buyBoxOffer.IsActive)
            {
                _logger.Warning(
                    "No active BuyBox offer found for combination {CombinationId}",
                    request.ItemCombinationId
                );

                return new MoveToCartResponse
                {
                    Success = false,
                    Message = "Product is not available for purchase.",
                    AddedToCart = false,
                    RemovedFromWishlist = false,
                    ErrorReason = "No active offer found"
                };
            }

            // 4. Add to cart using ICartService
            var addToCartRequest = new AddToCartRequest
            {
                OfferCombinationPricingId = buyBoxOffer.OfferPricingId,
                Quantity = request.Quantity
            };

            CartSummaryDto cartSummary;
            try
            {
                cartSummary = await _cartService.AddToCartAsync(customerId, addToCartRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(
                    ex,
                    "Error adding item to cart for customer {CustomerId}",
                    customerId
                );

                return new MoveToCartResponse
                {
                    Success = false,
                    Message = "Failed to add item to cart.",
                    AddedToCart = false,
                    RemovedFromWishlist = false,
                    ErrorReason = ex.Message
                };
            }

            // 5. Remove from wishlist using repository
            var customerIdGuid = Guid.Parse(customerId);
            var removed = await _wishlistRepository.RemoveFromWishlistAsync(
                customerId,
                request.ItemCombinationId,
                customerIdGuid
            );

            _logger.Information(
                "Item combination {CombinationId} moved to cart for customer {CustomerId}. Cart now has {ItemCount} items.",
                request.ItemCombinationId, customerId, cartSummary.ItemCount
            );

            return new MoveToCartResponse
            {
                Success = true,
                Message = "Item moved to cart successfully.",
                AddedToCart = true,
                RemovedFromWishlist = removed
            };
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error moving item {CombinationId} to cart for customer {CustomerId}",
                request.ItemCombinationId, customerId
            );

            return new MoveToCartResponse
            {
                Success = false,
                Message = "Failed to move item to cart.",
                AddedToCart = false,
                RemovedFromWishlist = false,
                ErrorReason = ex.Message
            };
        }
    }

    // ==================== GET /api/wishlist/count ====================

    public async Task<int> GetWishlistCountAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return 0;

            return await _wishlistRepository.GetWishlistCountAsync(customerId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting wishlist count for customer {CustomerId}", customerId);
            return 0;
        }
    }

    // ==================== GET /api/wishlist/check/{itemCombinationId} ====================

    public async Task<bool> IsInWishlistAsync(string customerId, Guid itemCombinationId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId) || itemCombinationId == Guid.Empty)
                return false;

            return await _wishlistRepository.IsInWishlistAsync(customerId, itemCombinationId);
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error checking if combination {CombinationId} is in wishlist for customer {CustomerId}",
                itemCombinationId, customerId
            );
            return false;
        }
    }

    // ==================== HELPER METHODS ====================

    private async Task<BuyBoxOfferInfo?> GetBuyBoxOfferForCombinationAsync(Guid itemCombinationId)
    {
        try
        {
            // Get item combination
            var combination = await _combinationRepository.FindAsync(
                c => c.Id == itemCombinationId && !c.IsDeleted
            );

            if (combination == null)
                return null;

            // Get BuyBox offer pricing for this combination
            var pricing = await _offerCombinationPricingRepository.FindAsync(
                p => p.ItemCombinationId == itemCombinationId &&
                     p.IsBuyBoxWinner &&  // BuyBox winner
                     !p.IsDeleted &&
                     p.AvailableQuantity > 0
            );

            if (pricing == null)
                return null;

            // Get offer details
            var offer = await _offerRepository.FindAsync(o => o.Id == pricing.OfferId && !o.IsDeleted);

            if (offer == null)
                return null;

            return new BuyBoxOfferInfo
            {
                OfferPricingId = pricing.Id,
                Price = pricing.Price,
                SalesPrice = pricing.SalesPrice,
                IsActive = offer.VisibilityScope == OfferVisibilityScope.Active,
            };
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error getting BuyBox offer for combination {CombinationId}",
                itemCombinationId
            );
            return null;
        }
    }

    private async Task<WishlistItemDto> MapWishlistItemToDto(Domains.Entities.Customer.TbWishlistItem item)
    {
        // Get item combination
        var combination = await _combinationRepository.FindAsync(c => c.Id == item.ItemCombinationId);

        if (combination == null)
        {
            throw new InvalidOperationException($"Item combination {item.ItemCombinationId} not found");
        }

        // Get item (product)
        var product = await _itemRepository.FindAsync(i => i.Id == combination.ItemId);

        // Get BuyBox offer
        var buyBoxOffer = await GetBuyBoxOfferForCombinationAsync(item.ItemCombinationId);

        return new WishlistItemDto
        {
            WishlistItemId = item.Id,
            WishlistId = item.WishlistId,
            ItemCombinationId = item.ItemCombinationId,
            DateAdded = item.DateAdded,
            ItemId = combination.ItemId,
            ItemTitleAr = product?.TitleAr,
            ItemTitleEn = product?.TitleEn,
            ItemShortDescriptionAr = product?.ShortDescriptionAr,
            ItemShortDescriptionEn = product?.ShortDescriptionEn,
            ThumbnailImage = product?.ThumbnailImage,
            OfferPricingId = buyBoxOffer?.OfferPricingId,
            Price = buyBoxOffer?.Price ?? 0,
            SalesPrice = buyBoxOffer?.SalesPrice ?? 0,
        };
    }
}