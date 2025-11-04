using Api.Controllers.Base;
using BL.Contracts.Service.Testimonial;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Testimonial;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Testimonial
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialController : BaseController
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService, Serilog.ILogger logger)
            : base(logger)
        {
            _testimonialService = testimonialService;
        }

        /// <summary>
        /// Retrieves all testimonials.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var testimonials = await _testimonialService.GetAllAsync();

                return Ok(new ResponseModel<IEnumerable<TestimonialDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = testimonials
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a testimonial by ID.
        /// </summary>
        /// <param name="id">The ID of the testimonial.</param>
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
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var testimonial = await _testimonialService.GetByIdAsync(id);
                if (testimonial == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                return Ok(new ResponseModel<TestimonialDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = testimonial
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches testimonials with pagination and filtering.
        /// </summary>
        /// <param name="criteriaModel">Search criteria including pagination and filters.</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                if (criteriaModel == null)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var result = await _testimonialService.SearchAsync(criteriaModel);

                return Ok(new ResponseModel<PaginatedDataModel<TestimonialDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = result
                });
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Adds a new testimonial or updates an existing one.
        /// </summary>
        /// <param name="dto">The testimonial data.</param>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] TestimonialDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var success = await _testimonialService.SaveAsync(dto, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SaveFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
                });
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a testimonial by ID (soft delete).
        /// </summary>
        /// <param name="id">The ID of the testimonial to delete.</param>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var success = await _testimonialService.DeleteAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeleteFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeletedSuccessfully))
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}