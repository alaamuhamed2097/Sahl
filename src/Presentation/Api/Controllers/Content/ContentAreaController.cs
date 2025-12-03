using Api.Controllers.Base;
using BL.Contracts.Service.Content;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Content;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Content
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentAreaController : BaseController
    {
        private readonly IContentAreaService _contentAreaService;

        public ContentAreaController(IContentAreaService contentAreaService, Serilog.ILogger logger)
            : base(logger)
        {
            _contentAreaService = contentAreaService;
        }

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

        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
            criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

            var result = await _contentAreaService.SearchAsync(criteriaModel);

            return Ok(new ResponseModel<PaginatedDataModel<ContentAreaDto>>
            {
                Success = true,
                Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                Data = result
            });
        }

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

        [HttpPost("toggle-status")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            var success = await _contentAreaService.ToggleActiveStatusAsync(id, GuidUserId);
            return Ok(new ResponseModel<string>
            {
                Success = success,
                Message = success 
                    ? "Status updated successfully."
                    : "Failed to update status."
            });
        }
    }
}
