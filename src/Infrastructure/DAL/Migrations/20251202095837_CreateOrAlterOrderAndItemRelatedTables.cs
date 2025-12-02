using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateOrAlterOrderAndItemRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemAttribute",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "SellerRating",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItems");

            migrationBuilder.RenameTable(
                name: "TbItemAttribute",
                newName: "TbItemAttributes");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TbOfferCombinationPricings",
                newName: "IsDefault");

            migrationBuilder.RenameColumn(
                name: "ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                newName: "ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbMovitemsdetails_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                newName: "IX_TbMovitemsdetails_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_ItemId",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_CurrentState",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_AttributeId",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_AttributeId");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "TbOrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorgeLocation",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisibilityScope",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItemCombinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbItemCombinations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TbItemAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemAttributes",
                table: "TbItemAttributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbAttributeValuePriceModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CombinationAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierType = table.Column<int>(type: "int", nullable: false),
                    ModifierValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributeValuePriceModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_CombinationAttributeValueId",
                        column: x => x.CombinationAttributeValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "TbOfferPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TbItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TbOfferCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbItemCombinations_TbItemCombinationId",
                        column: x => x.TbItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                        column: x => x.OfferCombinationPricingId,
                        principalTable: "TbOfferCombinationPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                        column: x => x.TbOfferCombinationPricingId,
                        principalTable: "TbOfferCombinationPricings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbOfferStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewStatus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TbOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferStatusHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOfferStatusHistories_TbOffers_TbOfferId",
                        column: x => x.TbOfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbOrderPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_TbOrderPayments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrderShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FulfillmentType = table.Column<int>(type: "int", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "TbShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderShipmentItems_TbOrderShipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_TbShoppingCartItems_TbShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "TbShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_AttributeId",
                table: "TbAttributeValuePriceModifiers",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CombinationAttributeValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "CombinationAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CurrentState",
                table: "TbAttributeValuePriceModifiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CurrentState",
                table: "TbCustomerAddresses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CustomerId",
                table: "TbCustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_CurrentState",
                table: "TbOfferPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_ItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_TbItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "TbItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_CurrentState",
                table: "TbOfferStatusHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_OfferId",
                table: "TbOfferStatusHistories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_TbOfferId",
                table: "TbOfferStatusHistories",
                column: "TbOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_CurrentState",
                table: "TbOrderPayments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_OrderId",
                table: "TbOrderPayments",
                column: "OrderId");

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
                name: "IX_TbOrderShipments_CurrentState",
                table: "TbOrderShipments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_OrderId",
                table: "TbOrderShipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_CurrentState",
                table: "TbShoppingCartItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_ItemId",
                table: "TbShoppingCartItems",
                column: "ItemId");

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
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttributes",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttributes",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemCombinations_ItemCombinationId",
                table: "TbMovitemsdetails",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemCombinations_ItemCombinationId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropTable(
                name: "TbCustomerAddresses");

            migrationBuilder.DropTable(
                name: "TbOfferPriceHistories");

            migrationBuilder.DropTable(
                name: "TbOfferStatusHistories");

            migrationBuilder.DropTable(
                name: "TbOrderPayments");

            migrationBuilder.DropTable(
                name: "TbOrderShipmentItems");

            migrationBuilder.DropTable(
                name: "TbShoppingCartItems");

            migrationBuilder.DropTable(
                name: "TbOrderShipments");

            migrationBuilder.DropTable(
                name: "TbShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemAttributes",
                table: "TbItemAttributes");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "ItemCombinationId",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "OfferId",
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

            migrationBuilder.DropColumn(
                name: "HandlingTimeInDays",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "StorgeLocation",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VisibilityScope",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TbItemAttributes");

            migrationBuilder.RenameTable(
                name: "TbItemAttributes",
                newName: "TbItemAttribute");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "TbOfferCombinationPricings",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "ItemCombinationId",
                table: "TbMovitemsdetails",
                newName: "ItemAttributeCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbMovitemsdetails_ItemCombinationId",
                table: "TbMovitemsdetails",
                newName: "IX_TbMovitemsdetails_ItemAttributeCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_ItemId",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_CurrentState",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_AttributeId",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_AttributeId");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "TbOffers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SellerRating",
                table: "TbOffers",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemAttribute",
                table: "TbItemAttribute",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbItemAttributeCombinationPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeIds = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    LastPriceUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuggestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemAttributeCombinationPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemAttributeCombinationPricings_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers",
                columns: new[] { "ItemId", "IsActive", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_AttributeIds",
                table: "TbItemAttributeCombinationPricings",
                column: "AttributeIds");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_CurrentState",
                table: "TbItemAttributeCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings",
                column: "Price");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                column: "ItemAttributeCombinationPricingId",
                principalTable: "TbItemAttributeCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");
        }
    }
}
