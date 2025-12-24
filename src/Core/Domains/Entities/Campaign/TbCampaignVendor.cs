using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Campaign
{
    public class TbCampaignVendor : BaseEntity
    {
        [Required]
        [ForeignKey("Campaign")]
        public Guid CampaignId { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        // Approval
        public bool IsApproved { get; set; } = false;
        public DateTime? ApprovedAt { get; set; }

        // Optional notes (rejection reason, terms, etc.)
        [StringLength(1000)]
        public string? Notes { get; set; }

        // Relations
        public virtual TbCampaign Campaign { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
    }
}
