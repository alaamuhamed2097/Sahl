using Api.Controllers.Base;
using BL.Contracts.Service.PromoCode;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodesController : BaseController
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodesController(IPromoCodeService promoCodeService, Serilog.ILogger logger)
            : base(logger)
        {
            _promoCodeService = promoCodeService;
        }

        /// <summary>
        /// Applies a promo code to a shopping cart.
        /// </summary>
        [HttpPost("apply")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> ApplyPromoCode([FromBody] ApplyPromoCodeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _promoCodeService.ApplyPromoCode(request);

                if (!result.Success)
                    return Ok(new ResponseModel<AppliedPromoCodeResult>
                    {
                        Success = false,
                        Message = result.Message
                    });

                return Ok(new ResponseModel<AppliedPromoCodeResult>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.PromoCodeAppliedSuccessfully,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Validates a promo code against specified products.
        /// </summary>
        /// <param name="request">The validation request containing promo code and product IDs.</param>
        /// <returns>Validation result with discount information if valid.</returns>
        [HttpPost("validate")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> ValidatePromoCodeAsync(string code)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _promoCodeService.ValidatePromoCodeAsync(code, UserId);

                if (!result.Success)
                    return Ok(new ResponseModel<PromoCodeValidationResult>
                    {
                        Success = false,
                        Message = result.Message
                    });

                return Ok(new ResponseModel<PromoCodeValidationResult>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.Successful,
                    Data = result.Data
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves all promo codes.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Get()
        {
            try
            {
                var promoCodes = _promoCodeService.GetAll();
                if (promoCodes == null || !promoCodes.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<IEnumerable<PromoCodeDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = promoCodes
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a promo code by ID.
        /// </summary>
        /// <param name="id">The ID of the promo code.</param>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
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

                var promoCode = _promoCodeService.GetById(id);
                if (promoCode == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<PromoCodeDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = promoCode
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches promo codes with pagination and filtering.
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = _promoCodeService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<PromoCodeDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<PromoCodeDto>>
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
        /// Creates or updates a promo code.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] PromoCodeDto promoCodeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = await _promoCodeService.Save(promoCodeDto, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<bool>
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
        /// Deletes a promo code by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Delete([FromBody] Guid promoCodeId)
        {
            try
            {
                if (promoCodeId == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = _promoCodeService.Delete(promoCodeId, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<string>
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