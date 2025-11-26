using Domains.Entities.Content;
using Domains.Entities.Notification;
using Shared.DTOs.Content;
using Shared.DTOs.Notification;

namespace BL.Mapper
{
    public partial class MappingProfile
    {
        private void ConfigureContentAndNotificationChannelMappings()
        {
            // Content Management
            CreateMap<TbContentArea, ContentAreaDto>()
                .ReverseMap();

            CreateMap<TbMediaContent, MediaContentDto>()
                .ReverseMap();

            // Notification Channels
            CreateMap<TbNotificationChannel, NotificationChannelDto>()
                .ReverseMap();

            // Notifications and Preferences
            CreateMap<TbNotifications, NotificationsDto>()
                .ReverseMap();

            CreateMap<TbNotificationPreferences, NotificationPreferencesDto>()
                .ReverseMap();
        }
    }
}
