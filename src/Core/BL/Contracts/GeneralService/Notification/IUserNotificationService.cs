using DAL.Models;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace BL.Contracts.GeneralService.Notification
{
    public interface IUserNotificationService
    {
        UserNotificationResult<PaginatedDataModel<UserNotificationRequest>> GetPage(BaseSearchCriteriaModel criteriaModel, string userId);
        UserNotificationResult<IEnumerable<UserNotificationRequest>> GetAll(string userId);
        UserNotificationRequest FindById(Guid Id);
        Task<bool> Save(UserNotificationRequest dto, Guid userId);
        Task<UserNotificationResult<bool>> MarkAsRead(IEnumerable<UserNotificationRequest> userNotificationRequests, string userId);
        Task<bool> Delete(Guid id, Guid userId);
    }
}