using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVwCategoryItemsToUseCombinationPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update VwCategoryItems to get price from default combination or first combination
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwCategoryItems] AS
WITH CategoryHierarchy AS (
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
    WHERE c.CurrentState = 1
    
    UNION ALL
    
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
    WHERE c.CurrentState = 1
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
            -- Get price from default combination or first combination
            ISNULL((
                SELECT TOP 1 cp.Price 
                FROM dbo.TbItemAttributeCombinationPricings cp
                WHERE cp.ItemId = i.Id 
                    AND cp.CurrentState = 1
                ORDER BY 
                    CASE WHEN cp.IsDefault = 1 THEN 0 ELSE 1 END,
                    cp.CreatedDateUtc
            ), 0) AS Price,
            i.CreatedDateUtc,
            i.CategoryId,
			ch.PriceRequired
        FROM TbItems i
        INNER JOIN CategoryHierarchy ch ON ch.Id = i.CategoryId
        WHERE ch.RootCategoryId = rc.Id
            AND i.CurrentState = 1
        FOR JSON PATH
    ) AS ItemsJson
FROM (
    SELECT DISTINCT RootCategoryId
    FROM CategoryHierarchy
) roots
INNER JOIN dbo.TbCategories rc ON rc.Id = roots.RootCategoryId
WHERE rc.CurrentState = 1;
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert back to using AttributeIds = '' as default indicator
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwCategoryItems] AS
WITH CategoryHierarchy AS (
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
    WHERE c.CurrentState = 1
    
    UNION ALL
    
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
    WHERE c.CurrentState = 1
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
            ISNULL((
                SELECT TOP 1 cp.Price 
                FROM dbo.TbItemAttributeCombinationPricings cp
                WHERE cp.ItemId = i.Id 
                    AND cp.CurrentState = 1
                ORDER BY 
                    CASE WHEN cp.AttributeIds = '' THEN 0 ELSE 1 END,
                    cp.CreatedDateUtc
            ), 0) AS Price,
            i.CreatedDateUtc,
            i.CategoryId,
			ch.PriceRequired
        FROM TbItems i
        INNER JOIN CategoryHierarchy ch ON ch.Id = i.CategoryId
        WHERE ch.RootCategoryId = rc.Id
            AND i.CurrentState = 1
        FOR JSON PATH
    ) AS ItemsJson
FROM (
    SELECT DISTINCT RootCategoryId
    FROM CategoryHierarchy
) roots
INNER JOIN dbo.TbCategories rc ON rc.Id = roots.RootCategoryId
WHERE rc.CurrentState = 1;
GO");
        }
    }
}
