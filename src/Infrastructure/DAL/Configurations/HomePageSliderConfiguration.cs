using Domains.Entities.Merchandising.HomePage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class HomePageSliderConfiguration : IEntityTypeConfiguration<TbHomePageSlider>
    {
        public void Configure(EntityTypeBuilder<TbHomePageSlider> builder)
        {
            // Table name
            builder.ToTable("TbHomePageSlider");

            // Primary Key (inherited from BaseEntity)
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.TitleAr)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(e => e.TitleEn)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.DisplayOrder)
                .IsRequired();

            // Indexes for better performance
            builder.HasIndex(e => e.DisplayOrder)
                .HasDatabaseName("IX_HomePageSlider_DisplayOrder");

            builder.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_HomePageSlider_IsDeleted");
        }
    }
}
