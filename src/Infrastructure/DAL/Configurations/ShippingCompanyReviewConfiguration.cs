using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ShippingCompanyReviewConfiguration : IEntityTypeConfiguration<TbShippingCompanyReview>
    {
        public void Configure(EntityTypeBuilder<TbShippingCompanyReview> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            // Properties
            entity.Property(e => e.Rating)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(e => e.ReviewText)
                  .HasColumnType("nvarchar(max)");

            entity.Property(e => e.IsDeleted)
                  .HasDefaultValue(false);

            entity.Property(e => e.CreatedDateUtc)
                  .HasColumnType("datetime2(2)")
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                  .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.OrderDetailId);
            entity.HasIndex(e => e.ShippingCompanyId);


            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.ShippingCompanyReviews)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OrderDetail)
                  .WithMany()
                  .HasForeignKey(e => e.OrderDetailId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ShippingCompany)
                  .WithMany(s => s.ShippingCompanyReviews)
                  .HasForeignKey(e => e.ShippingCompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
