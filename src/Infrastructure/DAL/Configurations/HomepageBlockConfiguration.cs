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

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.BlockType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DisplayOrder)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.IsVisible)
                .HasDefaultValue(true);

            entity.Property(e => e.BackgroundColor)
                .HasMaxLength(50);

            entity.Property(e => e.BackgroundImagePath)
                .HasMaxLength(500);

            entity.Property(e => e.TextColor)
                .HasMaxLength(50);

            entity.Property(e => e.CssClass)
                .HasMaxLength(100);

            entity.Property(e => e.ShowViewAllLink)
                .HasDefaultValue(true);

            entity.Property(e => e.ViewAllLinkUrl)
                .HasMaxLength(200);

            entity.Property(e => e.ActiveFrom)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ActiveTo)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.BlockType);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.IsVisible);

            entity.HasIndex(e => new { e.IsActive, e.DisplayOrder });

            entity.HasIndex(e => new { e.ActiveFrom, e.ActiveTo });
        }
    }
}
