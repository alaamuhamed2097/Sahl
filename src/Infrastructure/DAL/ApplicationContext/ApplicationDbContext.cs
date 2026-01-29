using Domains.Entities.Base;
using Domains.Entities.BuyBox;
using Domains.Entities.Campaign;
using Domains.Entities.Catalog.Attribute;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Catalog.Pricing;
using Domains.Entities.Catalog.Unit;
using Domains.Entities.Currency;
using Domains.Entities.Customer;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Support;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Location;
using Domains.Entities.Loyalty;
using Domains.Entities.Merchandising;
using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.Merchandising.HomePage;
using Domains.Entities.Merchandising.HomePageBlocks;
using Domains.Entities.Notification;
using Domains.Entities.Offer;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Order;
using Domains.Entities.Order.Cart;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Domains.Entities.Order.Shipping;
using Domains.Entities.Page;
using Domains.Entities.SellerRequest;
using Domains.Entities.SellerTier;
using Domains.Entities.Setting;
using Domains.Entities.VideoProvider;
using Domains.Entities.Visibility;
using Domains.Entities.Wallet.Customer;
using Domains.Entities.Warehouse;
using Domains.Entities.WithdrawalMethods;
using Domains.Identity;
using Domains.Procedures;
using Domains.Views.Brand;
using Domains.Views.Category;
using Domains.Views.Item;
using Domains.Views.Offer;
using Domains.Views.Order.Refund;
using Domains.Views.Unit;
using Domains.Views.UserNotification;
using Domains.Views.Vendor;
using Domains.Views.WithdrawalMethods;
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
        public DbSet<TbItemImage> TbItemImages { get; set; }
        public DbSet<TbItemCombination> TbItemCombinations { get; set; }
        public DbSet<TbCombinationAttribute> TbCombinationAttributes { get; set; }
        public DbSet<TbItemCombinationImage> TbItemCombinationImages { get; set; }
        public DbSet<TbCombinationAttributesValue> TbCombinationAttributesValues { get; set; }

        // Wishlist Management
        public DbSet<TbWishlist> TbWishlists { get; set; }
        public DbSet<TbWishlistItem> TbWishlistItems { get; set; }

        // Notification Management
        public DbSet<TbNotification> TbNotifications { get; set; }
        public DbSet<TbUserNotification> TbUserNotifications { get; set; }
        public DbSet<TbNotificationChannel> TbNotificationChannels { get; set; }
        public DbSet<TbNotificationPreferences> TbNotificationPreferences { get; set; }

        // Page Management
        public DbSet<TbPage> TbPages { get; set; }

        // Coupon Code Management
        public DbSet<TbCouponCode> TbCouponCodes { get; set; }
        public DbSet<TbCouponCodeScope> CouponCodeScopes { get; set; }

        // Settings
        public DbSet<TbGeneralSettings> TbGeneralSettings { get; set; }
        public DbSet<TbSystemSettings> TbSystemSettings { get; set; }
        public DbSet<TbDevelopmentSettings> TbDevelopmentSettings { get; set; }

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
        public DbSet<TbItemReview> TbItemReviews { get; set; }
        public DbSet<TbSalesReview> TbSalesReviews { get; set; }
        public DbSet<TbReviewVote> TbReviewVotes { get; set; }
        public DbSet<TbReviewReport> TbReviewReports { get; set; }
        public DbSet<TbVendorReview> TbVendorReviews { get; set; }

        // Warehouse Management
        public DbSet<TbWarehouse> TbWarehouses { get; set; }

        // Loyalty System
        public DbSet<TbLoyaltyTier> TbLoyaltyTiers { get; set; }
        public DbSet<TbCustomerLoyalty> TbCustomerLoyalties { get; set; }
        public DbSet<TbLoyaltyPointsTransaction> TbLoyaltyPointsTransactions { get; set; }

        // Customer Wallet System
        public DbSet<TbCustomerWallet> TbCustomerWallets { get; set; }
        public DbSet<TbWalletChargingRequest> TbWalletChargingRequests { get; set; }
        public DbSet<TbCustomerWalletTransaction> TbWalletTransactions { get; set; }
        public DbSet<TbWalletSetting> TbWalletSettings { get; set; }

        // Buy Box System
        public DbSet<TbBuyBoxCalculation> TbBuyBoxCalculations { get; set; }
        public DbSet<TbBuyBoxHistory> TbBuyBoxHistories { get; set; }
        public DbSet<TbSellerPerformanceMetrics> TbSellerPerformanceMetrics { get; set; }

        // Seller Requests
        public DbSet<TbSellerRequest> TbSellerRequests { get; set; }
        public DbSet<TbRequestComment> TbRequestComments { get; set; }
        public DbSet<TbRequestDocument> TbRequestDocuments { get; set; }


        // Advanced Pricing
        public DbSet<TbQuantityTierPricing> TbQuantityTierPricings { get; set; }
        public DbSet<TbCustomerSegmentPricing> TbCustomerSegmentPricings { get; set; }
        public DbSet<TbPriceHistory> TbPriceHistories { get; set; }

        // Homepage Merchandising
        public DbSet<TbHomepageBlock> TbHomepageBlocks { get; set; }
        public DbSet<TbBlockItem> TbBlockItems { get; set; }
        public DbSet<TbBlockCategory> TbBlockCategories { get; set; }
        public DbSet<TbUserItemView> TbUserItemViews { get; set; }

        // Campaign & Flash Sales
        public DbSet<TbCampaign> TbCampaigns { get; set; }
        public DbSet<TbCampaignItem> TbCampaignItems { get; set; }
        public DbSet<TbCampaignVendor> TbCampaignVendors { get; set; }

        // Seller Tiers
        public DbSet<TbSellerTier> TbSellerTiers { get; set; }
        public DbSet<TbSellerTierBenefit> TbSellerTierBenefits { get; set; }
        public DbSet<TbVendorTierHistory> TbVendorTierHistories { get; set; }

        // Product Visibility Rules
        public DbSet<TbProductVisibilityRule> TbProductVisibilityRules { get; set; }
        public DbSet<TbSuppressionReason> TbSuppressionReasons { get; set; }
        public DbSet<TbVisibilityLog> TbVisibilityLogs { get; set; }

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
        public DbSet<TbRefund> TbRefunds { get; set; }
        public DbSet<TbRefundStatusHistory> TbRefundStatusHistories { get; set; }
        public DbSet<TbRefundItemVideo> TbRefundItemVideos { get; set; }
        public DbSet<TbShippingDetail> TbShippingDetails { get; set; }

        // New E-commerce tables: Shopping Cart, Shipments, Payments, Customer Addresses
        public DbSet<TbShoppingCart> TbShoppingCarts { get; set; }
        public DbSet<TbShoppingCartItem> TbShoppingCartItems { get; set; }
        public DbSet<TbOrderShipment> TbOrderShipments { get; set; }
        public DbSet<TbOrderShipmentItem> TbOrderShipmentItems { get; set; }
        public DbSet<TbPaymentMethod> TbPaymentMethods { get; set; }
        public DbSet<TbOrderPayment> TbOrderPayments { get; set; }
        public DbSet<TbShipmentPayment> TbShipmentPayments { get; set; }

        // Pricing System Settings
        public DbSet<TbPricingSystemSetting> TbPricingSystemSettings { get; set; }

        // E-Commerce System - Customers
        public DbSet<TbCustomer> TbCustomers { get; set; }

        // Customer Related
        public DbSet<TbCustomerAddress> TbCustomerAddresses { get; set; }
        public DbSet<TbCustomerItemView> TbCustomerItemViews { get; set; }

        // Withdrawal methods
        public DbSet<TbWithdrawalMethod> TbWithdrawalMethods { get; set; }
        public DbSet<TbUserWithdrawalMethod> TbUserWithdrawalMethods { get; set; }
        public DbSet<TbWithdrawalMethodField> TbWithdrawalMethodFields { get; set; }
        public DbSet<TbField> TbFields { get; set; }


        #endregion

        #region DbSets - Views

        // Category Views
        public DbSet<VwAttributeWithOptions> VwAttributeWithOptions { get; set; }
        public DbSet<VwCategoryItems> VwCategoryItems { get; set; }
        public DbSet<VwCategoryWithAttributes> VwCategoryWithAttributes { get; set; }

        // Item Views
        public DbSet<VwItem> VwItems { get; set; }

        //Vendor Items
        public DbSet<VwVendorItem> VwVendorItems { get; set; }

        // Offer Views
        public DbSet<VwRefundDetails> VwRefundDetails { get; set; }

        // Item Search Views (from stored procedure and denormalized view)
        public DbSet<VwItemBestPrice> VwItemBestPrices { get; set; }
        public DbSet<SpSearchItemsMultiVendor> SpSearchItemsMultiVendor { get; set; }
        public DbSet<SpGetAvailableSearchFilters> SpGetAvailableSearchFilters { get; set; }
        public DbSet<SpGetItemDetails> SpGetItemDetails { get; set; }
        public DbSet<SpGetAvailableOptionsForSelection> SpGetAvailableOptionsForSelection { get; set; }
        public DbSet<SpGetCustomerRecommendedItems> SpGetCustomerRecommendedItems { get; set; }

        // Unit Views
        public DbSet<VwUnitWithConversionsUnits> VwUnitWithConversionsUnits { get; set; }

        // User Notification Views
        public DbSet<VwUserNotification> VwUserNotifications { get; set; }
        // Vendor Details Views
        public DbSet<VwVendorPublicDetails> VwVendorPublicDetails { get; set; }
        public DbSet<VwVendorOwnerDetails> VwVendorOwnerDetails { get; set; }
        public DbSet<VwVendorAdminDetails> VwVendorAdminDetails { get; set; }

        // Brand Views
        public DbSet<VwBrandOverview> VwBrandOverview { get; set; }
        public DbSet<VwBrandProducts> VwBrandProducts { get; set; }
        public DbSet<VwBrandRegistrationRequests> VwBrandRegistrationRequests { get; set; }
        public DbSet<VwBrandAuthorizedDistributors> VwBrandAuthorizedDistributors { get; set; }
        public DbSet<VwBrandSalesAnalysis> VwBrandSalesAnalysis { get; set; }
        public DbSet<VwBrandCampaigns> VwBrandCampaigns { get; set; }

        // Withdrawal Methods Viewa
        public DbSet<VwWithdrawalMethodsWithFields> VwWithdrawalMethodsWithFields { get; set; }
        public DbSet<VwWithdrawalMethodsFieldsValues> VwWithdrawalMethodsFieldsValues { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //modelBuilder.ApplyConfiguration(new CustomerConfiguration());

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
                      .WithMany(c => c.OfferPriceHistories)
                      .HasForeignKey(e => e.OfferCombinationPricingId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Offer status history relations
            modelBuilder.Entity<TbOfferStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Offer)
                      .WithMany(o => o.OfferStatusHistories)
                      .HasForeignKey(e => e.OfferId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OfferId);
            });

            // Offer combination pricing relations (ensure FKs for item combination and offer)
            modelBuilder.Entity<TbOfferCombinationPricing>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ItemCombination)
                      .WithMany(i => i.OfferCombinationPricings)
                      .HasForeignKey(e => e.ItemCombinationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Offer)
                      .WithMany(o => o.OfferCombinationPricings)
                      .HasForeignKey(e => e.OfferId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.ItemCombinationId);
                entity.HasIndex(e => e.OfferId);
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
                          .HasDefaultValue(false);

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

            // Vendor Item Views
            modelBuilder.Entity<VwRefundDetails>().HasNoKey().ToView("VwRefundDetails");
            modelBuilder.Entity<VwVendorItem>().HasNoKey().ToView("VwVendorItems");

            // Item Search Result View (from stored procedure - no actual view, used for mapping results)
            modelBuilder.Entity<SpSearchItemsMultiVendor>().HasNoKey().ToView("SpSearchItemsMultiVendor");
            modelBuilder.Entity<SpGetAvailableSearchFilters>().HasNoKey().ToView("SpGetAvailableSearchFilters");
            modelBuilder.Entity<SpGetItemDetails>().HasNoKey().ToView("SpGetItemDetails");
            modelBuilder.Entity<SpGetAvailableOptionsForSelection>().HasNoKey().ToView("SpGetAvailableOptionsForSelection");
            modelBuilder.Entity<SpGetCustomerRecommendedItems>().HasNoKey().ToView("SpGetCustomerRecommendedItems");

            // Item Best Price View (from denormalized database view)
            modelBuilder.Entity<VwItemBestPrice>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwItemBestPrices");
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
            // Vendor Details Views

            modelBuilder.Entity<VwVendorPublicDetails>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwVendorPublicDetails");
            });

            modelBuilder.Entity<VwVendorOwnerDetails>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwVendorOwnerDetails");
            });

            modelBuilder.Entity<VwVendorAdminDetails>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwVendorAdminDetails");
            });

            // Brand Views

            //1.Brand Overview
            modelBuilder.Entity<VwBrandOverview>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandOverview");

                // Indexes for performance
                entity.HasIndex(e => e.BrandId);
                entity.HasIndex(e => e.IsPopular);
                entity.HasIndex(e => e.IsDeleted);

                // Decimal precision
                entity.Property(e => e.AverageRating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.TotalSales).HasColumnType("decimal(18,2)");

                // String lengths
                entity.Property(e => e.NameAr).HasMaxLength(50);
                entity.Property(e => e.NameEn).HasMaxLength(50);
                entity.Property(e => e.TitleAr).HasMaxLength(100);
                entity.Property(e => e.TitleEn).HasMaxLength(100);
                entity.Property(e => e.LogoPath).HasMaxLength(200);
                entity.Property(e => e.WebsiteUrl).HasMaxLength(200);
            });
            //2.Brand Products
            modelBuilder.Entity<VwBrandProducts>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandProducts");

                // Indexes
                entity.HasIndex(e => e.BrandId);
                entity.HasIndex(e => e.ItemId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => new { e.BrandId, e.IsActive });

                // Decimal precision
                entity.Property(e => e.BasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinimumPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaximumPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinOfferPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaxOfferPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ItemAverageRating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.TotalRevenue).HasColumnType("decimal(18,2)");

                // String lengths
                entity.Property(e => e.SKU).HasMaxLength(200);
                entity.Property(e => e.Barcode).HasMaxLength(200);
                entity.Property(e => e.ThumbnailImage).HasMaxLength(200);
            });
            //3. Brand Registration Requests
            modelBuilder.Entity<VwBrandRegistrationRequests>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandRegistrationRequests");

                // Indexes
                entity.HasIndex(e => e.RequestId);
                entity.HasIndex(e => e.VendorId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.SubmittedAt);

                // Enum conversions
                entity.Property(e => e.BrandType).HasConversion<int>();
                entity.Property(e => e.Status).HasConversion<int>();

                // String lengths
                entity.Property(e => e.BrandNameAr).HasMaxLength(200);
                entity.Property(e => e.BrandNameEn).HasMaxLength(200);
                entity.Property(e => e.TrademarkNumber).HasMaxLength(100);
                entity.Property(e => e.CommercialRegistrationNumber).HasMaxLength(100);
            });
            //4. Brand Authorized Distributors
            modelBuilder.Entity<VwBrandAuthorizedDistributors>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandAuthorizedDistributors");

                // Indexes
                entity.HasIndex(e => e.DistributorId);
                entity.HasIndex(e => e.BrandId);
                entity.HasIndex(e => e.VendorId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => new { e.BrandId, e.IsActive });

                // Decimal precision
                entity.Property(e => e.VendorRating).HasColumnType("decimal(3,2)");

                // String lengths
                entity.Property(e => e.AuthorizationNumber).HasMaxLength(100);
                entity.Property(e => e.AuthorizationStatus).HasMaxLength(20);
            });
            //5. Brand Sales Analysis
            modelBuilder.Entity<VwBrandSalesAnalysis>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandSalesAnalysis");

                // Indexes
                entity.HasIndex(e => e.BrandId);
                entity.HasIndex(e => e.OrderYear);
                entity.HasIndex(e => e.OrderMonth);
                entity.HasIndex(e => new { e.BrandId, e.OrderYear, e.OrderMonth });

                // Decimal precision
                entity.Property(e => e.TotalRevenue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AverageUnitPrice).HasColumnType("decimal(18,2)");

                // String lengths
                entity.Property(e => e.MonthName).HasMaxLength(20);
                entity.Property(e => e.TopSellingProduct).HasMaxLength(100);
            });
            //6. Brand Campaigns
            modelBuilder.Entity<VwBrandCampaigns>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwBrandCampaigns");

                // Indexes (Logical for query optimization)
                entity.HasIndex(e => e.BrandId);
                entity.HasIndex(e => e.CampaignId);
                entity.HasIndex(e => e.CampaignIsActive);
                entity.HasIndex(e => new { e.CampaignStartDate, e.CampaignEndDate });

                // Decimal precision
                entity.Property(e => e.AverageCampaignPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.AverageRegularPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.AverageDiscountPercentage)
                      .HasColumnType("decimal(5,2)");

                // String lengths
                entity.Property(e => e.CampaignStatus)
                      .HasMaxLength(20);

                entity.Property(e => e.CampaignTitleAr)
                      .HasMaxLength(200);

                entity.Property(e => e.CampaignTitleEn)
                      .HasMaxLength(200);

                entity.Property(e => e.BrandNameAr)
                      .HasMaxLength(200);

                entity.Property(e => e.BrandNameEn)
                      .HasMaxLength(200);
            });

            // Withdrawal Methods Views
            modelBuilder.Entity<VwWithdrawalMethodsWithFields>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwWithdrawalMethodsWithFields");
            });

            modelBuilder.Entity<VwWithdrawalMethodsFieldsValues>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwWithdrawalMethodsFieldsValues");
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