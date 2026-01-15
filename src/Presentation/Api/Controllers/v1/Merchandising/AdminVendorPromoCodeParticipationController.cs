using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Merchandising.PromoCode;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Merchandising.PromoCode;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Merchandising
{
    /// <summary>
    /// Admin endpoints for viewing vendor promo code participation requests
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/merchandising/vendorpromocodeparticipation")]
    public class AdminVendorPromoCodeParticipationController : BaseController
    {
        private readonly IVendorPromoCodeParticipationService _participationService;

        public AdminVendorPromoCodeParticipationController(IVendorPromoCodeParticipationService participationService)
        {
            _participationService = participationService;
        }

        /// <summary>
        /// Admin: list vendor promo code participation requests (optionally filtered by promoCodeId)
        /// </summary>
        [HttpPost("admin/list")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(typeof(ResponseModel<AdvancedPagedResult<AdminVendorPromoCodeParticipationRequestListDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAsync([FromBody] AdminVendorPromoCodeParticipationListRequestDto request)
        {
            request ??= new AdminVendorPromoCodeParticipationListRequestDto();
            request.Criteria ??= new BaseSearchCriteriaModel();
            ValidateBaseSearchCriteriaModel(request.Criteria);

            var result = await _participationService.GetAdminParticipationRequestsAsync(request.PromoCodeId, request.Criteria);

            if (!result.Success || result.Data == null)
                return Ok(CreateSuccessResponse(new AdvancedPagedResult<AdminVendorPromoCodeParticipationRequestListDto>(), NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result.Data, NotifiAndAlertsResources.DataRetrieved));
        }
    }
}

