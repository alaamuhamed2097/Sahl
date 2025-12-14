using Domains.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for ApplicationUser
    /// Configures phone-based authentication and email as optional
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Phone Code - Required
            builder.Property(u => u.PhoneCode)
                .IsRequired()
                .HasMaxLength(5)
                .HasColumnType("nvarchar(5)");

            // Phone Number - Required (inherited from IdentityUser)
            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("nvarchar(15)");

            // Normalized Phone - Required for phone-based lookups
            // Format: {PhoneCode}{PhoneNumber} (e.g., "+201001234567")
            builder.Property(u => u.NormalizedPhone)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)")
                .IsRequired(false);

            // Email - Now optional (nullable)
            builder.Property(u => u.Email)
                .IsRequired(false)
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            // UserName - Can be optional (nullable for flexibility)
            builder.Property(u => u.UserName)
                .IsRequired(false)
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            // First Name - Required
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // Last Name - Required
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            // Profile Image Path - Optional
            builder.Property(u => u.ProfileImagePath)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            // Created By - Guid
            builder.Property(u => u.CreatedBy)
                .HasColumnType("uniqueidentifier");

            // Created Date UTC - DateTime
            builder.Property(u => u.CreatedDateUtc)
                .IsRequired()
                .HasColumnType("datetime2(7)");

            // Updated By - Guid (nullable)
            builder.Property(u => u.UpdatedBy)
                .HasColumnType("uniqueidentifier")
                .IsRequired(false);

            // Updated Date UTC - DateTime (nullable)
            builder.Property(u => u.UpdatedDateUtc)
                .HasColumnType("datetime2(7)")
                .IsRequired(false);

            // Last Login Date - DateTime (nullable)
            builder.Property(u => u.LastLoginDate)
                .HasColumnType("datetime2(7)")
                .IsRequired(false);

            // User State - Enum
            builder.Property(u => u.UserState)
                .IsRequired()
                .HasConversion<int>();

            // Indexes
            // Unique index on NormalizedPhone for phone-based authentication
            builder.HasIndex(u => u.NormalizedPhone)
                .IsUnique()
                .HasDatabaseName("IX_Users_NormalizedPhone")
                .HasFilter("[NormalizedPhone] IS NOT NULL");

            // Unique index on Email (allowing nulls for optional email)
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email")
                .HasFilter("[Email] IS NOT NULL");

            // Unique index on UserName (allowing nulls for optional username)
            builder.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName")
                .HasFilter("[UserName] IS NOT NULL");

            // Index on Created Date for sorting
            builder.HasIndex(u => u.CreatedDateUtc)
                .HasDatabaseName("IX_Users_CreatedDateUtc");

            // Index on User State for filtering
            builder.HasIndex(u => u.UserState)
                .HasDatabaseName("IX_Users_UserState");
        }
    }
}
