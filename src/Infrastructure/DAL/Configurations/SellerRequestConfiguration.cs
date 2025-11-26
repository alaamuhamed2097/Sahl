using Domains.Entities.SellerRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSellerRequest
    /// </summary>
    public class SellerRequestConfiguration : IEntityTypeConfiguration<TbSellerRequest>
    {
        public void Configure(EntityTypeBuilder<TbSellerRequest> entity)
        {
            // Table name
            entity.ToTable("TbSellerRequests");

            // Property configurations
            entity.Property(e => e.RequestType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionEn)
                .IsRequired();

            entity.Property(e => e.DescriptionAr)
                .IsRequired();

            entity.Property(e => e.RequestData)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ReviewedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ProcessedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ReviewNotes)
                .HasMaxLength(1000);

            entity.Property(e => e.RejectionReason)
                .HasMaxLength(1000);

            entity.Property(e => e.Priority)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ReviewedByUser)
                .WithMany()
                .HasForeignKey(e => e.ReviewedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.RequestType);

            entity.HasIndex(e => e.Status);

            entity.HasIndex(e => e.Priority);

            entity.HasIndex(e => e.SubmittedAt);

            entity.HasIndex(e => new { e.VendorId, e.Status });

            entity.HasIndex(e => new { e.RequestType, e.Status });
        }
    }
}
