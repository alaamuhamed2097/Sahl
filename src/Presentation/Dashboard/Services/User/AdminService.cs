using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.User;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;

namespace Dashboard.Services.User
{
    public class AdminService : IAdminService
    {
        private readonly IApiService _apiService;

        public AdminService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all admins with optional filters.
        /// </summary>
        public async Task<ResponseModel<IEnumerable<AdminProfileDto>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<IEnumerable<AdminProfileDto>>($"{ApiEndpoints.Admin.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<IEnumerable<AdminProfileDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get admin by ID.
        /// </summary>
        public async Task<ResponseModel<AdminProfileDto>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<AdminProfileDto>($"{ApiEndpoints.Admin.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<AdminProfileDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Create a admin.
        /// </summary>
        public async Task<ResponseModel<bool>> CreateAsync(AdminRegistrationDto admin)
        {
            if (admin == null) throw new ArgumentNullException(nameof(admin));

            try
            {
                return await _apiService.PostAsync<AdminRegistrationDto, bool>($"{ApiEndpoints.Admin.Create}", admin);
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
        /// update a admin.
        /// </summary>
        public async Task<ResponseModel<AdminProfileDto>> UpdateAsync(Guid id, AdminProfileUpdateDto admin)
        {
            if (admin == null) throw new ArgumentNullException(nameof(admin));

            try
            {
                return await _apiService.PostAsync<AdminProfileUpdateDto, AdminProfileDto>($"{ApiEndpoints.Admin.Update}/{id}", admin);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<AdminProfileDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Delete a admin by ID.
        /// </summary>
        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _apiService.PostAsync<Guid, bool>($"{ApiEndpoints.Admin.Delete}", id);
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
                    Message = "Failed to delete admin"
                };
            }
        }
    }
}
