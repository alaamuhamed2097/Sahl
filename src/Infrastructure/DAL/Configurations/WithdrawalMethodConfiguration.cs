using Domains.Entities.WithdrawalMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class WithdrawalMethodConfiguration :
        IEntityTypeConfiguration<TbWithdrawalMethod>,
        IEntityTypeConfiguration<TbUserWithdrawalMethod>,
        IEntityTypeConfiguration<TbWithdrawalMethodField>,
        IEntityTypeConfiguration<TbField>
    {
        public void Configure(EntityTypeBuilder<TbWithdrawalMethod> entity)
        {
            entity.Property(e => e.TitleAr)
                    .IsRequired()
                    .HasMaxLength(100);

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ImagePath)
                .IsRequired()
                .HasMaxLength(200);
        }

        public void Configure(EntityTypeBuilder<TbUserWithdrawalMethod> entity)
        {
            entity.Property(e => e.UserId)
                      .IsRequired()
                      .HasMaxLength(450);

            entity.HasOne(e => e.User)  // Assuming navigation property not declared
                  .WithMany(u => u.UserWithdrawalMethods)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.WithdrawalMethod) // Assuming navigation property not declared
                  .WithMany(m => m.UserWithdrawalMethods)
                  .HasForeignKey(e => e.WithdrawalMethodId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.UserId).IsUnique(false);
            entity.HasIndex(e => e.WithdrawalMethodId).IsUnique(false);
        }

        public void Configure(EntityTypeBuilder<TbWithdrawalMethodField> entity)
        {
            entity.Property(e => e.Value)
                    .HasMaxLength(100);

            entity.HasOne(p => p.Field)
                .WithMany(f => f.WithdrawalMethodField)
                .HasForeignKey(p => p.FieldId)
                .HasConstraintName("FK_TbWithdrawalMethodFields_TbFields_FieldId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.UserWithdrawalMethod)
                .WithMany(upm => upm.WithdrawalMethodFields)
                .HasForeignKey(p => p.UserWithdrawalMethodId)
                .HasConstraintName("FK_TbWithdrawalMethodFields_TbUserWithdrawalMethods_UserWithdrawalMethodId")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.FieldId).IsUnique(false);
            entity.HasIndex(e => e.UserWithdrawalMethodId).IsUnique(false);
        }

        public void Configure(EntityTypeBuilder<TbField> entity)
        {
            entity.Property(e => e.TitleAr)
                    .IsRequired()
                    .HasMaxLength(100);

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.WithdrawalMethod)
                .WithMany(p => p.Fields)
                .HasForeignKey(d => d.WithdrawalMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbFields_TbWithdrawalMethods_WithdrawalMethodId");

            entity.HasIndex(e => e.FieldType).IsUnique(false);
            entity.HasIndex(e => e.WithdrawalMethodId).IsUnique(false);
        }
    }
}
