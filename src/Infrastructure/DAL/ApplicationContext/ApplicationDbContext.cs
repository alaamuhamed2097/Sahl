using Domains.Entities.Base;
using Domains.Entities.BrandManagement;
using Domains.Entities.BuyBox;
using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Attribute;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Catalog.Unit;
using Domains.Entities.Content;
using Domains.Entities.CouponCode;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Support;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Fulfillment;
using Domains.Entities.Inventory;
using Domains.Entities.Location;
using Domains.Entities.Loyalty;
using Domains.Entities.Merchandising;
using Domains.Entities.Notification;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Page;
using Domains.Entities.Pricing;
using Domains.Entities.SellerRequest;
using Domains.Entities.SellerTier;
using Domains.Entities.Setting;
using Domains.Entities.Shipping;
using Domains.Entities.VideoProvider;
using Domains.Entities.Visibility;
using Domains.Entities.Wallet;
using Domains.Entities.Warehouse;
using Domains.Identity;
using Domains.Views.Category;
using Domains.Views.Item;
using Domains.Views.Unit;
using Domains.Views.UserNotification;
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
        public DbSet<TbItemCombination> TbItemCombinations { get; set; }
        public DbSet<TbCombinationAttribute> TbCombinationAttributes { get; set; }
        public DbSet<TbCombinationAttributesValue> TbCombinationAttributesValues { get; set; }

        // Notification Management
        public DbSet<TbNotification> TbNotifications { get; set; }
        public DbSet<TbUserNotification> TbUserNotifications { get; set; }
        public DbSet<TbNotificationChannel> TbNotificationChannels { get; set; }
        public DbSet<TbNotifications> Notifications { get; set; }
        public DbSet<TbNotificationPreferences> NotificationPreferences { get; set; }

        // Page Management
        public DbSet<TbPage> TbPages { get; set; }

        // Promo Code Management
        public DbSet<TbCouponCode> TbCouponCodes { get; set; }

        // Settings
        public DbSet<TbSetting> TbSettings { get; set; }

        // Shipping Management
        public DbSet<TbShippingCompany> TbShippingCompanies { get; set; }

        // Unit Management
        public DbSet<TbUnit> TbUnits { get; set; }
        public DbSet<TbUnitConversion> TbUnitConversions { get; set; }

        // Location Management
        public DbSet<TbCountry> TbCountries { get; set; }
        public DbSet<TbState> TbStates { get; set; }
        public DbSet<TbCity> TbCities { get; set; }

        // Vendor
        public DbSet<TbVendor> TbVendors { get; set; }

        // Customer
        public DbSet<TbCustomer> TbCustomers { get; set; }

        // Video Provider Management
        public DbSet<TbVideoProvider> TbVideoProviders { get; set; }

        // Support Management
        public DbSet<TbDispute> TbDisputes { get; set; }
        public DbSet<TbDisputeMessage> TbDisputeMessages { get; set; }
        public DbSet<TbSupportTicket> TbSupportTickets { get; set; }
        public DbSet<TbSupportTicketMessage> TbSupportTicketMessages { get; set; }

        // Review Management
        public DbSet<TbProductReview> TbProductReviews { get; set; }
        public DbSet<TbSalesReview> TbSalesReviews { get; set; }
        public DbSet<TbDeliveryReview> TbDeliveryReviews { get; set; }
        public DbSet<TbReviewVote> TbReviewVotes { get; set; }

        // Warehouse Management
        public DbSet<TbWarehouse> TbWarehouses { get; set; }

        // Inventory Management
        public DbSet<TbMoitem> TbMoitems { get; set; }
        public DbSet<TbMortem> TbMortems { get; set; }
        public DbSet<TbMovitemsdetail> TbMovitemsdetails { get; set; }

        // Content Management
        public DbSet<TbContentArea> TbContentAreas { get; set; }
        public DbSet<TbMediaContent> TbMediaContents { get; set; }

        // Loyalty System
        public DbSet<TbLoyaltyTier> TbLoyaltyTiers { get; set; }
        public DbSet<TbCustomerLoyalty> TbCustomerLoyalties { get; set; }
        public DbSet<TbLoyaltyPointsTransaction> TbLoyaltyPointsTransactions { get; set; }

        // Wallet System
        public DbSet<TbCustomerWallet> TbCustomerWallets { get; set; }
        public DbSet<TbVendorWallet> TbVendorWallets { get; set; }
        public DbSet<TbWalletTransaction> TbWalletTransactions { get; set; }
        public DbSet<TbPlatformTreasury> TbPlatformTreasuries { get; set; }

        // Buy Box System
        public DbSet<TbBuyBoxCalculation> TbBuyBoxCalculations { get; set; }
        public DbSet<TbBuyBoxHistory> TbBuyBoxHistories { get; set; }
        public DbSet<TbSellerPerformanceMetrics> TbSellerPerformanceMetrics { get; set; }

        // Seller Requests
        public DbSet<TbSellerRequest> TbSellerRequests { get; set; }
        public DbSet<TbRequestComment> TbRequestComments { get; set; }
        public DbSet<TbRequestDocument> TbRequestDocuments { get; set; }

        // Campaign & Flash Sales
        public DbSet<TbCampaign> TbCampaigns { get; set; }
        public DbSet<TbCampaignProduct> TbCampaignProducts { get; set; }
        public DbSet<TbCampaignVendor> TbCampaignVendors { get; set; }
        public DbSet<TbFlashSale> TbFlashSales { get; set; }
        public DbSet<TbFlashSaleProduct> TbFlashSaleProducts { get; set; }

        // Fulfillment System (FBS/FBM)
        public DbSet<TbFulfillmentMethod> TbFulfillmentMethods { get; set; }
        public DbSet<TbFBMInventory> TbFBMInventories { get; set; }
        public DbSet<TbFulfillmentFee> TbFulfillmentFees { get; set; }
        public DbSet<TbFBMShipment> TbFBMShipments { get; set; }

        // Advanced Pricing
        public DbSet<TbQuantityPricing> TbQuantityPricings { get; set; }
        public DbSet<TbCustomerSegmentPricing> TbCustomerSegmentPricings { get; set; }
        public DbSet<TbPriceHistory> TbPriceHistories { get; set; }

        // Homepage Merchandising
        public DbSet<TbHomepageBlock> TbHomepageBlocks { get; set; }
        public DbSet<TbBlockProduct> TbBlockProducts { get; set; }

        // Seller Tiers
        public DbSet<TbSellerTier> TbSellerTiers { get; set; }
        public DbSet<TbSellerTierBenefit> TbSellerTierBenefits { get; set; }
        public DbSet<TbVendorTierHistory> TbVendorTierHistories { get; set; }

        // Product Visibility Rules
        public DbSet<TbProductVisibilityRule> TbProductVisibilityRules { get; set; }
        public DbSet<TbSuppressionReason> TbSuppressionReasons { get; set; }
        public DbSet<TbVisibilityLog> TbVisibilityLogs { get; set; }

        // Brand Management Advanced
        public DbSet<TbBrandRegistrationRequest> TbBrandRegistrationRequests { get; set; }
        public DbSet<TbBrandDocument> TbBrandDocuments { get; set; }
        public DbSet<TbAuthorizedDistributor> TbAuthorizedDistributors { get; set; }

        // Offer Management
        public DbSet<Domains.Entities.Offer.TbOffer> TbOffers { get; set; }
        public DbSet<Domains.Entities.Offer.TbOfferCombinationPricing> TbOfferCombinationPricings { get; set; }
        public DbSet<Domains.Entities.Offer.TbOfferCondition> TbOfferConditions { get; set; }
        public DbSet<TbWarranty> TbWarranties { get; set; }
        public DbSet<TbUserOfferRating> TbUserOfferRatings { get; set; }

        // Order Management
        public DbSet<Domains.Entities.Order.TbOrder> TbOrders { get; set; }
        public DbSet<Domains.Entities.Order.TbOrderDetail> TbOrderDetails { get; set; }
        public DbSet<Domains.Entities.Order.TbRefundRequest> TbRefundRequests { get; set; }
        public DbSet<Domains.Entities.Shipping.TbShippingDetail> TbShippingDetails { get; set; }

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
            try
            {
                base.OnModelCreating(modelBuilder);

                // ✅ ADD THIS: Disable cascade delete globally to prevent cascade path issues
                DisableCascadeDeleteGlobally(modelBuilder);

                // Check if we're in design-time (EF Core Tools)
                var isDesignTime = IsDesignTimeEnvironment();

                if (isDesignTime)
                {
                    ConfigureForDesignTime(modelBuilder);
                    return;
                }

                ConfigureForRuntime(modelBuilder);
            }
            catch (Exception ex)
            {
                LogModelCreatingError(ex, modelBuilder);
                throw;
            }
        }

        /// <summary>
        /// Disables cascade delete globally for all relationships to prevent SQL Server cascade path errors
        /// </summary>
        private static void DisableCascadeDeleteGlobally(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            Console.WriteLine("[OnModelCreating] Disabled cascade delete globally for all relationships.");
        }

        /// <summary>
        /// Configures the model for design-time operations (migrations, scaffolding)
        /// </summary>
        private void ConfigureForDesignTime(ModelBuilder modelBuilder)
        {
            Console.WriteLine("[OnModelCreating] Design-time detected: applying minimal configuration.");

            // Apply only base entity configurations for design time
            ConfigureBaseEntities(modelBuilder);

            // Configure views as keyless for design time
            ConfigureViews(modelBuilder);
        }

        /// <summary>
        /// Configures the model for runtime operations
        /// </summary>
        private void ConfigureForRuntime(ModelBuilder modelBuilder)
        {
            Console.WriteLine("[OnModelCreating] Runtime detected: applying full configuration.");

            // Apply base entity configurations
            ConfigureBaseEntities(modelBuilder);

            // Apply all entity configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure views
            ConfigureViews(modelBuilder);
        }

        /// <summary>
        /// Configures common settings for all entities inheriting from BaseEntity
        /// </summary>
        private void ConfigureBaseEntities(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType != null && typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var entity = modelBuilder.Entity(entityType.ClrType);

                    // Configure ID with NEWID() default
                    entity.Property(nameof(BaseEntity.Id))
                          .HasDefaultValueSql("NEWID()")
                          .ValueGeneratedOnAdd();

                    // Configure CreatedDateUtc with GETUTCDATE() default
                    entity.Property(nameof(BaseEntity.CreatedDateUtc))
                          .HasDefaultValueSql("GETUTCDATE()")
                          .HasColumnType("datetime2(2)");

                    // Configure UpdatedDateUtc as nullable
                    entity.Property(nameof(BaseEntity.UpdatedDateUtc))
                          .HasColumnType("datetime2(2)")
                          .IsRequired(false);

                    // Configure CurrentState with default value
                    entity.Property(nameof(BaseEntity.CurrentState))
                          .HasDefaultValue(1);

                    // Add index for CurrentState for better query performance
                    entity.HasIndex(nameof(BaseEntity.CurrentState))
                          .IsUnique(false);
                }
            }
        }

        /// <summary>
        /// Configures database views as keyless entities
        /// </summary>
        private void ConfigureViews(ModelBuilder modelBuilder)
        {
            // Category Views
            modelBuilder.Entity<VwAttributeWithOptions>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwAttributeWithOptions");
            });

            modelBuilder.Entity<VwCategoryItems>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwCategoryItems");
            });

            modelBuilder.Entity<VwCategoryWithAttributes>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwCategoryWithAttributes");
            });

            // Item Views
            modelBuilder.Entity<VwItem>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwItems");
            });

            // Unit Views
            modelBuilder.Entity<VwUnitWithConversionsUnits>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwUnitWithConversionsUnits");
            });

            // User Notification Views
            modelBuilder.Entity<VwUserNotification>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwUserNotifications");
            });
        }

        /// <summary>
        /// Determines if the current environment is design-time (EF Core Tools)
        /// </summary>
        private static bool IsDesignTimeEnvironment()
        {
            return string.Equals(Environment.GetEnvironmentVariable("EF_DESIGN_TIME"), "true", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "false", StringComparison.OrdinalIgnoreCase) &&
                   AppDomain.CurrentDomain.FriendlyName.Contains("ef.dll");
        }

        /// <summary>
        /// Logs detailed error information when model creation fails
        /// </summary>
        private static void LogModelCreatingError(Exception ex, ModelBuilder modelBuilder)
        {
            Console.Error.WriteLine("[OnModelCreating] Exception while building model: " + ex.Message);
            Console.Error.WriteLine(ex.ToString());

            try
            {
                // Log discovered entity types for debugging
                var entityTypes = modelBuilder?.Model?.GetEntityTypes()?
                    .Select(t => t.ClrType?.FullName ?? "<no-clr>")
                    .OrderBy(name => name)
                    .ToArray();

                if (entityTypes != null && entityTypes.Any())
                {
                    Console.Error.WriteLine("[OnModelCreating] Discovered entity CLR types:");
                    foreach (var typeName in entityTypes.Take(25))
                    {
                        Console.Error.WriteLine(" - " + typeName);
                    }

                    if (entityTypes.Length > 25)
                    {
                        Console.Error.WriteLine($" - ... and {entityTypes.Length - 25} more");
                    }
                }
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine($"[OnModelCreating] Error logging entity types: {logEx.Message}");
            }
        }

        /// <summary>
        /// Saves changes with automatic UTC timestamp handling
        /// </summary>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves changes asynchronously with automatic UTC timestamp handling
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Automatically sets CreatedDateUtc and UpdatedDateUtc for BaseEntity objects
        /// </summary>
        private void UpdateTimestamps()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDateUtc = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDateUtc = utcNow;
                }
            }
        }
    }
}