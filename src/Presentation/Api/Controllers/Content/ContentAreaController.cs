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
            try
            {
                var areas = await _contentAreaService.GetAllAsync();
                return Ok(new ResponseModel<IEnumerable<ContentAreaDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = areas
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var areas = await _contentAreaService.GetActiveAreasAsync();
                return Ok(new ResponseModel<IEnumerable<ContentAreaDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = areas
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("by-code/{areaCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string areaCode)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] ContentAreaDto dto)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("toggle-status")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
