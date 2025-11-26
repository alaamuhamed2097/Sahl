using Api.Controllers.Base;
using BL.Contracts.Service.CouponCode;
using BL.Service.CouponCode;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponCodeController : BaseController
    {
        private readonly ICouponCodeService _couponCodeService;

        public CouponCodeController(ICouponCodeService couponCodeService, Serilog.ILogger logger)
            : base(logger)
        {
            _couponCodeService = couponCodeService;
        }

        /// <summary>
        /// Applies a coupon code to a shopping cart.
        /// </summary>
        [HttpPost("apply")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> ApplyCouponCode([FromBody] ApplyCouponCodeRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _couponCodeService.ApplyCouponCode(request);

                if (!result.Success)
                    return Ok(new ResponseModel<AppliedCouponCodeResult>
                    {
                        Success = false,
                        Message = result.Message
                    });

                return Ok(new ResponseModel<AppliedCouponCodeResult>
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
        /// Validates a coupon code against specified products.
        /// </summary>
        /// <param name="request">The validation request containing promo code and product IDs.</param>
        /// <returns>Validation result with discount information if valid.</returns>
        [HttpPost("validate")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> ValidateCouponCodeAsync(string code)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var result = await _couponCodeService.ValidateCouponCodeAsync(code, UserId);

                if (!result.Success)
                    return Ok(new ResponseModel<CouponCodeValidationResult>
                    {
                        Success = false,
                        Message = result.Message
                    });

                return Ok(new ResponseModel<CouponCodeValidationResult>
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
        /// Retrieves all couponCode codes.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult Get()
        {
            try
            {
                var couponCodes = _couponCodeService.GetAll();
                if (couponCodes == null || !couponCodes.Any())
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = couponCodes
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

                var couponCode = _couponCodeService.GetById(id);
                if (couponCode == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<CouponCodeDto>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = couponCode
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

                var result = _couponCodeService.GetPage(criteria);

                if (result == null || !result.Items.Any())
                {
                    return Ok(new ResponseModel<PaginatedDataModel<CouponCodeDto>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<PaginatedDataModel<CouponCodeDto>>
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
        public async Task<IActionResult> Save([FromBody] CouponCodeDto couponCodeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = await _couponCodeService.Save(couponCodeDto, GuidUserId);
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
        public IActionResult Delete([FromBody] Guid couponCodeId)
        {
            try
            {
                if (couponCodeId == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = _couponCodeService.Delete(couponCodeId, GuidUserId);
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