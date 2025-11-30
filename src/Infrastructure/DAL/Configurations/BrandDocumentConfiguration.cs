using Domains.Entities.BrandManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBrandDocument
    /// </summary>
    public class BrandDocumentConfiguration : IEntityTypeConfiguration<TbBrandDocument>
    {
        public void Configure(EntityTypeBuilder<TbBrandDocument> entity)
        {
            // Table name
            entity.ToTable("TbBrandDocuments");

            // Property configurations
            entity.Property(e => e.DocumentType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DocumentName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DocumentPath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.FileExtension)
                .HasMaxLength(50);

            entity.Property(e => e.FileSize)
                .IsRequired();

            entity.Property(e => e.UploadedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsVerified)
                .HasDefaultValue(false);

            entity.Property(e => e.VerifiedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.VerificationNotes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.BrandRegistrationRequest)
                .WithMany(r => r.Documents)
                .HasForeignKey(e => e.BrandRegistrationRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.VerifiedByUser)
                .WithMany()
                .HasForeignKey(e => e.VerifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.BrandRegistrationRequestId);

            entity.HasIndex(e => e.DocumentType);

            entity.HasIndex(e => e.IsVerified);

            entity.HasIndex(e => e.UploadedAt);

            entity.HasIndex(e => new { e.BrandRegistrationRequestId, e.DocumentType });
        }
    }
}
