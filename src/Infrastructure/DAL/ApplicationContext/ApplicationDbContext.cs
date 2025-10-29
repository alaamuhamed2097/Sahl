using Domains.Entities.Base;
using Domains.Identity;
using Domins.Entities.Category;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region Tables

        public DbSet<TbCategory> TbCategories { get; set; }
        public DbSet<TbCategoryAttribute> TbCategoryAttributes { get; set; }
        public DbSet<TbAttribute> TbAttributes { get; set; }
        public DbSet<TbAttributeOption> TbAttributeOptions { get; set; }

        #endregion

        #region Views

        #endregion

        #region Stored Procedures

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region General
            // Apply default NEWID() for all entities inheriting from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Auto-generate NEWID() in SQL Server
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.Id))
                        .HasDefaultValueSql("NEWID()") // SQL Server default
                        .ValueGeneratedOnAdd();

                    // Optional: Configure other common BaseEntity properties
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.CreatedDateUtc))
                        .HasDefaultValueSql("GETUTCDATE()")
                        .HasColumnType("datetime2(2)"); // Auto-set creation time in DB

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.UpdatedDateUtc))
                        .HasColumnType("datetime2(2)")
                        .IsRequired(false); // Auto-set creation time in DB

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.CurrentState))
                        .HasDefaultValue(1);

                    modelBuilder.Entity(entityType.ClrType).HasIndex(nameof(BaseEntity.CurrentState)).IsUnique(false);
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            #endregion

            #region Tables


            #endregion

            #region Views

            #endregion

            #region Stored Procedures

            #endregion
        }
    }
}
