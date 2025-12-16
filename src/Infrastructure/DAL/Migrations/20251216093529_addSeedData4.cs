using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- =============================================
-- ULTRA-MINIMAL: Only ItemCombinations, Offers, Pricings
-- Run this directly in SQL Server Management Studio
-- =============================================

USE [Basit]
GO

PRINT 'Adding missing data for search/filtering...'
GO

DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @Vendor1 UNIQUEIDENTIFIER = 'a95c3ac2-cd41-4832-a951-51443f5fd18b' 
DECLARE @Vendor2 UNIQUEIDENTIFIER = 'a8a68f35-64f1-4567-95a8-811e9b3981f0'
DECLARE @Vendor3 UNIQUEIDENTIFIER = '76f6a18a-c6f1-4922-b383-713b3ab57682'
DECLARE @NewCondition UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE IsNew = 1)

-- =============================================
-- 1. ITEM COMBINATIONS
-- =============================================
IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 'BC-IP15-128', 'IP15-128GB', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 'BC-IP15-256', 'IP15-256GB', 0, @SystemUserId, @CurrentDate, 0)
END

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 'BC-S24-256', 'S24-256GB', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 'BC-S24-512', 'S24-512GB', 0, @SystemUserId, @CurrentDate, 0)
END

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '8fd3d755-7e2a-4c2a-b084-22c2f1085e35', 'BC-P60-512', 'P60-512GB', 1, @SystemUserId, @CurrentDate, 0)

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '6e716d5d-1d19-44b4-b555-e3758b7a0429')
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '6e716d5d-1d19-44b4-b555-e3758b7a0429', 'BC-MBP-16', 'MBP-16-512', 1, @SystemUserId, @CurrentDate, 0)

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32', 'BC-NIKE-M', 'NIKE-M', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32', 'BC-NIKE-L', 'NIKE-L', 0, @SystemUserId, @CurrentDate, 0)
END

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '07aad4e9-a182-42f6-ac08-26105ed11799')
BEGIN
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
    (NEWID(), '07aad4e9-a182-42f6-ac08-26105ed11799', 'BC-ZARA-M', 'ZARA-M', 1, @SystemUserId, @CurrentDate, 0),
    (NEWID(), '07aad4e9-a182-42f6-ac08-26105ed11799', 'BC-ZARA-L', 'ZARA-L', 0, @SystemUserId, @CurrentDate, 0)
END

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3')
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', 'BC-AIRMAX', 'AIRMAX-42', 1, @SystemUserId, @CurrentDate, 0)

IF NOT EXISTS (SELECT 1 FROM TbItemCombinations WHERE ItemId = 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430')
    INSERT INTO TbItemCombinations ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES (NEWID(), 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430', 'BC-ULTRA', 'ULTRA-43', 1, @SystemUserId, @CurrentDate, 0)

PRINT 'Item Combinations added'
GO

-- =============================================
-- 2. OFFERS
-- =============================================
DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @Vendor1 UNIQUEIDENTIFIER = 'a95c3ac2-cd41-4832-a951-51443f5fd18b'
DECLARE @Vendor2 UNIQUEIDENTIFIER = 'a8a68f35-64f1-4567-95a8-811e9b3981f0'
DECLARE @Vendor3 UNIQUEIDENTIFIER = '76f6a18a-c6f1-4922-b383-713b3ab57682'

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.8, 250, 1),
    (NEWID(), '6c587394-618e-4d5d-b764-0453a262e249', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 0, 4.5, 180, 1)
END

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES
    (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.7, 320, 1),
    (NEWID(), 'aa70111b-f99f-4ed5-8478-2f54ff7c2884', 3, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 0, 4.3, 150, 0)
END

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '8fd3d755-7e2a-4c2a-b084-22c2f1085e35', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 1, 4.6, 195, 1)

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '6e716d5d-1d19-44b4-b555-e3758b7a0429')
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '6e716d5d-1d19-44b4-b555-e3758b7a0429', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor1, 0, 0, 1, 4.9, 145, 1)

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32')
BEGIN
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES
    (NEWID(), 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor2, 0, 0, 1, 4.4, 520, 0),
    (NEWID(), 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 0, 4.2, 310, 1)
END

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '07aad4e9-a182-42f6-ac08-26105ed11799')
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '07aad4e9-a182-42f6-ac08-26105ed11799', 1, 0, NULL, @SystemUserId, @CurrentDate, @Vendor2, 0, 0, 1, 4.5, 285, 1)

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3')
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor2, 0, 0, 1, 4.6, 410, 1)

IF NOT EXISTS (SELECT 1 FROM TbOffers WHERE ItemId = 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430')
    INSERT INTO TbOffers ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], 
     [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorRatingForThisItem], [VendorSalesCountForThisItem], [IsFreeShipping])
    VALUES (NEWID(), 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430', 2, 0, NULL, @SystemUserId, @CurrentDate, @Vendor3, 0, 0, 1, 4.7, 295, 1)

PRINT 'Offers added'
GO

-- =============================================
-- 3. OFFER COMBINATION PRICINGS
-- =============================================
DECLARE @SystemUserId UNIQUEIDENTIFIER = 'b8a9963f-f520-41e2-ada0-9054747da9cb'
DECLARE @CurrentDate DATETIME2(2) = GETDATE()
DECLARE @NewCondition UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE IsNew = 1)

-- Get IDs
DECLARE @IP15_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND IsDefault = 1)
DECLARE @IP15_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND IsDefault = 0)
DECLARE @IP15_O1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '6c587394-618e-4d5d-b764-0453a262e249' AND IsBuyBoxWinner = 1)

DECLARE @S24_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND IsDefault = 1)
DECLARE @S24_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND IsDefault = 0)
DECLARE @S24_O1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'aa70111b-f99f-4ed5-8478-2f54ff7c2884' AND IsBuyBoxWinner = 1)

DECLARE @P60_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')
DECLARE @P60_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '8fd3d755-7e2a-4c2a-b084-22c2f1085e35')

DECLARE @MBP_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '6e716d5d-1d19-44b4-b555-e3758b7a0429')
DECLARE @MBP_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '6e716d5d-1d19-44b4-b555-e3758b7a0429')

DECLARE @NIKE_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32' AND IsDefault = 1)
DECLARE @NIKE_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32' AND IsDefault = 0)
DECLARE @NIKE_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'b8d4eb38-e0aa-4dc8-8f88-50ec17701e32' AND IsBuyBoxWinner = 1)

DECLARE @ZARA_C1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '07aad4e9-a182-42f6-ac08-26105ed11799' AND IsDefault = 1)
DECLARE @ZARA_C2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '07aad4e9-a182-42f6-ac08-26105ed11799' AND IsDefault = 0)
DECLARE @ZARA_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '07aad4e9-a182-42f6-ac08-26105ed11799')

DECLARE @AIR_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3')
DECLARE @AIR_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = '3ea4bc7a-8888-4dab-ab84-d5febdb37fd3')

DECLARE @ULT_C UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbItemCombinations WHERE ItemId = 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430')
DECLARE @ULT_O UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOffers WHERE ItemId = 'a8c7fb69-ceeb-4d91-b73f-8093fae0b430')

-- Insert pricings
IF @IP15_C1 IS NOT NULL AND @IP15_O1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @IP15_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @IP15_C1, @IP15_O1, 4999, 4799, 4200, 45, 5, 0, 0, 3, 0, 2, 1, 1, 5, 10, @CurrentDate, @IP15_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @IP15_C2, @IP15_O1, 5499, 5299, 4700, 38, 3, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @IP15_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
END

IF @S24_C1 IS NOT NULL AND @S24_O1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @S24_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @S24_C1, @S24_O1, 3999, 3799, 3200, 65, 7, 0, 0, 4, 0, 3, 1, 1, 5, 12, @CurrentDate, @S24_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @S24_C2, @S24_O1, 4499, 4299, 3700, 58, 5, 0, 0, 3, 0, 2, 1, 1, 5, 12, @CurrentDate, @S24_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
END

IF @P60_C IS NOT NULL AND @P60_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @P60_C)
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @P60_C, @P60_O, 3499, 3299, 2900, 42, 4, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @P60_C, @SystemUserId, @CurrentDate, 0, @NewCondition)

IF @MBP_C IS NOT NULL AND @MBP_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @MBP_C)
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @MBP_C, @MBP_O, 12999, 12499, 10800, 18, 2, 0, 0, 1, 0, 1, 1, 1, 2, 5, @CurrentDate, @MBP_C, @SystemUserId, @CurrentDate, 0, @NewCondition)

IF @NIKE_C1 IS NOT NULL AND @NIKE_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @NIKE_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @NIKE_C1, @NIKE_O, 129, 99, 65, 150, 10, 0, 0, 5, 0, 5, 1, 1, 10, 20, @CurrentDate, @NIKE_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @NIKE_C2, @NIKE_O, 129, 99, 65, 125, 7, 0, 0, 3, 0, 2, 1, 1, 10, 20, @CurrentDate, @NIKE_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
END

IF @ZARA_C1 IS NOT NULL AND @ZARA_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @ZARA_C1)
BEGIN
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES
    (NEWID(), @ZARA_C1, @ZARA_O, 399, 349, 220, 48, 4, 0, 0, 2, 0, 1, 1, 1, 5, 10, @CurrentDate, @ZARA_C1, @SystemUserId, @CurrentDate, 0, @NewCondition),
    (NEWID(), @ZARA_C2, @ZARA_O, 399, 349, 220, 42, 3, 0, 0, 1, 0, 1, 1, 1, 5, 10, @CurrentDate, @ZARA_C2, @SystemUserId, @CurrentDate, 0, @NewCondition)
END

IF @AIR_C IS NOT NULL AND @AIR_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @AIR_C)
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @AIR_C, @AIR_O, 549, 499, 320, 72, 6, 0, 0, 3, 0, 2, 1, 1, 5, 12, @CurrentDate, @AIR_C, @SystemUserId, @CurrentDate, 0, @NewCondition)

IF @ULT_C IS NOT NULL AND @ULT_O IS NOT NULL AND NOT EXISTS (SELECT 1 FROM TbOfferCombinationPricings WHERE ItemCombinationId = @ULT_C)
    INSERT INTO TbOfferCombinationPricings ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], 
     [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], 
     [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], [LastStockUpdate], [TbItemCombinationId], [CreatedBy], [CreatedDateUtc], [IsDeleted], [OfferConditionId])
    VALUES (NEWID(), @ULT_C, @ULT_O, 699, 649, 420, 55, 5, 0, 0, 2, 0, 1, 1, 1, 5, 12, @CurrentDate, @ULT_C, @SystemUserId, @CurrentDate, 0, @NewCondition)

PRINT 'Offer Combination Pricings added'
PRINT 'DONE! Search and filtering are now enabled!'
GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}