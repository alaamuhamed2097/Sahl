using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Notification
{
    public class NotificationDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string? Url { get; set; }
        [JsonIgnore]
        public int SenderType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
