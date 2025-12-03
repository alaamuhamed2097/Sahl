using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Cart;
using BL.Services.Order;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 1: Add to Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPost("add-item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartSummaryDto>> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} adding item {request.ItemId} to cart");

                var cartSummary = await _cartService.AddToCartAsync(customerId, request);
                return Ok(cartSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Cart Summary
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CartSummaryDto>> GetCartSummary()
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                var cartSummary = await _cartService.GetCartSummaryAsync(customerId);
                return Ok(cartSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart summary");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Remove Item from Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpDelete("remove-item/{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartSummaryDto>> RemoveFromCart(Guid cartItemId)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} removing item {cartItemId} from cart");

                var cartSummary = await _cartService.RemoveFromCartAsync(customerId, cartItemId);
                return Ok(cartSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update Cart Item Quantity
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPut("update-item")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartSummaryDto>> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} updating item {request.CartItemId} quantity to {request.Quantity}");

                var cartSummary = await _cartService.UpdateCartItemAsync(customerId, request);
                return Ok(cartSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Clear Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CartSummaryDto>> ClearCart()
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                _logger.LogInformation($"Customer {customerId} clearing cart");

                var cartSummary = await _cartService.ClearCartAsync(customerId);
                return Ok(cartSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get Cart Item Count
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetCartItemCount()
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                var count = await _cartService.GetCartItemCountAsync(customerId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart item count");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
