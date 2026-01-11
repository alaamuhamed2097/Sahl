using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Location;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Location;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Location
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CityController : BaseController
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var cities = await _cityService.GetAllAsync();
            if (cities == null || !cities.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No cities found."
                });
            return Ok(new ResponseModel<IEnumerable<CityDto>>
            {
                Success = true,
                Message = "cities retrieved successfully.",
                Data = cities
            });
        }

        /// <summary>
        /// Retrieves a city by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid city ID."
                });

            var city = await _cityService.FindByIdAsync(id);
            if (city == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "city not found."
                });

            return Ok(new ResponseModel<CityDto>
            {
                Success = true,
                Message = "city retrieved successfully.",
                Data = city
            });
        }

        /// <summary>
        /// Retrieves cities by state ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        /// <param name="stateId">The state ID to filter cities</param>
        [HttpGet("by-state/{stateId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStateId(Guid stateId)
        {
            if (stateId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid state ID."
                });

            var cities = await _cityService.GetByStateIdAsync(stateId);
            if (cities == null || !cities.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No cities found for this state."
                });

            return Ok(new ResponseModel<IEnumerable<CityDto>>
            {
                Success = true,
                Message = "Cities retrieved successfully.",
                Data = cities
            });
        }

        /// <summary>
        /// Searches cities with pagination and filtering.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _cityService.GetPage(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<PagedResult<CityDto>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = result
                });
            }

            return Ok(new ResponseModel<PagedResult<CityDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Saves a city.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] CityDto CityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid city data."
                });

            var result = await _cityService.SaveAsync(CityDto, GuidUserId);
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

        /// <summary>
        /// Deletes a city by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid city ID."
                });

            var success = await _cityService.DeleteAsync(id, GuidUserId);
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
    }
}
