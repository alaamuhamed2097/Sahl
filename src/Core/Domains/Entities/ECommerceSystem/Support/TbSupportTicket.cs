namespace Domains.Entities.ECommerceSystem.Support
{
    public class TbSupportTicket : BaseEntity
    {
        public int TicketNumber { get; set; }
        public string UserID { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public DateTime? TicketCreatedDate { get; set; }
        public string? AssignedTo { get; set; }
        public string? AssignedTeam { get; set; }

        // Navigation Properties
        public virtual ICollection<TbSupportTicketMessage> SupportTicketMessages { get; set; } = new List<TbSupportTicketMessage>();
    }
}
