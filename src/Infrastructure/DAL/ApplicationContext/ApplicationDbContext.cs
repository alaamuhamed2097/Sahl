using Domains.Entities;
using Domains.Entities.Base;
using Domains.Entities.Notification;
using Domains.Entities.Page;
using Domains.Entities.PromoCode;
using Domains.Entities.Setting;
using Domains.Entities.Testimonial;
using Domains.Identity;
using Domins.Entities.Brand;
using Domins.Entities.Category;
using Domins.Entities.Currency;
using Domins.Entities.Item;
using Domins.Entities.Location;
using Domins.Entities.Unit;
using Domins.Entities.VideoProvider;
using Domins.Views;
using Domins.Views.Category;
using Domins.Views.Item;
using Domins.Views.Unit;
using Domins.Views.UserNotification;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        #region DbSets - Tables

        // Attribute & Category Management
        public DbSet<TbAttribute> TbAttributes { get; set; }
        public DbSet<TbAttributeOption> TbAttributeOptions { get; set; }
        public DbSet<TbCategory> TbCategories { get; set; }
        public DbSet<TbCategoryAttribute> TbCategoryAttributes { get; set; }

        // Brand Management
        public DbSet<TbBrand> TbBrands { get; set; }

        // Currency Management
        public DbSet<TbCurrency> TbCurrencies { get; set; }

        // Item Management
        public DbSet<TbItem> TbItems { get; set; }
        public DbSet<TbItemAttribute> TbItemAttribute { get; set; }
        public DbSet<TbItemAttributeCombinationPricing> TbItemAttributeCombinationPricings { get; set; }
        public DbSet<TbItemImage> TbItemImages { get; set; }

        // Notification Management
        public DbSet<TbNotification> TbNotifications { get; set; }
        public DbSet<TbUserNotification> TbUserNotifications { get; set; }

        // Page Management
        public DbSet<TbPage> TbPages { get; set; }

        // Promo Code Management
        public DbSet<TbPromoCode> TbPromoCodes { get; set; }

        // Settings
        public DbSet<TbSetting> TbSettings { get; set; }

        // Shipping Management
        public DbSet<TbShippingCompany> TbShippingCompanies { get; set; }

        // Testimonial Management
        public DbSet<TbTestimonial> TbTestimonials { get; set; }

        // Unit Management
        public DbSet<TbUnit> TbUnits { get; set; }
        public DbSet<TbUnitConversion> TbUnitConversions { get; set; }

        // Location Management
        public DbSet<TbCountry> TbCountries { get; set; }
        public DbSet<TbState> TbStates { get; set; }
        public DbSet<TbCity> TbCities { get; set; }

        // Video Provider Management
        public DbSet<TbVideoProvider> TbVideoProviders { get; set; }

        #endregion

        #region DbSets - Views

        // Category Views
        public DbSet<VwAttributeWithOptions> VwAttributeWithOptions { get; set; }
        public DbSet<VwCategoryItems> VwCategoryItems { get; set; }
        public DbSet<VwCategoryWithAttributes> VwCategoryWithAttributes { get; set; }

        // Item Views
        public DbSet<VwItem> VwItems { get; set; }

        // Unit Views
        public DbSet<VwUnitWithConversionsUnits> VwUnitWithConversionsUnits { get; set; }

        // User Notification Views
        public DbSet<VwUserNotification> VwUserNotifications { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Global Configuration - Base Entity

            // Apply default NEWID() for all entities inheriting from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Auto-generate NEWID() in SQL Server
                    modelBuilder.Entity(entityType.ClrType)
           .Property(nameof(BaseEntity.Id))
             .HasDefaultValueSql("NEWID()")
               .ValueGeneratedOnAdd();

                    // Configure CreatedDateUtc
                    modelBuilder.Entity(entityType.ClrType)
                  .Property(nameof(BaseEntity.CreatedDateUtc))
                  .HasDefaultValueSql("GETUTCDATE()")
                 .HasColumnType("datetime2(2)");

                    // Configure UpdatedDateUtc
                    modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseEntity.UpdatedDateUtc))
                   .HasColumnType("datetime2(2)")
                         .IsRequired(false);

                    // Configure CurrentState
                    modelBuilder.Entity(entityType.ClrType)
                         .Property(nameof(BaseEntity.CurrentState))
                .HasDefaultValue(1);

                    // Index on CurrentState
                    modelBuilder.Entity(entityType.ClrType)
                      .HasIndex(nameof(BaseEntity.CurrentState))
                    .IsUnique(false);
                }
            }

            #endregion

            #region Apply Configurations from Assembly

            // This will automatically apply all IEntityTypeConfiguration<T> classes from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            #endregion

            #region View Configurations

            // Category Views
            modelBuilder.Entity<VwAttributeWithOptions>()
               .HasNoKey()
           .ToView("VwAttributeWithOptions");

            modelBuilder.Entity<VwCategoryItems>()
                 .HasNoKey()
         .ToView("VwCategoryItems");

            modelBuilder.Entity<VwCategoryWithAttributes>()
                     .HasNoKey()
                 .ToView("VwCategoryWithAttributes");

            // Item Views
            modelBuilder.Entity<VwItem>()
       .HasNoKey()
         .ToView("VwItems");

            // Unit Views
            modelBuilder.Entity<VwUnitWithConversionsUnits>()
   .HasNoKey()
     .ToView("VwUnitWithConversionsUnits");

            // User Notification Views
            modelBuilder.Entity<VwUserNotification>()
              .HasNoKey()
                   .ToView("VwUserNotifications");

            #endregion
        }
    }
}
