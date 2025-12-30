using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterVwVendorItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER   VIEW [dbo].[VwVendorItems]
AS
SELECT
    -- Offer basic fields
    o.Id AS VendorItemId,
    o.HandlingTimeInDays,
    o.FulfillmentType,
    o.CreatedDateUtc,
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
    ON o.Id = ocp.OfferId AND  o.IsDeleted = 0 AND o.VisibilityScope = 1
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
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwVendorItems]
AS
SELECT
    -- Offer basic fields
    o.Id AS VendorItemId,
    o.HandlingTimeInDays,
    o.FulfillmentType,
    o.CreatedDateUtc,
	ocp.IsBuyBoxWinner,
    -- Warranty
    o.WarrantyId,
    w.WarrantyType,
    w.WarrantyPeriodMonths,
    w.WarrantyPolicy,
    
    -- Vendor fields
    o.VendorId,
    uv.FirstName + ' ' + uv.LastName AS VendorFullName,
    v.CompanyName AS VendorCompanyName,
    
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
    ON o.Id = ocp.OfferId AND  o.IsDeleted = 0 AND o.VisibilityScope = 1
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
WHERE ic.IsDeleted = 0;");
        }
    }
}
