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
            try
            {
                var campaigns = await _campaignService.GetAllCampaignsAsync();
                return Ok(new ResponseModel<List<CampaignDto>>
                {
                    Success = true,
                    Data = campaigns,
                    Message = "Campaigns retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all campaigns");
                return StatusCode(500, new ResponseModel<List<CampaignDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving campaigns",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets a campaign by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaign {Id}", id);
                return StatusCode(500, new ResponseModel<CampaignDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets active campaigns
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var campaigns = await _campaignService.GetActiveCampaignsAsync();
                return Ok(new ResponseModel<List<CampaignDto>>
                {
                    Success = true,
                    Data = campaigns,
                    Message = "Active campaigns retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active campaigns");
                return StatusCode(500, new ResponseModel<List<CampaignDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving active campaigns",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Creates a new campaign
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CampaignCreateDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign");
                return StatusCode(500, new ResponseModel<CampaignDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Updates an existing campaign
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseModel<CampaignDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CampaignUpdateDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating campaign {Id}", id);
                return StatusCode(500, new ResponseModel<CampaignDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting campaign {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deleting the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Activates a campaign
        /// </summary>
        [HttpPost("{id}/activate")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Activate(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating campaign {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while activating the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Deactivates a campaign
        /// </summary>
        [HttpPost("{id}/deactivate")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating campaign {Id}", id);
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "An error occurred while deactivating the campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Searches campaigns with filters
        /// </summary>
        [HttpPost("search")]
        [ProducesResponseType(typeof(ResponseModel<List<CampaignDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromBody] CampaignSearchRequest request)
        {
            try
            {
                var campaigns = await _campaignService.SearchCampaignsAsync(request);
                return Ok(new ResponseModel<List<CampaignDto>>
                {
                    Success = true,
                    Data = campaigns,
                    Message = "Campaigns retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching campaigns");
                return StatusCode(500, new ResponseModel<List<CampaignDto>>
                {
                    Success = false,
                    Message = "An error occurred while searching campaigns",
                    Errors = new List<string> { ex.Message }
                });
            }
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
            try
            {
                var products = await _campaignService.GetCampaignProductsAsync(id);
                return Ok(new ResponseModel<List<CampaignProductDto>>
                {
                    Success = true,
                    Data = products,
                    Message = "Campaign products retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaign products for {Id}", id);
                return StatusCode(500, new ResponseModel<List<CampaignProductDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving campaign products",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Adds a product to campaign
        /// </summary>
        [HttpPost("products")]
        [ProducesResponseType(typeof(ResponseModel<CampaignProductDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProduct([FromBody] CampaignProductCreateDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to campaign");
                return StatusCode(500, new ResponseModel<CampaignProductDto>
                {
                    Success = false,
                    Message = "An error occurred while adding product to campaign",
                    Errors = new List<string> { ex.Message }
                });
            }
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
            try
            {
                var stats = await _campaignService.GetCampaignStatisticsAsync();
                return Ok(new ResponseModel<CampaignStatisticsDto>
                {
                    Success = true,
                    Data = stats,
                    Message = "Statistics retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting campaign statistics");
                return StatusCode(500, new ResponseModel<CampaignStatisticsDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving statistics",
                    Errors = new List<string> { ex.Message }
                });
            }
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
            try
            {
                var flashSales = await _campaignService.GetAllFlashSalesAsync();
                return Ok(new ResponseModel<List<FlashSaleDto>>
                {
                    Success = true,
                    Data = flashSales,
                    Message = "Flash sales retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flash sales");
                return StatusCode(500, new ResponseModel<List<FlashSaleDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving flash sales",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Gets active flash sales
        /// </summary>
        [HttpGet("flashsales/active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<List<FlashSaleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveFlashSales()
        {
            try
            {
                var flashSales = await _campaignService.GetActiveFlashSalesAsync();
                return Ok(new ResponseModel<List<FlashSaleDto>>
                {
                    Success = true,
                    Data = flashSales,
                    Message = "Active flash sales retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active flash sales");
                return StatusCode(500, new ResponseModel<List<FlashSaleDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving active flash sales",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Creates a new flash sale
        /// </summary>
        [HttpPost("flashsales")]
        [ProducesResponseType(typeof(ResponseModel<FlashSaleDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateFlashSale([FromBody] FlashSaleCreateDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating flash sale");
                return StatusCode(500, new ResponseModel<FlashSaleDto>
                {
                    Success = false,
                    Message = "An error occurred while creating flash sale",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion
    }
}
