using Domains.Entities.Base;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BrandManagement
{
    public class TbAuthorizedDistributor : BaseEntity
    {
        [Required]
        [ForeignKey("Brand")]
        public Guid BrandId { get; set; }

        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [StringLength(100)]
        public string AuthorizationNumber { get; set; } = string.Empty;

        [Required]
        public DateTime AuthorizationStartDate { get; set; }

        public DateTime? AuthorizationEndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? AuthorizationDocumentPath { get; set; }

        public DateTime? VerifiedAt { get; set; }

        [ForeignKey("VerifiedByUser")]
        public string? VerifiedByUserId { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual TbBrand Brand { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual ApplicationUser? VerifiedByUser { get; set; }
    }
}
