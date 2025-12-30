using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.CouponCode;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Controller for managing coupon codes - STANDALONE
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CouponCodeController : BaseController
    {
        private readonly ICouponCodeService _couponCodeService;

        public CouponCodeController(ICouponCodeService couponCodeService)
        {
            _couponCodeService = couponCodeService;
        }

        #region Customer Endpoints

        /// <summary>
        /// Validates a coupon code for the current user.
        /// </summary>
        [HttpPost("validate")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(typeof(ResponseModel<CouponValidationResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateCouponCodeAsync([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var result = await _couponCodeService.ValidateCouponCodeAsync(code, UserId);

            if (!result.IsValid)
                return Ok(new ResponseModel<CouponValidationResultDto>
                {
                    Success = false,
                    Message = result.Message ?? "invalid coupon",
                    Data = result
                });

            return Ok(new ResponseModel<CouponValidationResultDto>
            {
                Success = true,
                Message = result.Message ?? NotifiAndAlertsResources.Successful,
                Data = result
            });
        }

        /// <summary>
        /// Gets all active coupon codes available for customers.
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<CouponCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveCoupons()
        {
            var coupons = await _couponCodeService.GetActiveCouponsAsync();

            return Ok(new ResponseModel<IEnumerable<CouponCodeDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = coupons
            });
        }

        /// <summary>
        /// Gets a coupon code by its code string.
        /// </summary>
        [HttpGet("by-code/{code}")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(typeof(ResponseModel<CouponCodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var coupon = await _couponCodeService.GetByCodeAsync(code);

            if (coupon == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<CouponCodeDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = coupon
            });
        }

        #endregion

        #region Admin Endpoints

        /// <summary>
        /// Retrieves all coupon codes.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<CouponCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var couponCodes = await _couponCodeService.GetAllAsync();

            if (couponCodes == null || !couponCodes.Any())
                return Ok(new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = Enumerable.Empty<CouponCodeDto>()
                });

            return Ok(new ResponseModel<IEnumerable<CouponCodeDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = couponCodes
            });
        }

        /// <summary>
        /// Retrieves a coupon code by ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<CouponCodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var couponCode = await _couponCodeService.GetByIdAsync(id);

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

        /// <summary>
        /// Searches coupon codes with pagination and filtering.
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<AdvancedPagedResult<CouponCodeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            // Validate and set defaults
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _couponCodeService.GetPageAsync(criteria);

            if (result == null || !result.Items.Any())
            {
                return Ok(new ResponseModel<AdvancedPagedResult<CouponCodeDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                    Data = new AdvancedPagedResult<CouponCodeDto>
                    {
                        Items = new List<CouponCodeDto>(),
                        TotalRecords = 0,
                        PageNumber = criteria.PageNumber,
                        PageSize = criteria.PageSize,
                        TotalPages = 0
                    }
                });
            }

            return Ok(new ResponseModel<AdvancedPagedResult<CouponCodeDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Creates or updates a coupon code.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<CouponCodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] CouponCodeDto couponCodeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var savedCoupon = await _couponCodeService.SaveAsync(couponCodeDto, GuidUserId);

            return Ok(new ResponseModel<CouponCodeDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = savedCoupon
            });
        }

        /// <summary>
        /// Deletes a coupon code by ID (soft delete).
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] Guid couponCodeId)
        {
            if (couponCodeId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var success = await _couponCodeService.DeleteAsync(couponCodeId, GuidUserId);

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

        #endregion

        #region Vendor Endpoints

        /// <summary>
        /// Gets all coupon codes for a specific vendor.
        /// </summary>
        [HttpGet("vendor/{vendorId}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Vendor)}")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<CouponCodeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVendorCoupons(Guid vendorId)
        {
            if (vendorId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var coupons = await _couponCodeService.GetVendorCouponsAsync(vendorId);

            return Ok(new ResponseModel<IEnumerable<CouponCodeDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = coupons
            });
        }

        #endregion
    }
}