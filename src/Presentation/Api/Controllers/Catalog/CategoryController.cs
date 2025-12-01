using Api.Controllers.Base;
using BL.Contracts.Service.ECommerce.Category;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Catalog
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, Serilog.ILogger logger) : base(logger)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches categories with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        //[Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = await _categoryService.GetPageAsync(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<CategoryDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<CategoryDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all main categories.
        /// </summary>\
        [Route("/api/Home/main-categories")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMainCategories()
        {
            try
            {
                var categories = await _categoryService.GetMainCategoriesAsync();
                return Ok(new ResponseModel<IEnumerable<MainCategoryDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all featured categories.
        /// </summary>
        [Route("/api/Home/featured-categories")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedCategories(bool IsFeaturedCategory, bool isParent)
        {
            try
            {
                var categories = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
                return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Saves a category.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Save([FromBody] CategoryDto categoryDto)
        {
            try
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

                // Return boolean Data so clients expecting ResponseModel<bool> can deserialize
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Change tree view serials.
        /// </summary>
        [HttpPost("changeTreeViewSerials")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> ChangeTreeViewSerialsAsyns([FromBody] Dictionary<Guid, string> serialAssignments)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all categories in hierarchy way.
        /// </summary>
        [HttpGet("tree")]
        public async Task<IActionResult> GetCategoryTree()
        {
            try
            {
                var tree = await _categoryService.BuildCategoryTree();
                return Ok(new ResponseModel<IEnumerable<CategoryTreeDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = tree
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all previews categories.
        /// </summary>
        [HttpGet("previews")]
        public async Task<IActionResult> GetAllCategoryPreviews([FromQuery] bool IsFeaturedCategory, bool isParent)
        {
            try
            {
                var previews = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
                return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = previews
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a category and its children by category ID.
        /// </summary>
        [HttpGet("CategoryWithChildren")]
        public async Task<IActionResult> GetCategoryWithChildren([FromQuery] Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryWithChildren(categoryId);
                return Ok(new ResponseModel<CategoryTreeDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
