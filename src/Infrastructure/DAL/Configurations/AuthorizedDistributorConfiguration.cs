using Domains.Entities.BrandManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbAuthorizedDistributor
    /// </summary>
    public class AuthorizedDistributorConfiguration : IEntityTypeConfiguration<TbAuthorizedDistributor>
    {
        public void Configure(EntityTypeBuilder<TbAuthorizedDistributor> entity)
        {
            // Table name
            entity.ToTable("TbAuthorizedDistributors");

            // Property configurations
            entity.Property(e => e.AuthorizationNumber)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.AuthorizationStartDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.AuthorizationEndDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.AuthorizationDocumentPath)
                .HasMaxLength(500);

            entity.Property(e => e.VerifiedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Brand)
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.VerifiedByUser)
                .WithMany()
                .HasForeignKey(e => e.VerifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.BrandId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.AuthorizationNumber)
                .IsUnique();

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.BrandId, e.VendorId })
                .IsUnique();

            entity.HasIndex(e => new { e.AuthorizationStartDate, e.AuthorizationEndDate });
        }
    }
}
