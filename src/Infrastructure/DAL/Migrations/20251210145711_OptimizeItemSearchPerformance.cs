using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class OptimizeItemSearchPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Phase 1: Create Indexes on TbItems
            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleAr",
                table: "TbItems",
                column: "TitleAr");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleEn",
                table: "TbItems",
                column: "TitleEn");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CategoryId_BrandId",
                table: "TbItems",
                columns: new[] { "CategoryId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsActive_CreatedDate",
                table: "TbItems",
                columns: new[] { "IsActive", "CreatedDateUtc" });

            // Phase 2: Create Indexes on TbOffers
            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_VendorId",
                table: "TbOffers",
                columns: new[] { "ItemId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_VisibilityScope",
                table: "TbOffers",
                column: "VisibilityScope");

            // Phase 3: Create Indexes on TbOfferCombinationPricing
            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_OfferId_SalesPrice",
                table: "TbOfferCombinationPricing",
                columns: new[] { "OfferId", "SalesPrice" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_SalesPrice",
                table: "TbOfferCombinationPricing",
                column: "SalesPrice");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_StockStatus_AvailableQuantity",
                table: "TbOfferCombinationPricing",
                columns: new[] { "StockStatus", "AvailableQuantity" });

            // Phase 4: Create Stored Procedure - FIXED VERSION
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpSearchItemsMultiVendor]
                    @SearchTerm NVARCHAR(255) = NULL,
                    @CategoryIds NVARCHAR(MAX) = NULL,
                    @BrandIds NVARCHAR(MAX) = NULL,
                    @MinPrice DECIMAL(18,2) = NULL,
                    @MaxPrice DECIMAL(18,2) = NULL,
                    @VendorIds NVARCHAR(MAX) = NULL,
                    @InStockOnly BIT = 0,
                    @OnSaleOnly BIT = 0,
                    @BuyBoxWinnersOnly BIT = 0,
                    @SortBy NVARCHAR(50) = 'newest',
                    @PageNumber INT = 1,
                    @PageSize INT = 20
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                    DECLARE @CategoryIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @BrandIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @VendorIdTable TABLE (Id UNIQUEIDENTIFIER);
                    
                    IF @CategoryIds IS NOT NULL AND @CategoryIds != ''
                    BEGIN
                        INSERT INTO @CategoryIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@CategoryIds, ',')
                        WHERE value != '';
                    END
                    
                    IF @BrandIds IS NOT NULL AND @BrandIds != ''
                    BEGIN
                        INSERT INTO @BrandIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@BrandIds, ',')
                        WHERE value != '';
                    END
                    
                    IF @VendorIds IS NOT NULL AND @VendorIds != ''
                    BEGIN
                        INSERT INTO @VendorIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER)
                        FROM STRING_SPLIT(@VendorIds, ',')
                        WHERE value != '';
                    END
                    
                    ;WITH ItemOffers AS (
                        SELECT 
                            i.Id AS ItemId,
                            i.TitleAr,
                            i.TitleEn,
                            i.ShortDescriptionAr,
                            i.ShortDescriptionEn,
                            i.CategoryId,
                            i.BrandId,
                            i.ThumbnailImage,
                            i.CreatedDateUtc,
                            o.Id AS OfferId,
                            o.VendorId,
                            o.HandlingTimeInDays,
                            p.SalesPrice,
                            p.Price AS OriginalPrice,
                            p.AvailableQuantity,
                            p.StockStatus,
                            o.IsBuyBoxWinner
                        FROM TbItems i
                        INNER JOIN TbOffers o ON i.Id = o.ItemId
                        INNER JOIN TbOfferCombinationPricing p ON o.Id = p.OfferId
                        WHERE 
                            i.IsActive = 1 AND i.IsDeleted = 0
                            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
                            AND p.IsDeleted = 0
                            AND (@SearchTerm IS NULL 
                                OR i.TitleAr LIKE '%' + @SearchTerm + '%'
                                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionAr LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%')
                            AND (NOT EXISTS (SELECT 1 FROM @CategoryIdTable)
                                OR i.CategoryId IN (SELECT Id FROM @CategoryIdTable))
                            AND (NOT EXISTS (SELECT 1 FROM @BrandIdTable)
                                OR i.BrandId IN (SELECT Id FROM @BrandIdTable))
                            AND (NOT EXISTS (SELECT 1 FROM @VendorIdTable)
                                OR o.VendorId IN (SELECT Id FROM @VendorIdTable))
                            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
                            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
                            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
                            AND (@OnSaleOnly = 0 OR p.SalesPrice < p.Price)
                            AND (@BuyBoxWinnersOnly = 0 OR o.IsBuyBoxWinner = 1)
                    ),
                    GroupedItems AS (
                        SELECT 
                            ItemId,
                            TitleAr,
                            TitleEn,
                            ShortDescriptionAr,
                            ShortDescriptionEn,
                            CategoryId,
                            BrandId,
                            ThumbnailImage,
                            CreatedDateUtc,
                            MIN(SalesPrice) AS MinPrice,
                            MAX(SalesPrice) AS MaxPrice,
                            COUNT(DISTINCT OfferId) AS OffersCount,
                            (
                                SELECT TOP 1 
                                    CONCAT(
                                        OfferId, '|',
                                        VendorId, '|',
                                        SalesPrice, '|',
                                        OriginalPrice, '|',
                                        AvailableQuantity, '|',
                                        CAST(IsBuyBoxWinner AS INT)
                                    )
                                FROM ItemOffers io2
                                WHERE io2.ItemId = io.ItemId
                                ORDER BY IsBuyBoxWinner DESC, SalesPrice ASC
                            ) AS BestOfferData
                        FROM ItemOffers io
                        GROUP BY 
                            ItemId, TitleAr, TitleEn, ShortDescriptionAr, 
                            ShortDescriptionEn, CategoryId, BrandId, 
                            ThumbnailImage, CreatedDateUtc
                    ),
                    RankedItems AS (
                        SELECT 
                            *,
                            ROW_NUMBER() OVER (
                                ORDER BY 
                                    CASE WHEN @SortBy = 'price_asc' THEN MinPrice END ASC,
                                    CASE WHEN @SortBy = 'price_desc' THEN MaxPrice END DESC,
                                    CASE WHEN @SortBy = 'newest' THEN CreatedDateUtc END DESC,
                                    ItemId
                            ) AS RowNum
                        FROM GroupedItems
                    )
                    SELECT 
                        ItemId, TitleAr, TitleEn, ShortDescriptionAr, ShortDescriptionEn,
                        CategoryId, BrandId, ThumbnailImage, CreatedDateUtc,
                        MinPrice, MaxPrice, OffersCount, BestOfferData
                    FROM RankedItems
                    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
                    ORDER BY RowNum;
                    
                    SELECT COUNT(DISTINCT ItemId) AS TotalRecords FROM ItemOffers;
                END
            ");

            // Phase 5: Create View
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[VwItemBestPrices] AS
                SELECT 
                    i.Id AS ItemId,
                    MIN(p.SalesPrice) AS BestPrice,
                    SUM(p.AvailableQuantity) AS TotalStock,
                    COUNT(DISTINCT o.Id) AS TotalOffers
                FROM TbItems i
                INNER JOIN TbOffers o ON i.Id = o.ItemId
                INNER JOIN TbOfferCombinationPricing p ON o.Id = p.OfferId
                WHERE 
                    i.IsActive = 1 AND i.IsDeleted = 0
                    AND o.VisibilityScope = 1 AND o.IsDeleted = 0
                    AND p.IsDeleted = 0
                    AND p.AvailableQuantity > 0
                GROUP BY i.Id
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[VwItemBestPrices]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SpSearchItemsMultiVendor]");

            migrationBuilder.DropIndex(name: "IX_TbOfferCombinationPricings_OfferId_SalesPrice", table: "TbOfferCombinationPricing");
            migrationBuilder.DropIndex(name: "IX_TbOfferCombinationPricings_SalesPrice", table: "TbOfferCombinationPricing");
            migrationBuilder.DropIndex(name: "IX_TbOfferCombinationPricings_StockStatus_AvailableQuantity", table: "TbOfferCombinationPricing");
            migrationBuilder.DropIndex(name: "IX_TbOffers_ItemId_VendorId", table: "TbOffers");
            migrationBuilder.DropIndex(name: "IX_TbOffers_VisibilityScope", table: "TbOffers");
            migrationBuilder.DropIndex(name: "IX_TbItems_TitleAr", table: "TbItems");
            migrationBuilder.DropIndex(name: "IX_TbItems_TitleEn", table: "TbItems");
            migrationBuilder.DropIndex(name: "IX_TbItems_CategoryId_BrandId", table: "TbItems");
            migrationBuilder.DropIndex(name: "IX_TbItems_IsActive_CreatedDate", table: "TbItems");
        }
    }
}