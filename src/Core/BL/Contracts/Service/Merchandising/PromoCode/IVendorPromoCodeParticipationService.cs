using Shared.DTOs.Merchandising.PromoCode;
using Shared.GeneralModels;
using DAL.Models;
using Common.Filters;

namespace BL.Contracts.Service.Merchandising.PromoCode
{
    /// <summary>
    /// Service interface for vendor promo code participation requests
    /// </summary>
    public interface IVendorPromoCodeParticipationService
    {
        /// <summary>
        /// Submits a participation request for a vendor to join a public promo code
        /// </summary>
        Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> SubmitParticipationRequestAsync(
            CreateVendorPromoCodeParticipationRequestDto request,
            Guid userId);

        /// <summary>
        /// Gets all promo code participation requests for a specific vendor
        /// </summary>
        Task<(bool Success, AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>? Data)> GetVendorParticipationRequestsAsync(
            Guid vendorId,
            BaseSearchCriteriaModel criteria);

        /// <summary>
        /// Admin: Gets promo code participation requests (optionally filtered by promoCodeId)
        /// </summary>
        Task<(bool Success, AdvancedPagedResult<AdminVendorPromoCodeParticipationRequestListDto>? Data)> GetAdminParticipationRequestsAsync(
            Guid? promoCodeId,
            BaseSearchCriteriaModel criteria);

        /// <summary>
        /// Gets details of a specific participation request
        /// </summary>
        Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> GetParticipationRequestAsync(
            Guid requestId,
            Guid vendorId);

        /// <summary>
        /// Cancels a pending participation request
        /// </summary>
        Task<(bool Success, string Message)> CancelParticipationRequestAsync(
            Guid requestId,
            Guid vendorId,
            Guid userId);
    }
}
