using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SalesReviewConfiguration : IEntityTypeConfiguration<TbSalesReview>
    {
        public void Configure(EntityTypeBuilder<TbSalesReview> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ReviewNumber)
                .IsRequired();

            entity.Property(e => e.ProductAccuracyRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.ShippingSpeedRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.CommunicationRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.ServiceRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.OverallRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CurrentState)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.CurrentState);
            entity.HasIndex(e => e.VendorID);
            entity.HasIndex(e => e.CustomerID);
            entity.HasIndex(e => e.OrderItemID);
            entity.HasIndex(e => e.OverallRating);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ReviewNumber).IsUnique();
            entity.HasIndex(e => e.ReviewDate);
            entity.HasIndex(e => new { e.VendorID, e.CustomerID });
        }
    }
}
