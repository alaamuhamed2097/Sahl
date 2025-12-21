using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.ECommerce.Item;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ECommerce.Item;

namespace Api.Controllers.v1.Catalog
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemDetailsController : BaseController
    {
        private readonly IItemDetailsService _itemDetailsService;

        public ItemDetailsController(IItemDetailsService itemDetailsService)
        {
            _itemDetailsService = itemDetailsService ?? throw new ArgumentNullException(nameof(itemDetailsService));
        }

        /// <summary>
        /// Get complete item details including attributes, pricing, and default combination
        /// The default combination matches the price shown in search results
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Complete item details</returns>
        /// <response code="200">Returns the item details</response>
        /// <response code="404">Item not found</response>
        [HttpGet("{itemCombinationId}")]
        [ProducesResponseType(typeof(ItemDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemDetails([FromRoute] Guid itemCombinationId )
        {
            var result = await _itemDetailsService.GetItemDetailsAsync(itemCombinationId);
            return Ok(result);
        }

        /// <summary>
        /// Get combination details by selected attributes
        /// Returns images, prices, and offers for the specific combination
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="request">Selected attribute values</param>
        /// <returns>Combination details with all vendor offers</returns>
        /// <response code="200">Returns the combination details</response>
        /// <response code="400">Invalid or incomplete attribute selection</response>
        /// <response code="404">Item not found or combination not available</response>
        [HttpPost("{id}/combination")]
        [ProducesResponseType(typeof(CombinationDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCombinationByAttributes(
            [FromRoute] Guid id,
            [FromBody] GetCombinationRequest request)
        {
            if (request?.SelectedAttributes == null || request.SelectedAttributes.Count == 0)
            {
                return BadRequest(new { message = "Selected attributes are required" });
            }

            var result = await _itemDetailsService.GetCombinationByAttributesAsync(id, request);

            // Return appropriate response based on availability
            if (!result.IsAvailable)
            {
                // Combination not found or out of stock
                if (result.CombinationId == null)
                {
                    // Combination doesn't exist
                    if (result.MissingAttributes != null && result.MissingAttributes.Count > 0)
                    {
                        // Incomplete selection
                        return BadRequest(new
                        {
                            message = result.Message,
                            missingAttributes = result.MissingAttributes
                        });
                    }
                    else
                    {
                        // Invalid combination
                        return NotFound(new
                        {
                            message = result.Message,
                            selectedAttributes = result.SelectedAttributes
                        });
                    }
                }
                else
                {
                    // Combination exists but out of stock
                    return Ok(result); // Return 200 but with IsAvailable = false
                }
            }

            return Ok(result);
        }
    }
}
