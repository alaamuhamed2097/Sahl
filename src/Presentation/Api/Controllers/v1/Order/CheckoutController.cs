using Api.Controllers.Base;
using Asp.Versioning;
using BL.Services.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Checkout;
using System.Security.Claims;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CheckoutController : BaseController
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService;
            _logger = logger;
        }

        /// <summary>
        /// Stage 2: Prepare Checkout and Verify Availability
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPost("prepare")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CheckoutSummaryDto>> PrepareCheckout([FromBody] PrepareCheckoutRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                var checkoutSummary = await _checkoutService.PrepareCheckoutAsync(customerId, request);
                return Ok(checkoutSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing checkout");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Preview Expected Shipments
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPost("preview-shipments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckoutSummaryDto>> PreviewShipments()
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                var previewSummary = await _checkoutService.PreviewShipmentsAsync(customerId);
                return Ok(previewSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing shipments");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Validate Checkout
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> ValidateCheckout([FromBody] PrepareCheckoutRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized();

                await _checkoutService.ValidateCheckoutAsync(customerId, request.DeliveryAddressId);
                return Ok(new { message = "Checkout is valid and ready to proceed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating checkout");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
