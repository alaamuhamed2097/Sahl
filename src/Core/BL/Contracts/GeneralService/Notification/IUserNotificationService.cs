using Shared.DTOs.Notification;

namespace BL.Contracts.GeneralService.Notification
{
    public interface IUserNotificationService
    {
        //PaginatedDataModel<NotificationDto> GetPage(string userId, int page = 1, int pageSize = 10);
        Task<IEnumerable<NotificationDto>> GetAllAsync(string userId);
        Task<NotificationDto> FindByIdAsync(Guid Id);
        Task<bool> Save(NotificationDto dto, Guid userId);
        Task<bool> SaveBulk(NotificationDto dto, IEnumerable<string> recipients);
        Task<bool> MarkAsRead(List<Guid> NotificationIds, string userId);
        Task<bool> Delete(Guid id, Guid userId);
    }
}