using Domains.Entities.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PricingSystemSettingConfiguration : IEntityTypeConfiguration<TbPricingSystemSetting>
    {
        public void Configure(EntityTypeBuilder<TbPricingSystemSetting> entity)
        {
            entity.ToTable("TbPricingSystemSettings");

            entity.Property(e => e.SystemNameAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.SystemNameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.SystemType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true);

            entity.Property(e => e.DisplayOrder)
                .IsRequired();

            // Ensure SystemType is unique (one row per system type)
            entity.HasIndex(e => e.SystemType)
                .IsUnique()
                .HasDatabaseName("IX_TbPricingSystemSettings_SystemType");

            // Seed default systems (provide base entity audit fields)
            entity.HasData(
                new TbPricingSystemSetting
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    SystemNameAr = "التسعير القياسي",
                    SystemNameEn = "Standard Pricing",
                    SystemType = Common.Enumerations.Pricing.PricingSystemType.Standard,
                    IsEnabled = true,
                    DisplayOrder = 1,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    CreatedDateUtc = new System.DateTime(2025, 11, 30, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new TbPricingSystemSetting
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    SystemNameAr = "تسعير بالتركيبات",
                    SystemNameEn = "Combination Pricing",
                    SystemType = Common.Enumerations.Pricing.PricingSystemType.Combination,
                    IsEnabled = true,
                    DisplayOrder = 2,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    CreatedDateUtc = new System.DateTime(2025, 11, 30, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new TbPricingSystemSetting
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    SystemNameAr = "تسعير حسب الكمية",
                    SystemNameEn = "Quantity Pricing",
                    SystemType = Common.Enumerations.Pricing.PricingSystemType.Quantity,
                    IsEnabled = true,
                    DisplayOrder = 3,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    CreatedDateUtc = new System.DateTime(2025, 11, 30, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new TbPricingSystemSetting
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    SystemNameAr = "التركيبات مع مستويات الكمية",
                    SystemNameEn = "Combination + Quantity",
                    SystemType = Common.Enumerations.Pricing.PricingSystemType.CombinationWithQuantity,
                    IsEnabled = true,
                    DisplayOrder = 4,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    CreatedDateUtc = new System.DateTime(2025, 11, 30, 0, 0, 0, System.DateTimeKind.Utc)
                },
                new TbPricingSystemSetting
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    SystemNameAr = "تسعير حسب شريحة العميل",
                    SystemNameEn = "Customer Segment Pricing",
                    SystemType = Common.Enumerations.Pricing.PricingSystemType.CustomerSegmentPricing,
                    IsEnabled = true,
                    DisplayOrder = 5,
                    IsDeleted = false,
                    CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    CreatedDateUtc = new System.DateTime(2025, 11, 30, 0, 0, 0, System.DateTimeKind.Utc)
                }
            );
        }
    }
}
