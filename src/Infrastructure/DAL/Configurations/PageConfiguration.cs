using Domains.Entities.Page;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbPage
    /// </summary>
    public class PageConfiguration : IEntityTypeConfiguration<TbPage>
    {
        public void Configure(EntityTypeBuilder<TbPage> entity)
        {
            // Property configurations
            entity.Property(e => e.TitleEn)
                .IsRequired()
                        .HasMaxLength(100);

            entity.Property(e => e.TitleAr)
              .IsRequired()
           .HasMaxLength(100);

            entity.Property(e => e.ContentEn)
      .IsRequired()
         .HasColumnType("nvarchar(max)");

            entity.Property(e => e.ContentAr)
           .IsRequired()
      .HasColumnType("nvarchar(max)");

            entity.Property(e => e.ShortDescriptionEn)
               .HasMaxLength(500);

            entity.Property(e => e.ShortDescriptionAr)
       .HasMaxLength(500);
        }
    }
}
