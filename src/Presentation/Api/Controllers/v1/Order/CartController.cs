using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Cart;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Order.Cart;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
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
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.UnauthorizedAccess
                });
            }

            var cartSummary = await _cartService.AddToCartAsync(UserId, request);

            return Ok(new ResponseModel<CartSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = cartSummary
            });
        }

        /// <summary>
        /// NEW: Add multiple items to cart in a single operation
        /// </summary>
        /// <remarks>
        /// This endpoint allows adding multiple items to the cart at once.
        /// It validates all items first, then adds them individually.
        /// Returns details about successful and failed additions.
        /// </remarks>
        [HttpPost("items/bulk")]
        [ProducesResponseType(typeof(BulkAddToCartResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMultipleToCart([FromBody] BulkAddToCartRequest request)
        {
            if (request?.Items == null || !request.Items.Any())
            {
                return BadRequest(new { message = "At least one item must be provided" });
            }

            var result = await _cartService.AddMultipleToCartAsync(UserId, request);

            // Return different status based on result
            if (result.IsCompleteFailure)
            {
                return BadRequest(new
                {
                    message = "Failed to add any items to cart",
                    result
                });
            }

            if (result.IsPartialSuccess)
            {
                return Ok(new
                {
                    message = $"Successfully added {result.TotalItemsAdded} items, {result.TotalItemsFailed} failed",
                    result
                });
            }

            return Ok(new
            {
                message = $"Successfully added all {result.TotalItemsAdded} items",
                result
            });
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
        public async Task<IActionResult> GetCartSummary()
        {
            var cartSummary = await _cartService.GetCartSummaryAsync(UserId);

            if (cartSummary == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<CartSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = cartSummary
            });
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
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var cartSummary = await _cartService.RemoveFromCartAsync(UserId, cartItemId);

            return Ok(new ResponseModel<CartSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = cartSummary
            });
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
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            var cartSummary = await _cartService.UpdateCartItemAsync(UserId, request);

            return Ok(new ResponseModel<CartSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = cartSummary
            });
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
        public async Task<IActionResult> ClearCart()
        {
            var cartSummary = await _cartService.ClearCartAsync(UserId);

            return Ok(new ResponseModel<CartSummaryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = cartSummary
            });
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
            var count = await _cartService.GetCartItemCountAsync(UserId);

            return Ok(new ResponseModel<int>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = count
            });
        }
    }
}