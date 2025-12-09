using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class replaceCurrentStateWithIsDeletedInViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER   VIEW [dbo].[VwAttributeWithOptions] AS
                SELECT 
                    a.Id,
                    a.TitleAr,
                    a.TitleEn,
                    a.FieldType,
					a.IsRangeFieldType,
                    a.MaxLength,
					a.CreatedDateUtc,
                    ISNULL((
                        SELECT
                            ao.Id,
                            ao.AttributeId,
                            ao.TitleAr,
                            ao.TitleEn,
							ao.DisplayOrder
                        FROM dbo.TbAttributeOptions ao
                        WHERE ao.AttributeId = a.Id
                        AND ao.IsDeleted = 0
                        FOR JSON PATH
                    ),'[]') AS AttributeOptionsJson
                FROM dbo.TbAttributes a
                WHERE a.IsDeleted = 0;
GO
/****** Object:  View [dbo].[VwCategoryItems]    Script Date: 12/9/2025 5:28:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   VIEW [dbo].[VwCategoryItems] AS
WITH CategoryHierarchy AS (
    -- Anchor: start with each category itself
    SELECT 
        c.Id AS RootCategoryId,
        c.Id,
        c.TitleAr,
        c.TitleEn,
        c.ParentId,
        c.IsFinal,
        c.ImageUrl,
        c.Icon,
        c.DisplayOrder,
        c.PriceRequired,
        c.TreeViewSerial,
        c.IsFeaturedCategory,
        c.IsHomeCategory,
        c.IsMainCategory,
        c.CreatedDateUtc
    FROM dbo.TbCategories c
    WHERE c.IsDeleted = 0
    
    UNION ALL
    
    -- Recursive: get children
    SELECT 
        ch.RootCategoryId,
        c.Id,
        c.TitleAr,
        c.TitleEn,
        c.ParentId,
        c.IsFinal,
        c.ImageUrl,
        c.Icon,
        c.DisplayOrder,
        c.PriceRequired,
        c.TreeViewSerial,
        c.IsFeaturedCategory,
        c.IsHomeCategory,
        c.IsMainCategory,
        c.CreatedDateUtc
    FROM dbo.TbCategories c
    INNER JOIN CategoryHierarchy ch ON c.ParentId = ch.Id
    WHERE c.IsDeleted = 0
)
SELECT 
    rc.Id, 
    rc.TitleAr, 
    rc.TitleEn, 
    rc.ParentId, 
    rc.IsFinal, 
    rc.ImageUrl, 
    rc.Icon, 
    rc.DisplayOrder, 
    rc.PriceRequired,
    rc.TreeViewSerial,
    rc.IsFeaturedCategory, 
    rc.IsHomeCategory, 
    rc.IsMainCategory, 
    rc.CreatedDateUtc,
    (
        SELECT 
            i.Id,
            i.TitleAr,
            i.TitleEn,
            i.ThumbnailImage,
            i.BasePrice AS Price,  -- تغيير هنا من Price إلى BasePrice
            i.CreatedDateUtc,
            i.CategoryId,
            ch.PriceRequired
        FROM TbItems i
        INNER JOIN CategoryHierarchy ch ON ch.Id = i.CategoryId
        WHERE ch.RootCategoryId = rc.Id
            AND i.IsDeleted = 0
        FOR JSON PATH
    ) AS ItemsJson
FROM (
    SELECT DISTINCT RootCategoryId
    FROM CategoryHierarchy
) roots
INNER JOIN dbo.TbCategories rc ON rc.Id = roots.RootCategoryId
WHERE rc.IsDeleted = 0;
GO
/****** Object:  View [dbo].[VwCategoryWithAttributes]    Script Date: 12/9/2025 5:28:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter   VIEW [dbo].[VwCategoryWithAttributes] AS
SELECT 
    c.Id,
    c.TitleAr,
    c.TitleEn,
    c.ParentId,
    c.IsFinal,
	c.ImageUrl,
	c.Icon,
	c.DisplayOrder,
	c.PriceRequired,
	c.TreeViewSerial,
	c.IsFeaturedCategory,
	c.IsHomeCategory,
	c.IsMainCategory,
	c.CreatedDateUtc,
    (
        SELECT 
            ca.Id,
            ca.CategoryId,
            ca.AttributeId,
            ca.AffectsPricing,
            ca.IsRequired,
			ca.DisplayOrder,
            a.TitleAr,
            a.TitleEn,
            a.FieldType,
			a.IsRangeFieldType,
            a.MaxLength,
            (
                SELECT ao.Id, ao.AttributeId, ao.TitleAr, ao.TitleEn, ao.DisplayOrder
                FROM dbo.TbAttributeOptions ao
                WHERE ao.AttributeId = a.Id AND ao.IsDeleted = 0
                FOR JSON PATH
            ) AS AttributeOptionsJson
        FROM dbo.TbCategoryAttributes ca
        INNER JOIN dbo.TbAttributes a ON ca.AttributeId = a.Id
        WHERE ca.CategoryId = c.Id
          AND ca.IsDeleted = 0
          AND a.IsDeleted = 0
        FOR JSON PATH
    ) AS AttributesJson
FROM dbo.TbCategories c
WHERE c.IsDeleted = 0;
GO
/****** Object:  View [dbo].[VwItems]    Script Date: 12/9/2025 5:28:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
                
Alter VIEW [dbo].[VwItems]
AS
SELECT 
    i.Id, 
    i.TitleAr, 
    i.TitleEn,
    i.ShortDescriptionAr, 
    i.ShortDescriptionEn,
    i.DescriptionAr, 
    i.DescriptionEn,
    i.VideoUrl AS VideoLink, -- Changed from VideoLink to VideoUrl
    i.CategoryId, 
    c.TitleAr AS CategoryTitleAr, 
    c.TitleEn AS CategoryTitleEn, 
    i.UnitId, 
    u.TitleAr AS UnitTitleAr, 
    u.TitleEn AS UnitTitleEn, 
    i.ThumbnailImage, 
    -- StockStatus might need to come from OfferCombinationPricings or Items
    -- Removed incorrect StockStatus reference
    -- Get default quantity from combinations
    ISNULL((
        SELECT TOP 1 ocp.AvailableQuantity 
        FROM dbo.TbOfferCombinationPricings ocp
        INNER JOIN dbo.TbItemCombinations ic ON ocp.ItemCombinationId = ic.Id
        WHERE ic.ItemId = i.Id 
            AND ocp.IsDeleted = 0
        ORDER BY 
            CASE WHEN ic.IsDefault = 1 THEN 0 ELSE 1 END,
            ocp.CreatedDateUtc
    ), 0) AS DefaultQuantity,
    -- Get default price from combinations
    ISNULL((
        SELECT TOP 1 ocp.Price 
        FROM dbo.TbOfferCombinationPricings ocp
        INNER JOIN dbo.TbItemCombinations ic ON ocp.ItemCombinationId = ic.Id
        WHERE ic.ItemId = i.Id 
            AND ocp.IsDeleted = 0
        ORDER BY 
            CASE WHEN ic.IsDefault = 1 THEN 0 ELSE 1 END,
            ocp.CreatedDateUtc
    ), 0) AS DefaultPrice,
    i.CreatedDateUtc,
    -- You might need to add IsNewArrival, IsBestSeller, IsRecommended columns if they exist
    -- Get all item images as JSON
    (
        SELECT 
            im.Id,
            im.Path,
            im.[Order]
        FROM 
            dbo.TbItemImages im
        WHERE 
            im.ItemId = i.Id 
            AND im.IsDeleted = 0
        ORDER BY 
            im.[Order]
        FOR JSON PATH
    ) AS ItemImagesJson
    -- CombinationsJson would need to be more complex with your actual schema
FROM     
    dbo.TbItems AS i 
    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
WHERE  
    (i.IsDeleted = 0)
GO
/****** Object:  View [dbo].[VwUnitWithConversionsUnits]    Script Date: 12/9/2025 5:28:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter   VIEW [dbo].[VwUnitWithConversionsUnits] AS
                SELECT 
                    u.Id,
                    u.TitleAr,
                    u.TitleEn,
                    ISNULL((
                        SELECT 
                            uc.FromUnitId As ConversionUnitId,
                            uf.TitleAr,
                            uf.TitleEn,
                            uc.ConversionFactor
                        FROM dbo.TbUnitConversions uc
                        INNER JOIN dbo.TbUnits uf ON uf.Id = uc.FromUnitId 
                        WHERE uc.ToUnitId = u.Id
                        AND uc.IsDeleted = 0
                        AND uf.IsDeleted = 0
                        FOR JSON PATH
                    ),'[]') AS ConversionUnitsFromJson,
                    ISNULL((
                        SELECT 
                            uct.ToUnitId As ConversionUnitId,
                            ut.TitleAr,
                            ut.TitleEn,
                            uct.ConversionFactor
                        FROM dbo.TbUnitConversions uct
                        INNER JOIN dbo.TbUnits ut ON ut.Id = uct.ToUnitId 
                        WHERE uct.FromUnitId = u.Id
                        AND uct.IsDeleted = 0
                        AND ut.IsDeleted = 0
                        FOR JSON PATH
                    ),'[]') AS ConversionUnitsToJson
                FROM dbo.TbUnits u
                WHERE u.IsDeleted = 0;
            
GO
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
