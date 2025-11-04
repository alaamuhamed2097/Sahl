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
    public class CountryController : BaseController
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService, Serilog.ILogger logger)
            : base(logger)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// Retrieves all countries.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var country = _countryService.GetAll();
                if (country == null || !country.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No countries found."
                    });

                return Ok(new ResponseModel<IEnumerable<CountryDto>>
                {
                    Success = true,
                    Message = "Country retrieved successfully.",
                    Data = country
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a country by ID.
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
                        Message = "Invalid country ID."
                    });

                var country = _countryService.FindById(id);
                if (country == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "country not found."
                    });

                return Ok(new ResponseModel<CountryDto>
                {
                    Success = true,
                    Message = "Country retrieved successfully.",
                    Data = country
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches countries with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpGet("search")]
        public IActionResult Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = _countryService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<CountryDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<CountryDto>>
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
        /// Saves a country.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("save")]
        public IActionResult Save([FromBody] CountryDto countryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid country data."
                    });

                var success = _countryService.Save(countryDto, GuidUserId);
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
        /// Deletes a country by ID.
        /// </summary>
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost("delete")]
        public IActionResult Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid country ID."
                    });

                var success = _countryService.Delete(id, GuidUserId);
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
