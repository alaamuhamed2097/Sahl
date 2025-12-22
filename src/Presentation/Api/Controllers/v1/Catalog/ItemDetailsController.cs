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
            if (result == null)
            {
                return NotFound(new { message = "Item not found" });
            }
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
            [FromBody] CombinationRequest request)
        {
            if (request?.SelectedValueIds == null || request.SelectedValueIds.Count == 0)
            {
                return BadRequest(new { message = "Selected attributes are required" });
            }

            var result = await _itemDetailsService.GetCombinationByAttributesAsync(id, request);

           if (result == null)
                return NotFound(new { message = "Item or combination not found" });

           return Ok(result);
        }
    }
}
