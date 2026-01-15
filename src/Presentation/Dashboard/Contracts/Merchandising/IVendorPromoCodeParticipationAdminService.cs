using Shared.DTOs.Merchandising.PromoCode;
using Shared.GeneralModels;
using Dashboard.Models.pagintion;

namespace Dashboard.Contracts.Merchandising
{
    public interface IVendorPromoCodeParticipationAdminService
    {
        Task<ResponseModel<PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>> ListAsync(
            AdminVendorPromoCodeParticipationListRequestDto request);
    }
}

