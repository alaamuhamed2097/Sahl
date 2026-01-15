using Common.Filters;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Merchandising;
using Dashboard.Models.pagintion;
using Resources;
using Shared.DTOs.Merchandising.PromoCode;
using Shared.GeneralModels;

namespace Dashboard.Services.Merchandising
{
    public class VendorPromoCodeParticipationAdminService : IVendorPromoCodeParticipationAdminService
    {
        private readonly IApiService _apiService;

        public VendorPromoCodeParticipationAdminService(IApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        public async Task<ResponseModel<PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>> ListAsync(
            AdminVendorPromoCodeParticipationListRequestDto request)
        {
            try
            {
                request ??= new AdminVendorPromoCodeParticipationListRequestDto();
                request.Criteria ??= new BaseSearchCriteriaModel();

                // Call API which returns ResponseModel<AdvancedPagedResult<AdminVendorPromoCodeParticipationRequestListDto>>
                var apiResult = await _apiService.PostAsync<AdminVendorPromoCodeParticipationListRequestDto, PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>(
                    ApiEndpoints.VendorPromoCodeParticipation.AdminList,
                    request);

                if (apiResult.Success && apiResult.Data != null)
                {
                    var paged = new PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>(
                        apiResult.Data.Items ?? Enumerable.Empty<AdminVendorPromoCodeParticipationRequestListDto>(),
                        apiResult.Data.TotalRecords);

                    return new ResponseModel<PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>
                    {
                        Success = true,
                        Message = apiResult.Message,
                        Data = paged,
                        Errors = apiResult.Errors
                    };
                }

                return new ResponseModel<PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>
                {
                    Success = false,
                    Message = apiResult.Message ?? NotifiAndAlertsResources.NoDataFound,
                    Data = new PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>(
                        Enumerable.Empty<AdminVendorPromoCodeParticipationRequestListDto>(),
                        0),
                    Errors = apiResult.Errors
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ListAsync: {ex.Message}");
                return new ResponseModel<PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SomethingWentWrongAlert,
                    Data = new PaginatedDataModel<AdminVendorPromoCodeParticipationRequestListDto>(
                        Enumerable.Empty<AdminVendorPromoCodeParticipationRequestListDto>(), 0)
                };
            }
        }
    }
}

