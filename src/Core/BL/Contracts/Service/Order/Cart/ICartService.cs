using Shared.DTOs.Order.Cart;

namespace BL.Contracts.Service.Order.Cart;

public interface ICartService
{
    Task<CartSummaryDto> GetCartSummaryAsync(string customerId);
    Task<CartSummaryDto> AddToCartAsync(string customerId, AddToCartRequest request);
    Task<CartSummaryDto> RemoveFromCartAsync(string customerId, Guid cartItemId);
    Task<CartSummaryDto> UpdateCartItemAsync(string customerId, UpdateCartItemRequest request);
    Task<CartSummaryDto> ClearCartAsync(string customerId);
    Task<int> GetCartItemCountAsync(string customerId);
}
