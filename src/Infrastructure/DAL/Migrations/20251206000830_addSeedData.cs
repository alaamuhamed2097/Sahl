usin










    Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- ============================================
-- SEED DATA FOR TESTING SHOPPING CART
-- Items, Offers, and Related Data
-- ============================================

USE [Basit]
GO

-- Check if data already exists to avoid duplicates
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE UserName = N'admin@Basit.com')
BEGIN
    DECLARE @AdminUserId UNIQUEIDENTIFIER = NEWID()
    DECLARE @VendorUserId1 NVARCHAR(450) = NEWID()
    DECLARE @VendorUserId2 NVARCHAR(450) = NEWID()
    DECLARE @CustomerUserId NVARCHAR(450) = NEWID()

    -- ============================================
    -- 1. INSERT USERS (Admin, Vendors, Customer)
    -- ============================================

    -- Insert Admin User
    INSERT INTO [dbo].[AspNetUsers] 
        ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
         [EmailConfirmed], [PhoneNumber], [PhoneCode], [PhoneNumberConfirmed], [ProfileImagePath], 
         [CreatedBy], [CreatedDateUtc], [UserState], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES 
        (@AdminUserId, N'Admin', N'User', N'admin@Basit.com', N'ADMIN@Basit.COM', N'admin@Basit.com', N'ADMIN@Basit.COM',
         1, N'1234567890', N'+20', 1, N'/images/default.jpg', @AdminUserId, GETUTCDATE(), 1, 0, 0, 0);

    -- Insert Vendor 1
    INSERT INTO [dbo].[AspNetUsers] 
        ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
         [EmailConfirmed], [PhoneNumber], [PhoneCode], [PhoneNumberConfirmed], [ProfileImagePath], 
         [CreatedBy], [CreatedDateUtc], [UserState], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES 
        (@VendorUserId1, N'Electronics', N'Store', N'vendor1@Basit.com', N'VENDOR1@Basit.COM', N'vendor1@Basit.com', N'VENDOR1@Basit.COM',
         1, N'1111111111', N'+20', 1, N'/images/vendor1.jpg', @AdminUserId, GETUTCDATE(), 1, 0, 0, 0);

    -- Insert Vendor 2
    INSERT INTO [dbo].[AspNetUsers] 
        ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
         [EmailConfirmed], [PhoneNumber], [PhoneCode], [PhoneNumberConfirmed], [ProfileImagePath], 
         [CreatedBy], [CreatedDateUtc], [UserState], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES 
        (@VendorUserId2, N'Fashion', N'Hub', N'vendor2@Basit.com', N'VENDOR2@Basit.COM', N'vendor2@Basit.com', N'VENDOR2@Basit.COM',
         1, N'2222222222', N'+20', 1, N'/images/vendor2.jpg', @AdminUserId, GETUTCDATE(), 1, 0, 0, 0);

    -- Insert Customer
    INSERT INTO [dbo].[AspNetUsers] 
        ([Id], [FirstName], [LastName], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
         [EmailConfirmed], [PhoneNumber], [PhoneCode], [PhoneNumberConfirmed], [ProfileImagePath], 
         [CreatedBy], [CreatedDateUtc], [UserState], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES 
        (@CustomerUserId, N'John', N'Doe', N'customer@test.com', N'CUSTOMER@TEST.COM', N'customer@test.com', N'CUSTOMER@TEST.COM',
         1, N'1234567890', N'+20', 1, N'/images/customer.jpg', @AdminUserId, GETUTCDATE(), 1, 0, 0, 0);

    -- ============================================
    -- 2. INSERT VENDORS
    -- ============================================

    DECLARE @VendorId1 UNIQUEIDENTIFIER = NEWID()
    DECLARE @VendorId2 UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbVendors] 
        ([Id], [UserId], [VendorType], [CompanyName], [ContactName], [VendorCode], 
         [IsActive], [VATRegistered], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@VendorId1, @VendorUserId1, 1, N'Electronics Store LLC', N'Ahmed Mohamed', N'VEN001', 
         1, 1, @AdminUserId, GETUTCDATE(), 1),
        (@VendorId2, @VendorUserId2, 1, N'Fashion Hub Co.', N'Sara Ali', N'VEN002', 
         1, 1, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 3. GET OR INSERT PRICING SYSTEM
    -- ============================================

    DECLARE @PricingSystemId UNIQUEIDENTIFIER
    
    -- Check if a pricing system with SystemType = 0 already exists
    SELECT @PricingSystemId = Id FROM [dbo].[TbPricingSystemSettings] WHERE SystemType = 0
    
    -- If not exists, create a new one
    IF @PricingSystemId IS NULL
    BEGIN
        SET @PricingSystemId = NEWID()
        INSERT INTO [dbo].[TbPricingSystemSettings]
            ([Id], [SystemNameEn], [SystemNameAr], [SystemType], [IsEnabled], [DisplayOrder], 
             [CreatedBy], [CreatedDateUtc], [CurrentState])
        VALUES
            (@PricingSystemId, N'Standard Pricing', N'التسعير القياسي', 0, 1, 1, 
             @AdminUserId, GETUTCDATE(), 1)
    END

    -- ============================================
    -- 4. INSERT CATEGORIES
    -- ============================================

    DECLARE @CategoryElectronics UNIQUEIDENTIFIER = NEWID()
    DECLARE @CategoryFashion UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbCategories] 
        ([Id], [TitleAr], [TitleEn], [ParentId], [IsFinal], [IsHomeCategory], [IsFeaturedCategory], 
         [IsMainCategory], [PriceRequired], [DisplayOrder], [TreeViewSerial], [IsRoot], 
         [PricingSystemType], [PricingSystemId], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@CategoryElectronics, N'إلكترونيات', N'Electronics', NULL, 1, 1, 1, 1, 1, 1, N'001', 1, 
         0, @PricingSystemId, @AdminUserId, GETUTCDATE(), 1),
        (@CategoryFashion, N'أزياء', N'Fashion', NULL, 1, 1, 1, 1, 1, 2, N'002', 1, 
         0, @PricingSystemId, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 5. INSERT BRANDS
    -- ============================================

    DECLARE @BrandSamsung UNIQUEIDENTIFIER = NEWID()
    DECLARE @BrandApple UNIQUEIDENTIFIER = NEWID()
    DECLARE @BrandNike UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbBrands] 
        ([Id], [NameEn], [NameAr], [TitleEn], [TitleAr], [DescriptionEn], [DescriptionAr], 
         [LogoPath], [WebsiteUrl], [DisplayOrder], [IsPopular], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@BrandSamsung, N'Samsung', N'سامسونج', N'Samsung Electronics', N'سامسونج للإلكترونيات', 
         N'Leading electronics brand', N'علامة تجارية رائدة في الإلكترونيات', 
         N'/images/samsung.jpg', N'https://samsung.com', 1, 1, @AdminUserId, GETUTCDATE(), 1),
        (@BrandApple, N'Apple', N'أبل', N'Apple Inc.', N'شركة أبل', 
         N'Premium technology products', N'منتجات تقنية فاخرة', 
         N'/images/apple.jpg', N'https://apple.com', 2, 1, @AdminUserId, GETUTCDATE(), 1),
        (@BrandNike, N'Nike', N'نايكي', N'Nike Sportswear', N'نايكي للملابس الرياضية', 
         N'Athletic apparel and footwear', N'ملابس وأحذية رياضية', 
         N'/images/nike.jpg', N'https://nike.com', 3, 1, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 6. INSERT UNITS
    -- ============================================

    DECLARE @UnitPiece UNIQUEIDENTIFIER = NEWID()
    DECLARE @UnitPair UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbUnits] 
        ([Id], [TitleAr], [TitleEn], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@UnitPiece, N'قطعة', N'Piece', @AdminUserId, GETUTCDATE(), 1),
        (@UnitPair, N'زوج', N'Pair', @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 7. INSERT ITEMS (PRODUCTS)
    -- ============================================

    -- Item 1: Samsung Galaxy S24
    DECLARE @ItemPhone UNIQUEIDENTIFIER = NEWID()
    INSERT INTO [dbo].[TbItems] 
        ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], 
         [DescriptionAr], [DescriptionEn], [CategoryId], [UnitId], [BrandId], 
         [ThumbnailImage], [SEOTitle], [SEODescription], [SEOMetaTags], 
         [VisibilityScope], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@ItemPhone, N'سامسونج جالاكسي S24', N'Samsung Galaxy S24', 
         N'هاتف ذكي بتقنية 5G', N'5G Smartphone', 
         N'أحدث هاتف ذكي من سامسونج بكاميرا 200 ميجابكسل', 
         N'Latest Samsung smartphone with 200MP camera', 
         @CategoryElectronics, @UnitPiece, @BrandSamsung, 
         N'/images/galaxy-s24.jpg', N'Samsung Galaxy S24 - Latest Flagship', 
         N'Buy Samsung Galaxy S24 with best price', N'samsung,galaxy,s24,smartphone', 
         0, @AdminUserId, GETUTCDATE(), 1);

    -- Item 2: iPhone 15 Pro
    DECLARE @ItemiPhone UNIQUEIDENTIFIER = NEWID()
    INSERT INTO [dbo].[TbItems] 
        ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], 
         [DescriptionAr], [DescriptionEn], [CategoryId], [UnitId], [BrandId], 
         [ThumbnailImage], [SEOTitle], [SEODescription], [SEOMetaTags], 
         [VisibilityScope], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@ItemiPhone, N'آيفون 15 برو', N'iPhone 15 Pro', 
         N'هاتف أبل الرائد', N'Apple Flagship Phone', 
         N'آيفون 15 برو مع شريحة A17 Pro وكاميرا متطورة', 
         N'iPhone 15 Pro with A17 Pro chip and advanced camera', 
         @CategoryElectronics, @UnitPiece, @BrandApple, 
         N'/images/iphone-15-pro.jpg', N'iPhone 15 Pro - Premium Smartphone', 
         N'Buy iPhone 15 Pro with best price', N'iphone,15,pro,apple', 
         0, @AdminUserId, GETUTCDATE(), 1);

    -- Item 3: Nike Air Max
    DECLARE @ItemShoes UNIQUEIDENTIFIER = NEWID()
    INSERT INTO [dbo].[TbItems] 
        ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], 
         [DescriptionAr], [DescriptionEn], [CategoryId], [UnitId], [BrandId], 
         [ThumbnailImage], [SEOTitle], [SEODescription], [SEOMetaTags], 
         [VisibilityScope], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES 
        (@ItemShoes, N'نايكي إير ماكس', N'Nike Air Max', 
         N'حذاء رياضي مريح', N'Comfortable Athletic Shoes', 
         N'حذاء نايكي إير ماكس للرجال والنساء', 
         N'Nike Air Max shoes for men and women', 
         @CategoryFashion, @UnitPair, @BrandNike, 
         N'/images/nike-air-max.jpg', N'Nike Air Max - Athletic Footwear', 
         N'Buy Nike Air Max with best price', N'nike,air,max,shoes', 
         0, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 8. INSERT ITEM COMBINATIONS (VARIANTS)
    -- ============================================

    -- Samsung Galaxy S24 Variants
    DECLARE @CombPhone256Black UNIQUEIDENTIFIER = NEWID()
    DECLARE @CombPhone512Silver UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbItemCombinations]
        ([Id], [ItemId], [SKU], [Barcode], [BasePrice], [IsDefault], 
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (@CombPhone256Black, @ItemPhone, N'SGS24-256-BLK', N'1234567890123', 35000.00, 1, 
         @AdminUserId, GETUTCDATE(), 1),
        (@CombPhone512Silver, @ItemPhone, N'SGS24-512-SLV', N'1234567890124', 42000.00, 0, 
         @AdminUserId, GETUTCDATE(), 1);

    -- iPhone 15 Pro Variants
    DECLARE @CombiPhone256Blue UNIQUEIDENTIFIER = NEWID()
    DECLARE @CombiPhone512Titan UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbItemCombinations]
        ([Id], [ItemId], [SKU], [Barcode], [BasePrice], [IsDefault], 
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (@CombiPhone256Blue, @ItemiPhone, N'IPH15P-256-BLU', N'2234567890123', 48000.00, 1, 
         @AdminUserId, GETUTCDATE(), 1),
        (@CombiPhone512Titan, @ItemiPhone, N'IPH15P-512-TIT', N'2234567890124', 55000.00, 0, 
         @AdminUserId, GETUTCDATE(), 1);

    -- Nike Air Max Variants
    DECLARE @CombShoes42Black UNIQUEIDENTIFIER = NEWID()
    DECLARE @CombShoes43White UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbItemCombinations]
        ([Id], [ItemId], [SKU], [Barcode], [BasePrice], [IsDefault], 
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (@CombShoes42Black, @ItemShoes, N'NAM-42-BLK', N'3234567890123', 3500.00, 1, 
         @AdminUserId, GETUTCDATE(), 1),
        (@CombShoes43White, @ItemShoes, N'NAM-43-WHT', N'3234567890124', 3500.00, 0, 
         @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 9. INSERT OFFERS
    -- ============================================

    -- Offers from Vendor 1 (Electronics Store)
    DECLARE @OfferPhone1 UNIQUEIDENTIFIER = NEWID()
    DECLARE @OfferPhone2 UNIQUEIDENTIFIER = NEWID()
    DECLARE @OfferiPhone1 UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbOffers]
        ([Id], [ItemId], [UserId], [HandlingTimeInDays], [StorgeLocation], 
         [VisibilityScope], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (@OfferPhone1, @ItemPhone, @VendorUserId1, 2, 0, 0, @AdminUserId, GETUTCDATE(), 1),
        (@OfferPhone2, @ItemPhone, @VendorUserId2, 3, 0, 0, @AdminUserId, GETUTCDATE(), 1),
        (@OfferiPhone1, @ItemiPhone, @VendorUserId1, 2, 0, 0, @AdminUserId, GETUTCDATE(), 1);

    -- Offers from Vendor 2 (Fashion Hub)
    DECLARE @OfferShoes1 UNIQUEIDENTIFIER = NEWID()

    INSERT INTO [dbo].[TbOffers]
        ([Id], [ItemId], [UserId], [HandlingTimeInDays], [StorgeLocation], 
         [VisibilityScope], [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (@OfferShoes1, @ItemShoes, @VendorUserId2, 1, 0, 0, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- 10. INSERT OFFER COMBINATION PRICING (STOCK & PRICE)
    -- ============================================

    -- Samsung Galaxy S24 - Vendor 1
    INSERT INTO [dbo].[TbOfferCombinationPricings]
        ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], 
         [AvailableQuantity], [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], 
         [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
         [IsDefault], [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold],
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (NEWID(), @CombPhone256Black, @OfferPhone1, 34500.00, 34500.00, 
         50, 0, 0, 0, 0, 0, 0, 1, 1, 1, 10, 5, @AdminUserId, GETUTCDATE(), 1),
        (NEWID(), @CombPhone512Silver, @OfferPhone1, 41500.00, 41500.00, 
         30, 0, 0, 0, 0, 0, 0, 1, 0, 1, 10, 5, @AdminUserId, GETUTCDATE(), 1);

    -- Samsung Galaxy S24 - Vendor 2 (Different Prices)
    INSERT INTO [dbo].[TbOfferCombinationPricings]
        ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], 
         [AvailableQuantity], [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], 
         [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
         [IsDefault], [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold],
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (NEWID(), @CombPhone256Black, @OfferPhone2, 34800.00, 34800.00, 
         40, 0, 0, 0, 0, 0, 0, 1, 1, 1, 10, 5, @AdminUserId, GETUTCDATE(), 1),
        (NEWID(), @CombPhone512Silver, @OfferPhone2, 41800.00, 41800.00, 
         25, 0, 0, 0, 0, 0, 0, 1, 0, 1, 10, 5, @AdminUserId, GETUTCDATE(), 1);

    -- iPhone 15 Pro - Vendor 1
    INSERT INTO [dbo].[TbOfferCombinationPricings]
        ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], 
         [AvailableQuantity], [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], 
         [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
         [IsDefault], [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold],
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (NEWID(), @CombiPhone256Blue, @OfferiPhone1, 47500.00, 47500.00, 
         35, 0, 0, 0, 0, 0, 0, 1, 1, 1, 5, 3, @AdminUserId, GETUTCDATE(), 1),
        (NEWID(), @CombiPhone512Titan, @OfferiPhone1, 54500.00, 54500.00, 
         20, 0, 0, 0, 0, 0, 0, 1, 0, 1, 5, 3, @AdminUserId, GETUTCDATE(), 1);

    -- Nike Air Max - Vendor 2
    INSERT INTO [dbo].[TbOfferCombinationPricings]
        ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], 
         [AvailableQuantity], [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], 
         [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
         [IsDefault], [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold],
         [CreatedBy], [CreatedDateUtc], [CurrentState])
    VALUES
        (NEWID(), @CombShoes42Black, @OfferShoes1, 3299.00, 3299.00, 
         100, 0, 0, 0, 0, 0, 0, 1, 1, 1, 20, 10, @AdminUserId, GETUTCDATE(), 1),
        (NEWID(), @CombShoes43White, @OfferShoes1, 3299.00, 3299.00, 
         80, 0, 0, 0, 0, 0, 0, 1, 0, 1, 20, 10, @AdminUserId, GETUTCDATE(), 1);

    -- ============================================
    -- SUCCESS MESSAGE
    -- ============================================

    PRINT '============================================'
    PRINT 'SEED DATA INSERTED SUCCESSFULLY!'
    PRINT '============================================'
    PRINT ''
    PRINT 'Test Credentials:'
    PRINT '  Customer: customer@test.com'
    PRINT '  Vendor 1: vendor1@Basit.com (Electronics)'
    PRINT '  Vendor 2: vendor2@Basit.com (Fashion)'
    PRINT ''
    PRINT 'Available Items:'
    PRINT '  1. Samsung Galaxy S24 (2 vendors, 2 variants)'
    PRINT '  2. iPhone 15 Pro (1 vendor, 2 variants)'
    PRINT '  3. Nike Air Max (1 vendor, 2 variants)'
    PRINT ''
    PRINT 'Customer UserId for testing: ' + CAST(@CustomerUserId AS NVARCHAR(450))
    PRINT '============================================'
END
ELSE
BEGIN
    PRINT '============================================'
    PRINT 'SEED DATA ALREADY EXISTS - SKIPPING INSERTION'
    PRINT '============================================'
END

-- Display sample offer IDs and prices for quick reference
SELECT 
    'OFFERS SUMMARY' AS [Info],
    i.TitleEn AS [Product],
    oc.Price AS [Price],
    oc.AvailableQuantity AS [Stock],
    u.UserName AS [Vendor],
    o.Id AS [OfferId]
FROM TbOffers o
INNER JOIN TbItems i ON o.ItemId = i.Id
INNER JOIN TbOfferCombinationPricings oc ON o.Id = oc.OfferId
INNER JOIN AspNetUsers u ON o.UserId = u.Id
WHERE oc.IsDefault = 1
ORDER BY i.TitleEn;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Clean up seed data
                -- Note: This is a simple cleanup and might need adjustment based on your database constraints
                
                -- Delete from dependent tables first
                DELETE FROM [dbo].[TbOfferCombinationPricings] 
                WHERE OfferId IN (
                    SELECT o.Id FROM [dbo].[TbOffers] o
                    INNER JOIN [dbo].[AspNetUsers] u ON o.UserId = u.Id
                    WHERE u.UserName IN (N'vendor1@Basit.com', N'vendor2@Basit.com')
                )
                
                DELETE FROM [dbo].[TbOffers] 
                WHERE UserId IN (
                    SELECT Id FROM [dbo].[AspNetUsers]
                    WHERE UserName IN (N'vendor1@Basit.com', N'vendor2@Basit.com')
                )
                
                DELETE FROM [dbo].[TbItemCombinations]
                WHERE ItemId IN (
                    SELECT Id FROM [dbo].[TbItems]
                    WHERE TitleEn IN (N'Samsung Galaxy S24', N'iPhone 15 Pro', N'Nike Air Max')
                )
                
                DELETE FROM [dbo].[TbItems]
                WHERE TitleEn IN (N'Samsung Galaxy S24', N'iPhone 15 Pro', N'Nike Air Max')
                
                DELETE FROM [dbo].[TbVendors]
                WHERE UserId IN (
                    SELECT Id FROM [dbo].[AspNetUsers]
                    WHERE UserName IN (N'vendor1@Basit.com', N'vendor2@Basit.com')
                )
                
                DELETE FROM [dbo].[AspNetUsers]
                WHERE UserName IN (N'admin@Basit.com', N'vendor1@Basit.com', N'vendor2@Basit.com', N'customer@test.com')
                
                PRINT 'SEED DATA REMOVED SUCCESSFULLY!'
            ");
        }
    }
}