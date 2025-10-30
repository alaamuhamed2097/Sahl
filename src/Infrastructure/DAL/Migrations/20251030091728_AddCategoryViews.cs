using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add VwCategoriesWithAttributes view
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwCategoryWithAttributes] AS
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
                WHERE ao.AttributeId = a.Id AND ao.CurrentState = 1
                FOR JSON PATH
            ) AS AttributeOptionsJson
        FROM dbo.TbCategoryAttributes ca
        INNER JOIN dbo.TbAttributes a ON ca.AttributeId = a.Id
        WHERE ca.CategoryId = c.Id
          AND ca.CurrentState = 1
          AND a.CurrentState = 1
        FOR JSON PATH
    ) AS AttributesJson
FROM dbo.TbCategories c
WHERE c.CurrentState = 1;
GO");

            // Add VwAttributesWithOptions view
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwAttributeWithOptions] AS
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
                        AND ao.CurrentState = 1
                        FOR JSON PATH
                    ),'[]') AS AttributeOptionsJson
                FROM dbo.TbAttributes a
                WHERE a.CurrentState = 1;
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW VwCategoriesWithAttributes;");
            migrationBuilder.Sql(@"DROP VIEW VwAttributesWithOptions;");
        }
    }
}
