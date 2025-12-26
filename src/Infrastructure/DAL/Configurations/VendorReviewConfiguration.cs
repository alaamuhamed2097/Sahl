using Common.Enumerations.Review;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class VendorReviewConfiguration : IEntityTypeConfiguration<TbVendorReview>
    {
        public void Configure(EntityTypeBuilder<TbVendorReview> entity)
        {
			entity.HasKey(e => e.Id);

			entity.Property(e => e.Id)
				  .HasDefaultValueSql("NEWID()");

			entity.Property(e => e.Rating)
				  .HasColumnType("decimal(2,1)")
				  .IsRequired();

			entity.Property(e => e.ReviewText)
				  .HasMaxLength(1000);

			entity.Property(e => e.Status)
				  .HasConversion<int>()
				  .HasDefaultValue(ReviewStatus.Pending);

			entity.Property(e => e.IsEdited)
				  .HasDefaultValue(false);

			entity.Property(e => e.IsVerifiedPurchase)
				  .HasDefaultValue(false);

			// Indexes
			entity.HasIndex(e => e.CustomerId);
			entity.HasIndex(e => e.VendorId);
			entity.HasIndex(e => e.OrderDetailId);
			entity.HasIndex(e => e.Status);
			entity.HasIndex(e => e.Rating);

			entity.HasIndex(e => new { e.CustomerId, e.VendorId })
				  .IsUnique();

			// Relationships
			entity.HasOne(e => e.Customer)
				.WithMany(c => c.VendorReviews)
				.HasForeignKey(e => e.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Vendor)
				  .WithMany(v => v.VendorReviews)
				  .HasForeignKey(e => e.VendorId)
				  .OnDelete(DeleteBehavior.Restrict);

			// ✅ الحل: تحديد Foreign Key بشكل صريح
			entity.HasOne(e => e.OrderDetail)
				  .WithMany()
				  .HasForeignKey(e => e.OrderDetailId)  // ✅ هذا موجود بالفعل وصحيح
				  .OnDelete(DeleteBehavior.Restrict)
				  .IsRequired(false);




		}
	}
}
