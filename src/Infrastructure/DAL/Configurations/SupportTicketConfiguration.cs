using Domains.Entities.ECommerceSystem.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<TbSupportTicket>
    {
        public void Configure(EntityTypeBuilder<TbSupportTicket> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.TicketNumber)
                .IsRequired();

            entity.Property(e => e.UserID)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.UserType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Subject)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Priority)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.AssignedTo)
                .HasMaxLength(450);

            entity.Property(e => e.AssignedTeam)
                .HasMaxLength(100);

            entity.Property(e => e.TicketCreatedDate)
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
            entity.HasIndex(e => e.UserID);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.AssignedTo);
            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.HasIndex(e => e.CreatedDateUtc);

            // Relationships
            entity.HasMany(e => e.SupportTicketMessages)
                .WithOne(m => m.SupportTicket)
                .HasForeignKey(m => m.TicketID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
