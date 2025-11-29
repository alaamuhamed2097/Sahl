using Api.Controllers.Base;
using BL.Contracts.Service.ShippingCompny;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Shipping
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class ShippingCompanyController : BaseController
    {
        private readonly IShippingCompanyService _shippingCompanyService;

        public ShippingCompanyController(IShippingCompanyService shippingCompanyService, Serilog.ILogger logger)
            : base(logger)
        {
            _shippingCompanyService = shippingCompanyService;
        }

        /// <summary>
        /// Retrieves all shipping companies.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var companies = await _shippingCompanyService.GetAllAsync();
                if (companies == null || !companies.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No companies found."
                    });

                return Ok(new ResponseModel<IEnumerable<ShippingCompanyDto>>
                {
                    Success = true,
                    Message = "companies retrieved successfully.",
                    Data = companies
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a shipping company by ID.
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
                        Message = "Invalid company ID."
                    });

                var company = await _shippingCompanyService.FindByIdAsync(id);
                if (company == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "company not found."
                    });

                return Ok(new ResponseModel<ShippingCompanyDto>
                {
                    Success = true,
                    Message = "company retrieved successfully.",
                    Data = company
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches shipping companies with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = await _shippingCompanyService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<ShippingCompanyDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<ShippingCompanyDto>>
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
        /// Saves a shipping company.
        /// </summary>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] ShippingCompanyDto shippingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid shipping companies data."
                    });

                var success = await _shippingCompanyService.Save(shippingDto, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<string>
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
        /// Deletes a shipping company by ID.
        /// </summary>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid country ID."
                    });

                var success = await _shippingCompanyService.DeleteAsync(id, GuidUserId);
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
