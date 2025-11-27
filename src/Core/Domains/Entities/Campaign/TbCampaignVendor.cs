using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
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

        public bool IsApproved { get; set; }

        public DateTime? AppliedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [ForeignKey("ApprovedByUser")]
        public Guid? ApprovedByUserId { get; set; }

        public int TotalProductsSubmitted { get; set; } = 0;

        public int TotalProductsApproved { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSales { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCommissionPaid { get; set; } = 0m;

        [StringLength(1000)]
        public string? Notes { get; set; }

        public virtual TbCampaign Campaign { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual ApplicationUser? ApprovedByUser { get; set; }
    }
}
