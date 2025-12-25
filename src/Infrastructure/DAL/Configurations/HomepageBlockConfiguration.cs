using Domains.Entities.Merchandising;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbHomepageBlock
    /// </summary>
    public class HomepageBlockConfiguration : IEntityTypeConfiguration<TbHomepageBlock>
    {
        public void Configure(EntityTypeBuilder<TbHomepageBlock> entity)
        {
            // Table name
            entity.ToTable("TbHomepageBlocks");

            // Property configurations
            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DisplayOrder)
                .IsRequired();

            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true);

            // Indexes

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => e.IsVisible);

            entity.HasIndex(e => new { e.IsVisible, e.DisplayOrder });
        }
    }
}
