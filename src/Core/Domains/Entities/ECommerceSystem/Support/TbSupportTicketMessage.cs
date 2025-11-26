namespace Domains.Entities.ECommerceSystem.Support
{
    public class TbSupportTicketMessage : BaseEntity
    {
        public Guid TicketID { get; set; }
        public int MessageNumber { get; set; }
        public string SenderID { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Attachments { get; set; }
        public DateTime? SentDate { get; set; }
        public string? InternalNote { get; set; }

        // Navigation Properties
        public virtual TbSupportTicket SupportTicket { get; set; } = null!;
    }
}
