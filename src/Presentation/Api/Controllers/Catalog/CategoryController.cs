using Api.Controllers.Base;
using BL.Contracts.Service.ECommerce.Category;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce;
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

        public CategoryController(ICategoryService categoryService, Serilog.ILogger logger)
            : base(logger)
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
                    return Ok(new ResponseModel<IEnumerable<CategoryDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<CategoryDto>>
                {
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
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var category = await _categoryService.FindByIdAsync(id);
                if (category == null)
                    return Ok(new ResponseModel<string>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = string.Empty
                    });

                return Ok(new ResponseModel<CategoryDto>
                {
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

                var result = await _categoryService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<CategoryDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<CategoryDto>>
                {
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
                var result = await _categoryService.GetMainCategoriesAsync();
                if (result == null || !result.Any())
                    return Ok(new ResponseModel<IEnumerable<MainCategoryDto>>
                    {
                        Message = "No main categories found.",
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<MainCategoryDto>>
                {
                    Message = "Main categories retrieved successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        ///// <summary>
        ///// Retrieves all Home categories.
        ///// </summary>
        //[Route("/api/Home/home-categories")]
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetHomeCategories()
        //{
        //    try
        //    {
        //        var result = await _categoryService.GetHomeCategories(UserId);
        //        if (result == null || !result.Any())
        //            return Ok(new ResponseModel<IEnumerable<VwCategoryItemsDto>>
        //            {
        //                Message = NotifiAndAlertsResources.NoDataFound,
        //                Data = []
        //            });

        //        return Ok(new ResponseModel<IEnumerable<VwCategoryItemsDto>>
        //        {
        //            Message = NotifiAndAlertsResources.DataRetrieved,
        //            Data = result
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}

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
                var result = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
                if (result == null || !result.Any())
                    return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                    {
                        Message = "No featured categories found.",
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                {
                    Message = "Featured categories retrieved successfully.",
                    Data = result
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
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = await _categoryService.Save(categoryDto, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Data = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully,
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
                if (!ModelState.IsValid)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = await _categoryService.UpdateSerialsAsync(serialAssignments, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Data = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully,
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
                if (id == Guid.Empty)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _categoryService.Delete(id, GuidUserId);
                if (!result.Item1)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed,
                        Errors = result.Item2
                    });

                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Data = true,
                    Message = NotifiAndAlertsResources.DeletedSuccessfully
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

                if (tree == null || !tree.Any())
                    return Ok(new ResponseModel<IEnumerable<CategoryTreeDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<CategoryTreeDto>>
                {
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
                var categoryPreviews = await _categoryService.GetPreviewedCategories(IsFeaturedCategory, isParent);
                if (categoryPreviews == null || !categoryPreviews.Any())
                    return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<CategoryPreviewDto>>
                {
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = categoryPreviews
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("CategoryWithChildren")]
        public async Task<IActionResult> GetCategoryWithChildren([FromQuery] Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetCategoryWithChildren(categoryId);
                if (category == null)
                    return Ok(new ResponseModel<string>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = string.Empty
                    });

                return Ok(new ResponseModel<CategoryTreeDto>
                {
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
