using Shared.DTOs.Base;

namespace Shared.DTOs.Notification
{
    public class NotificationDto : BaseDto
    {
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
