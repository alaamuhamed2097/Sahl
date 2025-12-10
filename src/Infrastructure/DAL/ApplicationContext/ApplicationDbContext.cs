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
using Domains.Entities.ECommerceSystem;
using Domains.Entities.ECommerceSystem.Cart;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Support;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Location;
using Domains.Entities.Loyalty;
using Domains.Entities.Merchandising;
using Domains.Entities.Notification;
using Domains.Entities.Offer;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Order;
using Domains.Entities.Page;
using Domains.Entities.Payment;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// </summary>
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
        public DbSet<TbItemAttribute> TbItemAttributes { get; set; }
        public DbSet<TbAttributeValuePriceModifier> TbAttributeValuePriceModifiers { get; set; }
        public DbSet<TbItemImage> TbItemImages { get; set; }
        public DbSet<TbItemCombination> TbItemCombinations { get; set; }
        public DbSet<TbCombinationAttribute> TbCombinationAttributes { get; set; }
        public DbSet<TbCombinationAttributesValue> TbCombinationAttributesValues { get; set; }

        // Notification Management
        public DbSet<TbNotification> TbNotifications { get; set; }
        public DbSet<TbUserNotification> TbUserNotifications { get; set; }
        public DbSet<TbNotificationChannel> TbNotificationChannels { get; set; }
        public DbSet<TbNotificationPreferences> TbNotificationPreferences { get; set; }

        // Page Management
        public DbSet<TbPage> TbPages { get; set; }

        // Coupon Code Management
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
        public DbSet<TbReviewReport> TbReviewReports { get; set; }

        // Warehouse Management
        public DbSet<TbWarehouse> TbWarehouses { get; set; }


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
        public DbSet<TbOffer> TbOffers { get; set; }
        public DbSet<TbOfferCombinationPricing> TbOfferCombinationPricings { get; set; }
        public DbSet<TbOfferCondition> TbOfferConditions { get; set; }
        public DbSet<TbWarranty> TbWarranties { get; set; }
        public DbSet<TbUserOfferRating> TbUserOfferRatings { get; set; }
        public DbSet<TbOfferStatusHistory> TbOfferStatusHistories { get; set; }
        public DbSet<TbOfferPriceHistory> TbOfferPriceHistories { get; set; }

        // Order Management
        public DbSet<TbOrder> TbOrders { get; set; }
        public DbSet<TbOrderDetail> TbOrderDetails { get; set; }
        public DbSet<TbRefundRequest> TbRefundRequests { get; set; }
        public DbSet<TbShippingDetail> TbShippingDetails { get; set; }

        // New E-commerce tables: Shopping Cart, Shipments, Payments, Customer Addresses
        public DbSet<TbShoppingCart> TbShoppingCarts { get; set; }
        public DbSet<TbShoppingCartItem> TbShoppingCartItems { get; set; }
        public DbSet<TbOrderShipment> TbOrderShipments { get; set; }
        public DbSet<TbOrderShipmentItem> TbOrderShipmentItems { get; set; }
        public DbSet<TbCustomerAddress> TbCustomerAddresses { get; set; }
        public DbSet<TbPaymentMethod> TbPaymentMethods { get; set; }
        public DbSet<TbOrderPayment> TbOrderPayments { get; set; }

        // Pricing System Settings
        public DbSet<TbPricingSystemSetting> TbPricingSystemSettings { get; set; }

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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            ConfigureBaseEntities(modelBuilder);
            ConfigureViews(modelBuilder);

            // Apply missing/explicit relationship configurations that might not be covered
            // by IEntityTypeConfiguration classes in the DAL assembly (or are defined elsewhere)
            ConfigureMissingRelations(modelBuilder);
        }

        /// <summary>
        /// Explicitly configures relations that may be missing or defined in other assemblies.
        /// This avoids relying solely on conventions when some configuration classes live
        /// outside the DAL assembly or are not yet implemented.
        /// </summary>
        private void ConfigureMissingRelations(ModelBuilder modelBuilder)
        {
            // Offer price history relations
            modelBuilder.Entity<TbOfferPriceHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.OfferCombinationPricing)
                      .WithMany()
                      .HasForeignKey(e => e.OfferCombinationPricingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Offer status history relations
            modelBuilder.Entity<TbOfferStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Offer)
                      .WithMany()
                      .HasForeignKey(e => e.OfferId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OfferId);
            });

            // Offer combination pricing relations (ensure FKs for item combination and offer)
            modelBuilder.Entity<TbOfferCombinationPricing>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ItemCombination)
                      .WithMany()
                      .HasForeignKey(e => e.ItemCombinationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Offer)
                      .WithMany(o => o.OfferCombinationPricings)
                      .HasForeignKey(e => e.OfferId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.ItemCombinationId);
                entity.HasIndex(e => e.OfferId);
            });

            // Attribute value price modifier relations
            modelBuilder.Entity<TbAttributeValuePriceModifier>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.CombinationAttributesValue)
                      .WithMany()
                      .HasForeignKey(e => e.CombinationAttributeValueId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Attribute)
                      .WithMany()
                      .HasForeignKey(e => e.AttributeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.CombinationAttributeValueId);
                entity.HasIndex(e => e.AttributeId);
            });
        }

        /// <summary>
        /// Configures common settings for all entities inheriting from BaseEntity
        /// </summary>
        private void ConfigureBaseEntities(ModelBuilder modelBuilder)
        {
            // Take a snapshot of entity types to avoid mutating the model while enumerating
            var entityTypes = modelBuilder.Model
                .GetEntityTypes()
                .Where(t => t.ClrType != null && typeof(BaseEntity).IsAssignableFrom(t.ClrType))
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var clrType = entityType.ClrType!;
                var entity = modelBuilder.Entity(clrType);

                // Configure ID with NEWID() default if property exists
                if (entityType.FindProperty(nameof(BaseEntity.Id)) != null)
                {
                    entity.Property(nameof(BaseEntity.Id))
                          .HasDefaultValueSql("NEWID()")
                          .ValueGeneratedOnAdd();
                }

                // Configure CreatedDateUtc with GETUTCDATE() default if property exists
                if (entityType.FindProperty(nameof(BaseEntity.CreatedDateUtc)) != null)
                {
                    entity.Property(nameof(BaseEntity.CreatedDateUtc))
                          .HasDefaultValueSql("GETUTCDATE()")
                          .HasColumnType("datetime2(2)");
                }

                // Configure UpdatedDateUtc as nullable if property exists
                if (entityType.FindProperty(nameof(BaseEntity.UpdatedDateUtc)) != null)
                {
                    entity.Property(nameof(BaseEntity.UpdatedDateUtc))
                          .HasColumnType("datetime2(2)")
                          .IsRequired(false);
                }

                // Configure CurrentState with default value and index if property exists
                if (entityType.FindProperty(nameof(BaseEntity.IsDeleted)) != null)
                {
                    entity.Property(nameof(BaseEntity.IsDeleted))
                          .HasDefaultValue(0);

                    // Add index for CurrentState for better query performance
                    entity.HasIndex(nameof(BaseEntity.IsDeleted))
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
            modelBuilder.Entity<VwItem>().HasNoKey().ToView("VwItems");

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