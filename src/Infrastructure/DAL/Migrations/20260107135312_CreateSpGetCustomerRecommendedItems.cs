using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateSpGetCustomerRecommendedItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SpGetCustomerRecommendedItems]
    @CustomerId UNIQUEIDENTIFIER,
    @SearchTerm NVARCHAR(255) = NULL,
    @SortBy NVARCHAR(50) = 'relevance',
	@SortDirection NVARCHAR(50) = 'asc',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @LocalSearchTerm NVARCHAR(255) = @SearchTerm;
    DECLARE @LocalSortBy NVARCHAR(50) = @SortBy;
    
    -- Create temporary table for category IDs
    CREATE TABLE #CategoryIds (
        CategoryId UNIQUEIDENTIFIER PRIMARY KEY
    );

    -- Step 1: Get categories from last 3 views
    INSERT INTO #CategoryIds (CategoryId)
    SELECT DISTINCT i.CategoryId
    FROM (
        SELECT TOP 3 ItemCombinationId
        FROM TbCustomerItemViews WITH (NOLOCK)
        WHERE CustomerId = @CustomerId
        ORDER BY ViewedAt DESC
    ) v
    INNER JOIN TbItemCombinations ic WITH (NOLOCK) ON ic.Id = v.ItemCombinationId
    INNER JOIN TbItems i WITH (NOLOCK) ON i.Id = ic.ItemId;

    -- Early exit if no views found
    IF NOT EXISTS (SELECT 1 FROM #CategoryIds)
    BEGIN
        DROP TABLE #CategoryIds;
        
        SELECT 
            NULL AS ItemId, NULL AS ItemCombinationId, NULL AS OfferCombinationPricingId,
            NULL AS TitleAr, NULL AS TitleEn, NULL AS ShortDescriptionAr, NULL AS ShortDescriptionEn,
            NULL AS CategoryId, NULL AS BrandId, NULL AS BrandNameAr, NULL AS BrandNameEn,
            NULL AS ThumbnailImage, NULL AS CreatedDateUtc, NULL AS AverageRating,
            NULL AS Price, NULL AS SalesPrice, NULL AS AvailableQuantity, NULL AS StockStatus,
            NULL AS IsFreeShipping, 0 AS TotalRecords
        WHERE 1 = 0;
        RETURN;
    END;

    -- Step 2: Main query with optimized joins and filtering
    WITH RankedOffers AS (
        SELECT 
            i.Id AS ItemId,
            ic.Id AS ItemCombinationId,
            ocp.Id AS OfferCombinationPricingId,
            i.TitleAr,
            i.TitleEn,
            i.ShortDescriptionAr,
            i.ShortDescriptionEn,
            i.CategoryId,
            i.BrandId,
            b.TitleAr AS BrandNameAr,
            b.TitleEn AS BrandNameEn,
            i.ThumbnailImage,
            ic.CreatedDateUtc,
            i.AverageRating,
            ISNULL(ocp.SalesPrice, ic.BasePrice) AS SalesPrice,
            ISNULL(ocp.Price, ic.BasePrice) AS Price,
            ocp.AvailableQuantity,
            ocp.StockStatus,
            ocp.IsBuyBoxWinner,
            o.IsFreeShipping,
            o.EstimatedDeliveryDays,
            ROW_NUMBER() OVER (
                PARTITION BY ic.Id 
                ORDER BY ocp.IsBuyBoxWinner DESC, 
                         ISNULL(ocp.SalesPrice, ic.BasePrice) ASC,
                         o.EstimatedDeliveryDays ASC
            ) AS OfferRank
        FROM #CategoryIds cat
        INNER JOIN TbItems i WITH (NOLOCK)
            ON i.CategoryId = cat.CategoryId 
            AND i.VisibilityScope = 1 
            AND i.IsDeleted = 0
            AND (@LocalSearchTerm IS NULL 
                 OR i.TitleAr LIKE '%' + @LocalSearchTerm + '%' 
                 OR i.TitleEn LIKE '%' + @LocalSearchTerm + '%')
        INNER JOIN TbItemCombinations ic WITH (NOLOCK)
            ON ic.ItemId = i.Id 
            AND ic.IsDeleted = 0
        INNER JOIN TbOfferCombinationPricings ocp WITH (NOLOCK)
            ON ocp.ItemCombinationId = ic.Id
            AND ocp.IsDeleted = 0
        INNER JOIN TbOffers o WITH (NOLOCK)
            ON o.Id = ocp.OfferId 
            AND o.IsDeleted = 0
        LEFT JOIN TbBrands b WITH (NOLOCK)
            ON i.BrandId = b.Id 
            AND b.IsDeleted = 0
    ),
    BestOffers AS (
        SELECT 
            ItemId, ItemCombinationId, OfferCombinationPricingId,
            TitleAr, TitleEn, ShortDescriptionAr, ShortDescriptionEn,
            CategoryId, BrandId, BrandNameAr, BrandNameEn,
            ThumbnailImage, CreatedDateUtc, AverageRating,
            Price, SalesPrice, AvailableQuantity, StockStatus,
            IsBuyBoxWinner, IsFreeShipping, EstimatedDeliveryDays
        FROM RankedOffers
        WHERE OfferRank = 1
    ),
    PagedResults AS (
        SELECT 
            *,
            ROW_NUMBER() OVER (
                ORDER BY  
                    CASE WHEN @LocalSortBy = 'price' 
							AND @SortDirection = 'asc' 
						THEN SalesPrice END ASC,
                    CASE WHEN @LocalSortBy = 'price'
							AND @SortDirection = 'desc' 
						THEN SalesPrice END DESC,
                    CASE WHEN @LocalSortBy = 'rating' 
							AND @SortDirection = 'asc' 
						THEN AverageRating END ASC,
					CASE WHEN @LocalSortBy = 'rating'
							AND @SortDirection = 'desc'
						THEN AverageRating END DESC,
                    CASE WHEN @LocalSortBy = 'newest' 
							AND @SortDirection = 'asc' 
						THEN CreatedDateUtc END ASC,
					CASE WHEN @LocalSortBy = 'newest'
							AND @SortDirection = 'desc'
						THEN CreatedDateUtc END DESC,
                    CASE WHEN @LocalSortBy = 'relevance'
							AND @SortDirection = 'asc'
						THEN CAST(IsBuyBoxWinner AS INT) END ASC,
                    CASE WHEN @LocalSortBy = 'relevance'
							AND @SortDirection = 'desc'
						THEN CAST(IsBuyBoxWinner AS INT) END DESC,
                    ItemCombinationId
            ) AS RowNum,
            COUNT(*) OVER () AS TotalRecords
        FROM BestOffers
    )
    SELECT 
        ItemId, ItemCombinationId, OfferCombinationPricingId,
        TitleAr, TitleEn, ShortDescriptionAr, ShortDescriptionEn,
        CategoryId, BrandId, BrandNameAr, BrandNameEn,
        ThumbnailImage, CreatedDateUtc, AverageRating,
        Price, SalesPrice, AvailableQuantity,
		CASE StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
        IsFreeShipping, TotalRecords
    FROM PagedResults
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum
    OPTION (MAXDOP 4);

    -- Clean up temporary table
    DROP TABLE #CategoryIds;
END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS [dbo].[SpGetCustomerRecommendedItems]");
        }
    }
}
