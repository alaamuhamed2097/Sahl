using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterSpGetItemDetails5 : Migration
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
CREATE OR ALTER     PROCEDURE [dbo].[SpGetItemDetails]
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
      AND ic.IsDeleted = 0;  -- ← Added semicolon here

    IF @ItemId IS NULL
    BEGIN
        RAISERROR ('Item not found', 16, 1);
        RETURN;
    END
    
    -- Get item category and pricing system
    DECLARE @CategoryId UNIQUEIDENTIFIER;
    DECLARE @PricingSystemType INT;
    
    SELECT 
        @CategoryId = i.CategoryId,
        @PricingSystemType = c.PricingSystemType
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    WHERE i.Id = @ItemId;
    
    -- =============================================
    -- Find Default Combination (SAME AS SEARCH!)
    -- =============================================
    DECLARE @DefaultCombinationId UNIQUEIDENTIFIER;
    DECLARE @DefaultOfferId UNIQUEIDENTIFIER;
    
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
            ocp.IsBuyBoxWinner DESC,
            ocp.SalesPrice ASC,
            o.HandlingTimeInDays ASC;
    
    -- =============================================
    -- CTE for Pricing Attributes
    -- =============================================
    WITH PricingAttributes AS (
        SELECT 
            a.Id AS AttributeId,
            a.TitleAr AS AttributeNameAr,
            a.TitleEn AS AttributeNameEn,
            cattr.DisplayOrder,
            cav.Id AS CombinationValueId,
            ao.TitleAr AS ValueAr,
            ao.TitleEn AS ValueEn
        FROM TbCombinationAttributesValues cav
        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id AND a.IsDeleted = 0
        INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(cav.Value AS UNIQUEIDENTIFIER) AND ao.IsDeleted = 0
        INNER JOIN TbCategoryAttributes cattr ON a.Id = cattr.AttributeId 
                                                AND cattr.CategoryId = @CategoryId
                                                AND cattr.IsDeleted = 0
        WHERE cav.IsDeleted = 0
    )
    
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
				img.Id,
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
                COALESCE(
                    CASE 
                        WHEN a.FieldType = 6 THEN ao6.TitleAr
                        WHEN a.FieldType = 7 THEN ao7_agg.TitleAr
                        ELSE ia.Value
                    END, ia.Value
                ) AS ValueAr,
                COALESCE(
                    CASE 
                        WHEN a.FieldType = 6 THEN ao6.TitleEn
                        WHEN a.FieldType = 7 THEN ao7_agg.TitleEn
                        ELSE ia.Value
                    END, ia.Value
                ) AS ValueEn
            FROM TbItemAttributes ia
            INNER JOIN TbAttributes a ON ia.AttributeId = a.Id AND a.IsDeleted = 0
            LEFT JOIN TbAttributeOptions ao6 ON a.FieldType = 6 
                                              AND ao6.Id = TRY_CAST(ia.Value AS UNIQUEIDENTIFIER) 
                                              AND ao6.IsDeleted = 0
            OUTER APPLY (
                SELECT 
                    STRING_AGG(ao.TitleAr, ',') AS TitleAr,
                    STRING_AGG(ao.TitleEn, ',') AS TitleEn
                FROM OPENJSON(ia.Value) AS j
                INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(j.value AS UNIQUEIDENTIFIER)
                                                  AND ao.IsDeleted = 0
                WHERE a.FieldType = 7
            ) ao7_agg
            WHERE ia.ItemId = @ItemId
                AND ia.IsDeleted = 0
            ORDER BY ia.DisplayOrder
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- ========== Default Combination JSON ==========
            (    SELECT 
                    ic.Id AS CombinationId,
                    ic.SKU,
                    ic.Barcode,
                    ic.IsDefault,
					ic.CreatedBy,
                    -- Combined pricing attributes with IsSelected flag
                    (
                        SELECT 
                            pa.AttributeId,
                            pa.AttributeNameAr,
                            pa.AttributeNameEn,
                            pa.CombinationValueId,
                            pa.ValueAr,
                            pa.ValueEn,
                            CASE 
                                WHEN ca.AttributeValueId IS NOT NULL 
                                THEN CAST(1 AS bit) 
                                ELSE CAST(0 AS bit) 
                            END AS IsSelected
                        FROM PricingAttributes pa
                        LEFT JOIN TbCombinationAttributes ca 
                            ON ca.ItemCombinationId = ic.Id 
                            AND ca.AttributeValueId = pa.CombinationValueId
                            AND ca.IsDeleted = 0
                        ORDER BY pa.DisplayOrder
                        FOR JSON PATH
                    ) AS PricingAttributesJson,
                    -- Combination-specific images
                    (
                        SELECT 
						    img.Id,
                            img.Path AS ImageUrl,
                            img.[Order] AS DisplayOrder,
                            CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
                        FROM TbItemCombinationImages img
                        WHERE img.ItemCombinationId = ic.Id
                            AND img.IsDeleted = 0
                        ORDER BY img.[Order]
                        FOR JSON PATH
                    ) AS ImagesJson
                FROM TbItemCombinations ic
                WHERE ic.Id = @DefaultCombinationId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ) AS CurrentCombinationJson,
        
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
                            u.FirstName+' '+ u.LastName AS VendorName,
                            ISNULL(v.AverageRating, 0) AS VendorRating,
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
                                WHEN @PricingSystemType IN (3, 4) THEN (
                                   SELECT 
										qp.MinQuantity,
										qp.MaxQuantity,
										qp.PricePerUnit,
										qp.SalesPricePerUnit
									FROM TbQuantityTierPricings qp
									WHERE qp.OfferCombinationPricingId = ocp.Id
										AND qp.IsDeleted = 0
									ORDER BY qp.MinQuantity
                                    FOR JSON PATH
                                )
                                ELSE NULL
                            END AS QuantityTiersJson
                        FROM TbOffers o
                        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId AND ocp.ItemCombinationId = @DefaultCombinationId
                        LEFT JOIN TbVendors v ON o.VendorId = v.Id
						INNER JOIN AspNetUsers u ON u.Id = v.UserId AND u.UserState =1 
                        WHERE o.Id = @DefaultOfferId
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS BestOfferJson
                    
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS PricingJson
        
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id AND c.IsDeleted = 0
    LEFT JOIN TbPricingSystemSettings ps ON c.PricingSystemId = ps.Id AND ps.IsDeleted = 0
    LEFT JOIN TbBrands b ON i.BrandId = b.Id AND b.IsDeleted = 0
    WHERE i.Id = @ItemId;
    
END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"-- =============================================
-- Get Item Details - MATCHES SEARCH RESULT PRICE
-- Default combination uses same logic as search:
-- 1. BuyBoxWinner first
-- 2. Lowest price second
-- 3. Fastest delivery third
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[SpGetItemDetails]
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
      AND ic.IsDeleted = 0;  -- ← Added semicolon here

    IF @ItemId IS NULL
    BEGIN
        RAISERROR ('Item not found', 16, 1);
        RETURN;
    END
    
    -- Get item category and pricing system
    DECLARE @CategoryId UNIQUEIDENTIFIER;
    DECLARE @PricingSystemType INT;
    
    SELECT 
        @CategoryId = i.CategoryId,
        @PricingSystemType = c.PricingSystemType
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id
    WHERE i.Id = @ItemId;
    
    -- =============================================
    -- Find Default Combination (SAME AS SEARCH!)
    -- =============================================
    DECLARE @DefaultCombinationId UNIQUEIDENTIFIER;
    DECLARE @DefaultOfferId UNIQUEIDENTIFIER;
    
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
            ocp.IsBuyBoxWinner DESC,
            ocp.SalesPrice ASC,
            o.HandlingTimeInDays ASC;
    
    -- =============================================
    -- CTE for Pricing Attributes
    -- =============================================
    WITH PricingAttributes AS (
        SELECT 
            a.Id AS AttributeId,
            a.TitleAr AS AttributeNameAr,
            a.TitleEn AS AttributeNameEn,
            cattr.DisplayOrder,
            cav.Id AS CombinationValueId,
            ao.TitleAr AS ValueAr,
            ao.TitleEn AS ValueEn
        FROM TbCombinationAttributesValues cav
        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id AND a.IsDeleted = 0
        INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(cav.Value AS UNIQUEIDENTIFIER) AND ao.IsDeleted = 0
        INNER JOIN TbCategoryAttributes cattr ON a.Id = cattr.AttributeId 
                                                AND cattr.CategoryId = @CategoryId
                                                AND cattr.IsDeleted = 0
        WHERE cav.IsDeleted = 0
    )
    
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
				img.Id,
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
                COALESCE(
                    CASE 
                        WHEN a.FieldType = 6 THEN ao6.TitleAr
                        WHEN a.FieldType = 7 THEN ao7_agg.TitleAr
                        ELSE ia.Value
                    END, ia.Value
                ) AS ValueAr,
                COALESCE(
                    CASE 
                        WHEN a.FieldType = 6 THEN ao6.TitleEn
                        WHEN a.FieldType = 7 THEN ao7_agg.TitleEn
                        ELSE ia.Value
                    END, ia.Value
                ) AS ValueEn
            FROM TbItemAttributes ia
            INNER JOIN TbAttributes a ON ia.AttributeId = a.Id AND a.IsDeleted = 0
            LEFT JOIN TbAttributeOptions ao6 ON a.FieldType = 6 
                                              AND ao6.Id = TRY_CAST(ia.Value AS UNIQUEIDENTIFIER) 
                                              AND ao6.IsDeleted = 0
            OUTER APPLY (
                SELECT 
                    STRING_AGG(ao.TitleAr, ',') AS TitleAr,
                    STRING_AGG(ao.TitleEn, ',') AS TitleEn
                FROM OPENJSON(ia.Value) AS j
                INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(j.value AS UNIQUEIDENTIFIER)
                                                  AND ao.IsDeleted = 0
                WHERE a.FieldType = 7
            ) ao7_agg
            WHERE ia.ItemId = @ItemId
                AND ia.IsDeleted = 0
            ORDER BY ia.DisplayOrder
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- ========== Default Combination JSON ==========
            (    SELECT 
                    ic.Id AS CombinationId,
                    ic.SKU,
                    ic.Barcode,
                    ic.IsDefault,
					ic.CreatedBy,
                    -- Combined pricing attributes with IsSelected flag
                    (
                        SELECT 
                            pa.AttributeId,
                            pa.AttributeNameAr,
                            pa.AttributeNameEn,
                            pa.CombinationValueId,
                            pa.ValueAr,
                            pa.ValueEn,
                            CASE 
                                WHEN ca.AttributeValueId IS NOT NULL 
                                THEN CAST(1 AS bit) 
                                ELSE CAST(0 AS bit) 
                            END AS IsSelected
                        FROM PricingAttributes pa
                        LEFT JOIN TbCombinationAttributes ca 
                            ON ca.ItemCombinationId = ic.Id 
                            AND ca.AttributeValueId = pa.CombinationValueId
                            AND ca.IsDeleted = 0
                        ORDER BY pa.DisplayOrder
                        FOR JSON PATH
                    ) AS PricingAttributesJson,
                    -- Combination-specific images
                    (
                        SELECT 
						    img.Id,
                            img.Path AS ImageUrl,
                            img.[Order] AS DisplayOrder,
                            CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
                        FROM TbItemCombinationImages img
                        WHERE img.ItemCombinationId = ic.Id
                            AND img.IsDeleted = 0
                        ORDER BY img.[Order]
                        FOR JSON PATH
                    ) AS ImagesJson
                FROM TbItemCombinations ic
                WHERE ic.Id = @DefaultCombinationId
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ) AS CurrentCombinationJson,
        
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
                            u.FirstName+' '+ u.LastName AS VendorName,
                            ISNULL(v.AverageRating, 0) AS VendorRating,
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
                                WHEN @PricingSystemType IN (3, 4) THEN (
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
                        INNER JOIN TbOfferCombinationPricings ocp ON o.Id = ocp.OfferId AND ocp.ItemCombinationId = @DefaultCombinationId
                        LEFT JOIN TbVendors v ON o.VendorId = v.Id
						INNER JOIN AspNetUsers u ON u.Id = v.UserId AND u.UserState =1 
                        WHERE o.Id = @DefaultOfferId
                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    ) AS BestOfferJson
                    
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )
        ELSE NULL END AS PricingJson
        
    FROM TbItems i
    INNER JOIN TbCategories c ON i.CategoryId = c.Id AND c.IsDeleted = 0
    LEFT JOIN TbPricingSystemSettings ps ON c.PricingSystemId = ps.Id AND ps.IsDeleted = 0
    LEFT JOIN TbBrands b ON i.BrandId = b.Id AND b.IsDeleted = 0
    WHERE i.Id = @ItemId;
    
END
GO");
        }
    }
}
