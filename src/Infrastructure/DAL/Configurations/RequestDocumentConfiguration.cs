using Domains.Entities.SellerRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbRequestDocument
    /// </summary>
    public class RequestDocumentConfiguration : IEntityTypeConfiguration<TbRequestDocument>
    {
        public void Configure(EntityTypeBuilder<TbRequestDocument> entity)
        {
            // Table name
            entity.ToTable("TbRequestDocuments");

            // Property configurations
            entity.Property(e => e.DocumentName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DocumentPath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.DocumentType)
                .HasMaxLength(50);

            entity.Property(e => e.FileSize)
                .IsRequired();

            entity.Property(e => e.UploadedAt)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.SellerRequest)
                .WithMany(r => r.Documents)
                .HasForeignKey(e => e.SellerRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UploadedByUser)
                .WithMany()
                .HasForeignKey(e => e.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.SellerRequestId);

            entity.HasIndex(e => e.DocumentType);

            entity.HasIndex(e => e.UploadedAt);
        }
    }
}
