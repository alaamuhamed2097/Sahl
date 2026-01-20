using Domains.Entities.Notification;
using Shared.DTOs.Notification;

namespace BL.Mapper;

public partial class MappingProfile
{
    private void ConfigureNotificationChannelMappings()
    {
        // Notification Channels
        CreateMap<TbNotificationChannel, NotificationChannelDto>()
            .ReverseMap();

        // Notifications and Preferences
        CreateMap<TbNotification, NotificationsDto>()
            .ReverseMap();

        CreateMap<TbNotificationPreferences, NotificationPreferencesDto>()
            .ReverseMap();
    }
}
