using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Customer.Wishlist;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Customer.Wishlist;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Customer.Wishlist
{
    /// <summary>
    /// Wishlist Controller
    /// Manages customer wishlist operations
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/wishlist")]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public class WishlistController : BaseController
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // ==================== POST /api/wishlist/add ====================

        /// <summary>
        /// Add Item to Wishlist
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Operations:<br/>
        /// - Validates customer<br/>
        /// - Checks product exists<br/>
        /// - Checks product not already added (unique constraint)<br/>
        /// - Inserts into database<br/>
        /// <br/>
        /// Returns message if item already in wishlist.
        /// </remarks>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ResponseModel<AddToWishlistResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<AddToWishlistResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AddToWishlistResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<AddToWishlistResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid request data",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidFields,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var response = await _wishlistService.AddToWishlistAsync(UserId, request);

            if (!response.Success)
            {
                var errorResponse = new ResponseModel<AddToWishlistResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = response.Message,
                    Data = response
                };

                // Determine appropriate status code based on message content
                if (response.Message.Contains("already", StringComparison.OrdinalIgnoreCase) ||
                    response.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status409Conflict;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters;
                }
                else if (response.Message.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
                         response.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status404NotFound;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.NotFound;
                }

                return StatusCode(errorResponse.StatusCode, errorResponse);
            }

            return Ok(new ResponseModel<AddToWishlistResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = response.Message,
                Data = response
            });
        }

        // ==================== DELETE /api/wishlist/remove/{itemCombinationId} ====================

        /// <summary>
        /// Remove Item from Wishlist
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Operations:<br/>
        /// - Removes item from wishlist<br/>
        /// - Handles item not found<br/>
        /// </remarks>
        [HttpDelete("remove/{itemCombinationId}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromWishlist(Guid itemCombinationId)
        {
            if (itemCombinationId == Guid.Empty)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid item combination ID",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters
                });
            }

            var result = await _wishlistService.RemoveFromWishlistAsync(UserId, itemCombinationId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Item not found in wishlist.",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.NotFound
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Item removed from wishlist successfully.",
                Data = new { ItemCombinationId = itemCombinationId }
            });
        }

        // ==================== GET /api/wishlist ====================

        /// <summary>
        /// Get Wishlist Items (Paginated)
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Returns products with:<br/>
        /// - Product name<br/>
        /// - Images<br/>
        /// - Price<br/>
        /// - Stock status<br/>
        /// - Offer availability<br/>
        /// <br/>
        /// Default: page=1, pageSize=10<br/>
        /// Max pageSize: 100
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<AdvancedPagedResult<WishlistItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWishlistItems(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Validate pagination parameters
            if (page < 1)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Page number must be greater than 0",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters
                });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Page size must be between 1 and 100",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters
                });
            }

            var response = await _wishlistService.GetWishlistItemsAsync(UserId, page, pageSize);

            return Ok(new ResponseModel<AdvancedPagedResult<WishlistItemDto>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Wishlist items retrieved successfully.",
                Data = response
            });
        }

        // ==================== DELETE /api/wishlist/clear ====================

        /// <summary>
        /// Clear Wishlist
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Operations:<br/>
        /// - Removes all customer items from wishlist<br/>
        /// </remarks>
        [HttpDelete("clear")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearWishlist()
        {
            var result = await _wishlistService.ClearWishlistAsync(UserId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Wishlist not found or already empty.",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.NotFound
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Wishlist cleared successfully.",
                Data = new { Cleared = true }
            });
        }

        // ==================== POST /api/wishlist/move-to-cart ====================

        /// <summary>
        /// Move Wishlist Item to Cart
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Operations:<br/>
        /// - Validates stock availability<br/>
        /// - Gets BuyBox offer pricing for the item combination<br/>
        /// - Adds item to cart using OfferPricingId<br/>
        /// - Removes item from wishlist<br/>
        /// <br/>
        /// 🤝 Integration: Uses Cart Service to add items<br/>
        /// <br/>
        /// Note: Uses BuyBox offer (best available offer) for the item combination
        /// </remarks>
        [HttpPost("move-to-cart")]
        [ProducesResponseType(typeof(ResponseModel<MoveToCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<MoveToCartResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<MoveToCartResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseModel<MoveToCartResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseModel<MoveToCartResponse>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> MoveToCart([FromBody] MoveToCartRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<MoveToCartResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid request data",
                    ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidFields,
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var response = await _wishlistService.MoveToCartAsync(UserId, request);

            if (!response.Success)
            {
                var errorResponse = new ResponseModel<MoveToCartResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = response.Message,
                    Data = response
                };

                // Determine appropriate status code based on message content
                if (response.Message.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
                    response.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status404NotFound;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.NotFound;
                }
                else if (response.Message.Contains("already", StringComparison.OrdinalIgnoreCase) ||
                         response.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status409Conflict;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters;
                }
                else if (response.Message.Contains("out of stock", StringComparison.OrdinalIgnoreCase) ||
                         response.Message.Contains("insufficient stock", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Order.InsufficientStock;
                }
                else if (response.Message.Contains("invalid quantity", StringComparison.OrdinalIgnoreCase))
                {
                    errorResponse.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse.ErrorCode = Shared.ErrorCodes.ErrorCodes.Validation.InvalidParameters;
                }

                return StatusCode(errorResponse.StatusCode, errorResponse);
            }

            return Ok(new ResponseModel<MoveToCartResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = response.Message,
                Data = response
            });
        }

        // ==================== GET /api/wishlist/count ====================

        /// <summary>
        /// Check Wishlist Count
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// <br/>
        /// Returns:<br/>
        /// - Number of items in wishlist<br/>
        /// <br/>
        /// Used for displaying wishlist badge count in navigation.
        /// </remarks>
        [HttpGet("count")]
        [ProducesResponseType(typeof(ResponseModel<WishlistCountResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWishlistCount()
        {
            var count = await _wishlistService.GetWishlistCountAsync(UserId);

            return Ok(new ResponseModel<WishlistCountResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Count retrieved successfully.",
                Data = new WishlistCountResponse { Count = count }
            });
        }
    }
}