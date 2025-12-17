using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateVwOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW dbo.VwOffers
AS
SELECT
    -- Offer basic fields
    o.Id,
    o.ItemId,
    o.VendorId,
    o.HandlingTimeInDays,
    o.VisibilityScope,
    o.FulfillmentType,
    o.VendorRatingForThisItem,
    o.VendorSalesCountForThisItem,
    o.IsBuyBoxWinner,
    o.WarrantyId,
    o.CreatedDateUtc,
    
    -- Vendor fields
    uv.FirstName + ' ' + uv.LastName AS VendorFullName,
    v.CompanyName AS VendorCompanyName,
    uv.Email AS VendorEmail,
    uv.PhoneCode + uv.PhoneNumber AS VendorFullPhoneNumber,
    
    -- Item fields with proper aliases
    i.TitleAr AS ItemTitleAr,
    i.TitleEn AS ItemTitleEn,
    i.BrandId,
    b.TitleAr AS BrandTitleAr,
    b.TitleEn AS BrandTitleEn,
    i.CategoryId,
    c.TitleAr AS CategoryTitleAr,
    c.TitleEn AS CategoryTitleEn,
    c.PricingSystemType,
    
    -- Warranty
    w.WarrantyType,
    w.WarrantyPeriodMonths,
    w.WarrantyPolicy,
    
    -- JSON optimized with ISNULL to handle nulls
    ISNULL(
        (
            SELECT 
                ocp.Id,
                ocp.ItemCombinationId,
                ic.Barcode,
                ic.SKU,
                ic.IsDefault,
                ocp.OfferConditionId,
                ocp.Price,
                ocp.SalesPrice,
                ocp.CostPrice,
                ocp.AvailableQuantity,
                ocp.ReservedQuantity,
                ocp.RefundedQuantity,
                ocp.DamagedQuantity,
                ocp.InTransitQuantity,
                ocp.ReturnedQuantity,
                ocp.LockedQuantity,
                ocp.StockStatus,
                ocp.LastStockUpdate,
                ocp.MinOrderQuantity,
                ocp.MaxOrderQuantity,
                ocp.LowStockThreshold,
                oc.NameAr AS ConditionNameAr,
                oc.NameEn AS ConditionNameEn,
                oc.IsNew AS IsConditionNew,
                -- Nested attributes JSON
                ISNULL(
                    (
                        SELECT
                            cav.Id,
                            cav.AttributeId,
                            a.TitleAr AS AttributeTitleAr,
                            a.TitleEn AS AttributeTitleEn,
                            a.FieldType AS AttributeFieldType,
                            cav.Value
                        FROM TbCombinationAttributesValues cav
                        INNER JOIN TbAttributes a 
                            ON a.Id = cav.AttributeId AND a.IsDeleted = 0
                        WHERE cav.ItemCombinationId = ocp.ItemCombinationId
                          AND cav.IsDeleted = 0
                        FOR JSON PATH
                    ),
                    '[]'
                ) AS AttributesJson
            FROM TbOfferCombinationPricings ocp
            INNER JOIN TbItemCombinations ic
                ON ic.Id = ocp.ItemCombinationId AND ic.IsDeleted = 0
            LEFT JOIN TbOfferConditions oc
                ON oc.Id = ocp.OfferConditionId AND oc.IsDeleted = 0
            WHERE ocp.OfferId = o.Id
              AND ocp.IsDeleted = 0
            FOR JSON PATH
        ),
        '[]'
    ) AS OfferCombinationsJson
    
FROM TbOffers o
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
WHERE o.IsDeleted = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS dbo.VwOffers;");
        }
    }
}
