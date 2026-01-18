using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ItemReviewConfiguration : IEntityTypeConfiguration<TbItemReview>
    {
        public void Configure(EntityTypeBuilder<TbItemReview> entity)
        {
            entity.Property(e => e.Rating)
                  .IsRequired()
                  .HasColumnType("decimal(2,1)");

            entity.Property(e => e.ReviewNumber)
                  .IsRequired()
                  .HasMaxLength(12);

            entity.Property(e => e.ReviewTitle)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.ReviewText)
                  .IsRequired()
                  .HasMaxLength(300);

            entity.Property(e => e.HelpfulVoteCount)
                  .IsRequired()
                  .HasDefaultValue(0);

            // ✅ Relationships
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
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Rating);
            entity.HasIndex(e => e.ReviewNumber).IsUnique(true);
            entity.HasIndex(e => new { e.ItemId, e.CustomerId });
        }
    }
}
