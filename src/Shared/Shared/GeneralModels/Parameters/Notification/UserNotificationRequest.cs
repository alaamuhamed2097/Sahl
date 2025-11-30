using Resources;
using Resources.Enumerations;
using System.Text.Json.Serialization;

namespace Shared.GeneralModels.Parameters.Notification
{
    public class UserNotificationRequest
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        // Message property was missing and referenced in BL service
        public string Message { get; set; } = string.Empty;

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        [JsonIgnore]
        public string Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [JsonIgnore]
        public string Description => ResourceManager.CurrentLanguage == Language.Arabic ? DescriptionAr : DescriptionEn;

        public bool IsRead { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public string TimeAgo { get; set; }
    }
}
