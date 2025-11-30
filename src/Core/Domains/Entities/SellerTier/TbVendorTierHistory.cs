using Domains.Entities.Base;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.SellerTier
{
    public class TbVendorTierHistory : BaseEntity
    {
        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [ForeignKey("SellerTier")]
        public Guid SellerTierId { get; set; }

        [Required]
        public DateTime AchievedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        [Required]
        public int TotalOrdersAtTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSalesAtTime { get; set; }

        public bool IsAutomatic { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbSellerTier SellerTier { get; set; } = null!;
    }
}
