using Domains.Entities.SellerRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbRequestComment
    /// </summary>
    public class RequestCommentConfiguration : IEntityTypeConfiguration<TbRequestComment>
    {
        public void Configure(EntityTypeBuilder<TbRequestComment> entity)
        {
            // Table name
            entity.ToTable("TbRequestComments");

            // Property configurations
            entity.Property(e => e.Comment)
                .IsRequired();

            entity.Property(e => e.IsInternal)
                .HasDefaultValue(false);

            // Relationships
            entity.HasOne(e => e.SellerRequest)
                .WithMany(r => r.Comments)
                .HasForeignKey(e => e.SellerRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.SellerRequestId);

            entity.HasIndex(e => e.UserId);

            entity.HasIndex(e => e.CreatedDateUtc);

            entity.HasIndex(e => new { e.SellerRequestId, e.CreatedDateUtc });
        }
    }
}
