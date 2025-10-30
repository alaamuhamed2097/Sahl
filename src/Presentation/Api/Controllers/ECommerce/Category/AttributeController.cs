using Api.Controllers.Base;
using BL.Contracts.Service.ECommerce.Category;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace Api.Controllers.ECommerce.Category
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttributeController : BaseController
    {
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService, Serilog.ILogger logger)
            : base(logger)
        {
            _attributeService = attributeService;
        }

        /// <summary>
        /// Retrieves all attributes.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var attributes = await _attributeService.GetAllAsync();
                if (attributes == null || !attributes.Any())
                    return Ok(new ResponseModel<IEnumerable<AttributeDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = []
                    });

                return Ok(new ResponseModel<IEnumerable<AttributeDto>>
                {
                    Message = string.Format(ValidationResources.RetrievedSuccessfully, ECommerceResources.Attributes),
                    Data = attributes
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a attribute by ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
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

                var attribute = await _attributeService.FindByIdAsync(id);
                if (attribute == null)
                    return Ok(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<AttributeDto>
                {
                    Message = string.Format(ValidationResources.RetrievedSuccessfully, ECommerceResources.Attribute),
                    Data = attribute
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches attributes with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = await _attributeService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<AttributeDto>>
                    {
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<AttributeDto>>
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
        /// Saves a attribute.
        /// </summary>
        [HttpPost("save")]
        [Authorize]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Save([FromBody] AttributeDto attributeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var saveResult = await _attributeService.SaveAsync(attributeDto, GuidUserId);
                if (!saveResult.Success)
                    return Ok(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<bool>
                {
                    Message = NotifiAndAlertsResources.SavedSuccessfully,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a attribute by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _attributeService.DeleteAsync(id, UserId);
                if (!result.Success)
                    return Ok(new ResponseModel<DeleteResult>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed,
                        Data = result
                    });

                return Ok(new ResponseModel<DeleteResult>
                {
                    Message = NotifiAndAlertsResources.DeletedSuccessfully,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
