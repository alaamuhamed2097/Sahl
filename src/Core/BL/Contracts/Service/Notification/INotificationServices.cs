using DAL.Models;
using Shared.DTOs.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Common.Enumerations.Notification;

namespace BL.Contracts.Service.Notification
{
    public interface INotificationChannelService
    {
        Task<IEnumerable<NotificationChannelDto>> GetAllAsync();
        Task<IEnumerable<NotificationChannelDto>> GetActiveChannelsAsync();
        Task<NotificationChannelDto?> GetByIdAsync(Guid id);
        Task<NotificationChannelDto?> GetByChannelTypeAsync(NotificationChannel channelType);
        Task<PaginatedDataModel<NotificationChannelDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(NotificationChannelDto dto, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId);
    }

    public interface INotificationsService
    {
        Task<IEnumerable<NotificationsDto>> GetAllAsync();
        Task<IEnumerable<NotificationsDto>> GetByRecipientAsync(int recipientId, RecipientType recipientType);
        Task<IEnumerable<NotificationsDto>> GetUnreadByRecipientAsync(int recipientId, RecipientType recipientType);
        Task<NotificationsDto?> GetByIdAsync(Guid id);
        Task<PaginatedDataModel<NotificationsDto>> SearchAsync(BaseSearchCriteriaModel criteria);
        Task<bool> SaveAsync(NotificationsDto dto, Guid userId);
        Task<bool> MarkAsReadAsync(Guid id, Guid userId);
        Task<bool> MarkMultipleAsReadAsync(List<Guid> ids, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<int> GetUnreadCountAsync(int recipientId, RecipientType recipientType);
    }

    public interface INotificationPreferencesService
    {
        Task<IEnumerable<NotificationPreferencesDto>> GetByUserIdAsync(string userId);
        Task<NotificationPreferencesDto?> GetByUserAndTypeAsync(string userId, NotificationType notificationType);
        Task<NotificationPreferencesDto?> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(NotificationPreferencesDto dto, Guid userId);
        Task<bool> SaveRangeAsync(IEnumerable<NotificationPreferencesDto> dtos, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<bool> UpdatePreferenceAsync(string userId, NotificationType notificationType, bool enableEmail, bool enableSMS, bool enablePush, bool enableInApp, Guid updaterId);
    }
}
