using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class DevelopmentSettingsConfiguration : IEntityTypeConfiguration<TbDevelopmentSettings>
{
    private static readonly Guid SeedId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public void Configure(EntityTypeBuilder<TbDevelopmentSettings> builder)
    {
        // Index for potential querying (though likely not needed for a single-row table)
        builder.HasIndex(d => d.IsMultiVendorSystem)
               .HasDatabaseName("IX_TbDevelopmentSettings_IsMultiVendorSystem");

        // Seed the single global settings record
        builder.HasData(new TbDevelopmentSettings
        {
            Id = SeedId,
            IsMultiVendorSystem = true
        });

        // Optional: Enforce only one row allowed
        builder.HasCheckConstraint(
            "CK_TbDevelopmentSettings_SingleRow",
            $"Id = '{SeedId}'"
        );
    }
}