using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterSpGetItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Get Item Details - MATCHES SEARCH RESULT PRICE
-- Default combination uses same logic as search:
-- 1. BuyBoxWinner first
-- 2. Lowest price second
-- 3. Fastest delivery third
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SpGetItemDetails]
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
        ) > 1 THEN 1 ELSE 0 END AS IsMultiVendor,
        
        -- ========== General Images JSON ==========
        (
            SELECT 
                img.Path AS ImageUrl,
                img.[Order] AS DisplayOrder,
                CASE WHEN img.[Order] = 1 THEN 1 ELSE 0 END AS IsDefault
            FROM TbItemImages img
            WHERE img.ItemId = @ItemId 
                AND img.IsDeleted = 0
            ORDER BY img.[Order]
            FOR JSON PATH
        ) AS GeneralImagesJson,
        
        -- ========== ALL Category Attributes JSON ==========
        (
            SELECT 
                a.Id AS AttributeId,
                a.TitleAr AS NameAr,
                a.TitleEn AS NameEn,
                a.FieldType,
                ca.DisplayOrder,
                ca.AffectsPricing,
                ca.IsRequired,
                ca.IsVariant,
                CASE 
                    WHEN ca.AffectsPricing = 1 THEN (
                        -- Pricing attributes: show as selectable options
                        SELECT 
                            ao.Id AS ValueId,
                            ao.TitleAr AS ValueAr,
                            ao.TitleEn AS ValueEn,
                            ao.DisplayOrder,
                            -- Check if available
                            CASE WHEN EXISTS (
                                SELECT 1 
                                FROM TbItemCombinations ic2
                                INNER JOIN TbCombinationAttributesValues cav2 
                                    ON ic2.Id = cav2.ItemCombinationId
                                INNER JOIN TbOfferCombinationPricings ocp 
                                    ON ic2.Id = ocp.ItemCombinationId
                                INNER JOIN TbOffers o 
                                    ON ocp.OfferId = o.Id
                                WHERE ic2.ItemId = @ItemId
                                    AND cav2.AttributeId = a.Id
                                    AND cav2.Value = ao.Id
                                    AND o.VisibilityScope = 1
                                    AND o.IsDeleted = 0
                                    AND ocp.IsDeleted = 0
                                    AND ocp.AvailableQuantity > 0
                                    AND ocp.StockStatus = 1
                            ) THEN 1 ELSE 0 END AS IsAvailable
                        FROM TbAttributeOptions ao
                        WHERE ao.AttributeId = a.Id
                            AND ao.IsDeleted = 0
                        ORDER BY ao.DisplayOrder
                        FOR JSON PATH
                    )
                    ELSE (
                        -- Specification attributes: show as static value
                        SELECT 
                            ISNULL((
                                SELECT TOP 1 ao.TitleAr
                                FROM TbItemCombinations ic2
                                INNER JOIN TbCombinationAttributesValues cav2 
                                    ON ic2.Id = cav2.ItemCombinationId
                                INNER JOIN TbAttributeOptions ao 
                                    ON cav2.Value = ao.Id
                                WHERE ic2.ItemId = @ItemId
                                    AND cav2.AttributeId = a.Id
                            ), '') AS ValueAr,
                            ISNULL((
                                SELECT TOP 1 ao.TitleEn
                                FROM TbItemCombinations ic2
                                INNER JOIN TbCombinationAttributesValues cav2 
                                    ON ic2.Id = cav2.ItemCombinationId
                                INNER JOIN TbAttributeOptions ao 
                                    ON cav2.Value = ao.Id
                                WHERE ic2.ItemId = @ItemId
                                    AND cav2.AttributeId = a.Id
                            ), '') AS ValueEn
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    )
                END AS ValuesJson
            FROM TbCategoryAttributes ca
            INNER JOIN TbAttributes a ON ca.AttributeId = a.Id
            WHERE ca.CategoryId = @CategoryId
                AND ca.IsDeleted = 0
                AND a.IsDeleted = 0
            ORDER BY ca.DisplayOrder
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- ========== Default Combination JSON ==========
        CASE WHEN @HasCombinations = 1 AND @DefaultCombinationId IS NOT NULL THEN
            (
                SELECT 
                    ic.Id AS CombinationId,
                    ic.SKU,
                    -- Selected pricing attributes
                    (
                        SELECT 
                            a.Id AS AttributeId,
                            a.TitleAr AS AttributeNameAr,
                            a.TitleEn AS AttributeNameEn,
                            ao.Id AS ValueId,
                            ao.TitleAr AS ValueAr,
                            ao.TitleEn AS ValueEn
                        FROM TbCombinationAttributesValues cav
                        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id
                        INNER JOIN TbAttributeOptions ao ON cav.Value = ao.Id
                        INNER JOIN TbCategoryAttributes ca ON a.Id = ca.AttributeId AND ca.CategoryId = @CategoryId
                        WHERE cav.ItemCombinationId = ic.Id
                            AND ca.AffectsPricing = 1
                        ORDER BY ca.DisplayOrder
                        FOR JSON PATH
                    ) AS SelectedAttributesJson,
                    -- Combination-specific images
                    (
                        SELECT 
                            img.Path AS ImageUrl,
                            img.[Order] AS DisplayOrder,
                            CASE WHEN img.[Order] = 1 THEN 1 ELSE 0 END AS IsDefault
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
                        SELECT MIN(ocp2.SalesPrice)
                        FROM TbOffers o2
                        INNER JOIN TbOfferCombinationPricings ocp2 ON o2.Id = ocp2.OfferId
                        WHERE o2.ItemId = @ItemId
                            AND (@DefaultCombinationId IS NULL OR ocp2.ItemCombinationId = @DefaultCombinationId)
                            AND o2.VisibilityScope = 1
                            AND o2.IsDeleted = 0
                            AND ocp2.IsDeleted = 0
                    ) AS MinPrice,
                    
                    (
                        SELECT MAX(ocp2.SalesPrice)
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
                            CASE ocp.StockStatus
                                WHEN 1 THEN 'InStock'
                                WHEN 2 THEN 'OutOfStock'
                                WHEN 3 THEN 'LimitedStock'
                                WHEN 4 THEN 'ComingSoon'
                            END AS StockStatus,
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
        }
    }
}
