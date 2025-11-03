using Domins.Entities.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbItem
    /// </summary>
    public class ItemConfiguration : IEntityTypeConfiguration<TbItem>
    {
        public void Configure(EntityTypeBuilder<TbItem> entity)
        {
            // Property configurations
            entity.Property(e => e.TitleAr)
                    .IsRequired()
          .HasMaxLength(100);

            entity.Property(e => e.TitleEn)
                 .IsRequired()
                     .HasMaxLength(100);

            entity.Property(e => e.ShortDescriptionAr)
              .IsRequired()
         .HasMaxLength(200);

            entity.Property(e => e.ShortDescriptionEn)
            .IsRequired()
              .HasMaxLength(200);

            entity.Property(e => e.VideoLink)
                .HasMaxLength(200);

            entity.Property(e => e.ThumbnailImage)
   .IsRequired()
   .HasMaxLength(200);

            entity.Property(e => e.SEOTitle)
                      .IsRequired()
                   .HasMaxLength(100);

            entity.Property(e => e.SEOMetaTags)
        .IsRequired()
     .HasMaxLength(200);

            entity.Property(e => e.Price)
           .IsRequired()
           .HasColumnType("decimal(18,2)");

            entity.Property(e => e.IsNewArrival)
         .HasDefaultValue(false);

            entity.Property(e => e.IsBestSeller)
             .HasDefaultValue(false);

            entity.Property(e => e.IsRecommended)
                      .HasDefaultValue(false);

            // Indexes
            entity.HasIndex(e => e.IsNewArrival).IsUnique(false);
            entity.HasIndex(e => e.IsBestSeller).IsUnique(false);
            entity.HasIndex(e => e.IsRecommended).IsUnique(false);
            entity.HasIndex(e => e.Price).IsUnique(false);
            entity.HasIndex(e => e.CategoryId).IsUnique(false);
            entity.HasIndex(e => e.UnitId).IsUnique(false);
            entity.HasIndex(e => e.BrandId).IsUnique(false);
            entity.HasIndex(e => e.CurrentState).IsUnique(false);

            // Relationships
            entity.HasOne(i => i.Category)
          .WithMany(c => c.Items)
             .HasForeignKey(i => i.CategoryId)
       .HasConstraintName("FK_TbItems_TbCategories_CategoryId")
         .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Unit)
            .WithMany()
          .HasForeignKey(i => i.UnitId)
          .HasConstraintName("FK_TbItems_TbUnits_UnitId")
        .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Brand)
             .WithMany(b => b.Items)
       .HasForeignKey(i => i.BrandId)
           .HasConstraintName("FK_TbItems_TbBrands_BrandId")
      .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(i => i.VideoProvider)
              .WithMany()
               .HasForeignKey(i => i.VideoProviderId)
                  .HasConstraintName("FK_TbItems_TbVideoProviders_VideoProviderId")
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
