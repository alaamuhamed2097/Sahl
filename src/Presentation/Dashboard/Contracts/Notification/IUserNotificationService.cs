using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;
using Shared.ResultModels;

namespace Dashboard.Contracts.Notification
{
    public interface IUserNotificationService
    {
        Task<ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>> GetAllAsync();
        Task<ResponseModel<UserNotificationRequest>> GetByIdAsync(Guid id);
        Task<ResponseModel<UserNotificationResult<bool>>> MarkAsReadAsync(IEnumerable<UserNotificationRequest> userNotificationRequests);
        Task<ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>> SearchAsync(BaseSearchCriteriaModel model);
    }
}