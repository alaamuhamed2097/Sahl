using Domains.Entities.ECommerceSystem.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class DisputeMessageConfiguration : IEntityTypeConfiguration<TbDisputeMessage>
    {
        public void Configure(EntityTypeBuilder<TbDisputeMessage> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.MessageNumber)
                .IsRequired();

            entity.Property(e => e.SenderID)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.SenderType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Message)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Attachments)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.SentDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CurrentState)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.CurrentState);
            entity.HasIndex(e => e.DisputeID);
            entity.HasIndex(e => e.SenderID);
            entity.HasIndex(e => e.SentDate);
            entity.HasIndex(e => new { e.DisputeID, e.MessageNumber }).IsUnique();

            // Relationships are configured in DisputeConfiguration
        }
    }
}
