using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        USE [Basit]
GO

BEGIN TRANSACTION

PRINT 'Starting comprehensive migration with test data...'

-- ============================================
-- 1. INSERT ADDITIONAL CATEGORIES
-- ============================================
PRINT 'Inserting additional categories...'
DECLARE @ElectronicsCatId UNIQUEIDENTIFIER = (SELECT Id FROM TbCategories WHERE TitleEn = 'Electronics');
DECLARE @FashionCatId UNIQUEIDENTIFIER = (SELECT Id FROM TbCategories WHERE TitleEn = 'Fashion');

DECLARE @SmartphonesCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @LaptopsCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @HeadphonesCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @MenClothingCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @WomenClothingCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @ShoesCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @HomeAppliancesCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @RefrigeratorsCatId UNIQUEIDENTIFIER = NEWID();
DECLARE @WashingMachinesCatId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbCategories] 
    ([Id], [TitleAr], [TitleEn], [ParentId], [IsFinal], [IsHomeCategory], [IsRoot], [IsFeaturedCategory], [IsMainCategory], [PriceRequired], [DisplayOrder], [TreeViewSerial], [PricingSystemId], [PricingSystemType], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    -- Electronics Subcategories
    (@SmartphonesCatId, N'هواتف ذكية', 'Smartphones', @ElectronicsCatId, 1, 1, 0, 0, 0, 1, 1, N'001.001', '11111111-1111-1111-1111-111111111111', 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@LaptopsCatId, N'لابتوبات', 'Laptops', @ElectronicsCatId, 1, 1, 0, 0, 0, 1, 2, N'001.002', '11111111-1111-1111-1111-111111111111', 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@HeadphonesCatId, N'سماعات', 'Headphones', @ElectronicsCatId, 1, 1, 0, 0, 0, 1, 3, N'001.003', '22222222-2222-2222-2222-222222222222', 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    
    -- Fashion Subcategories
    (@MenClothingCatId, N'ملابس رجالية', 'Men Clothing', @FashionCatId, 1, 1, 0, 0, 0, 1, 1, N'002.001', '33333333-3333-3333-3333-333333333333', 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@WomenClothingCatId, N'ملابس نسائية', 'Women Clothing', @FashionCatId, 1, 1, 0, 0, 0, 1, 2, N'002.002', '44444444-4444-4444-4444-444444444444', 3, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@ShoesCatId, N'أحذية', 'Shoes', @FashionCatId, 1, 1, 0, 0, 0, 1, 3, N'002.003', '55555555-5555-5555-5555-555555555555', 4, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    
    -- Home Appliances (New Main Category)
    (@HomeAppliancesCatId, N'أجهزة منزلية', 'Home Appliances', NULL, 0, 1, 1, 1, 1, 1, 3, N'003', '11111111-1111-1111-1111-111111111111', 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@RefrigeratorsCatId, N'ثلاجات', 'Refrigerators', @HomeAppliancesCatId, 1, 1, 0, 0, 0, 1, 1, N'003.001', '22222222-2222-2222-2222-222222222222', 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@WashingMachinesCatId, N'غسالات', 'Washing Machines', @HomeAppliancesCatId, 1, 1, 0, 0, 0, 1, 2, N'003.002', '33333333-3333-3333-3333-333333333333', 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 2. INSERT ADDITIONAL BRANDS
-- ============================================
PRINT 'Inserting additional brands...'
DECLARE @HuaweiBrandId UNIQUEIDENTIFIER = NEWID();
DECLARE @AdidasBrandId UNIQUEIDENTIFIER = NEWID();
DECLARE @LGBrandId UNIQUEIDENTIFIER = NEWID();
DECLARE @SanyoBrandId UNIQUEIDENTIFIER = NEWID();
DECLARE @ZaraBrandId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbBrands] 
    ([Id], [NameAr], [NameEn], [TitleAr], [TitleEn], [DescriptionAr], [DescriptionEn], [LogoPath], [WebsiteUrl], [IsPopular], [DisplayOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@HuaweiBrandId, N'هواوي', 'Huawei', N'هواوي تكنولوجيز', 'Huawei Technologies', N'شركة رائدة في مجال الاتصالات', 'Leading telecommunications company', '/images/huawei.jpg', 'https://huawei.com', 1, 4, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@AdidasBrandId, N'أديداس', 'Adidas', N'أديداس للملابس الرياضية', 'Adidas Sportswear', N'ملابس وأحذية رياضية عالمية', 'Global athletic apparel and footwear', '/images/adidas.jpg', 'https://adidas.com', 1, 5, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@LGBrandId, N'إل جي', 'LG', N'إل جي للإلكترونيات', 'LG Electronics', N'أجهزة إلكترونية ومنزلية', 'Electronics and home appliances', '/images/lg.jpg', 'https://lg.com', 1, 6, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@SanyoBrandId, N'سانيو', 'Sanyo', N'سانيو للأجهزة المنزلية', 'Sanyo Home Appliances', N'أجهزة منزلية متنوعة', 'Various home appliances', '/images/sanyo.jpg', 'https://sanyo.com', 0, 7, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@ZaraBrandId, N'زارا', 'Zara', N'زارا للملابس', 'Zara Clothing', N'ملابس عصرية', 'Fashionable clothing', '/images/zara.jpg', 'https://zara.com', 1, 8, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 3. INSERT ATTRIBUTES AND ATTRIBUTE OPTIONS
-- ============================================
PRINT 'Inserting attributes and options...'

-- Color Attribute
DECLARE @ColorAttributeId UNIQUEIDENTIFIER = NEWID();
DECLARE @BlackOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @WhiteOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @BlueOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @RedOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @GoldOptionId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbAttributes] 
    ([Id], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@ColorAttributeId, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbAttributeOptions]
    ([Id], [TitleAr], [TitleEn], [DisplayOrder], [AttributeId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@BlackOptionId, N'أسود', 'Black', 1, @ColorAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@WhiteOptionId, N'أبيض', 'White', 2, @ColorAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@BlueOptionId, N'أزرق', 'Blue', 3, @ColorAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@RedOptionId, N'أحمر', 'Red', 4, @ColorAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@GoldOptionId, N'ذهبي', 'Gold', 5, @ColorAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Size Attribute
DECLARE @SizeAttributeId UNIQUEIDENTIFIER = NEWID();
DECLARE @SizeSOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @SizeMOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @SizeLOptionId UNIQUEIDENTIFIER = NEWID();
DECLARE @SizeXLOptionId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbAttributes] 
    ([Id], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@SizeAttributeId, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbAttributeOptions]
    ([Id], [TitleAr], [TitleEn], [DisplayOrder], [AttributeId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@SizeSOptionId, N'صغير', 'S', 1, @SizeAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@SizeMOptionId, N'متوسط', 'M', 2, @SizeAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@SizeLOptionId, N'كبير', 'L', 3, @SizeAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@SizeXLOptionId, N'كبير جداً', 'XL', 4, @SizeAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Storage Attribute
DECLARE @StorageAttributeId UNIQUEIDENTIFIER = NEWID();
DECLARE @Storage128Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Storage256Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Storage512Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Storage1TBId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbAttributes] 
    ([Id], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@StorageAttributeId, N'سعة التخزين', 'Storage', 0, 1, 20, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbAttributeOptions]
    ([Id], [TitleAr], [TitleEn], [DisplayOrder], [AttributeId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@Storage128Id, N'128 جيجا', '128GB', 1, @StorageAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@Storage256Id, N'256 جيجا', '256GB', 2, @StorageAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@Storage512Id, N'512 جيجا', '512GB', 3, @StorageAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@Storage1TBId, N'1 تيرا', '1TB', 4, @StorageAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Material Attribute
DECLARE @MaterialAttributeId UNIQUEIDENTIFIER = NEWID();
DECLARE @CottonMaterialId UNIQUEIDENTIFIER = NEWID();
DECLARE @LeatherMaterialId UNIQUEIDENTIFIER = NEWID();
DECLARE @PolyesterMaterialId UNIQUEIDENTIFIER = NEWID();
DECLARE @SilkMaterialId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbAttributes] 
    ([Id], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@MaterialAttributeId, N'المادة', 'Material', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbAttributeOptions]
    ([Id], [TitleAr], [TitleEn], [DisplayOrder], [AttributeId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@CottonMaterialId, N'قطن', 'Cotton', 1, @MaterialAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@LeatherMaterialId, N'جلد', 'Leather', 2, @MaterialAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@PolyesterMaterialId, N'بوليستر', 'Polyester', 3, @MaterialAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@SilkMaterialId, N'حرير', 'Silk', 4, @MaterialAttributeId, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 4. INSERT CATEGORY-ATTRIBUTE RELATIONSHIPS
-- ============================================
PRINT 'Setting up category-attribute relationships...'

-- Smartphones Category Attributes
INSERT INTO [dbo].[TbCategoryAttributes]
    ([Id], [CategoryId], [AttributeId], [AffectsPricing], [IsVariant], [AffectsStock], [IsRequired], [DisplayOrder], [SortOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @SmartphonesCatId, @ColorAttributeId, 0, 1, 0, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @SmartphonesCatId, @StorageAttributeId, 0, 1, 0, 1, 2, 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Laptops Category Attributes
INSERT INTO [dbo].[TbCategoryAttributes]
    ([Id], [CategoryId], [AttributeId], [AffectsPricing], [IsVariant], [AffectsStock], [IsRequired], [DisplayOrder], [SortOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @LaptopsCatId, @ColorAttributeId, 0, 1, 0, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @LaptopsCatId, @StorageAttributeId, 0, 1, 0, 1, 2, 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Men Clothing Category Attributes
INSERT INTO [dbo].[TbCategoryAttributes]
    ([Id], [CategoryId], [AttributeId], [AffectsPricing], [IsVariant], [AffectsStock], [IsRequired], [DisplayOrder], [SortOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @MenClothingCatId, @ColorAttributeId, 0, 1, 0, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @MenClothingCatId, @SizeAttributeId, 0, 1, 0, 1, 2, 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @MenClothingCatId, @MaterialAttributeId, 0, 1, 0, 1, 3, 3, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Women Clothing Category Attributes
INSERT INTO [dbo].[TbCategoryAttributes]
    ([Id], [CategoryId], [AttributeId], [AffectsPricing], [IsVariant], [AffectsStock], [IsRequired], [DisplayOrder], [SortOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @WomenClothingCatId, @ColorAttributeId, 0, 1, 0, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @WomenClothingCatId, @SizeAttributeId, 0, 1, 0, 1, 2, 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @WomenClothingCatId, @MaterialAttributeId, 0, 1, 0, 1, 3, 3, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Shoes Category Attributes
INSERT INTO [dbo].[TbCategoryAttributes]
    ([Id], [CategoryId], [AttributeId], [AffectsPricing], [IsVariant], [AffectsStock], [IsRequired], [DisplayOrder], [SortOrder], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @ShoesCatId, @ColorAttributeId, 0, 1, 0, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ShoesCatId, @SizeAttributeId, 0, 1, 0, 1, 2, 2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ShoesCatId, @MaterialAttributeId, 0, 1, 0, 1, 3, 3, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 5. INSERT DIVERSIFIED ITEMS
-- ============================================
PRINT 'Inserting diversified items for testing all filters...'

-- Get existing brand IDs
DECLARE @SamsungBrandId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbBrands WHERE NameEn = 'Samsung');
DECLARE @AppleBrandId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbBrands WHERE NameEn = 'Apple');
DECLARE @NikeBrandId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbBrands WHERE NameEn = 'Nike');

-- Get unit IDs
DECLARE @PieceUnitId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbUnits WHERE TitleEn = 'Piece');
DECLARE @PairUnitId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbUnits WHERE TitleEn = 'Pair');

-- Item 1: Huawei P60 Pro (Smartphone with multiple variants)
DECLARE @HuaweiP60ProId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@HuaweiP60ProId, 
     N'هواوي بي 60 برو', 
     'Huawei P60 Pro',
     N'هاتف هواوي الرائد بكاميرا متطورة', 
     'Huawei flagship phone with advanced camera',
     N'هواوي بي 60 برو مع كاميرا 48 ميجابكسل وذاكرة 512 جيجا', 
     'Huawei P60 Pro with 48MP camera and 512GB storage',
     @SmartphonesCatId, @PieceUnitId, '/images/huawei-p60-pro.jpg', @HuaweiBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Huawei P60 Pro - Premium Smartphone', 
     'Buy Huawei P60 Pro with best specifications', 
     'huawei,p60,pro,smartphone,5g', 0, 1);

-- Item 2: MacBook Pro 16"" (High-end laptop)
DECLARE @MacBookPro16Id UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@MacBookPro16Id, 
     N'ماك بوك برو 16 بوصة', 
     'MacBook Pro 16""',
     N'لابتوب أبل الاحترافي', 
     'Apple professional laptop',
     N'ماك بوك برو 16 بوصة بمعالج M2 Pro وذاكرة 32 جيجا', 
     'MacBook Pro 16"" with M2 Pro chip and 32GB RAM',
     @LaptopsCatId, @PieceUnitId, '/images/macbook-pro-16.jpg', @AppleBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'MacBook Pro 16"" - Professional Laptop', 
     'Buy MacBook Pro 16"" for professional work', 
     'macbook,pro,16,apple,laptop', 0, 1);

-- Item 3: Adidas Ultraboost (Shoes with size variants)
DECLARE @AdidasUltraboostId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@AdidasUltraboostId, 
     N'أديداس أولترابوست', 
     'Adidas Ultraboost',
     N'حذاء رياضي مريح', 
     'Comfortable athletic shoes',
     N'أديداس أولترابوست للحذاء الرياضي مع تقنية بوست', 
     'Adidas Ultraboost running shoes with Boost technology',
     @ShoesCatId, @PairUnitId, '/images/adidas-ultraboost.jpg', @AdidasBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Adidas Ultraboost - Running Shoes', 
     'Buy Adidas Ultraboost for maximum comfort', 
     'adidas,ultraboost,shoes,running', 0, 1);

-- Item 4: Zara Women Dress (Fashion with color variants)
DECLARE @ZaraDressId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@ZaraDressId, 
     N'فستان زارا للسيدات', 
     'Zara Women Dress',
     N'فستان أنيق للسيدات', 
     'Elegant dress for women',
     N'فستان زارا للسيدات من القطن الناعم', 
     'Zara women dress made of soft cotton',
     @WomenClothingCatId, @PieceUnitId, '/images/zara-dress.jpg', @ZaraBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Zara Women Dress - Elegant Fashion', 
     'Buy Zara women dress for special occasions', 
     'zara,dress,women,fashion', 0, 1);

-- Item 5: Samsung Galaxy Buds2 Pro (Headphones)
DECLARE @SamsungBuds2ProId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@SamsungBuds2ProId, 
     N'سماعات سامسونج جالاكسي بودز 2 برو', 
     'Samsung Galaxy Buds2 Pro',
     N'سماعات لاسلكية عالية الجودة', 
     'High-quality wireless earbuds',
     N'سماعات سامسونج جالاكسي بودز 2 برو مع إلغاء الضوضاء', 
     'Samsung Galaxy Buds2 Pro with noise cancellation',
     @HeadphonesCatId, @PieceUnitId, '/images/samsung-buds2-pro.jpg', @SamsungBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Samsung Galaxy Buds2 Pro - Wireless Earbuds', 
     'Buy Samsung Galaxy Buds2 Pro for best sound quality', 
     'samsung,buds2,pro,earbuds,wireless', 0, 1);

-- Item 6: LG French Door Refrigerator
DECLARE @LGFrenchDoorId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@LGFrenchDoorId, 
     N'ثلاجة إل جي باب فرنسي', 
     'LG French Door Refrigerator',
     N'ثلاجة كبيرة بسعة 800 لتر', 
     'Large 800L refrigerator',
     N'ثلاجة إل جي باب فرنسي بسعة 800 لتر وتقنية التبريد الذكي', 
     'LG French door refrigerator with 800L capacity and smart cooling',
     @RefrigeratorsCatId, @PieceUnitId, '/images/lg-french-door.jpg', @LGBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'LG French Door Refrigerator - Large Capacity', 
     'Buy LG French door refrigerator for your family', 
     'lg,refrigerator,french,door,800l', 0, 1);

-- Item 7: Sanyo Front Load Washing Machine
DECLARE @SanyoWashingMachineId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@SanyoWashingMachineId, 
     N'غسالة سانيو أمامية', 
     'Sanyo Front Load Washing Machine',
     N'غسالة أوتوماتيكية 10 كجم', 
     '10kg automatic washing machine',
     N'غسالة سانيو أمامية سعة 10 كجم مع تقنية التوفير في الطاقة', 
     'Sanyo front load washing machine 10kg with energy saving technology',
     @WashingMachinesCatId, @PieceUnitId, '/images/sanyo-washing-machine.jpg', @SanyoBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Sanyo Washing Machine - Front Load', 
     'Buy Sanyo washing machine for efficient laundry', 
     'sanyo,washing,machine,front,load,10kg', 0, 1);

-- Item 8: Nike Men T-Shirt (Clothing with size/color variants)
DECLARE @NikeTShirtId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbItems]
    ([Id], [TitleAr], [TitleEn], [ShortDescriptionAr], [ShortDescriptionEn], [DescriptionAr], [DescriptionEn], 
     [CategoryId], [UnitId], [ThumbnailImage], [BrandId], [VisibilityScope], [CreatedBy], [CreatedDateUtc],
     [SEOTitle], [SEODescription], [SEOMetaTags], [IsDeleted], [IsActive])
VALUES
    (@NikeTShirtId, 
     N'تي شيرت نايكي للرجال', 
     'Nike Men T-Shirt',
     N'تي شيرت رياضي مريح', 
     'Comfortable athletic t-shirt',
     N'تي شيرت نايكي للرجال من القطن عالي الجودة', 
     'Nike men t-shirt made of high-quality cotton',
     @MenClothingCatId, @PieceUnitId, '/images/nike-tshirt.jpg', @NikeBrandId, 1,
     'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(),
     'Nike Men T-Shirt - Athletic Wear', 
     'Buy Nike men t-shirt for sports and casual wear', 
     'nike,t-shirt,men,cotton,sports', 0, 1);

-- ============================================
-- 6. INSERT ITEM ATTRIBUTES
-- ============================================
PRINT 'Inserting item attributes...'

-- Huawei P60 Pro Attributes
INSERT INTO [dbo].[TbItemAttributes]
    ([Id], [ItemId], [AttributeId], [Value], [DisplayOrder], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @HuaweiP60ProId, @ColorAttributeId, 'أسود', 1, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @HuaweiP60ProId, @ColorAttributeId, 'أبيض', 2, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @HuaweiP60ProId, @StorageAttributeId, '256 جيجا', 3, N'سعة التخزين', 'Storage', 0, 1, 20, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @HuaweiP60ProId, @StorageAttributeId, '512 جيجا', 4, N'سعة التخزين', 'Storage', 0, 1, 20, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Adidas Ultraboost Attributes
INSERT INTO [dbo].[TbItemAttributes]
    ([Id], [ItemId], [AttributeId], [Value], [DisplayOrder], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @AdidasUltraboostId, @ColorAttributeId, 'أسود', 1, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasUltraboostId, @ColorAttributeId, 'أبيض', 2, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasUltraboostId, @SizeAttributeId, 'كبير', 3, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasUltraboostId, @SizeAttributeId, 'كبير جداً', 4, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasUltraboostId, @MaterialAttributeId, 'جلد', 5, N'المادة', 'Material', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Zara Dress Attributes
INSERT INTO [dbo].[TbItemAttributes]
    ([Id], [ItemId], [AttributeId], [Value], [DisplayOrder], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @ZaraDressId, @ColorAttributeId, 'أحمر', 1, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ZaraDressId, @ColorAttributeId, 'أزرق', 2, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ZaraDressId, @SizeAttributeId, 'صغير', 3, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ZaraDressId, @SizeAttributeId, 'متوسط', 4, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ZaraDressId, @MaterialAttributeId, 'قطن', 5, N'المادة', 'Material', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Nike T-Shirt Attributes
INSERT INTO [dbo].[TbItemAttributes]
    ([Id], [ItemId], [AttributeId], [Value], [DisplayOrder], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @NikeTShirtId, @ColorAttributeId, 'أبيض', 1, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @NikeTShirtId, @ColorAttributeId, 'أسود', 2, N'اللون', 'Color', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @NikeTShirtId, @SizeAttributeId, 'متوسط', 3, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @NikeTShirtId, @SizeAttributeId, 'كبير', 4, N'المقاس', 'Size', 0, 1, 10, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @NikeTShirtId, @MaterialAttributeId, 'قطن', 5, N'المادة', 'Material', 0, 1, 50, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 7. SIMPLIFIED TEST DATA INSERTION
-- ============================================
PRINT 'Inserting simplified test data for offers...'

-- Get vendor IDs
DECLARE @Vendor1Id UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbVendors WHERE VendorCode = 'VEN001');
DECLARE @Vendor2Id UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbVendors WHERE VendorCode = 'VEN002');

-- Insert offer conditions if they don't exist
IF NOT EXISTS (SELECT 1 FROM TbOfferConditions WHERE NameEn = 'New')
BEGIN
    DECLARE @NewConditionId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [dbo].[TbOfferConditions]
        ([Id], [NameAr], [NameEn], [IsNew], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
        (@NewConditionId, N'جديد', 'New', 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);
END

IF NOT EXISTS (SELECT 1 FROM TbOfferConditions WHERE NameEn = 'Used')
BEGIN
    DECLARE @UsedConditionId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [dbo].[TbOfferConditions]
        ([Id], [NameAr], [NameEn], [IsNew], [CreatedBy], [CreatedDateUtc], [IsDeleted])
    VALUES
        (@UsedConditionId, N'مستعمل', 'Used', 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);
END

-- Get condition IDs
DECLARE @NewConditionId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE NameEn = 'New');
DECLARE @UsedConditionId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbOfferConditions WHERE NameEn = 'Used');

-- Create item combinations
DECLARE @HuaweiBlack256Id UNIQUEIDENTIFIER = NEWID();
DECLARE @AdidasBlack10Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbItemCombinations]
    ([Id], [ItemId], [Barcode], [SKU], [IsDefault], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@HuaweiBlack256Id, @HuaweiP60ProId, 'HWP60P256BLK2', 'HUAWEI-P60-PRO-256-BLACK-2', 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (@AdidasBlack10Id, @AdidasUltraboostId, 'ADUBLK102', 'ADIDAS-ULTRABOOST-BLACK-10-2', 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Create offers
DECLARE @Offer1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Offer2Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO [dbo].[TbOffers]
    ([Id], [ItemId], [HandlingTimeInDays], [VisibilityScope], [WarrantyId], [CreatedBy], [CreatedDateUtc], [VendorId], [FulfillmentType], [IsDeleted], [IsBuyBoxWinner], [VendorSalesCountForThisItem], [IsFreeShipping])
VALUES
    (@Offer1Id, @HuaweiP60ProId, 2, 0, NULL, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), @Vendor1Id, 0, 0, 1, 150, 1),
    (@Offer2Id, @AdidasUltraboostId, 1, 0, NULL, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), @Vendor2Id, 0, 0, 0, 320, 1);

-- Insert offer combination pricings
INSERT INTO [dbo].[TbOfferCombinationPricings]
    ([Id], [ItemCombinationId], [OfferId], [Price], [SalesPrice], [CostPrice], [AvailableQuantity], [ReservedQuantity], [RefundedQuantity], [DamagedQuantity], 
     [InTransitQuantity], [ReturnedQuantity], [LockedQuantity], [StockStatus], [MinOrderQuantity], [MaxOrderQuantity], [LowStockThreshold], 
     [LastStockUpdate], [OfferConditionId], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @HuaweiBlack256Id, @Offer1Id, 3499.99, 3299.99, 2800.00, 25, 3, 0, 0, 2, 1, 0, 1, 1, 5, 5, GETUTCDATE(), @NewConditionId2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasBlack10Id, @Offer2Id, 699.99, 599.99, 450.00, 50, 2, 0, 0, 5, 2, 0, 1, 1, 10, 10, GETUTCDATE(), @NewConditionId2, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- Insert shipping details
DECLARE @CairoCityId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbCities WHERE TitleEn = 'Cairo');
DECLARE @RiyadhCityId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM TbCities WHERE TitleEn = 'Riyadh');

INSERT INTO [dbo].[TbShippingDetails]
    ([Id], [OfferId], [CityId], [ShippingCost], [ShippingMethod], [MinimumEstimatedDays], [MaximumEstimatedDays], [IsCODSupported], [FulfillmentType], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @Offer1Id, @CairoCityId, 0.00, 1, 2, 4, 1, 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @Offer2Id, @RiyadhCityId, 49.99, 2, 5, 10, 1, 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 8. INSERT PRODUCT VISIBILITY RULES
-- ============================================
PRINT 'Inserting product visibility rules...'

INSERT INTO [dbo].[TbProductVisibilityRules]
    ([Id], [ItemId], [VisibilityStatus], [HasActiveOffers], [HasStock], [IsApproved], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (NEWID(), @HuaweiP60ProId, 1, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @AdidasUltraboostId, 1, 1, 1, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0),
    (NEWID(), @ZaraDressId, 2, 0, 0, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

-- ============================================
-- 9. INSERT FLASH SALES
-- ============================================
PRINT 'Inserting flash sales...'

DECLARE @FlashSaleId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbFlashSales]
    ([Id], [TitleEn], [TitleAr], [StartDate], [EndDate], [DurationInHours], [MinimumDiscountPercentage], [MinimumSellerRating], [IsActive], [ShowCountdownTimer], [DisplayOrder], [TotalSales], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@FlashSaleId, 'Tech Flash Sale 2', N'عرض التكنولوجيا الخاطف 2', DATEADD(HOUR, -2, GETUTCDATE()), DATEADD(HOUR, 22, GETUTCDATE()), 24, 15.00, 4.0, 1, 1, 1, 0, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbFlashSaleProducts]
    ([Id], [FlashSaleId], [ItemId], [VendorId], [OriginalPrice], [FlashSalePrice], [DiscountPercentage], [AvailableQuantity], [SoldQuantity], [IsActive], [DisplayOrder], [ViewCount], [AddToCartCount], [CreatedBy], [CreatedDateUtc], [IsDeleted])
SELECT
    NEWID(),
    @FlashSaleId,
    @HuaweiP60ProId,
    @Vendor1Id,
    3499.99,
    2999.99,
    14.29,
    10,
    2,
    1,
    1,
    150,
    25,
    'b8a9963f-f520-41e2-ada0-9054747da9cb',
    GETUTCDATE(),
    0;

-- ============================================
-- 10. INSERT HOMEPAGE BLOCKS
-- ============================================
PRINT 'Inserting homepage blocks...'

DECLARE @FeaturedBlockId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [dbo].[TbHomepageBlocks]
    ([Id], [TitleEn], [TitleAr], [BlockType], [DisplayOrder], [IsActive], [IsVisible], [MaxItemsToDisplay], [ShowViewAllLink], [CreatedBy], [CreatedDateUtc], [IsDeleted])
VALUES
    (@FeaturedBlockId, 'Featured Products 2', N'منتجات مميزة 2', 1, 1, 1, 1, 10, 1, 'b8a9963f-f520-41e2-ada0-9054747da9cb', GETUTCDATE(), 0);

INSERT INTO [dbo].[TbBlockProducts]
    ([Id], [HomepageBlockId], [ItemId], [DisplayOrder], [IsActive], [IsFeatured], [BadgeText], [BadgeColor], [CreatedBy], [CreatedDateUtc], [IsDeleted])
SELECT
    NEWID(),
    @FeaturedBlockId,
    @HuaweiP60ProId,
    1,
    1,
    1,
    N'الأفضل مبيعاً',
    N'#FF0000',
    'b8a9963f-f520-41e2-ada0-9054747da9cb',
    GETUTCDATE(),
    0;

PRINT 'Migration completed successfully!';

COMMIT TRANSACTION
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
