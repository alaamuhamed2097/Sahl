using Domains.Entities.Catalog.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Enumerations.Pricing;

namespace DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<TbCategory>
    {
        public void Configure(EntityTypeBuilder<TbCategory> entity)
        {
            entity.ToTable("TbCategories");

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TreeViewSerial)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Persist enum as int and use enum constant as default (avoid passing raw int)
            entity.Property(e => e.PricingSystemType)
                .HasConversion<int>()
                .IsRequired()
                .HasDefaultValue(PricingSystemType.Standard);
            entity.HasOne(e => e.PricingSystemSetting)
                .WithMany()
                .HasForeignKey(e => e.PricingSystemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
