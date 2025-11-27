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
    /// 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
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

        // Offer Management - ADD THESE MISSING DbSets
        public DbSet<Domains.Entities.Offer.TbOffer> TbOffers { get; set; }
        public DbSet<Domains.Entities.Offer.TbOfferCombinationPricing> TbOfferCombinationPricings { get; set; }
        public DbSet<Domains.Entities.Offer.TbOfferCondition> TbOfferConditions { get; set; }
        public DbSet<TbWarranty> TbWarranties { get; set; }
        public DbSet<TbUserOfferRating> TbUserOfferRatings { get; set; }

        // Order Management - ADD THESE MISSING DbSets
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

                #region Global Configuration - Base Entity

                // Apply default NEWID() for all entities inheriting from BaseEntity
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    // FIXED: Single null-safe check instead of two consecutive if statements
                    if (entityType.ClrType != null && typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
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

                // User Views
                modelBuilder.Entity<VwUserNotification>()
                    .HasNoKey()
                    .ToView("VwUserNotifications");

                #endregion

                // Explicitly configure ambiguous relationships
                // Fix: Configure Offer <-> Warranty relationship to remove ambiguity
                modelBuilder.Entity<Domains.Entities.Offer.TbOffer>(entity =>
                {
                    entity.HasOne(o => o.Warranty)
                          .WithMany(w => w.OffersList)
                          .HasForeignKey(o => o.WarrantyId)
                          .OnDelete(DeleteBehavior.SetNull);
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("[OnModelCreating] Exception while building model: " + ex.Message);
                Console.Error.WriteLine(ex.ToString());

                try
                {
                    // try to print some entity types for diagnostics
                    var types = modelBuilder?.Model?.GetEntityTypes()?.Select(t => t.ClrType?.FullName ?? "<no-clr>").ToArray();
                    if (types != null)
                    {
                        Console.Error.WriteLine("[OnModelCreating] Discovered entity CLR types:");
                        foreach (var t in types.Take(20))
                        {
                            Console.Error.WriteLine(" - " + t);
                        }
                    }
                }
                catch { }

                throw;
            }
        }
    }
}