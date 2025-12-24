using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Catalog.Category;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Catalog.Category;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Catalog
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            if (categories == null || !categories.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<IEnumerable<CategoryDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = categories
            });
        }

        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _categoryService.FindByIdAsync(id);
            if (category == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<CategoryDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = category
            });
        }

        /// <summary>
        /// Searches categories with pagination and filtering
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _categoryService.GetPageAsync(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<CategoryDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<CategoryDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Retrieves all main categories.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [Route("/api/v{version:apiVersion}/Home/main-categories")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainCategories()
        {
            var categories = await _categoryService.GetMainCategoriesAsync();
            return Ok(new ResponseModel<IEnumerable<MainCategoryDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = categories
            });
        }

        /// <summary>
        /// Retrieves all featured categories.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [Route("/api/v{version:apiVersion}/Home/featured-categories")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedCategories(bool IsFeaturedCategory, bool isParent)
        {
            var categories = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
            return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = categories
            });
        }

        /// <summary>
        /// Saves a category.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Save([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInput
                });

            var success = await _categoryService.Save(categoryDto, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = true
            });
        }

        /// <summary>
        /// Change tree view serials.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("changeTreeViewSerials")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> ChangeTreeViewSerialsAsyns([FromBody] Dictionary<Guid, string> serialAssignments)
        {
            var updated = await _categoryService.UpdateSerialsAsync(serialAssignments, GuidUserId);
            if (!updated)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = true
            });
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var (success, errors) = await _categoryService.Delete(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<IEnumerable<string>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed,
                    Errors = errors
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully,
                Data = true
            });
        }

        /// <summary>
        /// Retrieves all categories in hierarchy way.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("tree")]
        public async Task<IActionResult> GetCategoryTree()
        {
            var tree = await _categoryService.BuildCategoryTree();
            return Ok(new ResponseModel<IEnumerable<CategoryTreeDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = tree
            });
        }

        /// <summary>
        /// Retrieves all previews categories.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("previews")]
        public async Task<IActionResult> GetAllCategoryPreviews([FromQuery] bool IsFeaturedCategory, bool isParent)
        {
            var previews = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
            return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = previews
            });
        }

        /// <summary>
        /// Retrieves a category and its children by category ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("CategoryWithChildren")]
        public async Task<IActionResult> GetCategoryWithChildren([FromQuery] Guid categoryId)
        {
            var category = await _categoryService.GetCategoryWithChildren(categoryId);
            return Ok(new ResponseModel<CategoryTreeDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = category
            });
        }
    }
}
