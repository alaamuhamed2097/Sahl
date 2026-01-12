using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.WithdrawalMethod;
using Resources;
using Shared.DTOs.WithdrawalMethod;
using Shared.GeneralModels;

namespace UI.Services.Withdrawal
{
    public class WithdrawalMethodService : IWithdrawalMethodService
    {
        private readonly IApiService _apiService;

        public WithdrawalMethodService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<WithdrawalMethodDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<WithdrawalMethodDto>>($"{ApiEndpoints.WithdrawalMethod.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<WithdrawalMethodDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<WithdrawalMethodDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<WithdrawalMethodDto>($"{ApiEndpoints.WithdrawalMethod.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<WithdrawalMethodDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<WithdrawalMethodDto>> SaveAsync(WithdrawalMethodDto WithdrawalMethod)
        {
            if (WithdrawalMethod == null) throw new ArgumentNullException(nameof(WithdrawalMethod));

            try
            {
                return await _apiService.PostAsync<WithdrawalMethodDto, WithdrawalMethodDto>($"{ApiEndpoints.WithdrawalMethod.Save}", WithdrawalMethod);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<WithdrawalMethodDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, ResponseModel<bool>>($"{ApiEndpoints.WithdrawalMethod.Delete}", id);
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
