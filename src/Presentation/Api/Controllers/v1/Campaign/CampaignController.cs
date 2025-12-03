using Asp.Versioning;
using BL.Services.Campaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Campaign
{
    /// <summary>
    /// Controller for Campaign and Flash Sale management
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(ICampaignService campaignService, ILogger<CampaignController> logger)
        {
            _campaignService = campaignService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all campaigns.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
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
        /// Gets a campaign by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign == null)
                return NotFound(new ResponseModel<object> { Success = false, Message = "Campaign not found" });

            return Ok(new ResponseModel<CampaignDto>
            {
                Success = true,
                Data = campaign,
                Message = "Campaign retrieved successfully"
            });
        }

        /// <summary>
        /// Gets active campaigns.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("active")]
        [AllowAnonymous]
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
        /// Creates a new campaign.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            var campaign = await _campaignService.CreateCampaignAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = campaign.Id },
                new ResponseModel<CampaignDto>
                {
                    Success = true,
                    Data = campaign,
                    Message = "Campaign created successfully"
                });
        }

        /// <summary>
        /// Gets all flash sales.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("flashsales")]
        [AllowAnonymous]
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
        /// Gets active flash sales.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("flashsales/active")]
        [AllowAnonymous]
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
    }
}
