using Api.Controllers.v1.Base;
using Asp.Versioning;
using AutoMapper;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.User;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Merchandising.Homepage;
using Shared.GeneralModels;
using System.Security.Claims;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Admin Block Controller - Manage homepage blocks
    /// Uses only IAdminBlockService (no direct repository access)
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/blocks")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class AdminBlockController : BaseController
    {
        private readonly IAdminBlockService _adminBlockService;
        private readonly IMapper _mapper;

        public AdminBlockController(IAdminBlockService adminBlockService, IMapper mapper)
        {
            _adminBlockService = adminBlockService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get current user ID from claims
        /// </summary>
        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        // ==================== POST /api/v1/admin/blocks ====================

        /// <summary>
        /// Create Homepage Block
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<AdminBlockCreateDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBlock([FromBody] AdminBlockCreateDto blockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid block data",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            try
            {
                var block = _mapper.Map<TbHomepageBlock>(blockDto);
                var userId = GetUserId();
                var createdBlock = await _adminBlockService.CreateBlockAsync(block, userId);
                var createdBlockDto = _mapper.Map<AdminBlockCreateDto>(createdBlock);

                return CreatedAtAction(
                    nameof(GetBlock),
                    new { blockId = createdBlock.Id },
                    new ResponseModel<AdminBlockCreateDto>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Block created successfully.",
                        Data = createdBlockDto
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the block.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // ==================== GET /api/v1/admin/blocks/{blockId} ====================

        /// <summary>
        /// Get Block by ID (Admin)
        /// </summary>
        [HttpGet("{blockId}")]
        [ProducesResponseType(typeof(ResponseModel<AdminBlockCreateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBlock(Guid blockId)
        {
            var block = await _adminBlockService.GetBlockByIdAsync(blockId);

            if (block == null)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Block not found."
                });
            }

            var blockDto = _mapper.Map<AdminBlockCreateDto>(block);
            return Ok(new ResponseModel<AdminBlockCreateDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Block retrieved successfully.",
                Data = blockDto
            });
        }

        // ==================== GET /api/v1/admin/blocks ====================

        /// <summary>
        /// Get All Blocks (Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<List<AdminBlockListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBlocks()
        {
            try
            {
                var blocks = await _adminBlockService.GetAllBlocksAsync();
                
                if (blocks == null)
                {
                    blocks = new List<TbHomepageBlock>();
                }

                // Map entities to DTOs
                var blockDtos = _mapper.Map<List<AdminBlockListDto>>(blocks);

                return Ok(new ResponseModel<List<AdminBlockListDto>>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Blocks retrieved successfully.",
                    Data = blockDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving blocks.",
                    Errors = new List<string> { ex.Message, ex.InnerException?.Message }
                });
            }
        }

        // ==================== PUT /api/v1/admin/blocks/{blockId} ====================

        /// <summary>
        /// Update Homepage Block
        /// </summary>
        [HttpPut("{blockId}")]
        [ProducesResponseType(typeof(ResponseModel<AdminBlockListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBlock(
            Guid blockId,
            [FromBody] AdminBlockCreateDto blockDto)
        {
            if (blockId != blockDto.Id && blockDto.Id != null && blockDto.Id != Guid.Empty)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Block ID mismatch."
                });
            }

            try
            {
                var block = _mapper.Map<TbHomepageBlock>(blockDto);
                block.Id = blockId; // Ensure the correct ID is used
                var userId = GetUserId();
                var updatedBlock = await _adminBlockService.UpdateBlockAsync(block, userId);
                var updatedBlockDto = _mapper.Map<AdminBlockListDto>(updatedBlock);

                return Ok(new ResponseModel<AdminBlockListDto>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Block updated successfully.",
                    Data = updatedBlockDto
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while updating the block.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        // ==================== DELETE /api/v1/admin/blocks/{blockId} ====================

        /// <summary>
        /// Delete Homepage Block (Soft Delete)
        /// </summary>
        [HttpDelete("{blockId}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBlock(Guid blockId)
        {
            var userId = GetUserId();
            var result = await _adminBlockService.DeleteBlockAsync(blockId, userId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Block not found."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Block deleted successfully.",
                Data = new { BlockId = blockId }
            });
        }

        // ==================== PATCH /api/v1/admin/blocks/{blockId}/display-order ====================

        /// <summary>
        /// Update Block Display Order
        /// </summary>
        [HttpPatch("{blockId}/display-order")]
        [HttpPut("{blockId}/display-order")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDisplayOrder(
            Guid blockId,
            [FromBody] dynamic request)
        {
            int newOrder;
            
            // Handle both raw int and { newOrder: value } formats
            if (request is int intValue)
            {
                newOrder = intValue;
            }
            else if (request is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("newOrder", out var newOrderProp))
                {
                    newOrder = newOrderProp.GetInt32();
                }
                else
                {
                    newOrder = jsonElement.GetInt32();
                }
            }
            else
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid display order format"
                });
            }

            var userId = GetUserId();
            var result = await _adminBlockService.UpdateDisplayOrderAsync(blockId, newOrder, userId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Block not found."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Display order updated successfully."
            });
        }

        // ==================== POST /api/v1/admin/blocks/{blockId}/products ====================

        /// <summary>
        /// Add Product to Block
        /// </summary>
        [HttpPost("{blockId}/products")]
        [ProducesResponseType(typeof(ResponseModel<TbBlockItem>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProductToBlock(
            Guid blockId,
            [FromBody] TbBlockItem blockProduct)
        {
            try
            {
                blockProduct.HomepageBlockId = blockId;
                var userId = GetUserId();

                var created = await _adminBlockService.AddProductToBlockAsync(blockProduct, userId);

                return CreatedAtAction(
                    nameof(GetBlock),
                    new { blockId = blockId },
                    new ResponseModel<TbBlockItem>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Product added to block successfully.",
                        Data = created
                    });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        // ==================== DELETE /api/v1/admin/blocks/products/{productId} ====================

        /// <summary>
        /// Remove Product from Block
        /// </summary>
        [HttpDelete("products/{productId}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProductFromBlock(Guid productId)
        {
            var userId = GetUserId();
            var result = await _adminBlockService.RemoveProductFromBlockAsync(productId, userId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Product not found in block."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Product removed from block successfully."
            });
        }

        // ==================== GET /api/v1/admin/blocks/{blockId}/products ====================

        /// <summary>
        /// Get Block Products
        /// </summary>
        [HttpGet("{blockId}/products")]
        [ProducesResponseType(typeof(ResponseModel<List<TbBlockItem>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBlockProducts(Guid blockId)
        {
            var products = await _adminBlockService.GetBlockProductsAsync(blockId);

            return Ok(new ResponseModel<List<TbBlockItem>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Products retrieved successfully.",
                Data = products
            });
        }

        // ==================== POST /api/v1/admin/blocks/{blockId}/categories ====================

        /// <summary>
        /// Add Category to Block
        /// </summary>
        [HttpPost("{blockId}/categories")]
        [ProducesResponseType(typeof(ResponseModel<TbBlockCategory>), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddCategoryToBlock(
            Guid blockId,
            [FromBody] TbBlockCategory blockCategory)
        {
            try
            {
                blockCategory.HomepageBlockId = blockId;
                var userId = GetUserId();

                var created = await _adminBlockService.AddCategoryToBlockAsync(blockCategory, userId);

                return CreatedAtAction(
                    nameof(GetBlock),
                    new { blockId = blockId },
                    new ResponseModel<TbBlockCategory>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Category added to block successfully.",
                        Data = created
                    });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        // ==================== DELETE /api/v1/admin/blocks/categories/{categoryId} ====================

        /// <summary>
        /// Remove Category from Block
        /// </summary>
        [HttpDelete("categories/{categoryId}")]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveCategoryFromBlock(Guid categoryId)
        {
            var userId = GetUserId();
            var result = await _adminBlockService.RemoveCategoryFromBlockAsync(categoryId, userId);

            if (!result)
            {
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Category not found in block."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Category removed from block successfully."
            });
        }
    }
}