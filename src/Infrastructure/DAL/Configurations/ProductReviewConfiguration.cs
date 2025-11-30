using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ProductReviewConfiguration : IEntityTypeConfiguration<TbProductReview>
    {
        public void Configure(EntityTypeBuilder<TbProductReview> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ReviewNumber)
                .IsRequired();

            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2,1)");

            entity.Property(e => e.ReviewTitle)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.ReviewText)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Images)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Videos)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ModerationNotes)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.VendorResponse)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.IsVerifiedPurchase)
                .HasDefaultValue(false);

            entity.Property(e => e.HelpfulCount)
                .HasDefaultValue(0);

            entity.Property(e => e.NotHelpfulCount)
                .HasDefaultValue(0);

            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ApprovedDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.VendorResponseDate)
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
            entity.HasIndex(e => e.ProductID);
            entity.HasIndex(e => e.CustomerID);
            entity.HasIndex(e => e.OrderItemID);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Rating);
            entity.HasIndex(e => e.IsVerifiedPurchase);
            entity.HasIndex(e => e.ReviewNumber).IsUnique();
            entity.HasIndex(e => e.ReviewDate);
            entity.HasIndex(e => new { e.ProductID, e.CustomerID });

            // Relationships
            entity.HasMany(e => e.ReviewVotes)
                .WithOne(v => v.Review)
                .HasForeignKey(v => v.ReviewID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
