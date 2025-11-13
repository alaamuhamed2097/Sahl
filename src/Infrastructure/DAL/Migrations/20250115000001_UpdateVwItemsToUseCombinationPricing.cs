using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVwItemsToUseCombinationPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update VwItems to get price from default combination or first combination
            migrationBuilder.Sql(@"                
CREATE OR ALTER VIEW [dbo].[VwItems]
AS
SELECT 
    i.Id, 
    i.TitleAr, 
    i.TitleEn,
    i.ShortDescriptionAr, 
    i.ShortDescriptionEn,
    i.DescriptionAr, 
    i.DescriptionEn,
    i.VideoLink,
    i.CategoryId, 
    c.TitleAr AS CategoryTitleAr, 
    c.TitleEn AS CategoryTitleEn, 
    i.UnitId, 
    u.TitleAr AS UnitTitleAr, 
    u.TitleEn AS UnitTitleEn, 
    i.ThumbnailImage, 
    i.StockStatus,
    -- Get quantity from default combination or first combination
    ISNULL((
        SELECT TOP 1 cp.Quantity 
        FROM dbo.TbItemAttributeCombinationPricings cp
        WHERE cp.ItemId = i.Id 
            AND cp.CurrentState = 1
        ORDER BY 
            CASE WHEN cp.IsDefault = 1 THEN 0 ELSE 1 END,
            cp.CreatedDateUtc
    ), 0) AS Quantity,
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
    i.IsNewArrival,
    i.IsBestSeller,
    i.IsRecommended,
    (
        SELECT 
            im.Id,
            im.Path,
            im.[Order]
        FROM 
            dbo.TbItemImages im
        WHERE 
            im.ItemId = i.Id 
            AND im.CurrentState = 1
        ORDER BY 
            im.[Order]
        FOR JSON PATH
    ) AS ItemImagesJson
FROM     
    dbo.TbItems AS i 
    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
WHERE  
    (i.CurrentState = 1)
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert back to using AttributeIds = '' as default indicator
            migrationBuilder.Sql(@"                
CREATE OR ALTER VIEW [dbo].[VwItems]
AS
SELECT 
    i.Id, 
    i.TitleAr, 
    i.TitleEn,
    i.ShortDescriptionAr, 
    i.ShortDescriptionEn,
    i.DescriptionAr, 
    i.DescriptionEn,
    i.VideoLink,
    i.CategoryId, 
    c.TitleAr AS CategoryTitleAr, 
    c.TitleEn AS CategoryTitleEn, 
    i.UnitId, 
    u.TitleAr AS UnitTitleAr, 
    u.TitleEn AS UnitTitleEn, 
    i.ThumbnailImage, 
    i.StockStatus,
    ISNULL((
        SELECT TOP 1 cp.Quantity 
        FROM dbo.TbItemAttributeCombinationPricings cp
        WHERE cp.ItemId = i.Id 
            AND cp.CurrentState = 1
        ORDER BY 
            CASE WHEN cp.AttributeIds = '' THEN 0 ELSE 1 END,
            cp.CreatedDateUtc
    ), 0) AS Quantity,
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
    i.IsNewArrival,
    i.IsBestSeller,
    i.IsRecommended,
    (
        SELECT 
            im.Id,
            im.Path,
            im.[Order]
        FROM 
            dbo.TbItemImages im
        WHERE 
            im.ItemId = i.Id 
            AND im.CurrentState = 1
        ORDER BY 
            im.[Order]
        FOR JSON PATH
    ) AS ItemImagesJson
FROM     
    dbo.TbItems AS i 
    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
WHERE  
    (i.CurrentState = 1)
GO");
        }
    }
}
