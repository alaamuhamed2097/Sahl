using Common.Enumerations.Brand;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.BrandManagement
{
    public class TbBrandDocument : BaseEntity
    {
        [Required]
        [ForeignKey("BrandRegistrationRequest")]
        public Guid BrandRegistrationRequestId { get; set; }

        [Required]
        public BrandDocumentType DocumentType { get; set; }

        [Required]
        [StringLength(200)]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string DocumentPath { get; set; } = string.Empty;

        [StringLength(50)]
        public string? FileExtension { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }

        public bool IsVerified { get; set; }

        public DateTime? VerifiedAt { get; set; }

        [ForeignKey("VerifiedByUser")]
        public string? VerifiedByUserId { get; set; }

        [StringLength(500)]
        public string? VerificationNotes { get; set; }

        public virtual TbBrandRegistrationRequest BrandRegistrationRequest { get; set; } = null!;
        public virtual ApplicationUser? VerifiedByUser { get; set; }
    }
}
