using Domins.Entities.Currency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCurrency
    /// </summary>
    public class CurrencyConfiguration : IEntityTypeConfiguration<TbCurrency>
    {
        public void Configure(EntityTypeBuilder<TbCurrency> entity)
        {
            // Property configurations
            entity.Property(e => e.Code)
 .IsRequired()
        .HasMaxLength(3);

            entity.Property(e => e.NameEn)
                 .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.NameAr)
               .IsRequired()
              .HasMaxLength(100);

            entity.Property(e => e.Symbol)
                      .IsRequired()
              .HasMaxLength(5);

            entity.Property(e => e.ExchangeRate)
                  .HasColumnType("decimal(18,6)")
                 .IsRequired();

            entity.Property(e => e.CountryCode)
          .HasMaxLength(3);

            // Indexes
            entity.HasIndex(e => e.Code)
            .IsUnique(true);

            entity.HasIndex(e => e.IsBaseCurrency)
         .IsUnique(false);

            entity.HasIndex(e => e.IsActive)
             .IsUnique(false);
        }
    }
}
