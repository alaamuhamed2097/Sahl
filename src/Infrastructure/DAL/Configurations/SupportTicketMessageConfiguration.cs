using Domains.Entities.ECommerceSystem.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SupportTicketMessageConfiguration : IEntityTypeConfiguration<TbSupportTicketMessage>
    {
        public void Configure(EntityTypeBuilder<TbSupportTicketMessage> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.MessageNumber)
                .IsRequired();

            entity.Property(e => e.SenderID)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.UserType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Message)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Attachments)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.InternalNote)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.SentDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(1);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.TicketID);
            entity.HasIndex(e => e.SenderID);
            entity.HasIndex(e => e.SentDate);
            entity.HasIndex(e => new { e.TicketID, e.MessageNumber }).IsUnique();

            // Relationships are configured in SupportTicketConfiguration
        }
    }
}
