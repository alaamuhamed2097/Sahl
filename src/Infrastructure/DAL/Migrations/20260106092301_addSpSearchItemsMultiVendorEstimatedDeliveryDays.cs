using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSpSearchItemsMultiVendorEstimatedDeliveryDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER   PROCEDURE [dbo].[SpSearchItemsMultiVendor]
    -- Search Context Parameters
    @SearchTerm NVARCHAR(255) = NULL,
    @CategoryId UNIQUEIDENTIFIER = NULL,
    @VendorId UNIQUEIDENTIFIER = NULL,
    @BrandId UNIQUEIDENTIFIER = NULL,
    
    -- Filter Parameters
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinItemRating DECIMAL(3,2) = NULL,
    @InStockOnly BIT = 0,
    @FreeShippingOnly BIT = 0,
    @ConditionId UNIQUEIDENTIFIER = NULL,
    @WithWarrantyOnly BIT = 0,
    @AttributeIds NVARCHAR(MAX) = NULL,
    @AttributeValues NVARCHAR(MAX) = NULL,
    
    -- Sorting & Pagination
    @SortBy NVARCHAR(50) = 'relevance',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- =============================
    -- Get Category and ALL its descendants (Recursive)
    -- =============================
    DECLARE @CategoryIds TABLE (CategoryId UNIQUEIDENTIFIER);
    
    IF @CategoryId IS NOT NULL
    BEGIN
        ;WITH CategoryHierarchy AS (
            -- Anchor: الـ Category المطلوب نفسه
            SELECT Id, ParentId, 0 AS [Level]
            FROM TbCategories
            WHERE Id = @CategoryId
              AND IsDeleted = 0
            
            UNION ALL
            
            -- Recursive: كل الـ Children على كل المستويات
            SELECT c.Id, c.ParentId, ch.[Level] + 1
            FROM TbCategories c
            INNER JOIN CategoryHierarchy ch ON c.ParentId = ch.Id
            WHERE c.IsDeleted = 0
        )
        INSERT INTO @CategoryIds (CategoryId)
        SELECT Id FROM CategoryHierarchy;
    END
    
    -- Temporary table for attribute filters
    DECLARE @AttributeIdTable TABLE (Id UNIQUEIDENTIFIER, ValueIds NVARCHAR(MAX));

    -- Parse attribute filters
    IF @AttributeIds IS NOT NULL AND @AttributeIds != ''
    BEGIN
        DECLARE @AttrIdList TABLE (RowNum INT, AttributeId UNIQUEIDENTIFIER);
        DECLARE @AttrValueList TABLE (RowNum INT, ValueIds NVARCHAR(MAX));
        
        INSERT INTO @AttrIdList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@AttributeIds, ',')
        WHERE value != '';
        
        INSERT INTO @AttrValueList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            value
        FROM STRING_SPLIT(@AttributeValues, '|')
        WHERE value != '';
        
        INSERT INTO @AttributeIdTable
        SELECT a.AttributeId, v.ValueIds
        FROM @AttrIdList a
        INNER JOIN @AttrValueList v ON a.RowNum = v.RowNum;
    END

    -- =============================
    -- Main Search Query
    -- =============================
    ;WITH ItemBase AS (
        SELECT
            i.Id AS ItemId,
            ic.Id AS ItemCombinationId,
            i.TitleAr,
            i.TitleEn,
            i.ShortDescriptionAr,
            i.ShortDescriptionEn,
            i.CategoryId,
            i.BrandId,
            b.NameAr AS BrandNameAr,
            b.NameEn AS BrandNameEn,
            i.ThumbnailImage,
            i.CreatedDateUtc,
            i.AverageRating,
            
            -- Offer data
            o.Id AS OfferId,
			p.Id AS OfferCombinationPricingId,
            ISNULL(o.IsFreeShipping,CAST(0 AS BIT)) AS IsFreeShipping,
            o.EstimatedDeliveryDays,
            
            -- Pricing data
            ISNULL(p.IsBuyBoxWinner,CAST(0 AS BIT)) AS IsBuyBoxWinner,
            ISNULL(p.SalesPrice,ISNULL(i.BasePrice,0)) AS SalesPrice,
            ISNULL(p.Price,ISNULL(i.BasePrice,0)) AS Price,
            ISNULL(p.AvailableQuantity,0) AS AvailableQuantity,
            ISNULL(p.StockStatus,2) AS StockStatus
            
        FROM TbItemCombinations ic 
        INNER JOIN TbItems i ON i.Id = ic.ItemId
        LEFT JOIN TbOffers o ON i.Id = o.ItemId
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
        LEFT JOIN TbOfferCombinationPricings p ON ic.Id = p.ItemCombinationId AND o.Id = p.OfferId
            AND p.IsDeleted = 0
            AND p.StockStatus IN (1, 2, 3, 4)
        LEFT JOIN TbBrands b ON i.BrandId = b.Id AND b.IsDeleted = 0
        WHERE
            i.IsActive = 1 AND i.IsDeleted = 0 
            AND ic.IsDeleted = 0
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.TitleEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
                OR i.ShortDescriptionAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.ShortDescriptionEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
            )
            
            -- Context filters - التعديل هنا
            AND (
                @CategoryId IS NULL 
                OR i.CategoryId IN (SELECT CategoryId FROM @CategoryIds)
            )
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
            AND (@BrandId IS NULL OR i.BrandId = @BrandId)
            
            -- Additional filters
            AND (@ConditionId IS NULL OR p.OfferConditionId = @ConditionId)
            AND (@WithWarrantyOnly = 0 OR o.WarrantyId IS NOT NULL)
            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
            AND (@MinItemRating IS NULL OR i.AverageRating >= @MinItemRating)
            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
            AND (@FreeShippingOnly = 0 OR o.IsFreeShipping = 1)
    ),
    
    -- Filter by attributes (if specified)
    AttributeFiltered AS (
        SELECT DISTINCT ib.*
        FROM ItemBase ib
        WHERE 
            NOT EXISTS (SELECT 1 FROM @AttributeIdTable)
            OR
            NOT EXISTS (
                SELECT 1 
                FROM @AttributeIdTable attr
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM TbCombinationAttributesValues cav
                    LEFT JOIN TbCombinationAttributes ca ON ca.ItemCombinationId = ib.ItemCombinationId
                    WHERE cav.Id = ca.AttributeValueId
                        AND cav.AttributeId = attr.Id
                        AND cav.Id IN (
                            SELECT CAST(value AS UNIQUEIDENTIFIER)
                            FROM STRING_SPLIT(attr.ValueIds, ',')
                        )
                )
            )
    ),
    
    -- Select BuyBox Winner or Best Offer per Item
    BestOffers AS (
        SELECT
            ItemId,
            ItemCombinationId,
			OfferCombinationPricingId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            SalesPrice,
            Price,
            AvailableQuantity,
            StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping,
            EstimatedDeliveryDays,
            ROW_NUMBER() OVER (
                PARTITION BY ItemCombinationId 
                ORDER BY 
                    IsBuyBoxWinner DESC,
                    SalesPrice ASC,
                    EstimatedDeliveryDays ASC
            ) AS OfferRank
        FROM AttributeFiltered
    ),
    
    -- Get only the best offer per item
    SelectedOffers AS (
        SELECT
            ItemId,
            ItemCombinationId,
			OfferCombinationPricingId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            Price,
            SalesPrice,
            AvailableQuantity,
            CASE StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping
        FROM BestOffers
        WHERE OfferRank = 1
    ),
    
    -- Apply sorting and ranking
    RankedItems AS (
        SELECT
            *,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN @SortBy = 'price_asc' THEN SalesPrice ELSE 99999999 END ASC,
                    CASE WHEN @SortBy = 'price_desc' THEN SalesPrice ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'newest arrival' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'best seller' THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'customer review' THEN ISNULL(AverageRating, 0) ELSE 0 END DESC,
                    CASE WHEN @SortBy IN ('relevance', 'Featured') THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    ItemId
            ) AS RowNum
        FROM SelectedOffers
    )
    
    -- Return paginated results with total count
    SELECT 
        ItemId,
        ItemCombinationId,
		OfferCombinationPricingId,
        TitleAr,
        TitleEn,
        ShortDescriptionAr,
        ShortDescriptionEn,
        CategoryId,
        BrandId,
        BrandNameAr,
        BrandNameEn,
        ThumbnailImage,
        CreatedDateUtc,
        AverageRating,
        Price,
        SalesPrice,
        AvailableQuantity,
        StockStatus,
        IsFreeShipping,
        (SELECT COUNT(*) FROM SelectedOffers) as TotalRecords
    FROM RankedItems
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum;

END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
