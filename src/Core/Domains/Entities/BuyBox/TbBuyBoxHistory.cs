using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BuyBox
{
    public class TbBuyBoxHistory : BaseEntity
    {
        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Score { get; set; }

        [Required]
        public DateTime WonAt { get; set; }

        public DateTime? LostAt { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [StringLength(500)]
        public string? LossReason { get; set; }

        public virtual TbItem Item { get; set; } = null!;
        public virtual TbOffer Offer { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
    }
}
