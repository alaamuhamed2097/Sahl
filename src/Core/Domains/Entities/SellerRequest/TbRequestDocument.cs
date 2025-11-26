using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.SellerRequest
{
    public class TbRequestDocument : BaseEntity
    {
        [Required]
        [ForeignKey("SellerRequest")]
        public Guid SellerRequestId { get; set; }

        [Required]
        [StringLength(200)]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string DocumentPath { get; set; } = string.Empty;

        [StringLength(50)]
        public string? DocumentType { get; set; }

        public long FileSize { get; set; }

        [ForeignKey("UploadedByUser")]
        public string? UploadedByUserId { get; set; }

        public DateTime? UploadedAt { get; set; }

        public virtual TbSellerRequest SellerRequest { get; set; } = null!;
        public virtual ApplicationUser? UploadedByUser { get; set; }
    }
}
