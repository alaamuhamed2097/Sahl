using Dashboard.Constants;
using Dashboard.Contracts;
using Dashboard.Contracts.General;
using Resources;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.GeneralModels;

namespace Dashboard.Services
{
    public class CouponCodeService : ICouponCodeService
    {
        private readonly IApiService _apiService;

        public CouponCodeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all CouponCodes with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<CouponCodeDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<CouponCodeDto>>($"{ApiEndpoints.CouponCode.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<CouponCodeDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get CouponCode by ID.
        /// </summary>
        public async Task<ResponseModel<CouponCodeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<CouponCodeDto>($"{ApiEndpoints.CouponCode.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<CouponCodeDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Save or update a CouponCode.
        /// </summary>
        public async Task<ResponseModel<bool>> SaveAsync(CouponCodeDto CouponCode)
        {
            if (CouponCode == null) throw new ArgumentNullException(nameof(CouponCode));

            try
            {
                return await _apiService.PostAsync<CouponCodeDto, bool>($"{ApiEndpoints.CouponCode.Save}", CouponCode);
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
        /// Delete a CouponCode by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.CouponCode.Delete}", id);
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
            catch (Exception)
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
