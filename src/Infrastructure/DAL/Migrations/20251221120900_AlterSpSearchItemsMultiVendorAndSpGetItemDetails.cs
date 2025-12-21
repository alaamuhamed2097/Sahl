using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterSpSearchItemsMultiVendorAndSpGetItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SpGetItemDetails
            migrationBuilder.Sql(@"-- =============================================
-- Get Item Details - MATCHES SEARCH RESULT PRICE
-- Default combination uses same logic as search:
-- 1. BuyBoxWinner first
-- 2. Lowest price second
-- 3. Fastest delivery third
-- =============================================
CREATE OR ALTER       PROCEDURE [dbo].[SpGetItemDetails]
    @ItemCombinationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
	DECLARE @ItemId UNIQUEIDENTIFIER;

	SELECT TOP 1
		@ItemId = i.Id
	FROM TbItems i
	INNER JOIN TbItemCombinations ic ON i.Id = ic.ItemId AND ic.Id = @ItemCombinationId
	WHERE i.IsActive = 1
	  AND i.IsDeleted = 0
	  AND ic.IsDeleted = 0 ;

	IF @ItemId IS NULL
	BEGIN
		RAISERROR ('Item not found', 16, 1);
		RETURN;
	END
    
    -- Get item category and pricing system
    DECLARE @CategoryId UNIQUEIDENTIFIER;
    DECLARE @PricingSystemType INT;
    DECLARE @HasCombinations BIT;
    
    SELECT 
        @CategoryId = i.CategoryId,
        @PricingSystemType = c.PricingSystemType,
        @HasCombinations = CASE 
            WHEN c.PricingSystemType IN (1, 3) THEN 1
            ELSE 0
        END
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    WHERE i.Id = @ItemId;
    
    -- =============================================
    -- Find Default Combination (SAME AS SEARCH!)
    -- =============================================
    DECLARE @DefaultCombinationId UNIQUEIDENTIFIER;
    DECLARE @DefaultOfferId UNIQUEIDENTIFIER;
    
    IF @HasCombinations = 1
    BEGIN
        -- Get the combination that matches search result price
        SELECT TOP 1
            @DefaultCombinationId = ISNULL(@ItemCombinationId, ic.Id),
            @DefaultOfferId = o.Id
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
        INNER JOIN TbItemCombinations ic ON ocp.ItemCombinationId = ic.Id
        WHERE i.Id = @ItemId
            AND i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND ocp.IsDeleted = 0
            AND ocp.AvailableQuantity > 0
            AND ocp.StockStatus = 1
            AND ic.IsDeleted = 0
        ORDER BY
            ocp.IsBuyBoxWinner DESC,      -- ← Priority 1: BuyBox Winner
            ocp.SalesPrice ASC,         -- ← Priority 2: Lowest Price
            o.HandlingTimeInDays ASC;   -- ← Priority 3: Fastest Delivery
    END
    ELSE
    BEGIN
        -- For items without combinations
        SELECT TOP 1
            @DefaultOfferId = o.Id
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
        WHERE i.Id = @ItemId
            AND i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND ocp.IsDeleted = 0
            AND ocp.AvailableQuantity > 0
            AND ocp.StockStatus = 1
        ORDER BY
            ocp.IsBuyBoxWinner DESC,
            ocp.SalesPrice ASC,
            o.HandlingTimeInDays ASC;
    END
    
    -- =============================================
    -- Single Result Set with All Data
    -- =============================================
    SELECT 
        -- ========== Basic Item Info ==========
        i.Id,
        i.TitleAr,
        i.TitleEn,
        i.DescriptionAr,
        i.DescriptionEn,
        i.ShortDescriptionAr,
        i.ShortDescriptionEn,
        i.ThumbnailImage,
        i.CategoryId,
        c.TitleAr AS CategoryNameAr,
        c.TitleEn AS CategoryNameEn,
        c.PricingSystemType,
        ps.SystemNameAr AS PricingSystemNameAr,
        ps.SystemNameEn AS PricingSystemNameEn,
        i.BrandId,
        b.NameAr AS BrandNameAr,
        b.NameEn AS BrandNameEn,
        b.LogoPath AS BrandLogoUrl,
        @HasCombinations AS HasCombinations,
        ISNULL(i.AverageRating, 0) AS AverageRating,
        
        -- Check if multi-vendor
        CASE WHEN (
            SELECT COUNT(DISTINCT o.VendorId)
            FROM TbOffers o
            WHERE o.ItemId = @ItemId 
                AND o.VisibilityScope = 1 
                AND o.IsDeleted = 0
        ) > 1 THEN CAST(1 AS bit)
			  ELSE CAST(0 AS bit) END AS IsMultiVendor,
        
        -- ========== General Images JSON ==========
        (
            SELECT 
                img.Path AS ImageUrl,
                img.[Order] AS DisplayOrder,
                CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
            FROM TbItemImages img
            WHERE img.ItemId = @ItemId 
                AND img.IsDeleted = 0
            ORDER BY img.[Order]
            FOR JSON PATH
        ) AS GeneralImagesJson,
        
        -- ========== ALL Item Attributes JSON ==========
        (
            SELECT 
                a.Id AS AttributeId,
                a.TitleAr AS NameAr,
                a.TitleEn AS NameEn,
                a.FieldType,
                ia.DisplayOrder,
				ia.Value
            FROM TbItemAttributes ia
            INNER JOIN TbAttributes a ON ia.AttributeId = a.Id
            WHERE ia.ItemId = @ItemId
                AND ia.IsDeleted = 0
                AND a.IsDeleted = 0
            ORDER BY ia.DisplayOrder
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- ========== Default Combination JSON ==========
        CASE WHEN @HasCombinations = 1 AND @DefaultCombinationId IS NOT NULL THEN
            (
                SELECT 
                    ic.Id AS CombinationId,
                    ic.SKU,
					ic.Barcode,
					ic.IsDefault,
                    -- Selected pricing attributes
                    (
                        SELECT 
                            a.Id AS AttributeId,
                            a.TitleAr AS AttributeNameAr,
                            a.TitleEn AS AttributeNameEn,
                            cav.Id AS CombinationValueId,
                            cav.Value 
                        FROM TbCombinationAttributesValues cav
                        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id
                        INNER JOIN TbCategoryAttributes ca ON a.Id = ca.AttributeId AND ca.CategoryId = @CategoryId
                        WHERE cav.ItemCombinationId = ic.Id
                        ORDER BY ca.DisplayOrder
                        FOR JSON PATH
                    ) AS SelectedAttributesJson,
                    -- Combination-specific images
                    (
                        SELECT 
                            img.Path AS ImageUrl,
                            img.[Order] AS DisplayOrder,
                            CASE WHEN img.[Order] = 1 THEN CAST( 1 AS bit) ELSE CAST( 0 AS bit) END AS IsDefault
                        FROM TbItemCombinationImages img
                        WHERE img.ItemCombinationId = ic.Id
                            AND img.IsDeleted = 0
                        ORDER BY img.[Order]
                        FOR JSON PATH
                    ) AS ImagesJson
                FROM TbItemCombinations ic
                WHERE ic.Id = @DefaultCombinationId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS DefaultCombinationJson,
        
        -- ========== Pricing JSON (for default offer) ==========
        CASE WHEN @DefaultOfferId IS NOT NULL THEN
            (
                SELECT 
                    -- Count vendors for this item/combination
                    (
                        SELECT COUNT(DISTINCT o2.VendorId)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS VendorCount,
                    
                    -- Price range
                    (
                        SELECT ISNULL(MIN(ocp2.SalesPrice),0.0)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS MinPrice,
                    
                    (
                        SELECT ISNULL(MAX(ocp2.SalesPrice),0.0)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS MaxPrice,
                    
                    -- Best offer details (default offer)
                    (
                        SELECT 
                            o.Id AS OfferId,
                            o.VendorId,
                            v.CompanyName AS VendorName,
                            ISNULL(v.Rating, 0) AS VendorRating,
                            ocp.Price,
                            ocp.SalesPrice,
                            CASE 
                                WHEN ocp.Price > ocp.SalesPrice AND ocp.Price > 0 
                                THEN CAST(((ocp.Price - ocp.SalesPrice) / ocp.Price * 100) AS DECIMAL(5,2))
                                ELSE 0 
                            END AS DiscountPercentage,
                            ocp.AvailableQuantity,
                            ocp.StockStatus,
                            o.IsFreeShipping,
                            o.HandlingTimeInDays AS EstimatedDeliveryDays,
                            ocp.IsBuyBoxWinner,
                            ocp.MinOrderQuantity,
                            ocp.MaxOrderQuantity,
                            -- Quantity tiers if applicable
                            CASE 
                                WHEN @PricingSystemType IN (2, 3) THEN (
                                    SELECT 
                                        qp.MinimumQuantity,
                                        qp.MaximumQuantity,
                                        qp.UnitPrice
                                    FROM TbQuantityPricings qp
                                    WHERE qp.OfferId = o.Id
                                        AND qp.IsDeleted = 0
                                    ORDER BY qp.MinimumQuantity
                                    FOR JSON PATH
                                )
                                ELSE NULL
                            END AS QuantityTiersJson
                        FROM TbOffers o
                        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
                        LEFT JOIN TbVendors v ON o.VendorId = v.Id
                        WHERE o.Id = @DefaultOfferId
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS BestOfferJson
                    
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS PricingJson
        
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    LEFT JOIN TbPricingSystemSettings ps ON c.PricingSystemId = ps.Id
    LEFT JOIN TbBrands b ON i.BrandId = b.Id
    WHERE i.Id = @ItemId;
    
END
GO");
            // SpSearchItemsMultiVendor
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
    -- Search Context Parameters
    @SearchTerm NVARCHAR(255) = NULL,      -- من البحث في الصفحة الرئيسية
    @CategoryId UNIQUEIDENTIFIER = NULL,   -- لما يفتح كاتيجوري معين
    @VendorId UNIQUEIDENTIFIER = NULL,     -- لما يفتح منتجات بائع معين
    @BrandId UNIQUEIDENTIFIER = NULL,      -- لما يفتح براند معين
    
    -- Filter Parameters
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinItemRating DECIMAL(3,2) = NULL,
    @InStockOnly BIT = 0,
    @FreeShippingOnly BIT = 0,
    @ConditionId UNIQUEIDENTIFIER = NULL,
    @WithWarrantyOnly BIT = 0,
    @AttributeIds NVARCHAR(MAX) = NULL,
    @AttributeValues NVARCHAR(MAX) = NULL,
    
    -- Sorting & Pagination
    @SortBy NVARCHAR(50) = 'relevance',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- Temporary table for attribute filters
    DECLARE @AttributeIdTable TABLE (Id UNIQUEIDENTIFIER, ValueIds NVARCHAR(MAX));

    -- Parse attribute filters
    IF @AttributeIds IS NOT NULL AND @AttributeIds != ''
    BEGIN
        DECLARE @AttrIdList TABLE (RowNum INT, AttributeId UNIQUEIDENTIFIER);
        DECLARE @AttrValueList TABLE (RowNum INT, ValueIds NVARCHAR(MAX));
        
        INSERT INTO @AttrIdList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@AttributeIds, ',')
        WHERE value != '';
        
        INSERT INTO @AttrValueList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            value
        FROM STRING_SPLIT(@AttributeValues, '|')
        WHERE value != '';
        
        INSERT INTO @AttributeIdTable
        SELECT a.AttributeId, v.ValueIds
        FROM @AttrIdList a
        INNER JOIN @AttrValueList v ON a.RowNum = v.RowNum;
    END

    -- =============================
    -- Main Search Query
    -- =============================
    ;WITH ItemBase AS (
        SELECT
            i.Id AS ItemId,
            i.TitleAr,
            i.TitleEn,
            i.ShortDescriptionAr,
            i.ShortDescriptionEn,
            i.CategoryId,
            i.BrandId,
            b.NameAr AS BrandNameAr,
            b.NameEn AS BrandNameEn,
            i.ThumbnailImage,
            i.CreatedDateUtc,
            i.AverageRating,
            
            -- Offer data
            o.Id AS OfferId,
            o.IsFreeShipping,
            o.HandlingTimeInDays,
            
            -- Pricing data
            p.ItemCombinationId,
            p.IsBuyBoxWinner,
            p.SalesPrice,
            p.Price,
            p.AvailableQuantity,
            p.StockStatus
            
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        LEFT JOIN TbBrands b ON i.BrandId = b.Id AND b.IsDeleted = 0
        WHERE
            -- Active items only
            i.IsActive = 1 AND i.IsDeleted = 0
            
            -- Active offers only (VisibilityScope: 1 = Active/Public)
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            
            -- Active pricing only
            AND p.IsDeleted = 0
            
            -- Stock Status Filter (InStock=1, OutOfStock=2, LimitedStock=3, ComingSoon=4)
            AND p.StockStatus IN (1, 2, 3, 4)
            
            -- Text search filter (Case-Insensitive for English, Accent-Insensitive for Arabic)
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.TitleEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
                OR i.ShortDescriptionAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.ShortDescriptionEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
            )
            
            -- Context filters
            AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
            AND (@BrandId IS NULL OR i.BrandId = @BrandId)
            
            -- Additional filters
            AND (@ConditionId IS NULL OR p.OfferConditionId = @ConditionId)
            AND (@WithWarrantyOnly = 0 OR o.WarrantyId IS NOT NULL)
            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
            AND (@MinItemRating IS NULL OR i.AverageRating >= @MinItemRating)
            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
            AND (@FreeShippingOnly = 0 OR o.IsFreeShipping = 1)
    ),
    
    -- Filter by attributes (if specified)
    AttributeFiltered AS (
        SELECT DISTINCT ib.*
        FROM ItemBase ib
        WHERE 
            NOT EXISTS (SELECT 1 FROM @AttributeIdTable)
            OR
            NOT EXISTS (
                SELECT 1 
                FROM @AttributeIdTable attr
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM TbCombinationAttributesValues cav
                    WHERE cav.ItemCombinationId = ib.ItemCombinationId
                        AND cav.AttributeId = attr.Id
                        AND cav.Id IN (
                            SELECT CAST(value AS UNIQUEIDENTIFIER)
                            FROM STRING_SPLIT(attr.ValueIds, ',')
                        )
                )
            )
    ),
    
    -- Select BuyBox Winner or Best Offer per Item
    BestOffers AS (
        SELECT
            ItemId,
			ItemCombinationId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            SalesPrice,
            Price,
            AvailableQuantity,
            StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping,
            HandlingTimeInDays,
            -- Prioritize BuyBox winner, then lowest price, then fastest shipping
            ROW_NUMBER() OVER (
                PARTITION BY ItemId 
                ORDER BY 
                    IsBuyBoxWinner DESC,
                    SalesPrice ASC,
                    HandlingTimeInDays ASC
            ) AS OfferRank
        FROM AttributeFiltered
    ),
    
    -- Get only the best offer per item
    SelectedOffers AS (
        SELECT
            ItemId,
			ItemCombinationId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            Price,
            SalesPrice,
            AvailableQuantity,
            CASE StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping
        FROM BestOffers
        WHERE OfferRank = 1
    ),
    
    -- Apply sorting and ranking
    RankedItems AS (
        SELECT
            *,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN @SortBy = 'price_asc' THEN SalesPrice ELSE 99999999 END ASC,
                    CASE WHEN @SortBy = 'price_desc' THEN SalesPrice ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'newest arrival' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'best seller' THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'customer review' THEN ISNULL(AverageRating, 0) ELSE 0 END DESC,
                    CASE WHEN @SortBy IN ('relevance', 'Featured') THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    ItemId
            ) AS RowNum
        FROM SelectedOffers
    )
    
    -- Return paginated results with total count
    SELECT 
        ItemId,
		ItemCombinationId,
        TitleAr,
        TitleEn,
        ShortDescriptionAr,
        ShortDescriptionEn,
        CategoryId,
        BrandId,
        BrandNameAr,
        BrandNameEn,
        ThumbnailImage,
        CreatedDateUtc,
        AverageRating,
        Price,
        SalesPrice,
        AvailableQuantity,
        StockStatus,
        IsFreeShipping,
        (SELECT COUNT(*) FROM SelectedOffers) as TotalRecords
    FROM RankedItems
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum;

END

GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // SpGetItemDetails
            migrationBuilder.Sql(@"-- =============================================
-- Get Item Details - MATCHES SEARCH RESULT PRICE
-- Default combination uses same logic as search:
-- 1. BuyBoxWinner first
-- 2. Lowest price second
-- 3. Fastest delivery third
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[SpGetItemDetails]
    @ItemId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if item exists
    IF NOT EXISTS (
        SELECT 1 FROM TbItems 
        WHERE Id = @ItemId AND IsActive = 1 AND IsDeleted = 0
    )
    BEGIN
        RAISERROR('Item not found', 16, 1);
        RETURN;
    END
    
    -- Get item category and pricing system
    DECLARE @CategoryId UNIQUEIDENTIFIER;
    DECLARE @PricingSystemType INT;
    DECLARE @HasCombinations BIT;
    
    SELECT 
        @CategoryId = i.CategoryId,
        @PricingSystemType = c.PricingSystemType,
        @HasCombinations = CASE 
            WHEN c.PricingSystemType IN (1, 3) THEN 1
            ELSE 0
        END
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    WHERE i.Id = @ItemId;
    
    -- =============================================
    -- Find Default Combination (SAME AS SEARCH!)
    -- =============================================
    DECLARE @DefaultCombinationId UNIQUEIDENTIFIER;
    DECLARE @DefaultOfferId UNIQUEIDENTIFIER;
    
    IF @HasCombinations = 1
    BEGIN
        -- Get the combination that matches search result price
        SELECT TOP 1
            @DefaultCombinationId = ic.Id,
            @DefaultOfferId = o.Id
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
        INNER JOIN TbItemCombinations ic ON ocp.ItemCombinationId = ic.Id
        WHERE i.Id = @ItemId
            AND i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND ocp.IsDeleted = 0
            AND ocp.AvailableQuantity > 0
            AND ocp.StockStatus = 1
            AND ic.IsDeleted = 0
        ORDER BY
            o.IsBuyBoxWinner DESC,      -- ← Priority 1: BuyBox Winner
            ocp.SalesPrice ASC,         -- ← Priority 2: Lowest Price
            o.HandlingTimeInDays ASC;   -- ← Priority 3: Fastest Delivery
    END
    ELSE
    BEGIN
        -- For items without combinations
        SELECT TOP 1
            @DefaultOfferId = o.Id
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
        WHERE i.Id = @ItemId
            AND i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND ocp.IsDeleted = 0
            AND ocp.AvailableQuantity > 0
            AND ocp.StockStatus = 1
        ORDER BY
            o.IsBuyBoxWinner DESC,
            ocp.SalesPrice ASC,
            o.HandlingTimeInDays ASC;
    END
    
    -- =============================================
    -- Single Result Set with All Data
    -- =============================================
    SELECT 
        -- ========== Basic Item Info ==========
        i.Id,
        i.TitleAr,
        i.TitleEn,
        i.DescriptionAr,
        i.DescriptionEn,
        i.ShortDescriptionAr,
        i.ShortDescriptionEn,
        i.ThumbnailImage,
        i.CategoryId,
        c.TitleAr AS CategoryNameAr,
        c.TitleEn AS CategoryNameEn,
        c.PricingSystemType,
        ps.SystemNameAr AS PricingSystemNameAr,
        ps.SystemNameEn AS PricingSystemNameEn,
        i.BrandId,
        b.NameAr AS BrandNameAr,
        b.NameEn AS BrandNameEn,
        b.LogoPath AS BrandLogoUrl,
        @HasCombinations AS HasCombinations,
        ISNULL(i.AverageRating, 0) AS AverageRating,
        
        -- Check if multi-vendor
        CASE WHEN (
            SELECT COUNT(DISTINCT o.VendorId)
            FROM TbOffers o
            WHERE o.ItemId = @ItemId 
                AND o.VisibilityScope = 1 
                AND o.IsDeleted = 0
        ) > 1 THEN CAST(1 AS bit)
			  ELSE CAST(0 AS bit) END AS IsMultiVendor,
        
        -- ========== General Images JSON ==========
        (
            SELECT 
                img.Path AS ImageUrl,
                img.[Order] AS DisplayOrder,
                CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
            FROM TbItemImages img
            WHERE img.ItemId = @ItemId 
                AND img.IsDeleted = 0
            ORDER BY img.[Order]
            FOR JSON PATH
        ) AS GeneralImagesJson,
        
        -- ========== ALL Item Attributes JSON ==========
        (
            SELECT 
                a.Id AS AttributeId,
                a.TitleAr AS NameAr,
                a.TitleEn AS NameEn,
                a.FieldType,
                ia.DisplayOrder,
				ia.Value
            FROM TbItemAttributes ia
            INNER JOIN TbAttributes a ON ia.AttributeId = a.Id
            WHERE ia.ItemId = @ItemId
                AND ia.IsDeleted = 0
                AND a.IsDeleted = 0
            ORDER BY ia.DisplayOrder
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- ========== Default Combination JSON ==========
        CASE WHEN @HasCombinations = 1 AND @DefaultCombinationId IS NOT NULL THEN
            (
                SELECT 
                    ic.Id AS CombinationId,
                    ic.SKU,
					ic.Barcode,
					ic.IsDefault,
                    -- Selected pricing attributes
                    (
                        SELECT 
                            a.Id AS AttributeId,
                            a.TitleAr AS AttributeNameAr,
                            a.TitleEn AS AttributeNameEn,
                            cav.Id AS CombinationValueId,
                            cav.Value 
                        FROM TbCombinationAttributesValues cav
                        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id
                        INNER JOIN TbCategoryAttributes ca ON a.Id = ca.AttributeId AND ca.CategoryId = @CategoryId
                        WHERE cav.ItemCombinationId = ic.Id
                        ORDER BY ca.DisplayOrder
                        FOR JSON PATH
                    ) AS SelectedAttributesJson,
                    -- Combination-specific images
                    (
                        SELECT 
                            img.Path AS ImageUrl,
                            img.[Order] AS DisplayOrder,
                            CASE WHEN img.[Order] = 1 THEN CAST( 1 AS bit) ELSE CAST( 0 AS bit) END AS IsDefault
                        FROM TbItemCombinationImages img
                        WHERE img.ItemCombinationId = ic.Id
                            AND img.IsDeleted = 0
                        ORDER BY img.[Order]
                        FOR JSON PATH
                    ) AS ImagesJson
                FROM TbItemCombinations ic
                WHERE ic.Id = @DefaultCombinationId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS DefaultCombinationJson,
        
        -- ========== Pricing JSON (for default offer) ==========
        CASE WHEN @DefaultOfferId IS NOT NULL THEN
            (
                SELECT 
                    -- Count vendors for this item/combination
                    (
                        SELECT COUNT(DISTINCT o2.VendorId)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS VendorCount,
                    
                    -- Price range
                    (
                        SELECT ISNULL(MIN(ocp2.SalesPrice),0.0)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS MinPrice,
                    
                    (
                        SELECT ISNULL(MAX(ocp2.SalesPrice),0.0)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS MaxPrice,
                    
                    -- Best offer details (default offer)
                    (
                        SELECT 
                            o.Id AS OfferId,
                            o.VendorId,
                            v.CompanyName AS VendorName,
                            ISNULL(v.Rating, 0) AS VendorRating,
                            ocp.Price,
                            ocp.SalesPrice,
                            CASE 
                                WHEN ocp.Price > ocp.SalesPrice AND ocp.Price > 0 
                                THEN CAST(((ocp.Price - ocp.SalesPrice) / ocp.Price * 100) AS DECIMAL(5,2))
                                ELSE 0 
                            END AS DiscountPercentage,
                            ocp.AvailableQuantity,
                            ocp.StockStatus,
                            o.IsFreeShipping,
                            o.HandlingTimeInDays AS EstimatedDeliveryDays,
                            o.IsBuyBoxWinner,
                            ocp.MinOrderQuantity,
                            ocp.MaxOrderQuantity,
                            -- Quantity tiers if applicable
                            CASE 
                                WHEN @PricingSystemType IN (2, 3) THEN (
                                    SELECT 
                                        qp.MinimumQuantity,
                                        qp.MaximumQuantity,
                                        qp.UnitPrice
                                    FROM TbQuantityPricings qp
                                    WHERE qp.OfferId = o.Id
                                        AND qp.IsDeleted = 0
                                    ORDER BY qp.MinimumQuantity
                                    FOR JSON PATH
                                )
                                ELSE NULL
                            END AS QuantityTiersJson
                        FROM TbOffers o
                        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId
                        LEFT JOIN TbVendors v ON o.VendorId = v.Id
                        WHERE o.Id = @DefaultOfferId
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS BestOfferJson
                    
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS PricingJson
        
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    LEFT JOIN TbPricingSystemSettings ps ON c.PricingSystemId = ps.Id
    LEFT JOIN TbBrands b ON i.BrandId = b.Id
    WHERE i.Id = @ItemId;
    
END
GO");
            // SpSearchItemsMultiVendor
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
    -- Parameters (same as before)
    @SearchTerm NVARCHAR(255) = NULL,
    @CategoryIds NVARCHAR(MAX) = NULL,
    @BrandIds NVARCHAR(MAX) = NULL,
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinItemRating DECIMAL(3,2) = NULL,
    @InStockOnly BIT = 0,
    @FreeShippingOnly BIT = 0,
    @VendorIds NVARCHAR(MAX) = NULL,
    @ConditionIds NVARCHAR(MAX) = NULL,
    @WithWarrantyOnly BIT = 0,
    @AttributeIds NVARCHAR(MAX) = NULL,
    @AttributeValues NVARCHAR(MAX) = NULL,
    @SortBy NVARCHAR(50) = 'relevance',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- Temporary tables
    DECLARE @CategoryIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @BrandIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @VendorIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @ConditionIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @AttributeIdTable TABLE (Id UNIQUEIDENTIFIER, ValueIds NVARCHAR(MAX));

    -- Parse category IDs
    IF @CategoryIds IS NOT NULL AND @CategoryIds != ''
    BEGIN
        INSERT INTO @CategoryIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER) 
        FROM STRING_SPLIT(@CategoryIds, ',')
        WHERE value != '';
    END

    -- Parse brand IDs
    IF @BrandIds IS NOT NULL AND @BrandIds != ''
    BEGIN
        INSERT INTO @BrandIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER) 
        FROM STRING_SPLIT(@BrandIds, ',')
        WHERE value != '';
    END

    -- Parse vendor IDs
    IF @VendorIds IS NOT NULL AND @VendorIds != ''
    BEGIN
        INSERT INTO @VendorIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@VendorIds, ',')
        WHERE value != '';
    END

    -- Parse condition IDs
    IF @ConditionIds IS NOT NULL AND @ConditionIds != ''
    BEGIN
        INSERT INTO @ConditionIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@ConditionIds, ',')
        WHERE value != '';
    END

    -- Parse attribute filters
    IF @AttributeIds IS NOT NULL AND @AttributeIds != ''
    BEGIN
        DECLARE @AttrIdList TABLE (RowNum INT, AttributeId UNIQUEIDENTIFIER);
        DECLARE @AttrValueList TABLE (RowNum INT, ValueIds NVARCHAR(MAX));
        
        INSERT INTO @AttrIdList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@AttributeIds, ',')
        WHERE value != '';
        
        INSERT INTO @AttrValueList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            value
        FROM STRING_SPLIT(@AttributeValues, '|')
        WHERE value != '';
        
        INSERT INTO @AttributeIdTable
        SELECT a.AttributeId, v.ValueIds
        FROM @AttrIdList a
        INNER JOIN @AttrValueList v ON a.RowNum = v.RowNum;
    END

    -- =============================
    -- Main Search Query
    -- =============================
    ;WITH ItemBase AS (
        SELECT
            i.Id AS ItemId,
            i.TitleAr,
            i.TitleEn,
            i.ShortDescriptionAr,
            i.ShortDescriptionEn,
            i.CategoryId,
            i.BrandId,
            i.ThumbnailImage,
            i.CreatedDateUtc,
            
            -- Offer data
            o.Id AS OfferId,
            o.VendorId,
            o.IsBuyBoxWinner,
            o.OfferConditionId,
            o.WarrantyId,
            o.IsFreeShipping,                    -- ✅ من TbOffer
            o.HandlingTimeInDays,                -- ✅ من TbOffer
            o.VendorRatingForThisItem,           -- ✅ من TbOffer
            o.FulfillmentType,                   -- ✅ من TbOffer
            
            -- Pricing data
            p.SalesPrice,
            p.Price AS OriginalPrice,
            p.AvailableQuantity,
            p.StockStatus,
            p.ItemCombinationId
            
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        WHERE
            -- Active items only
            i.IsActive = 1 AND i.IsDeleted = 0
            
            -- Active offers only (VisibilityScope: 1 = Active/Public)
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            
            -- Active pricing only
            AND p.IsDeleted = 0
            
            -- Text search filter
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr LIKE '%' + @SearchTerm + '%'
                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionAr LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
            )
            
            -- Category filter
            AND (
                NOT EXISTS (SELECT 1 FROM @CategoryIdTable)
                OR i.CategoryId IN (SELECT Id FROM @CategoryIdTable)
            )
            
            -- Brand filter
            AND (
                NOT EXISTS (SELECT 1 FROM @BrandIdTable)
                OR i.BrandId IN (SELECT Id FROM @BrandIdTable)
            )
            
            -- Vendor filter
            AND (
                NOT EXISTS (SELECT 1 FROM @VendorIdTable)
                OR o.VendorId IN (SELECT Id FROM @VendorIdTable)
            )
            
            -- Condition filter
            AND (
                NOT EXISTS (SELECT 1 FROM @ConditionIdTable)
                OR o.OfferConditionId IN (SELECT Id FROM @ConditionIdTable)
            )
            
            -- Warranty filter
            AND (@WithWarrantyOnly = 0 OR o.WarrantyId IS NOT NULL)
            
            -- Price range filter
            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
            
            -- Stock availability filter
            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
            
            -- Free shipping filter (✅ من TbOffer)
            AND (@FreeShippingOnly = 0 OR o.IsFreeShipping = 1)
    ),
    
    -- Filter by attributes (if specified)
    AttributeFiltered AS (
        SELECT DISTINCT ib.*
        FROM ItemBase ib
        WHERE 
            NOT EXISTS (SELECT 1 FROM @AttributeIdTable)
            OR
            NOT EXISTS (
                SELECT 1 
                FROM @AttributeIdTable attr
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM TbItemCombinations ic
                    INNER JOIN TbCombinationAttributes ca ON ic.Id = ca.ItemCombinationId
                    INNER JOIN TbItemAttributeValues iav ON ca.AttributeValueId = iav.Id
                    WHERE ic.ItemId = ib.ItemId
                        AND iav.AttributeId = attr.Id
                        AND iav.Id IN (
                            SELECT CAST(value AS UNIQUEIDENTIFIER)
                            FROM STRING_SPLIT(attr.ValueIds, ',')
                        )
                )
            )
    ),
    
    -- Apply scoring
    ScoredItems AS (
        SELECT
            *,
            -- Text relevance score
            CASE
                WHEN @SearchTerm IS NULL THEN 0
                WHEN TitleAr LIKE @SearchTerm + '%' OR TitleEn LIKE @SearchTerm + '%' THEN 1.0
                WHEN TitleAr LIKE '%' + @SearchTerm + '%' OR TitleEn LIKE '%' + @SearchTerm + '%' THEN 0.8
                WHEN ShortDescriptionAr LIKE '%' + @SearchTerm + '%' OR ShortDescriptionEn LIKE '%' + @SearchTerm + '%' THEN 0.5
                ELSE 0.3
            END AS TextScore,
            
            -- Buy box score
            CASE WHEN IsBuyBoxWinner = 1 THEN 1.0 ELSE 0 END AS BuyBoxScore,
            
            -- Stock score
            CASE
                WHEN AvailableQuantity >= 50 THEN 1.0
                WHEN AvailableQuantity >= 10 THEN 0.7
                WHEN AvailableQuantity > 0 THEN 0.4
                ELSE 0
            END AS StockScore,
            
            -- Price score (discount score)
            CASE
                WHEN SalesPrice < OriginalPrice THEN 1.0
                ELSE 0.5
            END AS PriceScore,
            
            -- Shipping score (✅ using HandlingTimeInDays from TbOffer)
            CASE
                WHEN IsFreeShipping = 1 THEN 1.0
                WHEN HandlingTimeInDays <= 2 THEN 0.8
                WHEN HandlingTimeInDays <= 5 THEN 0.5
                ELSE 0.3
            END AS ShippingScore,
            
            -- Vendor rating score
            CASE
                WHEN VendorRatingForThisItem >= 4.5 THEN 1.0
                WHEN VendorRatingForThisItem >= 4.0 THEN 0.8
                WHEN VendorRatingForThisItem >= 3.5 THEN 0.6
                WHEN VendorRatingForThisItem >= 3.0 THEN 0.4
                ELSE 0.2
            END AS VendorScore
        FROM AttributeFiltered
    ),
    
    -- Aggregate by item
    Aggregated AS (
        SELECT
            ItemId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            ThumbnailImage,
            CreatedDateUtc,
            MIN(SalesPrice) AS MinPrice,
            MAX(SalesPrice) AS MaxPrice,
            COUNT(DISTINCT OfferId) AS OffersCount,
            MIN(HandlingTimeInDays) AS FastestDelivery,  -- ✅ Using HandlingTimeInDays
            MAX(TextScore) AS TextScore,
            MAX(BuyBoxScore) AS BuyBoxScore,
            AVG(StockScore) AS StockScore,
            AVG(PriceScore) AS PriceScore,
            AVG(ShippingScore) AS ShippingScore,
            AVG(VendorScore) AS VendorScore,
            (
                SELECT TOP 1 
                    CONCAT(
                        OfferId, '|',
                        VendorId, '|',
                        SalesPrice, '|',
                        OriginalPrice, '|',
                        AvailableQuantity, '|',
                        CAST(IsFreeShipping AS INT), '|',
                        HandlingTimeInDays, '|',                    -- ✅ من TbOffer
                        CAST(IsBuyBoxWinner AS INT), '|',
                        FulfillmentType                              -- ✅ من TbOffer
                    )
                FROM ScoredItems si2
                WHERE si2.ItemId = si.ItemId
                ORDER BY IsBuyBoxWinner DESC, SalesPrice ASC, HandlingTimeInDays ASC
            ) AS BestOfferData
        FROM ScoredItems si
        GROUP BY 
            ItemId, TitleAr, TitleEn, ShortDescriptionAr, 
            ShortDescriptionEn, CategoryId, BrandId, 
            ThumbnailImage, CreatedDateUtc
    ),
    
    -- Calculate final score and rank
    RankedItems AS (
        SELECT
            *,
            -- Calculate final relevance score
            (
                (TextScore * 0.25) +
                (BuyBoxScore * 0.20) +
                (VendorScore * 0.15) +
                (StockScore * 0.10) +
                (PriceScore * 0.10) +
                (ShippingScore * 0.10) +
                (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
            ) AS FinalScore,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN @SortBy = 'price_asc' THEN MinPrice ELSE 99999999 END ASC,
                    CASE WHEN @SortBy = 'price_desc' THEN MaxPrice ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'newest arrival' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'best seller' THEN BuyBoxScore ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'customer review' THEN VendorScore ELSE 0 END DESC,
                    CASE WHEN @SortBy IN ('relevance', 'Featured') THEN
                        (
                            (TextScore * 0.25) +
                            (BuyBoxScore * 0.20) +
                            (VendorScore * 0.15) +
                            (StockScore * 0.10) +
                            (PriceScore * 0.10) +
                            (ShippingScore * 0.10) +
                            (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
                        )
                    ELSE 0 END DESC,
                    ItemId
            ) AS RowNum
        FROM Aggregated
    )
    
    -- Return paginated results
    SELECT 
        ItemId,
        TitleAr,
        TitleEn,
        ShortDescriptionAr,
        ShortDescriptionEn,
        CategoryId,
        BrandId,
        ThumbnailImage,
        CreatedDateUtc,
        MinPrice,
        MaxPrice,
        OffersCount,
        FastestDelivery,
        BestOfferData,
        FinalScore
    FROM RankedItems
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum;

    -- Return total count
    SELECT COUNT(*) AS TotalRecords FROM Aggregated;
END");
        }
    }
}
