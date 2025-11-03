using Domains.Entities.Base;
using Domains.Identity;
using Domins.Entities.Brand;
using Domins.Entities.Category;
using Domins.Entities.Item;
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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region Tables

        #region Category

        // Category Related
        public DbSet<TbCategory> TbCategories { get; set; }
        public DbSet<TbCategoryAttribute> TbCategoryAttributes { get; set; }
        public DbSet<TbAttribute> TbAttributes { get; set; }
        public DbSet<TbAttributeOption> TbAttributeOptions { get; set; }

        #endregion

        #region Item

        // Item Related
        public DbSet<TbItem> TbItems { get; set; }
        public DbSet<TbItemAttribute> TbItemAttribute { get; set; }
        public DbSet<TbItemImage> TbItemImages { get; set; }
        public DbSet<TbItemAttributeCombinationPricing> TbItemAttributeCombinationPricings { get; set; }

        #endregion

        #region Unit

        // Unit Related
        public DbSet<TbUnit> TbUnits { get; set; }
        public DbSet<TbUnitConversion> TbUnitConversions { get; set; }

        #endregion

        #region Brand

        // Brand Related
        public DbSet<TbBrand> TbBrands { get; set; }

        #endregion

        #region Video provider

        // Video Provider Related
        public DbSet<TbVideoProvider> TbVideoProviders { get; set; }

        #endregion

        #endregion

        #region Views

        // Category Related Views
        public DbSet<VwCategoryWithAttributes> VwCategoryWithAttributes { get; set; }
        public DbSet<VwAttributeWithOptions> VwAttributeWithOptions { get; set; }
        public DbSet<VwCategoryItems> VwCategoryItems { get; set; }

        // Item Related Views
        public DbSet<VwItem> VwItems { get; set; }
        public DbSet<VwUnitWithConversionsUnits> VwUnitWithConversionsUnits { get; set; }
        public DbSet<VwUserNotification> VwUserNotifications { get; set; }

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

            #region Item

            modelBuilder.Entity<TbItem>(entity =>
            {
                entity.Property(e => e.TitleAr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShortDescriptionAr)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ShortDescriptionEn)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.VideoLink)
                    .HasMaxLength(200);

                entity.Property(e => e.ThumbnailImage)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SEOTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SEOMetaTags)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.IsNewArrival)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsBestSeller)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsRecommended)
                    .HasDefaultValue(false);

                // Add indexes for better performance on flag-based queries
                entity.HasIndex(e => e.IsNewArrival).IsUnique(false);
                entity.HasIndex(e => e.IsBestSeller).IsUnique(false);
                entity.HasIndex(e => e.IsRecommended).IsUnique(false);

                // Relationships
                entity.HasOne(i => i.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(i => i.CategoryId)
                    .HasConstraintName("FK_TbItems_TbCategories_CategoryId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Unit)
                    .WithMany()
                    .HasForeignKey(i => i.UnitId)
                    .HasConstraintName("FK_TbItems_TbUnits_UnitId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Brand)
                    .WithMany(b => b.Items)
                    .HasForeignKey(i => i.BrandId)
                    .HasConstraintName("FK_TbItems_TbBrands_BrandId")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.VideoProvider)
                    .WithMany()
                    .HasForeignKey(i => i.VideoProviderId)
                    .HasConstraintName("FK_TbItems_TbVideoProviders_VideoProviderId")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.Price).IsUnique(false);
                entity.HasIndex(e => e.CategoryId).IsUnique(false);
                entity.HasIndex(e => e.UnitId).IsUnique(false);
                entity.HasIndex(e => e.BrandId).IsUnique(false);
                entity.HasIndex(e => e.CurrentState).IsUnique(false);
            });

            modelBuilder.Entity<TbItemAttribute>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(200); // Limited from max to 200 chars

                // Relationships
                entity.HasOne(ia => ia.Item)
                    .WithMany(i => i.ItemAttributes)
                    .HasForeignKey(ia => ia.ItemId)
                    .HasConstraintName("FK_TbItemAttributes_TbItems_ItemId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ia => ia.Attribute)
                    .WithMany(a => a.ItemAttributes)
                    .HasForeignKey(ia => ia.AttributeId)
                    .HasConstraintName("FK_TbItemAttributes_TbAttributes_AttributeId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TbItemAttributeCombinationPricing>(entity =>
            {
                entity.Property(e => e.AttributeIds)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FinalPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Image)
                    .HasMaxLength(200);

                // Relationships
                entity.HasOne(icp => icp.Item)
                    .WithMany(i => i.ItemAttributeCombinationPricings)
                    .HasForeignKey(icp => icp.ItemId)
                    .HasConstraintName("FK_TbItemAttributeCombinationPricings_TbItems_ItemId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.AttributeIds).IsUnique(false);
                entity.HasIndex(e => e.FinalPrice).IsUnique(false);
            });

            modelBuilder.Entity<TbItemImage>(entity =>
            {
                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

                // Relationships
                entity.HasOne(ii => ii.Item)
                    .WithMany(i => i.ItemImages)
                    .HasForeignKey(ii => ii.ItemId)
                    .HasConstraintName("FK_TbItemImages_TbItems_ItemId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.Order).IsUnique(false);
            });

            #endregion

            #region Unit

            modelBuilder.Entity<TbUnit>(entity =>
            {
                entity.Property(e => e.TitleAr)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.TitleEn)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<TbUnitConversion>(entity =>
            {
                entity.Property(e => e.ConversionFactor)
                      .HasColumnType("decimal(18,6)")
                      .IsRequired();

                entity.HasOne(uc => uc.FromUnit)
                      .WithMany()
                      .HasForeignKey(uc => uc.FromUnitId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(uc => uc.ToUnit)
                      .WithMany()
                      .HasForeignKey(uc => uc.ToUnitId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            #endregion

            #region Brand

            modelBuilder.Entity<TbBrand>(entity =>
            {
                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameAr)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LogoPath)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(200);

                entity.Property(e => e.IsFavorite)
                    .HasDefaultValue(false);

                entity.Property(e => e.DisplayOrder)
                    .HasDefaultValue(0);

                // Indexes
                entity.HasIndex(e => e.IsFavorite).IsUnique(false);
                entity.HasIndex(e => e.DisplayOrder).IsUnique(false);
            });

            #endregion

            #region Video provider

            modelBuilder.Entity<TbVideoProvider>(entity =>
            {
                entity.Property(e => e.TitleAr)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.TitleEn)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            #endregion

            #endregion

            #region Views

            // category related views
            modelBuilder.Entity<VwCategoryWithAttributes>().HasNoKey().ToView("VwCategoryWithAttributes");
            modelBuilder.Entity<VwAttributeWithOptions>().HasNoKey().ToView("VwAttributeWithOptions");
            modelBuilder.Entity<VwCategoryItems>().HasNoKey().ToView("VwCategoryItems");

            // Item related views
            modelBuilder.Entity<VwItem>().HasNoKey().ToView("VwItems");
            modelBuilder.Entity<VwUnitWithConversionsUnits>().HasNoKey().ToView("VwUnitWithConversionsUnits");

            modelBuilder.Entity<VwUserNotification>().HasNoKey().ToView("VwUserNotifications");
            #endregion

            #region Stored Procedures

            #endregion
        }
    }
}
