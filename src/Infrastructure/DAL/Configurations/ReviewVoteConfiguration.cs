using Domains.Entities.ECommerceSystem.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ReviewVoteConfiguration : IEntityTypeConfiguration<TbReviewVote>
    {
        public void Configure(EntityTypeBuilder<TbReviewVote> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.VoteType)
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
            entity.HasIndex(e => e.VoteType);
            entity.HasIndex(e => new { e.ReviewID, e.CustomerID, e.VoteType }).IsUnique();

            // Relationships are configured in ProductReviewConfiguration
        }
    }
}
