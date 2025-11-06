using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addMissingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbCurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IsBaseCurrency = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShortDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PageType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbPromoCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    EndDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PromoCodeType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FacebookUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WhatsAppCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    MainBannerPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SEOTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOMetaTags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbShippingCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    LogoImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbTestimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TestimonialText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbTestimonials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserNotifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "TbNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PVs = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    OrderDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentGatewayMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectSaleLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PromoCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrder_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrder_TbPromoCodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "TbPromoCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbOrder_TbShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbOrderDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPVs = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrderDetail_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrderDetail_TbOrder_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_Code",
                table: "TbCurrencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_CurrentState",
                table: "TbCurrencies",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_IsActive",
                table: "TbCurrencies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_IsBaseCurrency",
                table: "TbCurrencies",
                column: "IsBaseCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotifications_CurrentState",
                table: "TbNotifications",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_CurrentState",
                table: "TbOrder",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_PromoCodeId",
                table: "TbOrder",
                column: "PromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_ShippingCompanyId",
                table: "TbOrder",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_UserId",
                table: "TbOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetail_CurrentState",
                table: "TbOrderDetail",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetail_ItemId",
                table: "TbOrderDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetail_OrderId",
                table: "TbOrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPages_CurrentState",
                table: "TbPages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPromoCodes_Code",
                table: "TbPromoCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbPromoCodes_CurrentState",
                table: "TbPromoCodes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSettings_CurrentState",
                table: "TbSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanies_CurrentState",
                table: "TbShippingCompanies",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbTestimonials_CurrentState",
                table: "TbTestimonials",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbTestimonials_DisplayOrder",
                table: "TbTestimonials",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_CurrentState",
                table: "TbUserNotifications",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_IsRead",
                table: "TbUserNotifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_NotificationId",
                table: "TbUserNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_UserId",
                table: "TbUserNotifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbCurrencies");

            migrationBuilder.DropTable(
                name: "TbOrderDetail");

            migrationBuilder.DropTable(
                name: "TbPages");

            migrationBuilder.DropTable(
                name: "TbSettings");

            migrationBuilder.DropTable(
                name: "TbTestimonials");

            migrationBuilder.DropTable(
                name: "TbUserNotifications");

            migrationBuilder.DropTable(
                name: "TbOrder");

            migrationBuilder.DropTable(
                name: "TbNotifications");

            migrationBuilder.DropTable(
                name: "TbPromoCodes");

            migrationBuilder.DropTable(
                name: "TbShippingCompanies");
        }
    }
}
