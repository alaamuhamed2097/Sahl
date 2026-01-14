using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Vendor;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;

namespace Dashboard.Services.Vendor
{
    public class VendorService : IVendorService
    {
        private readonly IApiService _apiService;

        public VendorService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<VendorDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<VendorDto>>($"{ApiEndpoints.Vendor.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<VendorDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel<VendorDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<VendorDto>($"{ApiEndpoints.Vendor.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<VendorDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel<VendorRegistrationResponseDto>> CreateVendorAsync(VendorRegistrationRequestDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var result = await _apiService.PostAsync<VendorRegistrationRequestDto, VendorRegistrationResponseDto>(
                    ApiEndpoints.UserRegistration.CreateVendor, request);

                if (result.Success)
                {
                    return new ResponseModel<VendorRegistrationResponseDto>
                    {
                        Success = true,
                        Data = result.Data,
                        Message = result.Message
                    };
                }

                return new ResponseModel<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<VendorRegistrationResponseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel<VendorUpdateResponseDto>> UpdateVendorAsync(VendorUpdateRequestDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var result = await _apiService.PostAsync<VendorUpdateRequestDto, VendorUpdateResponseDto>(
                    ApiEndpoints.Vendor.Update, request);

                if (result.Success)
                {
                    return new ResponseModel<VendorUpdateResponseDto>
                    {
                        Success = true,
                        Data = result.Data,
                        Message = result.Message
                    };
                }

                return new ResponseModel<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<VendorUpdateResponseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResponseModel<bool>> UpdateVendorStatusAsync(UpdateVendorStatusDto dto)
        {
            try
            {
                return await _apiService.PostAsync<UpdateVendorStatusDto, bool>(ApiEndpoints.Vendor.UpdateVendorStatus, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ResponseModel<bool>> UpdateUserStatusAsync(UpdateUserStatusDto dto)
        {
            try
            {
                return await _apiService.PostAsync<UpdateUserStatusDto, bool>(ApiEndpoints.Vendor.UpdateUserStatus, dto);
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Vendor.Delete}", id);
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
                    Message = "Failed to delete Vendor"
                };
            }
        }
    }
}
