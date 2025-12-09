using Domains.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ContentAreaConfiguration : IEntityTypeConfiguration<TbContentArea>
    {
        public void Configure(EntityTypeBuilder<TbContentArea> builder)
        {
            builder.ToTable("TbContentAreas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.AreaCode)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(500);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(1);

            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.AreaCode).IsUnique();
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.DisplayOrder);
        }
    }
}
