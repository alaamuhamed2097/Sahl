using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.CouponCode;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.GeneralModels;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Sales
{
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

        /// <summary>
        /// Validates a coupon code against specified products.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Customer role.
        /// </remarks>
        /// <param name="code">The coupon code to validate.</param>
        [HttpPost("validate")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> ValidateCouponCodeAsync(string code)
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

        /// <summary>
        /// Retrieves all coupon codes.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            var couponCodes = await _couponCodeService.GetAll();
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

        /// <summary>
        /// Retrieves a coupon code by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        /// <param name="id">The ID of the coupon code.</param>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var couponCode = await _couponCodeService.GetById(id);
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
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        /// <param name="criteria">Search criteria including pagination parameters.</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _couponCodeService.GetPage(criteria);

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

        /// <summary>
        /// Creates or updates a coupon code.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] CouponCodeDto couponCodeDto)
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

        /// <summary>
        /// Deletes a coupon code by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid couponCodeId)
        {
            if (couponCodeId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var success = await _couponCodeService.Delete(couponCodeId, GuidUserId);
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
    }
}
