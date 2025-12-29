using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising;
using Common.Enumerations.User;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AdminBlockController(IAdminBlockService adminBlockService)
        {
            _adminBlockService = adminBlockService;
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
        [ProducesResponseType(typeof(ResponseModel<TbHomepageBlock>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBlock([FromBody] TbHomepageBlock block)
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
                var userId = GetUserId();
                var createdBlock = await _adminBlockService.CreateBlockAsync(block, userId);

                return CreatedAtAction(
                    nameof(GetBlock),
                    new { blockId = createdBlock.Id },
                    new ResponseModel<TbHomepageBlock>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Block created successfully.",
                        Data = createdBlock
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
        [ProducesResponseType(typeof(ResponseModel<TbHomepageBlock>), StatusCodes.Status200OK)]
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

            return Ok(new ResponseModel<TbHomepageBlock>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Block retrieved successfully.",
                Data = block
            });
        }

        // ==================== GET /api/v1/admin/blocks ====================

        /// <summary>
        /// Get All Blocks (Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<List<TbHomepageBlock>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBlocks()
        {
            var blocks = await _adminBlockService.GetAllBlocksAsync();

            return Ok(new ResponseModel<List<TbHomepageBlock>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Blocks retrieved successfully.",
                Data = blocks
            });
        }

        // ==================== PUT /api/v1/admin/blocks/{blockId} ====================

        /// <summary>
        /// Update Homepage Block
        /// </summary>
        [HttpPut("{blockId}")]
        [ProducesResponseType(typeof(ResponseModel<TbHomepageBlock>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBlock(
            Guid blockId,
            [FromBody] TbHomepageBlock block)
        {
            if (blockId != block.Id)
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
                var userId = GetUserId();
                var updatedBlock = await _adminBlockService.UpdateBlockAsync(block, userId);

                return Ok(new ResponseModel<TbHomepageBlock>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Block updated successfully.",
                    Data = updatedBlock
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
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDisplayOrder(
            Guid blockId,
            [FromBody] int newOrder)
        {
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