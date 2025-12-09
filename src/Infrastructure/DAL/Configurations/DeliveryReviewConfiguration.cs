using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class DeliveryReviewConfiguration : IEntityTypeConfiguration<TbDeliveryReview>
    {
        public void Configure(EntityTypeBuilder<TbDeliveryReview> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ReviewNumber)
                .IsRequired();

            entity.Property(e => e.PackagingRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.CourierDeliveryRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.OverallRating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.ShippingAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.OrderID);
            entity.HasIndex(e => e.CustomerID);
            entity.HasIndex(e => e.OverallRating);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ReviewNumber).IsUnique();
            entity.HasIndex(e => e.ReviewDate);
            entity.HasIndex(e => new { e.OrderID, e.CustomerID });
        }
    }
}
