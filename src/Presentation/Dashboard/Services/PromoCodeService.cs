using Dashboard.Constants;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels;

namespace Dashboard.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IApiService _apiService;

        public PromoCodeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all PromoCodes with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<PromoCodeDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<PromoCodeDto>>($"{ApiEndpoints.PromoCode.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<PromoCodeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get PromoCode by ID.
        /// </summary>
        public async Task<ResponseModel<PromoCodeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<PromoCodeDto>($"{ApiEndpoints.PromoCode.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<PromoCodeDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a PromoCode.
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(PromoCodeDto PromoCode)
        {
            if (PromoCode == null) throw new ArgumentNullException(nameof(PromoCode));

            try
            {
                return await _apiService.PostAsync<PromoCodeDto, bool>($"{ApiEndpoints.PromoCode.Save}", PromoCode);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a PromoCode by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.PromoCode.Delete}", id);
                if (result.Success)
                {
                    return new ResponseModel<bool>
                    {
                        Success = true,
                        Message = result.Message
                    };
                }
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                };
            }
        }
    }
}
