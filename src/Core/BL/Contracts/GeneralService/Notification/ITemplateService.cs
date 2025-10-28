using Common.Enumerations.Notification;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Contracts.GeneralService.Notification
{
    public interface ITemplateService
    {
        Task<NotificationTemplate> GetTemplateAsync(string templateName, NotificationChannel channel, NotificationType type, string language = "ar");
        Task<string> LoadTemplateAsync(string templateName, NotificationChannel channel, NotificationType type, string language = "ar");
        string ProcessTemplate(string template, Dictionary<string, string> parameters);
    }
}