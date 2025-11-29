using Common.Enumerations.Brand;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BrandManagement
{
    public class TbBrandRegistrationRequest : BaseEntity
    {
        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [StringLength(200)]
        public string BrandNameEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string BrandNameAr { get; set; } = string.Empty;

        [Required]
        public BrandType BrandType { get; set; }

        [Required]
        public BrandRegistrationStatus Status { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        [StringLength(200)]
        public string? OfficialWebsite { get; set; }

        [StringLength(100)]
        public string? TrademarkNumber { get; set; }

        public DateTime? TrademarkExpiryDate { get; set; }

        [StringLength(100)]
        public string? CommercialRegistrationNumber { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        [ForeignKey("ReviewedByUser")]
        public string? ReviewedByUserId { get; set; }

        [StringLength(1000)]
        public string? ReviewNotes { get; set; }

        [StringLength(1000)]
        public string? RejectionReason { get; set; }

        [ForeignKey("ApprovedBrand")]
        public Guid? ApprovedBrandId { get; set; }

        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual ApplicationUser? ReviewedByUser { get; set; }
        public virtual TbBrand? ApprovedBrand { get; set; }
        public ICollection<TbBrandDocument> Documents { get; set; } = new HashSet<TbBrandDocument>();
    }
}
