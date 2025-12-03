using BL.Services.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.Campaign
{
    /// <summary>
    /// Controller for Campaign and Flash Sale management
    /// Exception handling is centralized in ExceptionHandlingMiddleware - remove individual try-catch blocks
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(
            ICampaignService campaignService,
            ILogger<CampaignController> logger)
        {
            _campaignService = campaignService;
            _logger = logger;
        }

        #region Campaign Endpoints

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync();
            return Ok(new ResponseModel<List<CampaignDto>>
            {
                Success = true,
                Data = campaigns,
                Message = "Campaigns retrieved successfully"
            });
        }

        /// <summary>
        /// Gets a campaign by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
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

        /// <summary>
        /// Gets active campaigns
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
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
        /// Creates a new campaign
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var campaign = await _campaignService.CreateCampaignAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = campaign.Id },
                new ResponseModel<CampaignDto>
                {
                    Success = true,
                    Data = campaign,
                    Message = "Campaign created successfully"
                });
        }

        /// <summary>
        /// Updates an existing campaign
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "ID mismatch"
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var campaign = await _campaignService.UpdateCampaignAsync(dto);
            return Ok(new ResponseModel<CampaignDto>
            {
                Success = true,
                Data = campaign,
                Message = "Campaign updated successfully"
            });
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _campaignService.DeleteCampaignAsync(id);
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

        /// <summary>
        /// Activates a campaign
        /// </summary>
        [HttpPost("{id}/activate")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _campaignService.ActivateCampaignAsync(id);
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
                Message = "Campaign activated successfully"
            });
        }

        /// <summary>
        /// Deactivates a campaign
        /// </summary>
        [HttpPost("{id}/deactivate")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _campaignService.DeactivateCampaignAsync(id);
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
                Message = "Campaign deactivated successfully"
            });
        }

        /// <summary>
        /// Searches campaigns with filters
        /// </summary>
        [HttpPost("search")]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromBody] CampaignSearchRequest request)
        {
            var campaigns = await _campaignService.SearchCampaignsAsync(request);
            return Ok(new ResponseModel<List<CampaignDto>>
            {
                Success = true,
                Data = campaigns,
                Message = "Campaigns retrieved successfully"
            });
        }

        #endregion

        #region Campaign Products

        /// <summary>
        /// Gets products for a campaign
        /// </summary>
        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCampaignProducts(Guid id)
        {
            var products = await _campaignService.GetCampaignProductsAsync(id);
            return Ok(new ResponseModel<List<CampaignProductDto>>
            {
                Success = true,
                Data = products,
                Message = "Campaign products retrieved successfully"
            });
        }

        /// <summary>
        /// Adds a product to campaign
        /// </summary>
        [HttpPost("products")]
        [ProducesResponseType(typeof(ResponseModel<CampaignProductDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProduct([FromBody] CampaignProductCreateDto dto)
        {
            var product = await _campaignService.AddProductToCampaignAsync(dto);
            return CreatedAtAction(
                nameof(GetCampaignProducts),
                new { id = dto.CampaignId },
                new ResponseModel<CampaignProductDto>
                {
                    Success = true,
                    Data = product,
                    Message = "Product added to campaign successfully"
                });
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Gets campaign statistics
        /// </summary>
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(ResponseModel<CampaignStatisticsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await _campaignService.GetCampaignStatisticsAsync();
            return Ok(new ResponseModel<CampaignStatisticsDto>
            {
                Success = true,
                Data = stats,
                Message = "Statistics retrieved successfully"
            });
        }

        #endregion

        #region Flash Sales

        /// <summary>
        /// Gets all flash sales
        /// </summary>
        [HttpGet("flashsales")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<FlashSaleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFlashSales()
        {
            var flashSales = await _campaignService.GetAllFlashSalesAsync();
            return Ok(new ResponseModel<List<FlashSaleDto>>
            {
                Success = true,
                Data = flashSales,
                Message = "Flash sales retrieved successfully"
            });
        }

        /// <summary>
        /// Gets active flash sales
        /// </summary>
        [HttpGet("flashsales/active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<FlashSaleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveFlashSales()
        {
            var flashSales = await _campaignService.GetActiveFlashSalesAsync();
            return Ok(new ResponseModel<List<FlashSaleDto>>
            {
                Success = true,
                Data = flashSales,
                Message = "Active flash sales retrieved successfully"
            });
        }

        /// <summary>
        /// Creates a new flash sale
        /// </summary>
        [HttpPost("flashsales")]
        [ProducesResponseType(typeof(ResponseModel<FlashSaleDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateFlashSale([FromBody] FlashSaleCreateDto dto)
        {
            var flashSale = await _campaignService.CreateFlashSaleAsync(dto);
            return CreatedAtAction(
                nameof(GetAllFlashSales),
                new ResponseModel<FlashSaleDto>
                {
                    Success = true,
                    Data = flashSale,
                    Message = "Flash sale created successfully"
                });
        }

        #endregion
    }
}
