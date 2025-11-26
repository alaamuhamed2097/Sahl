namespace Domains.Entities.ECommerceSystem.Support
{
    public class TbDispute : BaseEntity
    {
        public int DisputeNumber { get; set; }
        public Guid MessageID { get; set; }
        public string SenderID { get; set; } = null!;
        public Guid OrderID { get; set; }
        public string Parties { get; set; } = null!;
        public string SenderType { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string RequiredResolution { get; set; } = null!;
        public string RequestedRefund { get; set; } = null!;
        public string Evidence { get; set; } = null!;
        public string PlatformDecision { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Resolution { get; set; } = null!;
        public DateTime? ResolutionDate { get; set; }
        public string ResolvedNotes { get; set; } = null!;
        public DateTime? OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string? AssignedAdminID { get; set; }

        // Navigation Properties
        public virtual ICollection<TbDisputeMessage> DisputeMessages { get; set; } = new List<TbDisputeMessage>();
    }
}
