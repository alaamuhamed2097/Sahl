using Domains.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MediaContentConfiguration : IEntityTypeConfiguration<TbMediaContent>
    {
        public void Configure(EntityTypeBuilder<TbMediaContent> builder)
        {
            builder.ToTable("TbMediaContents");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(500);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(500);

            builder.Property(x => x.MediaType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Image");

            builder.Property(x => x.MediaPath)
                .HasMaxLength(500);

            builder.Property(x => x.LinkUrl)
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

            builder.HasOne(x => x.ContentArea)
                .WithMany(c => c.MediaContents)
                .HasForeignKey(x => x.ContentAreaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.ContentAreaId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.DisplayOrder);
            builder.HasIndex(x => x.MediaType);
        }
    }
}
