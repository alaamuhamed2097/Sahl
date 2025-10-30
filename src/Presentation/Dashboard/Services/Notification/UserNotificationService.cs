using Dashboard.Constants;
using Dashboard.Contracts.General;
using Dashboard.Contracts.Notification;
using Dashboard.Models.pagintion;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace Dashboard.Services.Notification
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IApiService _apiService;

        public UserNotificationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Get all User notifications fields .
        /// </summary>
        public async Task<ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>> GetAllAsync()
        {
            try
            {
                return await _apiService.GetAsync<UserNotificationResult<IEnumerable<UserNotificationRequest>>>($"{ApiEndpoints.UserNotification.Get}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Get User notification fields by ID.
        /// </summary>
        public async Task<ResponseModel<UserNotificationRequest>> GetByIdAsync(Guid id)
        {
            try
            {
                return await _apiService.GetAsync<UserNotificationRequest>($"{ApiEndpoints.UserNotification.Get}/{id}");
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UserNotificationRequest>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Search with optional filters.
        /// </summary>
        public async Task<ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>> SearchAsync(BaseSearchCriteriaModel model)
        {
            try
            {
                var queryString = $"PageNumber={model.PageNumber}&PageSize={model.PageSize}&SearchTerm={model.SearchTerm}";
                string url = $"{ApiEndpoints.UserNotification.Search}?{queryString}";
                return await _apiService.GetAsync<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>(url);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        ///  Mark user notifications as read.
        /// </summary>
        public async Task<ResponseModel<UserNotificationResult<bool>>> MarkAsReadAsync(IEnumerable<UserNotificationRequest> userNotificationRequests)
        {
            if (userNotificationRequests == null) throw new ArgumentNullException(nameof(userNotificationRequests));

            try
            {
                return await _apiService.PostAsync<IEnumerable<UserNotificationRequest>, UserNotificationResult<bool>>($"{ApiEndpoints.UserNotification.MarkAsRead}", userNotificationRequests);
            }
            catch (Exception ex)
            {
                // Log error here
                return new ResponseModel<UserNotificationResult<bool>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
