using Api.Controllers.Base;
using Api.Extensions;
using BL.Contracts.Service.ECommerce.Item;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Catalog
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : BaseController
    {
        private readonly IItemService _itemService;

        public ItemController(
            IItemService itemService,
            Serilog.ILogger logger)
            : base(logger)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Retrieves all items with currency conversion based on client location.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientIp = HttpContext.GetClientIpAddress();
                var shouldApplyConversion = ShouldApplyCurrencyConversion();
                //var items = await _itemService.GetAllAsync(clientIp, shouldApplyConversion);
                var items = await _itemService.GetAllAsync();

                if (items?.Any() != true)
                    return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

                return Ok(CreateSuccessResponse(items, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves an item by ID with currency conversion based on client location.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

                var clientIp = HttpContext.GetClientIpAddress();
                var shouldApplyConversion = ShouldApplyCurrencyConversion();
                //var item = await _itemService.GetByIdWithCurrencyConversionAsync(id, clientIp, shouldApplyConversion);
                var item = await _itemService.FindByIdAsync(id);

                if (item == null)
                    return NotFound(CreateErrorResponse(NotifiAndAlertsResources.NoDataFound));

                return Ok(CreateSuccessResponse(item, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches items with pagination and filtering, with currency conversion based on client location.
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] ItemSearchCriteriaModel criteria)
        {
            try
            {
                ValidateAndNormalizePagination(criteria);

                var clientIp = HttpContext.GetClientIpAddress();
                var shouldApplyConversion = ShouldApplyCurrencyConversion();
                //var result = await _itemService.GetPageWithCurrencyConversionAsync(criteria, clientIp, shouldApplyConversion);
                var result = await _itemService.GetPage(criteria);

                if (result?.Items?.Any() != true)
                    return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        ///// <summary>
        ///// Retrieves best seller items with currency conversion.
        ///// </summary>
        //[HttpGet("best-sellers")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetBestSellers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    return await GetItemsByCategory(new ItemSearchCriteriaModel
        //    {
        //        IsBestSeller = true,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    });
        //}

        ///// <summary>
        ///// Retrieves recommended items with currency conversion.
        ///// </summary>
        //[HttpGet("recommended")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetRecommended([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    return await GetItemsByCategory(new ItemSearchCriteriaModel
        //    {
        //        IsRecommended = true,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize
        //    });
        //}

        /// <summary>
        /// Adds a new item.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] ItemDto itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

                var success = await _itemService.Save(itemDto, GuidUserId);
                if (!success)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

                return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        [HttpPost("update/{id:guid}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Update(Guid id, [FromBody] ItemDto itemDto)
        {
            try
            {
                if (id == Guid.Empty || !ModelState.IsValid)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

                itemDto.Id = id;
                var success = await _itemService.Save(itemDto, GuidUserId);
                if (!success)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.SaveFailed));

                return Ok(CreateSuccessResponse<string>(null, NotifiAndAlertsResources.SavedSuccessfully));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes an item by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.InvalidInputAlert));

                var success = await _itemService.DeleteAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(CreateErrorResponse(NotifiAndAlertsResources.DeleteFailed));

                return Ok(CreateSuccessResponse(true, NotifiAndAlertsResources.DeletedSuccessfully));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Determines whether currency conversion should be applied based on user role.
        /// Currency conversion is applied only for Customer role or unlogged users.
        /// </summary>
        private bool ShouldApplyCurrencyConversion()
        {
            // If user is not authenticated (unlogged), apply conversion
            if (RoleName == nameof(UserRole.Admin))
                return false;

            return true;
        }

        ///// <summary>
        ///// Common method for retrieving categorized items
        ///// </summary>
        //private async Task<IActionResult> GetItemsByCategory(ItemSearchCriteriaModel criteria)
        //{
        //    try
        //    {
        //        ValidateAndNormalizePagination(criteria);

        //        var clientIp = HttpContext.GetClientIpAddress();
        //        var shouldApplyConversion = ShouldApplyCurrencyConversion();
        //        var result = await _itemService.GetPageWithCurrencyConversionAsync(criteria, clientIp, shouldApplyConversion);

        //        if (result?.Items?.Any() != true)
        //        {
        //            return Ok(CreateSuccessResponse(
        //                new PaginatedDataModel<VwItemDto>(new List<VwItemDto>(), 0),
        //                NotifiAndAlertsResources.NoDataFound));
        //        }

        //        return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}

        /// <summary>
        /// Validates and normalizes pagination parameters
        /// </summary>
        private static void ValidateAndNormalizePagination(ItemSearchCriteriaModel criteria)
        {
            criteria.PageNumber = Math.Max(criteria.PageNumber, 1);
            criteria.PageSize = Math.Clamp(criteria.PageSize, 1, 100);
        }

        /// <summary>
        /// Creates a standardized success response
        /// </summary>
        private ResponseModel<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ResponseModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a standardized error response
        /// </summary>
        private ResponseModel<string> CreateErrorResponse(string message)
        {
            return new ResponseModel<string>
            {
                Success = false,
                Message = message
            };
        }

        #endregion
    }
}