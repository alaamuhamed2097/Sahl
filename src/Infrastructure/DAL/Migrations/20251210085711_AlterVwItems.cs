using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterVwItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwItems]
AS
SELECT 
    i.Id, 
	i.SEOTitle,
	i.SEODescription,
	i.SEOMetaTags,
    i.TitleAr, 
    i.TitleEn,
    i.ShortDescriptionAr, 
    i.ShortDescriptionEn,
    i.DescriptionAr, 
    i.DescriptionEn,
    i.CategoryId, 
    c.TitleAr AS CategoryTitleAr, 
    c.TitleEn AS CategoryTitleEn, 
    i.UnitId, 
    u.TitleAr AS UnitTitleAr, 
    u.TitleEn AS UnitTitleEn, 
	i.BrandId,
	b.TitleAr AS BrandTitleAr,
	b.TitleEn AS BrandTitleEn,
    i.VideoProviderId,
	vp.TitleAr AS VideoProviderTitleAr,
	vp.TitleEn AS VideoProviderTitleEn,
	i.VideoUrl,
    i.ThumbnailImage, 
	i.Barcode,
	i.SKU,
	i.BasePrice,
	i.MinimumPrice,
	i.MaximumPrice,
    i.CreatedDateUtc,
	i.VisibilityScope,
    -- Get all item images as JSON
    (
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		img.ItemId
        FROM dbo.TbItemImages img
        WHERE img.ItemId = i.Id AND img.IsDeleted = 0 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS Images,
	(
        SELECT 
		iattr.Id,
		iattr.ItemId,
		iattr.AttributeId,
		iattr.Value
        FROM dbo.TbItemAttributes iattr
        WHERE iattr.ItemId = i.Id AND iattr.IsDeleted = 0 
        FOR JSON PATH
    ) AS ItemAttributes
FROM     
    dbo.TbItems AS i 
    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
	INNER JOIN dbo.TbBrands AS b ON i.BrandId = b.Id
	LEFT JOIN dbo.TbVideoProviders AS vp ON i.VideoProviderId = vp.Id 
WHERE  
    (i.IsDeleted = 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwItems]
AS
SELECT 
    i.Id, 
	i.SEOTitle,
	i.SEODescription,
	i.SEOMetaTags,
    i.TitleAr, 
    i.TitleEn,
    i.ShortDescriptionAr, 
    i.ShortDescriptionEn,
    i.DescriptionAr, 
    i.DescriptionEn,
    i.CategoryId, 
    c.TitleAr AS CategoryTitleAr, 
    c.TitleEn AS CategoryTitleEn, 
    i.UnitId, 
    u.TitleAr AS UnitTitleAr, 
    u.TitleEn AS UnitTitleEn, 
	i.BrandId,
	b.TitleAr AS BrandTitleAr,
	b.TitleEn AS BrandTitleEn,
    i.VideoProviderId,
	vp.TitleAr AS VideoProviderTitleAr,
	vp.TitleEn AS VideoProviderTitleEn,
	i.VideoUrl,
    i.ThumbnailImage, 
	i.Barcode,
	i.SKU,
	i.BasePrice,
	i.MinimumPrice,
	i.MaximumPrice,
    i.CreatedDateUtc,
	i.VisibilityScope,
    -- Get all item images as JSON
    (
        SELECT 
		img.Id,
		img.Path,
		img.[Order],
		img.ItemId
        FROM dbo.TbItemImages img
        WHERE img.ItemId = i.Id AND img.IsDeleted = 1 
        ORDER BY 
            img.[Order]
        FOR JSON PATH
    ) AS Images,
	(
        SELECT 
		iattr.Id,
		iattr.ItemId,
		iattr.AttributeId,
		iattr.Value
        FROM dbo.TbItemAttributes iattr
        WHERE iattr.ItemId = i.Id AND iattr.IsDeleted = 1 
        FOR JSON PATH
    ) AS ItemAttributes
FROM     
    dbo.TbItems AS i 
    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
	INNER JOIN dbo.TbBrands AS b ON i.BrandId = b.Id
	LEFT JOIN dbo.TbVideoProviders AS vp ON i.VideoProviderId = vp.Id 
WHERE  
    (i.IsDeleted = 1)");
        }
    }
}
