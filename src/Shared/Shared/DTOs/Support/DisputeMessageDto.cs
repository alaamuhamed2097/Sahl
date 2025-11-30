using Shared.DTOs.Base;

namespace Shared.DTOs.Support
{
    public class DisputeMessageDto : BaseDto
    {
        public Guid DisputeID { get; set; }
        public int MessageNumber { get; set; }
        public string SenderID { get; set; } = null!;
        public string SenderType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Attachments { get; set; }
        public DateTime SentDate { get; set; }
    }
}
