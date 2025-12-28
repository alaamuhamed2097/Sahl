using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{

	public class ItemReviewConfiguration : IEntityTypeConfiguration<TbItemReview>
	{
		public void Configure(EntityTypeBuilder<TbItemReview> entity)
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

			entity.Property(e => e.ReviewNumber).IsRequired();

			entity.Property(e => e.Rating)
				  .HasColumnType("decimal(2,1)");

			entity.Property(e => e.ReviewTitle)
				  .IsRequired()
				  .HasMaxLength(200);

			entity.Property(e => e.ReviewText)
				  .IsRequired()
				  .HasColumnType("nvarchar(max)");

			entity.Property(e => e.Status)
				  .IsRequired()
				  .HasConversion<int>();

			entity.Property(e => e.HelpfulCount)
				  .HasDefaultValue(0);

			entity.Property(e => e.NotHelpfulCount)
				  .HasDefaultValue(0);

			entity.Property(e => e.IsDeleted)
				  .HasDefaultValue(false); // ✅ غيرتها من 1 إلى false

			entity.Property(e => e.CreatedDateUtc)
				  .HasColumnType("datetime2(2)")
				  .HasDefaultValueSql("GETUTCDATE()");

			entity.Property(e => e.UpdatedDateUtc)
				  .HasColumnType("datetime2(2)");

			// Indexes
			entity.HasIndex(e => e.IsDeleted);
			entity.HasIndex(e => e.ItemId);
			entity.HasIndex(e => e.CustomerId);
			entity.HasIndex(e => e.Status);
			entity.HasIndex(e => e.Rating);
			entity.HasIndex(e => e.ReviewNumber).IsUnique();
			entity.HasIndex(e => new { e.ItemId, e.CustomerId });

			// ✅ Relationships الصحيحة
			entity.HasOne(e => e.Customer)
				  .WithMany(c => c.ItemReviews)
				  .HasForeignKey(e => e.CustomerId)
				  .OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(e => e.Item)
				  .WithMany(i => i.ItemReviews)
				  .HasForeignKey(e => e.ItemId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(e => e.ReviewVotes)
				  .WithOne(v => v.ItemReview)
				  .HasForeignKey(v => v.ItemReviewId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasMany(e => e.ReviewReports)
				  .WithOne(r => r.ItemReview)
				  .HasForeignKey(r => r.ItemReviewId)
				  .OnDelete(DeleteBehavior.Restrict); // ✅ غيرتها من Cascade
		}
	}
}
