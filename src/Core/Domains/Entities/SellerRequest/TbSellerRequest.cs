using Common.Enumerations.SellerRequest;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.SellerRequest
{
    public class TbSellerRequest : BaseEntity
    {
        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        public SellerRequestType RequestType { get; set; }

        [Required]
        public SellerRequestStatus Status { get; set; }

        [Required]
        [StringLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required]
        public string DescriptionAr { get; set; } = string.Empty;

        public string? RequestData { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        [ForeignKey("ReviewedByUser")]
        public string? ReviewedByUserId { get; set; }

        [StringLength(1000)]
        public string? ReviewNotes { get; set; }

        [StringLength(1000)]
        public string? RejectionReason { get; set; }

        public int Priority { get; set; } = 0;

        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual ApplicationUser? ReviewedByUser { get; set; }
        public ICollection<TbRequestComment> Comments { get; set; } = new HashSet<TbRequestComment>();
        public ICollection<TbRequestDocument> Documents { get; set; } = new HashSet<TbRequestDocument>();
    }
}
