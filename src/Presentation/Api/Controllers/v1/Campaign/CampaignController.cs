using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Campaign
{
    /// <summary>
    /// Campaign and Flash Sale management
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/campaigns")]
    public class CampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(
            ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        #region Campaign Queries

        /// <summary>
        /// Get active campaigns (public)
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), 200)]
        public async Task<IActionResult> GetActiveCampaigns()
        {
            var campaigns = await _campaignService.GetActiveCampaignsAsync();

            return Ok(new ResponseModel<List<CampaignDto>>
            {
                Success = true,
                Data = campaigns,
                Message = "Active campaigns retrieved successfully"
            });
        }

        /// <summary>
        /// Get active flash sales (public)
        /// </summary>
        [HttpGet("flash-sales/active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), 200)]
        public async Task<IActionResult> GetActiveFlashSales()
        {
            var flashSales = await _campaignService.GetActiveFlashSalesAsync();

            return Ok(new ResponseModel<List<CampaignDto>>
            {
                Success = true,
                Data = flashSales,
                Message = "Active flash sales retrieved successfully"
            });
        }

        /// <summary>
        /// Get campaign by ID (admin only)
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCampaignById(Guid id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);

            if (campaign == null)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Campaign not found"
                });
            }

            return Ok(new ResponseModel<CampaignDto>
            {
                Success = true,
                Data = campaign,
                Message = "Campaign retrieved successfully"
            });
        }

        #endregion

        #region Campaign Management (Admin)

        /// <summary>
        /// Create new campaign (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var campaign = await _campaignService.CreateCampaignAsync(dto, GuidUserId);

            return CreatedAtAction(
                nameof(GetCampaignById),
                new { id = campaign.Id },
                new ResponseModel<CampaignDto>
                {
                    Success = true,
                    Data = campaign,
                    Message = "Campaign created successfully"
                });
        }

        /// <summary>
        /// Update campaign (admin only)
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCampaign(Guid id, [FromBody] UpdateCampaignDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Campaign ID mismatch"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var campaign = await _campaignService.UpdateCampaignAsync(dto, GuidUserId);

            return Ok(new ResponseModel<CampaignDto>
            {
                Success = true,
                Data = campaign,
                Message = "Campaign updated successfully"
            });
        }

        /// <summary>
        /// Delete campaign (admin only)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCampaign(Guid id)
        {
            var result = await _campaignService.DeleteCampaignAsync(id, GuidUserId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Campaign not found"
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Campaign deleted successfully"
            });
        }

        #endregion

        #region Campaign Items

        /// <summary>
        /// Get campaign items (public)
        /// </summary>
        [HttpGet("{id:guid}/items")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignItemDto>>), 200)]
        public async Task<IActionResult> GetCampaignItems(Guid id)
        {
            var items = await _campaignService.GetCampaignItemsAsync(id);

            return Ok(new ResponseModel<List<CampaignItemDto>>
            {
                Success = true,
                Data = items,
                Message = "Campaign items retrieved successfully"
            });
        }

        /// <summary>
        /// Add item to campaign (admin only)
        /// </summary>
        [HttpPost("{id:guid}/items")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<CampaignItemDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddItemToCampaign(Guid id, [FromBody] AddCampaignItemDto dto)
        {
            if (id != dto.CampaignId)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Campaign ID mismatch"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var item = await _campaignService.AddItemToCampaignAsync(dto, GuidUserId);

            return CreatedAtAction(
                nameof(GetCampaignItems),
                new { id = dto.CampaignId },
                new ResponseModel<CampaignItemDto>
                {
                    Success = true,
                    Data = item,
                    Message = "Item added to campaign successfully"
                });
        }

        /// <summary>
        /// Remove item from campaign (admin only)
        /// </summary>
        [HttpDelete("{campaignId:guid}/items/{itemId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseModel<object>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveItemFromCampaign(Guid campaignId, Guid itemId)
        {
            var result = await _campaignService.RemoveItemFromCampaignAsync(itemId, GuidUserId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Campaign item not found"
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Item removed from campaign successfully"
            });
        }

        #endregion
    }
}