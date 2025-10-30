using Api.Controllers.Base;
using BL.Contracts.Service.ECommerce.Unit;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Unit;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.ECommerce.Unit
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : BaseController
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService, Serilog.ILogger logger)
            : base(logger)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// Retrieves all units.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var units = _unitService.GetAll();
                if (units == null || !units.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<IEnumerable<UnitDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = units
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a unit by ID.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var unit = _unitService.FindById(id);
                if (unit == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<UnitDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = unit
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches units with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = _unitService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<UnitDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<UnitDto>>
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
        /// Saves a unit.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Save([FromBody] UnitDto unitDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = _unitService.Save(unitDto, GuidUserId);
                if (!success)
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
        /// Deletes a unit by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid unit ID."
                    });

                var success = _unitService.Delete(id, GuidUserId);
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
