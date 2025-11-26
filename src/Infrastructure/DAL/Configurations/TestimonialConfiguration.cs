using DAL.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbTestimonial
    /// </summary>
    public class TestimonialConfiguration : IEntityTypeConfiguration<TbTestimonial>
    {
        public void Configure(EntityTypeBuilder<TbTestimonial> entity)
        {
            // Property configurations
            entity.Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CustomerTitle)
                .IsRequired()
                 .HasMaxLength(100);

            entity.Property(e => e.CustomerImagePath)
           .HasMaxLength(200);

            entity.Property(e => e.TestimonialText)
         .IsRequired()
       .HasMaxLength(1000);

            entity.Property(e => e.DisplayOrder)
         .HasDefaultValue(0);

            // Indexes
            entity.HasIndex(e => e.DisplayOrder)
                .IsUnique(false);
        }
    }
}
