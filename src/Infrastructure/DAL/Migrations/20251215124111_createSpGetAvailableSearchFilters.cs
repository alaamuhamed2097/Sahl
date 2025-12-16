using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createSpGetAvailableSearchFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                OR i.TitleAr LIKE '%' + @SearchTerm + '%'
                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionAr LIKE '%' + @SearchTerm + '%'
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
                    INNER JOIN TbCombinationAttributesValues av2 ON b2.ItemCombinationId = av2.ItemCombinationId
                    WHERE av2.AttributeId = attr.Id
                    GROUP BY av2.Id, av2.Value
                    HAVING COUNT(DISTINCT b2.ItemId) > 0
                    ORDER BY ItemCount DESC
                    FOR JSON PATH
                )) AS [Values]
            FROM (
                SELECT DISTINCT a.Id, a.TitleAr, a.TitleEn
                FROM BaseItems b
                INNER JOIN TbCombinationAttributesValues ca ON b.ItemCombinationId = ca.ItemCombinationId
                INNER JOIN TbAttributes a ON ca.AttributeId = a.Id
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
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[SpGetAvailableSearchFilters]");
        }
    }
}
