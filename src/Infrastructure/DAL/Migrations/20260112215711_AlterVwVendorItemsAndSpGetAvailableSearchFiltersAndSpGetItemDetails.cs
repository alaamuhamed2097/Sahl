using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterVwVendorItemsAndSpGetAvailableSearchFiltersAndSpGetItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter VwVendorItems 
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwVendorItems]
AS
SELECT
    -- Offer basic fields
    o.Id AS VendorItemId,
    o.EstimatedDeliveryDays,
    o.FulfillmentType,
    o.CreatedDateUtc,
	 o.VisibilityScope, 
    CASE o.VisibilityScope
        WHEN 1 THEN 'Active'
        WHEN 2 THEN 'Inactive'
        WHEN 3 THEN 'Pending'
        WHEN 4 THEN 'Suspended'
        ELSE 'Unknown'
    END AS VisibilityScopeName,
	ocp.IsBuyBoxWinner,
    -- Warranty
    o.WarrantyId,
    w.WarrantyType,
    w.WarrantyPeriodMonths,
    w.WarrantyPolicy,
    
    -- Vendor fields
    o.VendorId,
    uv.FirstName + ' ' + uv.LastName AS VendorFullName,
    
	--Item combination details
	ic.Id AS ItemCombinationId,
	ocp.Barcode,
	ocp.SKU,
	ic.IsDefault,
    -- Item fields with proper aliases
    o.ItemId,
    i.TitleAr AS ItemTitleAr,
    i.TitleEn AS ItemTitleEn,
    i.BrandId,
    b.TitleAr AS BrandTitleAr,
    b.TitleEn AS BrandTitleEn,
    i.CategoryId,
    c.TitleAr AS CategoryTitleAr,
    c.TitleEn AS CategoryTitleEn,
    c.PricingSystemType,
        (
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
        FROM dbo.TbItemImages img
        WHERE img.ItemId = i.Id AND img.IsDeleted = 0 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS BaseItemImages,
	(
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
        FROM dbo.TbItemCombinationImages img
        WHERE img.ItemCombinationId = ic.Id AND img.IsDeleted = 0 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS ItemCombinationImages,
    
    -- pricing
	CASE ocp.StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
    ocp.OfferConditionId,
    oc.NameAr AS ConditionNameAr,
    oc.NameEn AS ConditionNameEn,
    oc.IsNew AS IsConditionNew,
    ocp.Price,
    ocp.SalesPrice,
    ocp.AvailableQuantity,
    -- Nested attributes JSON
    ISNULL(
        (
            SELECT 
            a.Id AS AttributeId,
            a.TitleAr AS AttributeNameAr,
            a.TitleEn AS AttributeNameEn,
            cav.Id AS CombinationValueId,
            ao.TitleAr AS ValueAr,
            ao.TitleEn AS ValueEn,
			CASE 
                                WHEN ca.AttributeValueId IS NOT NULL 
                                THEN CAST(1 AS bit) 
                                ELSE CAST(0 AS bit) 
                            END AS IsSelected
        FROM TbCombinationAttributesValues cav
		INNER JOIN TbCombinationAttributes ca ON ca.AttributeValueId = cav.Id AND ca.ItemCombinationId = ic.Id
        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id AND a.IsDeleted = 0
        INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(cav.Value AS UNIQUEIDENTIFIER) AND ao.IsDeleted = 0
        INNER JOIN TbCategoryAttributes cattr ON a.Id = cattr.AttributeId 
                                                AND cattr.CategoryId = i.CategoryId
                                                AND cattr.IsDeleted = 0
        WHERE cav.IsDeleted = 0 
        ORDER BY cattr.DisplayOrder
            FOR JSON PATH
        ) ,
        '[]'
    ) AS ConbinationAttributes
    
FROM TbItemCombinations ic
INNER JOIN TbOfferCombinationPricings ocp 
    ON ocp.ItemCombinationId = ic.Id AND ocp.IsDeleted = 0 
LEFT JOIN TbOfferConditions oc
    ON oc.Id = ocp.OfferConditionId AND oc.IsDeleted = 0
INNER JOIN TbOffers o 
    ON o.Id = ocp.OfferId AND  o.IsDeleted = 0 
INNER JOIN TbItems i 
    ON o.ItemId = i.Id AND i.IsDeleted = 0
INNER JOIN TbVendors v 
    ON o.VendorId = v.Id AND v.IsDeleted = 0
INNER JOIN AspNetUsers uv 
    ON uv.Id = v.UserId AND uv.UserState = 1
INNER JOIN TbCategories c 
    ON i.CategoryId = c.Id AND c.IsDeleted = 0
INNER JOIN TbUnits u 
    ON i.UnitId = u.Id AND u.IsDeleted = 0
INNER JOIN TbBrands b 
    ON i.BrandId = b.Id AND b.IsDeleted = 0
LEFT JOIN TbWarranties w 
    ON o.WarrantyId = w.Id AND w.IsDeleted = 0
WHERE ic.IsDeleted = 0;
                ");
            // Alter SPGetAvailableSearchFilters
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpGetAvailableSearchFilters]
    @SearchTerm NVARCHAR(255) = NULL,      -- من البحث في الصفحة الرئيسية
    @CategoryId UNIQUEIDENTIFIER = NULL,   -- لما يفتح كاتيجوري معين
    @VendorId UNIQUEIDENTIFIER = NULL      -- لما يفتح منتجات بائع معين
AS
BEGIN
    SET NOCOUNT ON;
    
    -- =============================
    -- Base Items Query
    -- =============================
    ;WITH BaseItems AS (
        SELECT DISTINCT
            i.Id AS ItemId,
            i.CategoryId,
            i.BrandId,
            o.VendorId,
            p.OfferConditionId,
            o.IsFreeShipping,
            o.WarrantyId,
            p.SalesPrice,
            p.AvailableQuantity,
            p.StockStatus,
            ic.Id AS ItemCombinationId
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        LEFT JOIN TbItemCombinations ic ON p.ItemCombinationId = ic.Id
        WHERE
            i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND p.IsDeleted = 0
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
            )
            AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
    )
    
    SELECT 
        -- Categories JSON
        (
            SELECT 
                c.Id,
                c.TitleAr,
                c.TitleEn,
                c.Icon,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbCategories c ON b.CategoryId = c.Id
            WHERE c.IsDeleted = 0
            GROUP BY c.Id, c.TitleAr, c.TitleEn, c.Icon
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS CategoriesJson,
        
        -- Brands JSON
        (
            SELECT 
                br.Id,
                br.NameAr,
                br.NameEn,
                br.LogoPath AS LogoUrl,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbBrands br ON b.BrandId = br.Id
            WHERE br.IsDeleted = 0
            GROUP BY br.Id, br.NameAr, br.NameEn, br.LogoPath
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS BrandsJson,
        
        -- Vendors JSON (بس لو مش داخل من صفحة بائع)
        (
            SELECT 
                v.Id,
                ISNULL(v.StoreName, '') AS StoreName,
                COUNT(DISTINCT b.ItemId) AS ItemCount,
                ISNULL(v.AverageRating, 0) AS AvgRating
            FROM BaseItems b
            INNER JOIN TbVendors v ON b.VendorId = v.Id
            WHERE v.Status = 3 AND v.IsDeleted = 0
                AND @VendorId IS NULL  -- بس لو مش داخل من صفحة بائع
            GROUP BY v.Id, v.StoreName, v.AverageRating
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS VendorsJson,
        
        -- Price Range JSON
        (
            SELECT 
                MIN(SalesPrice) AS MinPrice,
                MAX(SalesPrice) AS MaxPrice,
                CAST(AVG(SalesPrice) AS DECIMAL(18,2)) AS AvgPrice
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS PriceRangeJson,
        
        -- Attributes JSON
        (
            SELECT 
                attr.Id AS AttributeId,
                attr.TitleAr AS NameAr,
                attr.TitleEn AS NameEn,
                0 AS DisplayOrder,
                JSON_QUERY((
                    SELECT 
                        av2.Id AS ValueId,
                        av2.Value AS ValueAr,
                        av2.Value AS ValueEn,
                        COUNT(DISTINCT b2.ItemId) AS ItemCount
                    FROM BaseItems b2
					LEFT JOIN TbCombinationAttributes ca ON ca.ItemCombinationId = b2.ItemCombinationId
                    LEFT JOIN TbCombinationAttributesValues av2 ON ca.AttributeValueId = av2.Id
                    WHERE av2.AttributeId = attr.Id
                    GROUP BY av2.Id, av2.Value
                    HAVING COUNT(DISTINCT b2.ItemId) > 0
                    ORDER BY ItemCount DESC
                    FOR JSON PATH
                )) AS [Values]
            FROM (
                SELECT DISTINCT a.Id, a.TitleAr, a.TitleEn
                FROM BaseItems b
				LEFT JOIN TbCombinationAttributes ca ON b.ItemCombinationId = ca.ItemCombinationId
                LEFT JOIN TbCombinationAttributesValues cav ON cav.Id = ca.AttributeValueId
                INNER JOIN TbAttributes a ON cav.AttributeId = a.Id
                WHERE a.IsDeleted = 0
            ) attr
            ORDER BY attr.TitleEn
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- Conditions JSON
        (
            SELECT 
                oc.Id,
                oc.NameAr,
                oc.NameEn,
                '' AS Description,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbOfferConditions oc ON b.OfferConditionId = oc.Id
            WHERE oc.IsDeleted = 0
            GROUP BY oc.Id, oc.NameAr, oc.NameEn
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS ConditionsJson,
        
        -- Feature Flags JSON
        (
            SELECT 
                SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) AS FreeShippingCount,
                CASE WHEN SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasFreeShipping,
                SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) AS WithWarrantyCount,
                CASE WHEN SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasWarranty,
                SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) AS InStockCount,
                CASE WHEN SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasInStock,
                COUNT(DISTINCT ItemId) AS TotalItems
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS FeaturesJson;
END
");
            // Alter SPGetItemDetails
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpGetItemDetails]
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
            o.EstimatedDeliveryDays ASC;
    
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
							ocp.Id as OfferPricingId,
                            ocp.Price,
                            ocp.SalesPrice,
							ocp.SKU,
							ocp.Barcode,
                            CASE 
                                WHEN ocp.Price > ocp.SalesPrice AND ocp.Price > 0 
                                THEN CAST(((ocp.Price - ocp.SalesPrice) / ocp.Price * 100) AS DECIMAL(5,2))
                                ELSE 0 
                            END AS DiscountPercentage,
                            ocp.AvailableQuantity,
                            ocp.StockStatus,
                            o.IsFreeShipping,
                            o.EstimatedDeliveryDays AS EstimatedDeliveryDays,
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
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert VwVendorItems
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwVendorItems]
AS
SELECT
    -- Offer basic fields
    o.Id AS VendorItemId,
    o.EstimatedDeliveryDays,
    o.FulfillmentType,
    o.CreatedDateUtc,
	 o.VisibilityScope, 
    CASE o.VisibilityScope
        WHEN 1 THEN 'Active'
        WHEN 2 THEN 'Inactive'
        WHEN 3 THEN 'Pending'
        WHEN 4 THEN 'Suspended'
        ELSE 'Unknown'
    END AS VisibilityScopeName,
	ocp.IsBuyBoxWinner,
    -- Warranty
    o.WarrantyId,
    w.WarrantyType,
    w.WarrantyPeriodMonths,
    w.WarrantyPolicy,
    
    -- Vendor fields
    o.VendorId,
    uv.FirstName + ' ' + uv.LastName AS VendorFullName,
    
	--Item combination details
	ic.Id AS ItemCombinationId,
	ic.Barcode,
	ic.SKU,
	ic.IsDefault,
    -- Item fields with proper aliases
    o.ItemId,
    i.TitleAr AS ItemTitleAr,
    i.TitleEn AS ItemTitleEn,
    i.BrandId,
    b.TitleAr AS BrandTitleAr,
    b.TitleEn AS BrandTitleEn,
    i.CategoryId,
    c.TitleAr AS CategoryTitleAr,
    c.TitleEn AS CategoryTitleEn,
    c.PricingSystemType,
        (
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
        FROM dbo.TbItemImages img
        WHERE img.ItemId = i.Id AND img.IsDeleted = 0 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS BaseItemImages,
	(
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		CASE WHEN img.[Order] = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsDefault
        FROM dbo.TbItemCombinationImages img
        WHERE img.ItemCombinationId = ic.Id AND img.IsDeleted = 0 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS ItemCombinationImages,
    
    -- pricing
	CASE ocp.StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
    ocp.OfferConditionId,
    oc.NameAr AS ConditionNameAr,
    oc.NameEn AS ConditionNameEn,
    oc.IsNew AS IsConditionNew,
    ocp.Price,
    ocp.SalesPrice,
    ocp.AvailableQuantity,
    -- Nested attributes JSON
    ISNULL(
        (
            SELECT 
            a.Id AS AttributeId,
            a.TitleAr AS AttributeNameAr,
            a.TitleEn AS AttributeNameEn,
            cav.Id AS CombinationValueId,
            ao.TitleAr AS ValueAr,
            ao.TitleEn AS ValueEn,
			CASE 
                                WHEN ca.AttributeValueId IS NOT NULL 
                                THEN CAST(1 AS bit) 
                                ELSE CAST(0 AS bit) 
                            END AS IsSelected
        FROM TbCombinationAttributesValues cav
		INNER JOIN TbCombinationAttributes ca ON ca.AttributeValueId = cav.Id AND ca.ItemCombinationId = ic.Id
        INNER JOIN TbAttributes a ON cav.AttributeId = a.Id AND a.IsDeleted = 0
        INNER JOIN TbAttributeOptions ao ON ao.Id = TRY_CAST(cav.Value AS UNIQUEIDENTIFIER) AND ao.IsDeleted = 0
        INNER JOIN TbCategoryAttributes cattr ON a.Id = cattr.AttributeId 
                                                AND cattr.CategoryId = i.CategoryId
                                                AND cattr.IsDeleted = 0
        WHERE cav.IsDeleted = 0 
        ORDER BY cattr.DisplayOrder
            FOR JSON PATH
        ) ,
        '[]'
    ) AS ConbinationAttributes
    
FROM TbItemCombinations ic
INNER JOIN TbOfferCombinationPricings ocp 
    ON ocp.ItemCombinationId = ic.Id AND ocp.IsDeleted = 0 
LEFT JOIN TbOfferConditions oc
    ON oc.Id = ocp.OfferConditionId AND oc.IsDeleted = 0
INNER JOIN TbOffers o 
    ON o.Id = ocp.OfferId AND  o.IsDeleted = 0 
INNER JOIN TbItems i 
    ON o.ItemId = i.Id AND i.IsDeleted = 0
INNER JOIN TbVendors v 
    ON o.VendorId = v.Id AND v.IsDeleted = 0
INNER JOIN AspNetUsers uv 
    ON uv.Id = v.UserId AND uv.UserState = 1
INNER JOIN TbCategories c 
    ON i.CategoryId = c.Id AND c.IsDeleted = 0
INNER JOIN TbUnits u 
    ON i.UnitId = u.Id AND u.IsDeleted = 0
INNER JOIN TbBrands b 
    ON i.BrandId = b.Id AND b.IsDeleted = 0
LEFT JOIN TbWarranties w 
    ON o.WarrantyId = w.Id AND w.IsDeleted = 0
WHERE ic.IsDeleted = 0;
                
                ");
            // Revert SPGetAvailableSearchFilters
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpGetAvailableSearchFilters]
    @SearchTerm NVARCHAR(255) = NULL,      -- من البحث في الصفحة الرئيسية
    @CategoryId UNIQUEIDENTIFIER = NULL,   -- لما يفتح كاتيجوري معين
    @VendorId UNIQUEIDENTIFIER = NULL      -- لما يفتح منتجات بائع معين
AS
BEGIN
    SET NOCOUNT ON;
    
    -- =============================
    -- Base Items Query
    -- =============================
    ;WITH BaseItems AS (
        SELECT DISTINCT
            i.Id AS ItemId,
            i.CategoryId,
            i.BrandId,
            o.VendorId,
            p.OfferConditionId,
            o.IsFreeShipping,
            o.WarrantyId,
            p.SalesPrice,
            p.AvailableQuantity,
            p.StockStatus,
            ic.Id AS ItemCombinationId
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        LEFT JOIN TbItemCombinations ic ON p.ItemCombinationId = ic.Id
        WHERE
            i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND p.IsDeleted = 0
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
            )
            AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
    )
    
    SELECT 
        -- Categories JSON
        (
            SELECT 
                c.Id,
                c.TitleAr,
                c.TitleEn,
                c.Icon,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbCategories c ON b.CategoryId = c.Id
            WHERE c.IsDeleted = 0
            GROUP BY c.Id, c.TitleAr, c.TitleEn, c.Icon
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS CategoriesJson,
        
        -- Brands JSON
        (
            SELECT 
                br.Id,
                br.NameAr,
                br.NameEn,
                br.LogoPath AS LogoUrl,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbBrands br ON b.BrandId = br.Id
            WHERE br.IsDeleted = 0
            GROUP BY br.Id, br.NameAr, br.NameEn, br.LogoPath
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS BrandsJson,
        
        -- Vendors JSON (بس لو مش داخل من صفحة بائع)
        (
            SELECT 
                v.Id,
                ISNULL(v.CompanyName, '') AS StoreName,
                ISNULL(v.CompanyName, '') AS StoreNameAr,
                '' AS LogoUrl,
                COUNT(DISTINCT b.ItemId) AS ItemCount,
                ISNULL(v.Rating, 0) AS AvgRating
            FROM BaseItems b
            INNER JOIN TbVendors v ON b.VendorId = v.Id
            WHERE v.IsActive = 1 AND v.IsDeleted = 0
                AND @VendorId IS NULL  -- بس لو مش داخل من صفحة بائع
            GROUP BY v.Id, v.CompanyName, v.Rating
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS VendorsJson,
        
        -- Price Range JSON
        (
            SELECT 
                MIN(SalesPrice) AS MinPrice,
                MAX(SalesPrice) AS MaxPrice,
                CAST(AVG(SalesPrice) AS DECIMAL(18,2)) AS AvgPrice
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS PriceRangeJson,
        
        -- Attributes JSON
        (
            SELECT 
                attr.Id AS AttributeId,
                attr.TitleAr AS NameAr,
                attr.TitleEn AS NameEn,
                0 AS DisplayOrder,
                JSON_QUERY((
                    SELECT 
                        av2.Id AS ValueId,
                        av2.Value AS ValueAr,
                        av2.Value AS ValueEn,
                        COUNT(DISTINCT b2.ItemId) AS ItemCount
                    FROM BaseItems b2
					LEFT JOIN TbCombinationAttributes ca ON ca.ItemCombinationId = b2.ItemCombinationId
                    LEFT JOIN TbCombinationAttributesValues av2 ON ca.AttributeValueId = av2.Id
                    WHERE av2.AttributeId = attr.Id
                    GROUP BY av2.Id, av2.Value
                    HAVING COUNT(DISTINCT b2.ItemId) > 0
                    ORDER BY ItemCount DESC
                    FOR JSON PATH
                )) AS [Values]
            FROM (
                SELECT DISTINCT a.Id, a.TitleAr, a.TitleEn
                FROM BaseItems b
				LEFT JOIN TbCombinationAttributes ca ON b.ItemCombinationId = ca.ItemCombinationId
                LEFT JOIN TbCombinationAttributesValues cav ON cav.Id = ca.AttributeValueId
                INNER JOIN TbAttributes a ON cav.AttributeId = a.Id
                WHERE a.IsDeleted = 0
            ) attr
            ORDER BY attr.TitleEn
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- Conditions JSON
        (
            SELECT 
                oc.Id,
                oc.NameAr,
                oc.NameEn,
                '' AS Description,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbOfferConditions oc ON b.OfferConditionId = oc.Id
            WHERE oc.IsDeleted = 0
            GROUP BY oc.Id, oc.NameAr, oc.NameEn
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS ConditionsJson,
        
        -- Feature Flags JSON
        (
            SELECT 
                SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) AS FreeShippingCount,
                CASE WHEN SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasFreeShipping,
                SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) AS WithWarrantyCount,
                CASE WHEN SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasWarranty,
                SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) AS InStockCount,
                CASE WHEN SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasInStock,
                COUNT(DISTINCT ItemId) AS TotalItems
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS FeaturesJson;
END
");
            // Revert SPGetItemDetails
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpGetItemDetails]
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
            o.EstimatedDeliveryDays ASC;
    
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
							ocp.Id as OfferPricingId,
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
                            o.EstimatedDeliveryDays AS EstimatedDeliveryDays,
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
            ");
        }
    }
}
