using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Checkout;
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
    }
}
