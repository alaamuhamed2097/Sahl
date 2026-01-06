using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerAddress
    /// Configures indexes, constraints, and relationships for optimal performance and data integrity
    /// </summary>
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<TbCustomerAddress>
    {
        public void Configure(EntityTypeBuilder<TbCustomerAddress> entity)
        {
            // Table Configuration
            entity.ToTable("TbCustomerAddresses");
            entity.HasKey(e => e.Id);

            // Property Configurations
            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(450); // Standard ASP.NET Identity key length

            entity.Property(e => e.RecipientName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true); // Support international characters

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false); // Numbers only, no need for unicode

            entity.Property(e => e.PhoneCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false); // Format: +20, 20, etc.

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(true); // Support international characters

            entity.Property(e => e.CityId)
                .IsRequired();

            entity.Property(e => e.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships

            // Relationship with User (AspNetUsers)
            entity.HasOne(e => e.User)
                .WithMany() // User can have many addresses
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Note: If you have a City entity, configure the relationship:
            entity.HasOne(e => e.City)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for Performance

            // Index for finding user's addresses (most common query)
            entity.HasIndex(e => new { e.UserId, e.IsDeleted })
                .HasDatabaseName("IX_CustomerAddress_UserId_IsDeleted")
                .HasFilter("[IsDeleted] = 0"); // Filtered index for active addresses only

            // Index for finding default address quickly
            entity.HasIndex(e => new { e.UserId, e.IsDefault, e.IsDeleted })
                .HasDatabaseName("IX_CustomerAddress_UserId_IsDefault_IsDeleted")
                .HasFilter("[IsDefault] = 1 AND [IsDeleted] = 0"); // Only index default active addresses

            // Index for CityId (for location-based queries)
            entity.HasIndex(e => e.CityId)
                .HasDatabaseName("IX_CustomerAddress_CityId");

            // Index for soft delete queries
            entity.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_CustomerAddress_IsDeleted");

            // Query Filters

            // Global query filter to exclude soft-deleted records by default
            // Can be overridden with .IgnoreQueryFilters() when needed
            entity.HasQueryFilter(e => !e.IsDeleted);

            // Constraints & Validations

            // Check constraint: Ensure phone number is digits only
            entity.HasCheckConstraint(
                "CK_CustomerAddress_PhoneNumber_Format",
                "[PhoneNumber] NOT LIKE '%[^0-9]%'"
            );

            // Check constraint: Ensure phone code starts with + or is digits
            entity.HasCheckConstraint(
                "CK_CustomerAddress_PhoneCode_Format",
                "[PhoneCode] LIKE '+[0-9]%' OR [PhoneCode] NOT LIKE '%[^0-9]%'"
            );
        }
    }
}