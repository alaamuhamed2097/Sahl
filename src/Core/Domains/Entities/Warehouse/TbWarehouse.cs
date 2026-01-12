using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Warehouse
{
    public class TbWarehouse : BaseEntity
    {
        

        [MaxLength(500)]
        public string? Address { get; set; }

        // Warehouse type indicators 
        public bool IsDefaultPlatformWarehouse { get; set; } = false;

        [ForeignKey("Vendor")]
        public Guid? VendorId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual TbVendor? Vendor { get; set; }
        public virtual ICollection<TbOffer> Offers { get; set; } = new List<TbOffer>();
    }
}
