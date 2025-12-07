using Api.Controllers.v1.Order;
using BL.Services.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.DTOs.ECommerce.Cart;
using Shared.GeneralModels;
using System.Security.Claims;

namespace IntegrationTests.Controllers.Order
{
    public class CartControllerUnitTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly Mock<ILogger<CartController>> _loggerMock;
        private readonly CartController _controller;

        public CartControllerUnitTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _loggerMock = new Mock<ILogger<CartController>>();

            _controller = new CartController(_cartServiceMock.Object, _loggerMock.Object);
        }

        #region AddToCart Tests

        [Fact]
        public async Task AddToCart_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var request = new AddToCartRequest
            {
                ItemId = Guid.NewGuid(),
                OfferId = Guid.NewGuid(),
                Quantity = 2
            };

            var cartSummary = new CartSummaryDto
            {
                CartId = Guid.NewGuid(),
                Items = new List<CartItemDto>(),
                SubTotal = 0m,
                TotalEstimate = 0m
            };

            _cartServiceMock.Setup(s => s.AddToCartAsync(customerId, request))
                .ReturnsAsync(cartSummary);

            SetUserContext(customerId);

            // Act
            var result = await _controller.AddToCart(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            // Check if response is wrapped
            var responseModel = okResult.Value as ResponseModel<CartSummaryDto>;
            if (responseModel != null)
            {
                Assert.True(responseModel.Success);
                Assert.Equal(200, responseModel.StatusCode);
                Assert.NotNull(responseModel.Data);
            }
        }

        [Fact]
        public async Task AddToCart_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Arrange
            var request = new AddToCartRequest
            {
                ItemId = Guid.NewGuid(),
                OfferId = Guid.NewGuid(),
                Quantity = 1
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.AddToCart(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task AddToCart_WithServiceException_ThrowsInvalidOperationException()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var request = new AddToCartRequest
            {
                ItemId = Guid.NewGuid(),
                OfferId = Guid.NewGuid(),
                Quantity = 1
            };

            _cartServiceMock.Setup(s => s.AddToCartAsync(customerId, request))
                .ThrowsAsync(new InvalidOperationException("Item not found"));

            SetUserContext(customerId);

            // Act & Assert
            // Exception propagates; middleware handles it in production
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.AddToCart(request)
            );

            Assert.Equal("Item not found", exception.Message);
        }

        #endregion

        #region GetCartSummary Tests

        [Fact]
        public async Task GetCartSummary_WithValidCustomerId_ReturnsCartSummary()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var cartSummary = new CartSummaryDto
            {
                CartId = Guid.NewGuid(),
                Items = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        Id = Guid.NewGuid(),
                        ItemId = Guid.NewGuid(),
                        ItemName = "Test Item",
                        Quantity = 2,
                        UnitPrice = 50m,
                        SubTotal = 100m
                    }
                },
                SubTotal = 100m,
                ShippingEstimate = 50m,
                TaxEstimate = 14m,
                TotalEstimate = 164m,
                ItemCount = 2
            };

            _cartServiceMock.Setup(s => s.GetCartSummaryAsync(customerId))
                .ReturnsAsync(cartSummary);

            SetUserContext(customerId);

            // Act
            var result = await _controller.GetCartSummary();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            // Handle wrapped response
            var responseModel = okResult.Value as ResponseModel<CartSummaryDto>;
            if (responseModel != null)
            {
                // Response is wrapped
                Assert.True(responseModel.Success);
                Assert.Equal(200, responseModel.StatusCode);
                Assert.NotNull(responseModel.Data);
                Assert.Single(responseModel.Data.Items);
                Assert.Equal(164m, responseModel.Data.TotalEstimate);
            }
            else
            {
                // Response is not wrapped
                var returnedCart = okResult.Value as CartSummaryDto;
                Assert.NotNull(returnedCart);
                Assert.Single(returnedCart.Items);
                Assert.Equal(164m, returnedCart.TotalEstimate);
            }
        }

        #endregion

        #region RemoveFromCart Tests

        [Fact]
        public async Task RemoveFromCart_WithValidId_ReturnsUpdatedCart()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var cartItemId = Guid.NewGuid();
            var updatedCart = new CartSummaryDto
            {
                CartId = Guid.NewGuid(),
                Items = new List<CartItemDto>()
            };

            _cartServiceMock.Setup(s => s.RemoveFromCartAsync(customerId, cartItemId))
                .ReturnsAsync(updatedCart);

            SetUserContext(customerId);

            // Act
            var result = await _controller.RemoveFromCart(cartItemId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
        }

        #endregion

        #region UpdateCartItem Tests

        [Fact]
        public async Task UpdateCartItem_WithValidRequest_ReturnsUpdatedCart()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var request = new UpdateCartItemRequest
            {
                CartItemId = Guid.NewGuid(),
                Quantity = 5
            };
            var updatedCart = new CartSummaryDto
            {
                CartId = Guid.NewGuid(),
                Items = new List<CartItemDto>()
            };

            _cartServiceMock.Setup(s => s.UpdateCartItemAsync(customerId, request))
                .ReturnsAsync(updatedCart);

            SetUserContext(customerId);

            // Act
            var result = await _controller.UpdateCartItem(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region ClearCart Tests

        [Fact]
        public async Task ClearCart_WithValidCustomerId_ReturnsEmptyCart()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var emptyCart = new CartSummaryDto
            {
                CartId = Guid.NewGuid(),
                Items = new List<CartItemDto>()
            };

            _cartServiceMock.Setup(s => s.ClearCartAsync(customerId))
                .ReturnsAsync(emptyCart);

            SetUserContext(customerId);

            // Act
            var result = await _controller.ClearCart();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region GetCartItemCount Tests

        [Fact]
        public async Task GetCartItemCount_WithValidCustomerId_ReturnsCount()
        {
            // Arrange
            var customerId = Guid.NewGuid().ToString();
            var count = 5;

            _cartServiceMock.Setup(s => s.GetCartItemCountAsync(customerId))
                .ReturnsAsync(count);

            SetUserContext(customerId);

            // Act
            var result = await _controller.GetCartItemCount();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);

            // Handle wrapped response
            var responseModel = okResult.Value as ResponseModel<int>;
            if (responseModel != null)
            {
                // Response is wrapped
                Assert.True(responseModel.Success);
                Assert.Equal(200, responseModel.StatusCode);
                Assert.Equal(count, responseModel.Data);
            }
            else
            {
                // Response is not wrapped
                Assert.Equal(count, okResult.Value);
            }
        }

        #endregion

        /// <summary>
        /// Helper method to set user context for testing
        /// </summary>
        private void SetUserContext(string customerId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customerId)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }
    }
}