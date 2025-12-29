using Common.Enumerations.Notification;
using Shared.GeneralModels.Parameters.Notification;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface ITemplateService
    {
        Task<string> LoadTemplateAsync(
            string templateName,
            NotificationChannel channel,
            NotificationType type,
            string language = "ar");
        string ProcessTemplate(string template, Dictionary<string, string> parameters);
        Task<NotificationTemplate> GetTemplateAsync(
            string templateName,
            NotificationChannel channel,
            NotificationType type,
            string language = "ar");
    }
}
