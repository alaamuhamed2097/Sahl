using Shared.DTOs.Base;

namespace Shared.DTOs.Support
{
    public class SupportTicketMessageDto : BaseDto
    {
        public Guid TicketID { get; set; }
        public int MessageNumber { get; set; }
        public string SenderID { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Attachments { get; set; }
        public DateTime? SentDate { get; set; }
        public string? InternalNote { get; set; }
    }
}
