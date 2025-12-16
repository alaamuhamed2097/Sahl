using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- =============================================
-- Complete Missing Data for ALL 18 Items
-- Adds ItemCombinations, Offers, and Pricings
-- =============================================

USE [Basit]
GO

PRINT 'Adding missing data for all items...'
GO

DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @Vendor1 UNIQUEIDENTIFIER = 'a95c3ac2-cd41-4832-a951-51443f5fd18b' -- Electronics Store LLC
DECLARE @Vendor2 UNIQUEIDENTIFIER = 'a8a68f35-64f1-4567-95a8-811e9b3981f0' -- Fashion Hub Co.
DECLARE @Vendor3 UNIQUEIDENTIFIER = '76f6a18a-c6f1-4922-b383-713b3ab57682' -- Tech World Electronics
DECLARE @NewCondition UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE IsNew = 1)

-- =============================================
-- 1. ADD MISSING ITEM COMBINATIONS
-- =============================================
PRINT 'Step 1: Adding Item Combinations...'

-- iPhone 15 Pro (6c587394-618e-4d5d-b764-0453a262e249) - needs combinations
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND SKU = 'IP15-128GB-NEW')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 'BC-IP15-128', 'IP15-128GB-NEW', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 'BC-IP15-256', 'IP15-256GB-NEW', 0, @SystemUserId, @CurrentDate, 0)
    PRINT '  - iPhone 15 Pro combinations'
END

-- Samsung S24 (aa70111b-f99f-4ed5-8478-2f54ff7c2884) - already has some, add more if needed
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND SKU = 'S24-128GB-NEW')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 'BC-S24-128', 'S24-128GB-NEW', 0, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Samsung S24 additional combination'
END

-- Huawei P60 Pro (8fd3d755-7e2a-4c2a-b084-22c2f1085e35)
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '8fd3d755-7e2a-4c2a-b084-22c2f1085e35', 'BC-P60-512', 'P60-512GB', 1, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Huawei P60 Pro combination'
END

-- MacBook Pro 6e716d5d-1d19-44b4-b555-e3758b7a0429 - already exists
-- Nike T-Shirt b8d4eb38-e0aa-4dc8-8f88-50ec17701e32 - already exists  
-- Zara Dress 07aad4e9-a182-42f6-ac08-26105ed11799 - already exists
-- Nike Air Max 3ea4bc7a-8888-4dab-ab84-d5febdb37fd3 - already exists
-- Adidas Ultraboost a8c7fb69-ceeb-4d91-b73f-8093fae0b430 - already exists

-- Nike Men T-Shirt (a56d342d-8945-4909-8367-b311117055e6) - different instance
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'a56d342d-8945-4909-8367-b311117055e6')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), 'a56d342d-8945-4909-8367-b311117055e6', 'BC-NIKE2-M', 'NIKE2-M', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), 'a56d342d-8945-4909-8367-b311117055e6', 'BC-NIKE2-L', 'NIKE2-L', 0, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Nike T-Shirt (2nd instance) combinations'
END

-- Zara Dress (ab4097a6-70c9-4494-83b5-d55c2e8690d1) - different instance
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'ab4097a6-70c9-4494-83b5-d55c2e8690d1')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), 'ab4097a6-70c9-4494-83b5-d55c2e8690d1', 'BC-ZARA2-M', 'ZARA2-M', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), 'ab4097a6-70c9-4494-83b5-d55c2e8690d1', 'BC-ZARA2-L', 'ZARA2-L', 0, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Zara Dress (2nd instance) combinations'
END

-- MacBook Pro (6af7ddee-0102-4999-93e2-561a582aa250) - different instance
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '6af7ddee-0102-4999-93e2-561a582aa250')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '6af7ddee-0102-4999-93e2-561a582aa250', 'BC-MBP2-16', 'MBP2-16-512', 1, @SystemUserId, @CurrentDate, 0)
    PRINT '  - MacBook Pro (2nd instance) combination'
END

-- Samsung Buds (5a5a4b44-afd9-4d4d-b01c-1f6b19902b66)
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66', 'BC-BUDS2-BLK', 'BUDS2-BLACK', 1, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Samsung Buds combination'
END

-- LG Refrigerator (19932f5f-78e6-4d0f-a900-e2a35f183c73)
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '19932f5f-78e6-4d0f-a900-e2a35f183c73')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '19932f5f-78e6-4d0f-a900-e2a35f183c73', 'BC-LG-800', 'LG-FRIDGE-800L', 1, @SystemUserId, @CurrentDate, 0)
    PRINT '  - LG Refrigerator combination'
END

-- Sanyo Washing Machine (df63feea-b879-448a-bdd3-e3aa57672cfb)
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'df63feea-b879-448a-bdd3-e3aa57672cfb')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), 'df63feea-b879-448a-bdd3-e3aa57672cfb', 'BC-SANYO-10', 'SANYO-WASH-10KG', 1, @SystemUserId, @CurrentDate, 0)
    PRINT '  - Sanyo Washing Machine combination'
END

PRINT 'Item Combinations added'
GO

-- =============================================
-- 2. ADD MISSING OFFERS
-- =============================================
PRINT 'Step 2: Adding Offers...'

DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @Vendor1 UNIQUEIDENTIFIER = 'a95c3ac2-cd41-4832-a951-51443f5fd18b'
DECLARE @Vendor2 UNIQUEIDENTIFIER = 'a8a68f35-64f1-4567-95a8-811e9b3981f0'
DECLARE @Vendor3 UNIQUEIDENTIFIER = '76f6a18a-c6f1-4922-b383-713b3ab57682'

-- iPhone 15 Pro
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND VendorId = @Vendor1)
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.8, 250, 1),
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 0, 4.5, 180, 1)
    PRINT '  - iPhone 15 Pro offers'
END

-- Samsung S24
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND VendorId = @Vendor1)
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.7, 320, 1)
    PRINT '  - Samsung S24 offer'
END

-- Huawei P60 Pro (8fd3d755-7e2a-4c2a-b084-22c2f1085e35)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35' AND VendorId = @Vendor3)
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '8fd3d755-7e2a-4c2a-b084-22c2f1085e35', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 1, 4.6, 195, 1)
    PRINT '  - Huawei P60 Pro offer'
END

-- Nike T-Shirt (a56d342d-8945-4909-8367-b311117055e6)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'a56d342d-8945-4909-8367-b311117055e6')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), 'a56d342d-8945-4909-8367-b311117055e6', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor2, 0, 0, 1, 4.4, 520, 0)
    PRINT '  - Nike T-Shirt (2) offer'
END

-- Zara Dress (ab4097a6-70c9-4494-83b5-d55c2e8690d1)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'ab4097a6-70c9-4494-83b5-d55c2e8690d1')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), 'ab4097a6-70c9-4494-83b5-d55c2e8690d1', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor2, 0, 0, 1, 4.5, 285, 1)
    PRINT '  - Zara Dress (2) offer'
END

-- MacBook Pro (6af7ddee-0102-4999-93e2-561a582aa250)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '6af7ddee-0102-4999-93e2-561a582aa250')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '6af7ddee-0102-4999-93e2-561a582aa250', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.9, 145, 1)
    PRINT '  - MacBook Pro (2) offer'
END

-- Samsung Buds (5a5a4b44-afd9-4d4d-b01c-1f6b19902b66)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.6, 380, 1)
    PRINT '  - Samsung Buds offer'
END

-- LG Refrigerator (19932f5f-78e6-4d0f-a900-e2a35f183c73)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '19932f5f-78e6-4d0f-a900-e2a35f183c73')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '19932f5f-78e6-4d0f-a900-e2a35f183c73', 3, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 1, 4.3, 125, 1)
    PRINT '  - LG Refrigerator offer'
END

-- Sanyo Washing Machine (df63feea-b879-448a-bdd3-e3aa57672cfb)
IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'df63feea-b879-448a-bdd3-e3aa57672cfb')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), 'df63feea-b879-448a-bdd3-e3aa57672cfb', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 1, 4.1, 98, 0)
    PRINT '  - Sanyo Washing Machine offer'
END

PRINT 'Offers added'
GO

-- =============================================
-- 3. ADD OFFER COMBINATION PRICINGS (THE CRITICAL PART!)
-- =============================================
PRINT 'Step 3: Adding Offer Combination Pricings...'

DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @NewCondition UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE IsNew = 1)

-- iPhone 15 Pro
DECLARE @IP15_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND SKU = 'IP15-128GB-NEW')
DECLARE @IP15_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND SKU = 'IP15-256GB-NEW')
DECLARE @IP15_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND IsBuyBoxWinner = 1)

IF @IP15_C1 IS NOT NULL AND @IP15_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @IP15_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @IP15_C1, @IP15_O, 4999, 4799, 4200, 45, 5, 0, 0, 3, 0, 2, 1, 1, 5, 10, @CurrentDate, @IP15_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @IP15_C2, @IP15_O, 5499, 5299, 4700, 38, 3, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @IP15_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - iPhone 15 Pro pricings'
END

-- Samsung S24
DECLARE @S24_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND SKU = 'S24-128GB-NEW')
DECLARE @S24_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND IsBuyBoxWinner = 1)

IF @S24_C IS NOT NULL AND @S24_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @S24_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @S24_C, @S24_O, 3999, 3799, 3200, 65, 7, 0, 0, 4, 0, 3, 1, 1, 5, 12, @CurrentDate, @S24_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Samsung S24 pricing'
END

-- Huawei P60 Pro
DECLARE @P60_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')
DECLARE @P60_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')

IF @P60_C IS NOT NULL AND @P60_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @P60_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @P60_C, @P60_O, 3499, 3299, 2900, 42, 4, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @P60_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Huawei P60 Pro pricing'
END

-- Nike T-Shirt (a56d342d-8945-4909-8367-b311117055e6)
DECLARE @NIKE2_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'a56d342d-8945-4909-8367-b311117055e6' AND SKU = 'NIKE2-M')
DECLARE @NIKE2_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'a56d342d-8945-4909-8367-b311117055e6' AND SKU = 'NIKE2-L')
DECLARE @NIKE2_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'a56d342d-8945-4909-8367-b311117055e6')

IF @NIKE2_C1 IS NOT NULL AND @NIKE2_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @NIKE2_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @NIKE2_C1, @NIKE2_O, 129, 99, 65, 150, 10, 0, 0, 5, 0, 5, 1, 1, 10, 20, @CurrentDate, @NIKE2_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @NIKE2_C2, @NIKE2_O, 129, 99, 65, 125, 7, 0, 0, 3, 0, 2, 1, 1, 10, 20, @CurrentDate, @NIKE2_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Nike T-Shirt (2) pricings'
END

-- Zara Dress (ab4097a6-70c9-4494-83b5-d55c2e8690d1)
DECLARE @ZARA2_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'ab4097a6-70c9-4494-83b5-d55c2e8690d1' AND SKU = 'ZARA2-M')
DECLARE @ZARA2_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'ab4097a6-70c9-4494-83b5-d55c2e8690d1' AND SKU = 'ZARA2-L')
DECLARE @ZARA2_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'ab4097a6-70c9-4494-83b5-d55c2e8690d1')

IF @ZARA2_C1 IS NOT NULL AND @ZARA2_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @ZARA2_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @ZARA2_C1, @ZARA2_O, 399, 349, 220, 48, 4, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @ZARA2_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @ZARA2_C2, @ZARA2_O, 399, 349, 220, 42, 3, 0, 0, 1, 0, 1, 1, 1, 5, 10, @CurrentDate, @ZARA2_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Zara Dress (2) pricings'
END

-- MacBook Pro (6af7ddee-0102-4999-93e2-561a582aa250)
DECLARE @MBP2_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6af7ddee-0102-4999-93e2-561a582aa250')
DECLARE @MBP2_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '6af7ddee-0102-4999-93e2-561a582aa250')

IF @MBP2_C IS NOT NULL AND @MBP2_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @MBP2_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @MBP2_C, @MBP2_O, 12999, 12499, 10800, 18, 2, 0, 0, 1, 0, 1, 1, 1, 2, 5, @CurrentDate, @MBP2_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - MacBook Pro (2) pricing'
END

-- Samsung Buds (5a5a4b44-afd9-4d4d-b01c-1f6b19902b66)
DECLARE @BUDS_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66')
DECLARE @BUDS_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '5a5a4b44-afd9-4d4d-b01c-1f6b19902b66')

IF @BUDS_C IS NOT NULL AND @BUDS_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @BUDS_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @BUDS_C, @BUDS_O, 799, 699, 520, 85, 8, 0, 0, 4, 0, 3, 1, 1, 5, 15, @CurrentDate, @BUDS_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Samsung Buds pricing'
END

-- LG Refrigerator (19932f5f-78e6-4d0f-a900-e2a35f183c73)
DECLARE @LG_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '19932f5f-78e6-4d0f-a900-e2a35f183c73')
DECLARE @LG_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '19932f5f-78e6-4d0f-a900-e2a35f183c73')

IF @LG_C IS NOT NULL AND @LG_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @LG_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @LG_C, @LG_O, 8999, 8499, 7200, 12, 1, 0, 0, 2, 0, 0, 1, 1, 1, 3, @CurrentDate, @LG_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - LG Refrigerator pricing'
END

-- Sanyo Washing Machine (df63feea-b879-448a-bdd3-e3aa57672cfb)
DECLARE @SANYO_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'df63feea-b879-448a-bdd3-e3aa57672cfb')
DECLARE @SANYO_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'df63feea-b879-448a-bdd3-e3aa57672cfb')

IF @SANYO_C IS NOT NULL AND @SANYO_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @SANYO_C)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @SANYO_C, @SANYO_O, 2999, 2799, 2200, 28, 2, 0, 0, 3, 0, 1, 1, 1, 1, 5, @CurrentDate, @SANYO_C, @SystemUserId, @CurrentDate, 0, @NewCondition)
    PRINT '  - Sanyo Washing Machine pricing'
END

PRINT 'Offer Combination Pricings added'
PRINT ''
PRINT '============================================='
PRINT 'SUCCESS! All items now have complete data!'
PRINT 'Search and filtering are now fully enabled!'
PRINT '============================================='
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
