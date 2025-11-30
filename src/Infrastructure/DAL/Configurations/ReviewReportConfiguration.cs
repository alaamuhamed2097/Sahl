using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ReviewReportConfiguration : IEntityTypeConfiguration<TbReviewReport>
    {
        public void Configure(EntityTypeBuilder<TbReviewReport> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ReportID)
                .IsRequired();

            entity.Property(e => e.ReviewID)
                .IsRequired();

            entity.Property(e => e.CustomerID)
                .IsRequired();

            entity.Property(e => e.Reason)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Details)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.CurrentState)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.CurrentState);
            entity.HasIndex(e => e.ReviewID);
            entity.HasIndex(e => e.CustomerID);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ReportID).IsUnique();

            // Relationships
            entity.HasOne(e => e.Review)
                .WithMany(r => r.ReviewReports)
                .HasForeignKey(e => e.ReviewID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
