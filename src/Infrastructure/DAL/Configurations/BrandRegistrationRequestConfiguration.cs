using Domains.Entities.BrandManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBrandRegistrationRequest
    /// </summary>
    public class BrandRegistrationRequestConfiguration : IEntityTypeConfiguration<TbBrandRegistrationRequest>
    {
        public void Configure(EntityTypeBuilder<TbBrandRegistrationRequest> entity)
        {
            // Table name
            entity.ToTable("TbBrandRegistrationRequests");

            // Property configurations
            entity.Property(e => e.BrandNameEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.BrandNameAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.BrandType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.OfficialWebsite)
                .HasMaxLength(200);

            entity.Property(e => e.TrademarkNumber)
                .HasMaxLength(100);

            entity.Property(e => e.TrademarkExpiryDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CommercialRegistrationNumber)
                .HasMaxLength(100);

            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ReviewedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ReviewNotes)
                .HasMaxLength(1000);

            entity.Property(e => e.RejectionReason)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ReviewedByUser)
                .WithMany()
                .HasForeignKey(e => e.ReviewedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ApprovedBrand)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.BrandType);

            entity.HasIndex(e => e.Status);

            entity.HasIndex(e => e.SubmittedAt);

            entity.HasIndex(e => e.TrademarkNumber);

            entity.HasIndex(e => new { e.VendorId, e.Status });

            entity.HasIndex(e => new { e.BrandNameEn, e.VendorId });
        }
    }
}
