using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddCartAndShipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add columns to TbOrders
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "TbOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "TbOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TbOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "TbOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            // FK to TbCustomers if table exists
            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_CustomerId",
                table: "TbOrders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCustomers",
                table: "TbOrders",
                column: "CustomerId",
                principalTable: "TbCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 2. Alter TbOrderDetails
            migrationBuilder.AddColumn<Guid>(
                name: "ItemCombinationId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "TbOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_OfferId",
                table: "TbOrderDetails",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_VendorId",
                table: "TbOrderDetails",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_WarehouseId",
                table: "TbOrderDetails",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_ItemCombinationId",
                table: "TbOrderDetails",
                column: "ItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_Offer",
                table: "TbOrderDetails",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_Vendor",
                table: "TbOrderDetails",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_Warehouse",
                table: "TbOrderDetails",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_Combination",
                table: "TbOrderDetails",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // 3. Create TbCustomerAddresses
            migrationBuilder.CreateTable(
                name: "TbCustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BuildingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Apartment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Landmark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerAddresses_Customer",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCustomerAddresses_City",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CustomerId",
                table: "TbCustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CityId",
                table: "TbCustomerAddresses",
                column: "CityId");

            // 4. Create Shopping cart tables
            migrationBuilder.CreateTable(
                name: "TbShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShoppingCarts_TbCustomers",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_CustomerId",
                table: "TbShoppingCarts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_SessionId",
                table: "TbShoppingCarts",
                column: "SessionId");

            migrationBuilder.CreateTable(
                name: "TbShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_Cart",
                        column: x => x.ShoppingCartId,
                        principalTable: "TbShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_Item",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_Combination",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_Offer",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_CartId",
                table: "TbShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_ItemId",
                table: "TbShoppingCartItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_OfferId",
                table: "TbShoppingCartItems",
                column: "OfferId");

            // 5. Create TbOrderShipments
            migrationBuilder.CreateTable(
                name: "TbOrderShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FulfillmentType = table.Column<int>(type: "int", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipmentStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrderShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_Order",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_Vendor",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_Warehouse",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_ShippingCompany",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_OrderId",
                table: "TbOrderShipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_VendorId",
                table: "TbOrderShipments",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_Status",
                table: "TbOrderShipments",
                column: "ShipmentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_Number",
                table: "TbOrderShipments",
                column: "ShipmentNumber");

            // 6. Shipment items
            migrationBuilder.CreateTable(
                name: "TbOrderShipmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrderShipmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_Shipment",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_OrderDetail",
                        column: x => x.OrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_Item",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_ShipmentId",
                table: "TbOrderShipmentItems",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_ItemId",
                table: "TbOrderShipmentItems",
                column: "ItemId");

            // 7. Shipment status history
            migrationBuilder.CreateTable(
                name: "TbShipmentStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentStatusHistory_Shipment",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatusHistory_ShipmentId",
                table: "TbShipmentStatusHistory",
                column: "ShipmentId");

            // 8. Order Payments
            migrationBuilder.CreateTable(
                name: "TbOrderPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentGatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    RefundedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrderPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrderPayments_Order",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_OrderId",
                table: "TbOrderPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_Status",
                table: "TbOrderPayments",
                column: "PaymentStatus");

            // 9. Useful views and stored procedures (create if not exists)
            var createViewsAndSps = @"
IF OBJECT_ID('dbo.VW_OrderSummary', 'V') IS NOT NULL
    DROP VIEW dbo.VW_OrderSummary;
CREATE VIEW dbo.VW_OrderSummary AS
SELECT 
    o.Id AS OrderId,
    o.Number AS OrderNumber,
    o.CustomerId,
    o.TotalAmount,
    o.OrderStatus,
    COUNT(DISTINCT s.Id) AS TotalShipments,
    COUNT(DISTINCT s.VendorId) AS TotalVendors,
    SUM(CASE WHEN s.ShipmentStatus = 3 THEN 1 ELSE 0 END) AS DeliveredShipments,
    o.CreatedDateUtc AS OrderDate
FROM TbOrders o
LEFT JOIN TbOrderShipments s ON o.Id = s.OrderId
GROUP BY o.Id, o.Number, o.CustomerId, o.TotalAmount, o.OrderStatus, o.CreatedDateUtc;

IF OBJECT_ID('dbo.VW_ActiveCartDetails', 'V') IS NOT NULL
    DROP VIEW dbo.VW_ActiveCartDetails;
CREATE VIEW dbo.VW_ActiveCartDetails AS
SELECT 
    sc.Id AS CartId,
    sc.CustomerId,
    ci.ItemId,
    ci.OfferId,
    i.TitleEn AS ItemName,
    v.CompanyName AS VendorName,
    ci.Quantity,
    ci.UnitPrice,
    ci.Quantity * ci.UnitPrice AS SubTotal
FROM TbShoppingCarts sc
INNER JOIN TbShoppingCartItems ci ON sc.Id = ci.ShoppingCartId
INNER JOIN TbItems i ON ci.ItemId = i.Id
INNER JOIN TbOffers o ON ci.OfferId = o.Id
INNER JOIN TbVendors v ON o.UserId = v.UserId
WHERE sc.IsActive = 1 AND ci.IsAvailable = 1;

IF OBJECT_ID('dbo.SP_ConvertCartToOrder', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_ConvertCartToOrder;
EXEC('CREATE PROCEDURE dbo.SP_ConvertCartToOrder AS BEGIN SET NOCOUNT ON; END');

IF OBJECT_ID('dbo.SP_SplitOrderIntoShipments', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_SplitOrderIntoShipments;
EXEC('CREATE PROCEDURE dbo.SP_SplitOrderIntoShipments AS BEGIN SET NOCOUNT ON; END');
";

            migrationBuilder.Sql(createViewsAndSps);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop views and sps
            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.VW_OrderSummary', 'V') IS NOT NULL
    DROP VIEW dbo.VW_OrderSummary;
IF OBJECT_ID('dbo.VW_ActiveCartDetails', 'V') IS NOT NULL
    DROP VIEW dbo.VW_ActiveCartDetails;
IF OBJECT_ID('dbo.SP_ConvertCartToOrder', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_ConvertCartToOrder;
IF OBJECT_ID('dbo.SP_SplitOrderIntoShipments', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_SplitOrderIntoShipments;
");

            migrationBuilder.DropTable(name: "TbOrderPayments");
            migrationBuilder.DropTable(name: "TbShipmentStatusHistory");
            migrationBuilder.DropTable(name: "TbOrderShipmentItems");
            migrationBuilder.DropTable(name: "TbOrderShipments");
            migrationBuilder.DropTable(name: "TbShoppingCartItems");
            migrationBuilder.DropTable(name: "TbShoppingCarts");
            migrationBuilder.DropTable(name: "TbCustomerAddresses");

            // Remove added columns from TbOrderDetails
            migrationBuilder.DropForeignKey(name: "FK_TbOrderDetails_Offer", table: "TbOrderDetails");
            migrationBuilder.DropForeignKey(name: "FK_TbOrderDetails_Vendor", table: "TbOrderDetails");
            migrationBuilder.DropForeignKey(name: "FK_TbOrderDetails_Warehouse", table: "TbOrderDetails");
            migrationBuilder.DropForeignKey(name: "FK_TbOrderDetails_Combination", table: "TbOrderDetails");

            migrationBuilder.DropIndex(name: "IX_TbOrderDetails_OfferId", table: "TbOrderDetails");
            migrationBuilder.DropIndex(name: "IX_TbOrderDetails_VendorId", table: "TbOrderDetails");
            migrationBuilder.DropIndex(name: "IX_TbOrderDetails_WarehouseId", table: "TbOrderDetails");
            migrationBuilder.DropIndex(name: "IX_TbOrderDetails_ItemCombinationId", table: "TbOrderDetails");

            migrationBuilder.DropColumn(name: "ItemCombinationId", table: "TbOrderDetails");
            migrationBuilder.DropColumn(name: "OfferId", table: "TbOrderDetails");
            migrationBuilder.DropColumn(name: "VendorId", table: "TbOrderDetails");
            migrationBuilder.DropColumn(name: "WarehouseId", table: "TbOrderDetails");
            migrationBuilder.DropColumn(name: "DiscountAmount", table: "TbOrderDetails");
            migrationBuilder.DropColumn(name: "TaxAmount", table: "TbOrderDetails");

            // Remove columns from TbOrders
            migrationBuilder.DropForeignKey(name: "FK_TbOrders_TbCustomers", table: "TbOrders");
            migrationBuilder.DropIndex(name: "IX_TbOrders_CustomerId", table: "TbOrders");

            migrationBuilder.DropColumn(name: "CustomerId", table: "TbOrders");
            migrationBuilder.DropColumn(name: "OrderStatus", table: "TbOrders");
            migrationBuilder.DropColumn(name: "PaymentMethod", table: "TbOrders");
            migrationBuilder.DropColumn(name: "SubTotal", table: "TbOrders");
            migrationBuilder.DropColumn(name: "DiscountAmount", table: "TbOrders");
            migrationBuilder.DropColumn(name: "TotalAmount", table: "TbOrders");
            migrationBuilder.DropColumn(name: "DeliveryAddressId", table: "TbOrders");
            migrationBuilder.DropColumn(name: "Notes", table: "TbOrders");
            migrationBuilder.DropColumn(name: "IPAddress", table: "TbOrders");
        }
    }
}
