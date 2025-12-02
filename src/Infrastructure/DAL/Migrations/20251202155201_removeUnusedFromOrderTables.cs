using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeUnusedFromOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropTable(
                name: "TbFBMInventories");

            migrationBuilder.DropTable(
                name: "TbFBMShipments");

            migrationBuilder.DropTable(
                name: "TbFulfillmentFees");

            migrationBuilder.DropTable(
                name: "TbMovitemsdetails");

            migrationBuilder.DropTable(
                name: "TbFulfillmentMethods");

            migrationBuilder.DropTable(
                name: "TbMoitems");

            migrationBuilder.DropTable(
                name: "TbMortems");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "DirectSaleLinkId",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "UnitPVs",
                table: "TbOrderDetails");

            migrationBuilder.RenameColumn(
                name: "ShippingCompanyId",
                table: "TbOrders",
                newName: "TbShippingCompanyId");

            migrationBuilder.RenameColumn(
                name: "PaymentGatewayMethodId",
                table: "TbOrders",
                newName: "DeliveryAddressId");

            migrationBuilder.RenameColumn(
                name: "PVs",
                table: "TbOrders",
                newName: "OrderStatus");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_UserId",
                table: "TbOrders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_ShippingCompanyId",
                table: "TbOrders",
                newName: "IX_TbOrders_TbShippingCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetails_OrderId",
                table: "TbOrderDetails",
                newName: "IX_OrderDetails_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetails_ItemId",
                table: "TbOrderDetails",
                newName: "IX_OrderDetails_ItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultPlatformWarehouse",
                table: "TbWarehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbWarehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "TbOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferCombinationPricingId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "TbOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TbCustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_TbCustomerAddresses_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrderShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FulfillmentType = table.Column<int>(type: "int", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentStatus = table.Column<int>(type: "int", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_TbOrderShipments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipments_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MethodType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ProviderDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                        name: "FK_TbShoppingCarts_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrderShipmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_TbOrderShipmentItems_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_TbOrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_TbOrderShipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrderPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentGatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_TbOrderPayments_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderPayments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderPayments_TbPaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "TbPaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShoppingCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                        name: "FK_TbShoppingCartItems_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbShoppingCartItems_TbShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "TbShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_IsDefaultPlatformWarehouse",
                table: "TbWarehouses",
                column: "IsDefaultPlatformWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_VendorId",
                table: "TbWarehouses",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Date",
                table: "TbOrders",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Number",
                table: "TbOrders",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "TbOrders",
                column: "OrderStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_DeliveryAddressId",
                table: "TbOrders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_VendorId",
                table: "TbOrderDetails",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_WarehouseId",
                table: "TbOrderDetails",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CurrentState",
                table: "TbCustomerAddresses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CustomerId",
                table: "TbCustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_CurrencyId",
                table: "TbOrderPayments",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_OrderId",
                table: "TbOrderPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_PaymentMethodId",
                table: "TbOrderPayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_Status",
                table: "TbOrderPayments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_CurrentState",
                table: "TbOrderPayments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_CurrentState",
                table: "TbOrderShipmentItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_ItemId",
                table: "TbOrderShipmentItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_ShipmentId",
                table: "TbOrderShipmentItems",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_Number",
                table: "TbOrderShipments",
                column: "ShipmentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_OrderId",
                table: "TbOrderShipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_Status",
                table: "TbOrderShipments",
                column: "ShipmentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_VendorId",
                table: "TbOrderShipments",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_CurrentState",
                table: "TbOrderShipments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_ShippingCompanyId",
                table: "TbOrderShipments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_WarehouseId",
                table: "TbOrderShipments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentMethods_CurrentState",
                table: "TbPaymentMethods",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentMethods_IsActive",
                table: "TbPaymentMethods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentMethods_MethodType",
                table: "TbPaymentMethods",
                column: "MethodType");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_CurrentState",
                table: "TbShoppingCartItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_ItemId",
                table: "TbShoppingCartItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_OfferId",
                table: "TbShoppingCartItems",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_ShoppingCartId",
                table: "TbShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_CurrentState",
                table: "TbShoppingCarts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_CustomerId",
                table: "TbShoppingCarts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbVendors_VendorId",
                table: "TbOrderDetails",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCustomerAddresses_DeliveryAddressId",
                table: "TbOrders",
                column: "DeliveryAddressId",
                principalTable: "TbCustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_TbShippingCompanyId",
                table: "TbOrders",
                column: "TbShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWarehouses_TbVendors_VendorId",
                table: "TbWarehouses",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbVendors_VendorId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbCustomerAddresses_DeliveryAddressId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_TbShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarehouses_TbVendors_VendorId",
                table: "TbWarehouses");

            migrationBuilder.DropTable(
                name: "TbCustomerAddresses");

            migrationBuilder.DropTable(
                name: "TbOrderPayments");

            migrationBuilder.DropTable(
                name: "TbOrderShipmentItems");

            migrationBuilder.DropTable(
                name: "TbShoppingCartItems");

            migrationBuilder.DropTable(
                name: "TbPaymentMethods");

            migrationBuilder.DropTable(
                name: "TbOrderShipments");

            migrationBuilder.DropTable(
                name: "TbShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TbWarehouses_IsDefaultPlatformWarehouse",
                table: "TbWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_TbWarehouses_VendorId",
                table: "TbWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Date",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Number",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Status",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_DeliveryAddressId",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_VendorId",
                table: "TbOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_WarehouseId",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDefaultPlatformWarehouse",
                table: "TbWarehouses");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbWarehouses");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "TbOrderDetails");

            migrationBuilder.RenameColumn(
                name: "TbShippingCompanyId",
                table: "TbOrders",
                newName: "ShippingCompanyId");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "TbOrders",
                newName: "PVs");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddressId",
                table: "TbOrders",
                newName: "PaymentGatewayMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_TbShippingCompanyId",
                table: "TbOrders",
                newName: "IX_TbOrders_ShippingCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "TbOrders",
                newName: "IX_TbOrders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderId",
                table: "TbOrderDetails",
                newName: "IX_TbOrderDetails_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ItemId",
                table: "TbOrderDetails",
                newName: "IX_TbOrderDetails_ItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "TbOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TbOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DirectSaleLinkId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitPVs",
                table: "TbOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TbFBMInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DamagedQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    InTransitQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFBMInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFBMShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActualWeight = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VolumetricWeight = table.Column<decimal>(type: "decimal(10,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFBMShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFulfillmentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BuyBoxPriorityBoost = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequiresWarehouse = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFulfillmentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbMoitems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMoitems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMoitems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbMortems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMortems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMortems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbMortems_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbFulfillmentFees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FulfillmentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    FeeType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MaximumFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageFee = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VolumeBasedFeePerCubicMeter = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    WeightBasedFeePerKg = table.Column<decimal>(type: "decimal(10,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFulfillmentFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                        column: x => x.FulfillmentMethodId,
                        principalTable: "TbFulfillmentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbMovitemsdetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoitemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MortemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMovitemsdetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                        column: x => x.MoitemId,
                        principalTable: "TbMoitems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                        column: x => x.MortemId,
                        principalTable: "TbMortems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_CurrentState",
                table: "TbFBMInventories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_ItemId",
                table: "TbFBMInventories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_ItemId_WarehouseId_VendorId",
                table: "TbFBMInventories",
                columns: new[] { "ItemId", "WarehouseId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_SKU",
                table: "TbFBMInventories",
                column: "SKU");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_VendorId",
                table: "TbFBMInventories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_WarehouseId",
                table: "TbFBMInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_WarehouseId_AvailableQuantity",
                table: "TbFBMInventories",
                columns: new[] { "WarehouseId", "AvailableQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_CurrentState",
                table: "TbFBMShipments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_OrderId",
                table: "TbFBMShipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShipmentNumber",
                table: "TbFBMShipments",
                column: "ShipmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShippedDate",
                table: "TbFBMShipments",
                column: "ShippedDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShippingCompanyId",
                table: "TbFBMShipments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_Status",
                table: "TbFBMShipments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_Status_ShippedDate",
                table: "TbFBMShipments",
                columns: new[] { "Status", "ShippedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_TrackingNumber",
                table: "TbFBMShipments",
                column: "TrackingNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_WarehouseId",
                table: "TbFBMShipments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_CurrentState",
                table: "TbFulfillmentFees",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_EffectiveFrom_EffectiveTo",
                table: "TbFulfillmentFees",
                columns: new[] { "EffectiveFrom", "EffectiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FeeType",
                table: "TbFulfillmentFees",
                column: "FeeType");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FulfillmentMethodId",
                table: "TbFulfillmentFees",
                column: "FulfillmentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FulfillmentMethodId_FeeType",
                table: "TbFulfillmentFees",
                columns: new[] { "FulfillmentMethodId", "FeeType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_IsActive",
                table: "TbFulfillmentFees",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_Code",
                table: "TbFulfillmentMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_CurrentState",
                table: "TbFulfillmentMethods",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_DisplayOrder",
                table: "TbFulfillmentMethods",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_IsActive",
                table: "TbFulfillmentMethods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_CurrentState",
                table: "TbMoitems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_DocumentDate",
                table: "TbMoitems",
                column: "DocumentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_DocumentNumber",
                table: "TbMoitems",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_UserId",
                table: "TbMoitems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_CurrentState",
                table: "TbMortems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_DocumentNumber",
                table: "TbMortems",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_OrderId",
                table: "TbMortems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_Status",
                table: "TbMortems",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_UserId",
                table: "TbMortems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_CurrentState",
                table: "TbMovitemsdetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_ItemCombinationId",
                table: "TbMovitemsdetails",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_ItemId",
                table: "TbMovitemsdetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_MoitemId",
                table: "TbMovitemsdetails",
                column: "MoitemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_MortemId",
                table: "TbMovitemsdetails",
                column: "MortemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_WarehouseId",
                table: "TbMovitemsdetails",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");
        }
    }
}
