using DAL.Models;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace BL.Contracts.GeneralService.Notification
{
    public interface IUserNotificationService
    {
        Task<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>> GetPage(BaseSearchCriteriaModel criteriaModel, string userId);
        Task<UserNotificationResult<IEnumerable<UserNotificationRequest>>> GetAll(string userId);
        Task<UserNotificationRequest> FindById(Guid Id);
        Task<bool> Save(UserNotificationRequest dto, Guid userId);
        Task<UserNotificationResult<bool>> MarkAsRead(IEnumerable<UserNotificationRequest> userNotificationRequests, string userId);
        Task<bool> Delete(Guid id, Guid userId);
    }
}