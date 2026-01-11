using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Content;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Content;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Content
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ContentAreaController : BaseController
    {
        private readonly IContentAreaService _contentAreaService;

        public ContentAreaController(IContentAreaService contentAreaService)
        {
            _contentAreaService = contentAreaService;
        }

        /// <summary>
        /// Get all content areas.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            var areas = await _contentAreaService.GetAllAsync();
            return Ok(new ResponseModel<IEnumerable<ContentAreaDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = areas
            });
        }

        /// <summary>
        /// Get active content areas.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActive()
        {
            var areas = await _contentAreaService.GetActiveAreasAsync();
            return Ok(new ResponseModel<IEnumerable<ContentAreaDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = areas
            });
        }

        /// <summary>
        /// Get content area by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            var area = await _contentAreaService.GetByIdAsync(id);
            if (area == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<ContentAreaDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = area
            });
        }

        /// <summary>
        /// Get content area by area code.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("by-code/{areaCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string areaCode)
        {
            var area = await _contentAreaService.GetByAreaCodeAsync(areaCode);
            if (area == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                });

            return Ok(new ResponseModel<ContentAreaDto>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = area
            });
        }

        /// <summary>
        /// Search content areas.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
            criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

            var result = await _contentAreaService.SearchAsync(criteriaModel);

            return Ok(new ResponseModel<PagedResult<ContentAreaDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = result
            });
        }

        /// <summary>
        /// Save content area.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] ContentAreaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid data." });

            var success = await _contentAreaService.SaveAsync(dto, GuidUserId);
            return Ok(new ResponseModel<string>
            {
                Success = success,
                Message = success
                    ? GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
                    : GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SaveFailed))
            });
        }

        /// <summary>
        /// Delete content area.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var success = await _contentAreaService.DeleteAsync(id, GuidUserId);
            return Ok(new ResponseModel<string>
            {
                Success = success,
                Message = success
                    ? GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeletedSuccessfully))
                    : GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeleteFailed))
            });
        }
    }
}
