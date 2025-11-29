using Api.Controllers.Base;
using BL.Contracts.Service.Location;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Location
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : BaseController
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService, Serilog.ILogger logger)
            : base(logger)
        {
            _stateService = stateService;
        }

        /// <summary>
        /// Retrieves all states.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var states = await _stateService.GetAllAsync();
                if (states == null || !states.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No states found."
                    });

                return Ok(new ResponseModel<IEnumerable<StateDto>>
                {
                    Success = true,
                    Message = "states retrieved successfully.",
                    Data = states
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a state by ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid state ID."
                    });

                var state = await _stateService.FindByIdAsync(id);
                if (state == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "state not found."
                    });

                return Ok(new ResponseModel<StateDto>
                {
                    Success = true,
                    Message = "state retrieved successfully.",
                    Data = state
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches states with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = await _stateService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<StateDto>>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<StateDto>>
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
        /// Saves a state.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] StateDto StateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid state data."
                    });

                var result = await _stateService.SaveAsync(StateDto, GuidUserId);
                if (!result.Success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.SavedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a state by ID.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid state ID."
                    });

                var success = await _stateService.DeleteAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
