using Domains.Entities.ECommerceSystem.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class DisputeConfiguration : IEntityTypeConfiguration<TbDispute>
    {
        public void Configure(EntityTypeBuilder<TbDispute> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.DisputeNumber)
                .IsRequired();

            entity.Property(e => e.SenderID)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.Parties)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.SenderType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Details)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.RequiredResolution)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Evidence)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.PlatformDecision)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Resolution)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.ResolvedNotes)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.AssignedAdminID)
                .HasMaxLength(450);

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.OrderID);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.SenderID);
            entity.HasIndex(e => e.DisputeNumber).IsUnique();
            entity.HasIndex(e => e.CreatedDateUtc);

            // Relationships
            entity.HasMany(e => e.DisputeMessages)
                .WithOne(m => m.Dispute)
                .HasForeignKey(m => m.DisputeID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
