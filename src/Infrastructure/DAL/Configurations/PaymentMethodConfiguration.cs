using Domains.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<TbPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<TbPaymentMethod> builder)
        {
            builder.ToTable("TbPaymentMethods");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.MethodType)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.ProviderDetails)
                .HasMaxLength(500);

            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.MethodType);
        }
    }
}
