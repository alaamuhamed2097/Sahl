using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRelationBetweenOrderShipmentAndItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserState = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RecipientID = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DeliveryChannel = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRangeFieldType = table.Column<bool>(type: "bit", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampaignType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    MinimumDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 20m),
                    FundingModel = table.Column<int>(type: "int", nullable: false),
                    PlatformFundingPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    SellerFundingPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    BannerImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SlugEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SlugAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxParticipatingProducts = table.Column<int>(type: "int", nullable: true),
                    MaxProductsPerVendor = table.Column<int>(type: "int", nullable: true),
                    MinimumOrderValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BudgetLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbContentAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbContentAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbCouponCodes",
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
                    CouponCodeType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCouponCodes", x => x.Id);
                });

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
                name: "TbCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbDeliveryReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackagingRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    CourierDeliveryRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    OverallRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDeliveryReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbDisputes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DisputeNumber = table.Column<int>(type: "int", nullable: false),
                    MessageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Parties = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredResolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedRefund = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatformDecision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedAdminID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDisputes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbFlashSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    DurationInHours = table.Column<int>(type: "int", nullable: false),
                    MinimumDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 20m),
                    MinimumSellerRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 4.0m),
                    BannerImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ShowCountdownTimer = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxProducts = table.Column<int>(type: "int", nullable: true),
                    TotalSales = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSales", x => x.Id);
                    table.CheckConstraint("CK_FlashSale_DurationInHours", "[DurationInHours] >= 6 AND [DurationInHours] <= 48");
                });

            migrationBuilder.CreateTable(
                name: "TbHomepageBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BlockType = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    BackgroundColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BackgroundImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CssClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxItemsToDisplay = table.Column<int>(type: "int", nullable: true),
                    ShowViewAllLink = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ViewAllLinkUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ActiveTo = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbHomepageBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbLoyaltyTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TierNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TierNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumOrdersPerYear = table.Column<int>(type: "int", nullable: false),
                    MaximumOrdersPerYear = table.Column<int>(type: "int", nullable: false),
                    PointsMultiplier = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 1.0m),
                    CashbackPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    HasFreeShipping = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasPrioritySupport = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BadgeIconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbLoyaltyTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbNotificationChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Configuration = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbNotificationChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferConditions", x => x.Id);
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
                name: "TbPricingSystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SystemNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SystemNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SystemType = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPricingSystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbProductReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProductReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSalesReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAccuracyRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ShippingSpeedRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    CommunicationRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ServiceRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    OverallRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSalesReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TierCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TierNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TierNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinimumOrders = table.Column<int>(type: "int", nullable: false),
                    MaximumOrders = table.Column<int>(type: "int", nullable: true),
                    CommissionReductionPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    HasPrioritySupport = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasBuyBoxBoost = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasFeaturedListings = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasAdvancedAnalytics = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasDedicatedAccountManager = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BadgeIconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerTiers", x => x.Id);
                    table.CheckConstraint("CK_SellerTier_Orders", "[MaximumOrders] IS NULL OR [MaximumOrders] > [MinimumOrders]");
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
                    OrderTaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderExtraCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "TbSupportTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TicketNumber = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TicketCreatedDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AssignedTeam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSupportTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbVendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorType = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATRegistered = table.Column<bool>(type: "bit", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbVideoProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVideoProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbNotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    EnableEmail = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EnableSMS = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EnablePush = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EnableInApp = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbNotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_TbUserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbAttributeOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbMediaContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ContentAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Image"),
                    MediaPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMediaContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                        column: x => x.ContentAreaId,
                        principalTable: "TbContentAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbStates_TbCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "TbCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbPlatformTreasuries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CustomerWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    VendorWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PendingCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CollectedCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PendingPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ProcessedPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRefunds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastReconciliationDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPlatformTreasuries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "TbCustomerWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PendingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "TbDisputeMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DisputeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageNumber = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDisputeMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                        column: x => x.DisputeID,
                        principalTable: "TbDisputes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCustomerLoyalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoyaltyTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AvailablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    UsedPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ExpiredPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalOrdersThisYear = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalSpentThisYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    LastTierUpgradeDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    NextTierEligibilityDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerLoyalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                        column: x => x.LoyaltyTierId,
                        principalTable: "TbLoyaltyTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    IsHomeCategory = table.Column<bool>(type: "bit", nullable: false),
                    IsRoot = table.Column<bool>(type: "bit", nullable: false),
                    IsFeaturedCategory = table.Column<bool>(type: "bit", nullable: false),
                    IsMainCategory = table.Column<bool>(type: "bit", nullable: false),
                    PriceRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TreeViewSerial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PricingSystemType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PricingSystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCategories_TbPricingSystemSettings_PricingSystemId",
                        column: x => x.PricingSystemId,
                        principalTable: "TbPricingSystemSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbReviewReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbReviewReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbReviewReports_TbProductReviews_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbReviewVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoteType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbReviewVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerTierBenefits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BenefitNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerTierBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                        column: x => x.SellerTierId,
                        principalTable: "TbSellerTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbSupportTicketMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TicketID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageNumber = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    InternalNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSupportTicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSupportTicketMessages_TbSupportTickets_TicketID",
                        column: x => x.TicketID,
                        principalTable: "TbSupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbUnitConversions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FromUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUnitConversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                        column: x => x.FromUnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                        column: x => x.ToUnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbAuthorizedDistributors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuthorizationStartDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    AuthorizationEndDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AuthorizationDocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAuthorizedDistributors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbBrandRegistrationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OfficialWebsite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TrademarkNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrademarkExpiryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CommercialRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovedBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrandRegistrationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                        column: x => x.ApprovedBrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaignVendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalProductsSubmitted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalProductsApproved = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalCommissionPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaignVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "TbCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerPerformanceMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 0m),
                    TotalReviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OrderCompletionRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    OnTimeShippingRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    ReturnRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    CancellationRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    ResponseRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    AverageResponseTimeInHours = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BuyBoxWins = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BuyBoxWinRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    UsesFBM = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CalculatedForPeriodStart = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    CalculatedForPeriodEnd = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerPerformanceMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbSellerRequests_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorTierHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AchievedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    TotalOrdersAtTime = table.Column<int>(type: "int", nullable: false),
                    TotalSalesAtTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorTierHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                        column: x => x.SellerTierId,
                        principalTable: "TbSellerTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PendingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalWithdrawn = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalCommissionPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastWithdrawalDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    WithdrawalFeePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 2.5m),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbWarehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    IsDefaultPlatformWarehouse = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWarehouses_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TbCities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCities_TbStates_StateId",
                        column: x => x.StateId,
                        principalTable: "TbStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Number = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    OrderDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TbShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrders_TbCouponCodes_CouponId",
                        column: x => x.CouponId,
                        principalTable: "TbCouponCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbOrders_TbCustomerAddresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "TbCustomerAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOrders_TbShippingCompanies_TbShippingCompanyId",
                        column: x => x.TbShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbCategoryAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AffectsPricing = table.Column<bool>(type: "bit", nullable: false),
                    IsVariant = table.Column<bool>(type: "bit", nullable: false),
                    AffectsStock = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCategoryAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TbCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThumbnailImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinimumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibilityScope = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SEOTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOMetaTags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItems_TbBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbItems_TbCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TbCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbItems_TbUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbBrandDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandRegistrationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VerificationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrandDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                        column: x => x.BrandRegistrationRequestId,
                        principalTable: "TbBrandRegistrationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbRequestComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRequestComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRequestComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                        column: x => x.SellerRequestId,
                        principalTable: "TbSellerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbRequestDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRequestDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                        column: x => x.SellerRequestId,
                        principalTable: "TbSellerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbWarranties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    WarrantyType = table.Column<int>(type: "int", nullable: false),
                    WarrantyPeriodMonths = table.Column<int>(type: "int", nullable: false),
                    WarrantyPolicy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WarrantyServiceCenter = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWarranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWarranties_TbCities_CityId",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TbLoyaltyPointsTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerLoyaltyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferralId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbLoyaltyPointsTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                        column: x => x.CustomerLoyaltyId,
                        principalTable: "TbCustomerLoyalties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "TbRefundRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RefundAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefundReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundMethod = table.Column<int>(type: "int", nullable: true),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefundRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbRefundRequests_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbBlockProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    HomepageBlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BadgeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FeaturedFrom = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    FeaturedTo = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBlockProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                        column: x => x.HomepageBlockId,
                        principalTable: "TbHomepageBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbBlockProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaignProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampaignPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PlatformContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VendorContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaignProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "TbCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFlashSaleProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FlashSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlashSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AddToCartCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSaleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                        column: x => x.FlashSaleId,
                        principalTable: "TbFlashSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbItemAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRangeFieldType = table.Column<bool>(type: "bit", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbItemAttributes_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbItemCombinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemCombinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemCombinations_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbItemImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemImages_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbProductVisibilityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibilityStatus = table.Column<int>(type: "int", nullable: false),
                    HasActiveOffers = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasStock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SuppressedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SuppressedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProductVisibilityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                        column: x => x.SuppressedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbVisibilityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVisibilityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVisibilityLogs_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StorgeLocation = table.Column<int>(type: "int", nullable: false),
                    HandlingTimeInDays = table.Column<int>(type: "int", nullable: false),
                    VisibilityScope = table.Column<int>(type: "int", nullable: false),
                    WarrantyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfferConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOffers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOffers_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                        column: x => x.OfferConditionId,
                        principalTable: "TbOfferConditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbOffers_TbWarranties_WarrantyId",
                        column: x => x.WarrantyId,
                        principalTable: "TbWarranties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbWalletTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VendorWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ProcessedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                        column: x => x.CustomerWalletId,
                        principalTable: "TbCustomerWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                        column: x => x.RefundId,
                        principalTable: "TbRefundRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                        column: x => x.VendorWalletId,
                        principalTable: "TbVendorWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbSuppressionReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ProductVisibilityRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReasonType = table.Column<int>(type: "int", nullable: false),
                    ReasonDescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReasonDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSuppressionReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                        column: x => x.ProductVisibilityRuleId,
                        principalTable: "TbProductVisibilityRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbBuyBoxCalculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinningOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SellerRatingScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ShippingSpeedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FBMUsageScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StockLevelScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ReturnRateScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CalculationDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ScoreBreakdown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TbOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBuyBoxCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbOffers_TbOfferId",
                        column: x => x.TbOfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                        column: x => x.WinningOfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBuyBoxHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    WonAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    LostAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LossReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBuyBoxHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCustomerSegmentPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MinimumOrderQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerSegmentPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferCombinationPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    RefundedQuantity = table.Column<int>(type: "int", nullable: false),
                    DamagedQuantity = table.Column<int>(type: "int", nullable: false),
                    InTransitQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnedQuantity = table.Column<int>(type: "int", nullable: false),
                    LockedQuantity = table.Column<int>(type: "int", nullable: false),
                    StockStatus = table.Column<int>(type: "int", nullable: false),
                    MinOrderQuantity = table.Column<int>(type: "int", nullable: false),
                    MaxOrderQuantity = table.Column<int>(type: "int", nullable: false),
                    LowStockThreshold = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    LastPriceUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastStockUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TbItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferCombinationPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                        column: x => x.TbItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "TbPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangedPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbPriceHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbQuantityPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: false),
                    MaximumQuantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuantityPricings", x => x.Id);
                    table.CheckConstraint("CK_QuantityPricing_Quantities", "[MaximumQuantity] IS NULL OR [MaximumQuantity] > [MinimumQuantity]");
                    table.ForeignKey(
                        name: "FK_TbQuantityPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbShippingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingMethod = table.Column<int>(type: "int", nullable: false),
                    MinimumEstimatedDays = table.Column<int>(type: "int", nullable: false),
                    MaximumEstimatedDays = table.Column<int>(type: "int", nullable: false),
                    IsCODSupported = table.Column<bool>(type: "bit", nullable: false),
                    FulfillmentType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippingDetails_TbCities_CityId",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbShippingDetails_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
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

            migrationBuilder.CreateTable(
                name: "TbUserOfferRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserOfferRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributesValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CombinationAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributesValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributesValues_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                        column: x => x.CombinationAttributeId,
                        principalTable: "TbCombinationAttributes",
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
                name: "TbOrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOrderDetails_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                        column: x => x.OfferCombinationPricingId,
                        principalTable: "TbOfferCombinationPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderDetails_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOrderDetails_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbAttributeValuePriceModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CombinationAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierType = table.Column<int>(type: "int", nullable: false),
                    PriceModifierCategory = table.Column<int>(type: "int", nullable: false),
                    ModifierValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TbCombinationAttributesValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_TbCombinationAttributesValueId",
                        column: x => x.TbCombinationAttributesValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id");
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

            migrationBuilder.InsertData(
                table: "TbPricingSystemSettings",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "CurrentState", "DisplayOrder", "IsEnabled", "SystemNameAr", "SystemNameEn", "SystemType", "UpdatedBy", "UpdatedDateUtc" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, true, "التسعير القياسي", "Standard Pricing", 0, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, true, "تسعير بالتركيبات", "Combination Pricing", 1, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, true, "تسعير حسب الكمية", "Quantity Pricing", 2, null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, true, "التركيبات مع مستويات الكمية", "Combination + Quantity", 3, null, null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, true, "تسعير حسب شريحة العميل", "Customer Segment Pricing", 4, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CurrentState",
                table: "Notifications",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationType",
                table: "Notifications",
                column: "NotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientType_RecipientID",
                table: "Notifications",
                columns: new[] { "RecipientType", "RecipientID" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SentDate",
                table: "Notifications",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_AttributeId",
                table: "TbAttributeOptions",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_CurrentState",
                table: "TbAttributeOptions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributes_CurrentState",
                table: "TbAttributes",
                column: "CurrentState");

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
                name: "IX_TbAttributeValuePriceModifiers_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "TbCombinationAttributesValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationNumber",
                table: "TbAuthorizedDistributors",
                column: "AuthorizationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationStartDate_AuthorizationEndDate",
                table: "TbAuthorizedDistributors",
                columns: new[] { "AuthorizationStartDate", "AuthorizationEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_BrandId",
                table: "TbAuthorizedDistributors",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_BrandId_VendorId",
                table: "TbAuthorizedDistributors",
                columns: new[] { "BrandId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_CurrentState",
                table: "TbAuthorizedDistributors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_IsActive",
                table: "TbAuthorizedDistributors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VendorId",
                table: "TbAuthorizedDistributors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_CurrentState",
                table: "TbBlockProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_DisplayOrder",
                table: "TbBlockProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId",
                table: "TbBlockProducts",
                column: "HomepageBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_DisplayOrder",
                table: "TbBlockProducts",
                columns: new[] { "HomepageBlockId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_ItemId",
                table: "TbBlockProducts",
                columns: new[] { "HomepageBlockId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsActive",
                table: "TbBlockProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsFeatured",
                table: "TbBlockProducts",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_ItemId",
                table: "TbBlockProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId_DocumentType",
                table: "TbBrandDocuments",
                columns: new[] { "BrandRegistrationRequestId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_CurrentState",
                table: "TbBrandDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_DocumentType",
                table: "TbBrandDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_IsVerified",
                table: "TbBrandDocuments",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_UploadedAt",
                table: "TbBrandDocuments",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_VerifiedByUserId",
                table: "TbBrandDocuments",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_ApprovedBrandId",
                table: "TbBrandRegistrationRequests",
                column: "ApprovedBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_BrandNameEn_VendorId",
                table: "TbBrandRegistrationRequests",
                columns: new[] { "BrandNameEn", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_BrandType",
                table: "TbBrandRegistrationRequests",
                column: "BrandType");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_CurrentState",
                table: "TbBrandRegistrationRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_Status",
                table: "TbBrandRegistrationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_SubmittedAt",
                table: "TbBrandRegistrationRequests",
                column: "SubmittedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_TrademarkNumber",
                table: "TbBrandRegistrationRequests",
                column: "TrademarkNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_VendorId",
                table: "TbBrandRegistrationRequests",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_VendorId_Status",
                table: "TbBrandRegistrationRequests",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_CurrentState",
                table: "TbBrands",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_CalculatedAt",
                table: "TbBuyBoxCalculations",
                column: "CalculatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_CurrentState",
                table: "TbBuyBoxCalculations",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ExpiresAt",
                table: "TbBuyBoxCalculations",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemCombinationId",
                table: "TbBuyBoxCalculations",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemId",
                table: "TbBuyBoxCalculations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemId_CalculatedAt",
                table: "TbBuyBoxCalculations",
                columns: new[] { "ItemId", "CalculatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_TbOfferId",
                table: "TbBuyBoxCalculations",
                column: "TbOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_TotalScore",
                table: "TbBuyBoxCalculations",
                column: "TotalScore");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_WinningOfferId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_CurrentState",
                table: "TbBuyBoxHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_ItemId_WonAt",
                table: "TbBuyBoxHistories",
                columns: new[] { "ItemId", "WonAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_LostAt",
                table: "TbBuyBoxHistories",
                column: "LostAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_OfferId",
                table: "TbBuyBoxHistories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_VendorId",
                table: "TbBuyBoxHistories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_VendorId_WonAt",
                table: "TbBuyBoxHistories",
                columns: new[] { "VendorId", "WonAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_WonAt",
                table: "TbBuyBoxHistories",
                column: "WonAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId",
                table: "TbCampaignProducts",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId_IsActive",
                table: "TbCampaignProducts",
                columns: new[] { "CampaignId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId_ItemId",
                table: "TbCampaignProducts",
                columns: new[] { "CampaignId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CurrentState",
                table: "TbCampaignProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_DisplayOrder",
                table: "TbCampaignProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_IsActive",
                table: "TbCampaignProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_ItemId",
                table: "TbCampaignProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CampaignType",
                table: "TbCampaigns",
                column: "CampaignType");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CurrentState",
                table: "TbCampaigns",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_EndDate",
                table: "TbCampaigns",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_IsActive",
                table: "TbCampaigns",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_IsFeatured",
                table: "TbCampaigns",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_SlugEn",
                table: "TbCampaigns",
                column: "SlugEn",
                unique: true,
                filter: "[SlugEn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_StartDate",
                table: "TbCampaigns",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_StartDate_EndDate_IsActive",
                table: "TbCampaigns",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_ApprovedByUserId",
                table: "TbCampaignVendors",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CampaignId_VendorId",
                table: "TbCampaignVendors",
                columns: new[] { "CampaignId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CurrentState",
                table: "TbCampaignVendors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_IsApproved",
                table: "TbCampaignVendors",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_VendorId",
                table: "TbCampaignVendors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_CurrentState",
                table: "TbCategories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_PricingSystemId",
                table: "TbCategories",
                column: "PricingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_AttributeId",
                table: "TbCategoryAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_CategoryId",
                table: "TbCategoryAttributes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_CurrentState",
                table: "TbCategoryAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCities_CurrentState",
                table: "TbCities",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCities_StateId",
                table: "TbCities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_CurrentState",
                table: "TbCombinationAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_AttributeId",
                table: "TbCombinationAttributesValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                column: "CombinationAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_CurrentState",
                table: "TbCombinationAttributesValues",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_AreaCode",
                table: "TbContentAreas",
                column: "AreaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_CurrentState",
                table: "TbContentAreas",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_DisplayOrder",
                table: "TbContentAreas",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_IsActive",
                table: "TbContentAreas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCountries_CurrentState",
                table: "TbCountries",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_Code",
                table: "TbCouponCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_CurrentState",
                table: "TbCouponCodes",
                column: "CurrentState");

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
                name: "IX_TbCustomerAddresses_CurrentState",
                table: "TbCustomerAddresses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CustomerId",
                table: "TbCustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_AvailablePoints",
                table: "TbCustomerLoyalties",
                column: "AvailablePoints");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CurrentState",
                table: "TbCustomerLoyalties",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_LoyaltyTierId",
                table: "TbCustomerLoyalties",
                column: "LoyaltyTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomers_CurrentState",
                table: "TbCustomers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_CurrentState",
                table: "TbCustomerSegmentPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_EffectiveFrom_EffectiveTo",
                table: "TbCustomerSegmentPricings",
                columns: new[] { "EffectiveFrom", "EffectiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_IsActive",
                table: "TbCustomerSegmentPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_OfferId_SegmentType",
                table: "TbCustomerSegmentPricings",
                columns: new[] { "OfferId", "SegmentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_SegmentType",
                table: "TbCustomerSegmentPricings",
                column: "SegmentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_AvailableBalance",
                table: "TbCustomerWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrentState",
                table: "TbCustomerWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CurrentState",
                table: "TbDeliveryReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CustomerID",
                table: "TbDeliveryReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderID",
                table: "TbDeliveryReviews",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderID_CustomerID",
                table: "TbDeliveryReviews",
                columns: new[] { "OrderID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OverallRating",
                table: "TbDeliveryReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewDate",
                table: "TbDeliveryReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewNumber",
                table: "TbDeliveryReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_Status",
                table: "TbDeliveryReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_CurrentState",
                table: "TbDisputeMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_DisputeID",
                table: "TbDisputeMessages",
                column: "DisputeID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_DisputeID_MessageNumber",
                table: "TbDisputeMessages",
                columns: new[] { "DisputeID", "MessageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_SenderID",
                table: "TbDisputeMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_SentDate",
                table: "TbDisputeMessages",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_CreatedDateUtc",
                table: "TbDisputes",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_CurrentState",
                table: "TbDisputes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_DisputeNumber",
                table: "TbDisputes",
                column: "DisputeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_OrderID",
                table: "TbDisputes",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_SenderID",
                table: "TbDisputes",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_Status",
                table: "TbDisputes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_CurrentState",
                table: "TbFlashSaleProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_DisplayOrder",
                table: "TbFlashSaleProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_ItemId",
                table: "TbFlashSaleProducts",
                columns: new[] { "FlashSaleId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_SoldQuantity",
                table: "TbFlashSaleProducts",
                columns: new[] { "FlashSaleId", "SoldQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_IsActive",
                table: "TbFlashSaleProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_ItemId",
                table: "TbFlashSaleProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_VendorId",
                table: "TbFlashSaleProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_CurrentState",
                table: "TbFlashSales",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_EndDate",
                table: "TbFlashSales",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_IsActive",
                table: "TbFlashSales",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate",
                table: "TbFlashSales",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate_EndDate_IsActive",
                table: "TbFlashSales",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_ActiveFrom_ActiveTo",
                table: "TbHomepageBlocks",
                columns: new[] { "ActiveFrom", "ActiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_BlockType",
                table: "TbHomepageBlocks",
                column: "BlockType");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_CurrentState",
                table: "TbHomepageBlocks",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_DisplayOrder",
                table: "TbHomepageBlocks",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive",
                table: "TbHomepageBlocks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive_DisplayOrder",
                table: "TbHomepageBlocks",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsVisible",
                table: "TbHomepageBlocks",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributes_AttributeId",
                table: "TbItemAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributes_CurrentState",
                table: "TbItemAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributes_ItemId",
                table: "TbItemAttributes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_CurrentState",
                table: "TbItemCombinations",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_ItemId",
                table: "TbItemCombinations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_CurrentState",
                table: "TbItemImages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_ItemId",
                table: "TbItemImages",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_Order",
                table: "TbItemImages",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_BrandId",
                table: "TbItems",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CategoryId",
                table: "TbItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CurrentState",
                table: "TbItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_UnitId",
                table: "TbItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CreatedDateUtc",
                table: "TbLoyaltyPointsTransactions",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CurrentState",
                table: "TbLoyaltyPointsTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions",
                column: "CustomerLoyaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CustomerLoyaltyId_TransactionType",
                table: "TbLoyaltyPointsTransactions",
                columns: new[] { "CustomerLoyaltyId", "TransactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_IsExpired",
                table: "TbLoyaltyPointsTransactions",
                column: "IsExpired");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_OrderId",
                table: "TbLoyaltyPointsTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_TransactionType",
                table: "TbLoyaltyPointsTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_CurrentState",
                table: "TbLoyaltyTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_DisplayOrder",
                table: "TbLoyaltyTiers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_MinimumOrdersPerYear_MaximumOrdersPerYear",
                table: "TbLoyaltyTiers",
                columns: new[] { "MinimumOrdersPerYear", "MaximumOrdersPerYear" });

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_TierNameEn",
                table: "TbLoyaltyTiers",
                column: "TierNameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_ContentAreaId",
                table: "TbMediaContents",
                column: "ContentAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_CurrentState",
                table: "TbMediaContents",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_DisplayOrder",
                table: "TbMediaContents",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_IsActive",
                table: "TbMediaContents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_MediaType",
                table: "TbMediaContents",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_Channel",
                table: "TbNotificationChannels",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_CurrentState",
                table: "TbNotificationChannels",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_IsActive",
                table: "TbNotificationChannels",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_CurrentState",
                table: "TbNotificationPreferences",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_NotificationType",
                table: "TbNotificationPreferences",
                column: "NotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_UserId",
                table: "TbNotificationPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType",
                table: "TbNotificationPreferences",
                columns: new[] { "UserId", "UserType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType_NotificationType",
                table: "TbNotificationPreferences",
                columns: new[] { "UserId", "UserType", "NotificationType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_CurrentState",
                table: "TbOfferCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferConditions_CurrentState",
                table: "TbOfferConditions",
                column: "CurrentState");

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
                name: "IX_TbOffers_CurrentState",
                table: "TbOffers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId",
                table: "TbOffers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId");

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
                name: "IX_OrderDetails_ItemId",
                table: "TbOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "TbOrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_VendorId",
                table: "TbOrderDetails",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_WarehouseId",
                table: "TbOrderDetails",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_CurrentState",
                table: "TbOrderDetails",
                column: "CurrentState");

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
                name: "IX_Orders_CustomerId",
                table: "TbOrders",
                column: "UserId");

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
                name: "IX_TbOrders_CouponId",
                table: "TbOrders",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_CurrentState",
                table: "TbOrders",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_DeliveryAddressId",
                table: "TbOrders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_TbShippingCompanyId",
                table: "TbOrders",
                column: "TbShippingCompanyId");

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
                name: "IX_TbPages_CurrentState",
                table: "TbPages",
                column: "CurrentState");

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
                name: "IX_TbPlatformTreasuries_CurrencyId",
                table: "TbPlatformTreasuries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_CurrentState",
                table: "TbPlatformTreasuries",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_LastReconciliationDate",
                table: "TbPlatformTreasuries",
                column: "LastReconciliationDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_ChangedAt",
                table: "TbPriceHistories",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_ChangedByUserId",
                table: "TbPriceHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_CurrentState",
                table: "TbPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_IsAutomatic",
                table: "TbPriceHistories",
                column: "IsAutomatic");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_OfferId",
                table: "TbPriceHistories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_OfferId_ChangedAt",
                table: "TbPriceHistories",
                columns: new[] { "OfferId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_CurrentState",
                table: "TbPricingSystemSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_SystemType",
                table: "TbPricingSystemSettings",
                column: "SystemType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CurrentState",
                table: "TbProductReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CustomerID",
                table: "TbProductReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_IsVerifiedPurchase",
                table: "TbProductReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_OrderItemID",
                table: "TbProductReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID",
                table: "TbProductReviews",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID_CustomerID",
                table: "TbProductReviews",
                columns: new[] { "ProductID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Rating",
                table: "TbProductReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ReviewNumber",
                table: "TbProductReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Status",
                table: "TbProductReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_CurrentState",
                table: "TbProductVisibilityRules",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_SuppressedByUserId",
                table: "TbProductVisibilityRules",
                column: "SuppressedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_VisibilityStatus",
                table: "TbProductVisibilityRules",
                column: "VisibilityStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_CurrentState",
                table: "TbQuantityPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_IsActive",
                table: "TbQuantityPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_MinimumQuantity",
                table: "TbQuantityPricings",
                column: "MinimumQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId",
                table: "TbQuantityPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId_MinimumQuantity",
                table: "TbQuantityPricings",
                columns: new[] { "OfferId", "MinimumQuantity" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_AdminUserId",
                table: "TbRefundRequests",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_CurrentState",
                table: "TbRefundRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_OrderId",
                table: "TbRefundRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_CreatedDateUtc",
                table: "TbRequestComments",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_CurrentState",
                table: "TbRequestComments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_SellerRequestId_CreatedDateUtc",
                table: "TbRequestComments",
                columns: new[] { "SellerRequestId", "CreatedDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_UserId",
                table: "TbRequestComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_CurrentState",
                table: "TbRequestDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_DocumentType",
                table: "TbRequestDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_SellerRequestId",
                table: "TbRequestDocuments",
                column: "SellerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_UploadedAt",
                table: "TbRequestDocuments",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_UploadedByUserId",
                table: "TbRequestDocuments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CurrentState",
                table: "TbReviewReports",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CustomerID",
                table: "TbReviewReports",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_ReportID",
                table: "TbReviewReports",
                column: "ReportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_ReviewID",
                table: "TbReviewReports",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_Status",
                table: "TbReviewReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_CurrentState",
                table: "TbReviewVotes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_CustomerID",
                table: "TbReviewVotes",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_ReviewID_CustomerID_VoteType",
                table: "TbReviewVotes",
                columns: new[] { "ReviewID", "CustomerID", "VoteType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_VoteType",
                table: "TbReviewVotes",
                column: "VoteType");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_CurrentState",
                table: "TbSalesReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_CustomerID",
                table: "TbSalesReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_OrderItemID",
                table: "TbSalesReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_OverallRating",
                table: "TbSalesReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_ReviewDate",
                table: "TbSalesReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_ReviewNumber",
                table: "TbSalesReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_Status",
                table: "TbSalesReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_VendorID",
                table: "TbSalesReviews",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_VendorID_CustomerID",
                table: "TbSalesReviews",
                columns: new[] { "VendorID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_AverageRating",
                table: "TbSellerPerformanceMetrics",
                column: "AverageRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_BuyBoxWinRate",
                table: "TbSellerPerformanceMetrics",
                column: "BuyBoxWinRate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_CurrentState",
                table: "TbSellerPerformanceMetrics",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_OrderCompletionRate",
                table: "TbSellerPerformanceMetrics",
                column: "OrderCompletionRate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_UsesFBM",
                table: "TbSellerPerformanceMetrics",
                column: "UsesFBM");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_VendorId",
                table: "TbSellerPerformanceMetrics",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_VendorId_CalculatedForPeriodStart",
                table: "TbSellerPerformanceMetrics",
                columns: new[] { "VendorId", "CalculatedForPeriodStart" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_CurrentState",
                table: "TbSellerRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_Priority",
                table: "TbSellerRequests",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_RequestType",
                table: "TbSellerRequests",
                column: "RequestType");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_RequestType_Status",
                table: "TbSellerRequests",
                columns: new[] { "RequestType", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_ReviewedByUserId",
                table: "TbSellerRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_Status",
                table: "TbSellerRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_SubmittedAt",
                table: "TbSellerRequests",
                column: "SubmittedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_VendorId",
                table: "TbSellerRequests",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_VendorId_Status",
                table: "TbSellerRequests",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_CurrentState",
                table: "TbSellerTierBenefits",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_DisplayOrder",
                table: "TbSellerTierBenefits",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_IsActive",
                table: "TbSellerTierBenefits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_SellerTierId",
                table: "TbSellerTierBenefits",
                column: "SellerTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_CurrentState",
                table: "TbSellerTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_DisplayOrder",
                table: "TbSellerTiers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_IsActive",
                table: "TbSellerTiers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_MinimumOrders_MaximumOrders",
                table: "TbSellerTiers",
                columns: new[] { "MinimumOrders", "MaximumOrders" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_TierCode",
                table: "TbSellerTiers",
                column: "TierCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSettings_CurrentState",
                table: "TbSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanies_CurrentState",
                table: "TbShippingCompanies",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_CityId",
                table: "TbShippingDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_CurrentState",
                table: "TbShippingDetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_OfferId",
                table: "TbShippingDetails",
                column: "OfferId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TbStates_CountryId",
                table: "TbStates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbStates_CurrentState",
                table: "TbStates",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_CurrentState",
                table: "TbSupportTicketMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SenderID",
                table: "TbSupportTicketMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SentDate",
                table: "TbSupportTicketMessages",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_TicketID",
                table: "TbSupportTicketMessages",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_TicketID_MessageNumber",
                table: "TbSupportTicketMessages",
                columns: new[] { "TicketID", "MessageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_AssignedTo",
                table: "TbSupportTickets",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Category",
                table: "TbSupportTickets",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_CreatedDateUtc",
                table: "TbSupportTickets",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_CurrentState",
                table: "TbSupportTickets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Priority",
                table: "TbSupportTickets",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Status",
                table: "TbSupportTickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_TicketNumber",
                table: "TbSupportTickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_UserID",
                table: "TbSupportTickets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_CurrentState",
                table: "TbSuppressionReasons",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_DetectedAt",
                table: "TbSuppressionReasons",
                column: "DetectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_IsResolved",
                table: "TbSuppressionReasons",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_IsResolved_DetectedAt",
                table: "TbSuppressionReasons",
                columns: new[] { "IsResolved", "DetectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ProductVisibilityRuleId",
                table: "TbSuppressionReasons",
                column: "ProductVisibilityRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ProductVisibilityRuleId_ReasonType",
                table: "TbSuppressionReasons",
                columns: new[] { "ProductVisibilityRuleId", "ReasonType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ReasonType",
                table: "TbSuppressionReasons",
                column: "ReasonType");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_CurrentState",
                table: "TbUnitConversions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_FromUnitId",
                table: "TbUnitConversions",
                column: "FromUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_ToUnitId",
                table: "TbUnitConversions",
                column: "ToUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnits_CurrentState",
                table: "TbUnits",
                column: "CurrentState");

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

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_CurrentState",
                table: "TbUserOfferRatings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_OfferId",
                table: "TbUserOfferRatings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_UserId",
                table: "TbUserOfferRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendors_CurrentState",
                table: "TbVendors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_AchievedAt",
                table: "TbVendorTierHistories",
                column: "AchievedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_CurrentState",
                table: "TbVendorTierHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_EndedAt",
                table: "TbVendorTierHistories",
                column: "EndedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_SellerTierId",
                table: "TbVendorTierHistories",
                column: "SellerTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_VendorId_AchievedAt",
                table: "TbVendorTierHistories",
                columns: new[] { "VendorId", "AchievedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_AvailableBalance",
                table: "TbVendorWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrencyId",
                table: "TbVendorWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrentState",
                table: "TbVendorWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets",
                column: "VendorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbVideoProviders_CurrentState",
                table: "TbVideoProviders",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ChangedAt",
                table: "TbVisibilityLogs",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ChangedByUserId",
                table: "TbVisibilityLogs",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_CurrentState",
                table: "TbVisibilityLogs",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_IsAutomatic",
                table: "TbVisibilityLogs",
                column: "IsAutomatic");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ItemId_ChangedAt",
                table: "TbVisibilityLogs",
                columns: new[] { "ItemId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CreatedDateUtc",
                table: "TbWalletTransactions",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CurrentState",
                table: "TbWalletTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "CustomerWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ReferenceNumber",
                table: "TbWalletTransactions",
                column: "ReferenceNumber",
                unique: true,
                filter: "[ReferenceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_Status",
                table: "TbWalletTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_TransactionType",
                table: "TbWalletTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "VendorWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_CurrentState",
                table: "TbWarehouses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_IsActive",
                table: "TbWarehouses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_IsDefaultPlatformWarehouse",
                table: "TbWarehouses",
                column: "IsDefaultPlatformWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_VendorId",
                table: "TbWarehouses",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CityId",
                table: "TbWarranties",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CurrentState",
                table: "TbWarranties",
                column: "CurrentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "TbAttributeOptions");

            migrationBuilder.DropTable(
                name: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropTable(
                name: "TbAuthorizedDistributors");

            migrationBuilder.DropTable(
                name: "TbBlockProducts");

            migrationBuilder.DropTable(
                name: "TbBrandDocuments");

            migrationBuilder.DropTable(
                name: "TbBuyBoxCalculations");

            migrationBuilder.DropTable(
                name: "TbBuyBoxHistories");

            migrationBuilder.DropTable(
                name: "TbCampaignProducts");

            migrationBuilder.DropTable(
                name: "TbCampaignVendors");

            migrationBuilder.DropTable(
                name: "TbCategoryAttributes");

            migrationBuilder.DropTable(
                name: "TbCustomerSegmentPricings");

            migrationBuilder.DropTable(
                name: "TbDeliveryReviews");

            migrationBuilder.DropTable(
                name: "TbDisputeMessages");

            migrationBuilder.DropTable(
                name: "TbFlashSaleProducts");

            migrationBuilder.DropTable(
                name: "TbItemAttributes");

            migrationBuilder.DropTable(
                name: "TbItemImages");

            migrationBuilder.DropTable(
                name: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropTable(
                name: "TbMediaContents");

            migrationBuilder.DropTable(
                name: "TbNotificationChannels");

            migrationBuilder.DropTable(
                name: "TbNotificationPreferences");

            migrationBuilder.DropTable(
                name: "TbOfferPriceHistories");

            migrationBuilder.DropTable(
                name: "TbOfferStatusHistories");

            migrationBuilder.DropTable(
                name: "TbOrderPayments");

            migrationBuilder.DropTable(
                name: "TbOrderShipmentItems");

            migrationBuilder.DropTable(
                name: "TbPages");

            migrationBuilder.DropTable(
                name: "TbPlatformTreasuries");

            migrationBuilder.DropTable(
                name: "TbPriceHistories");

            migrationBuilder.DropTable(
                name: "TbQuantityPricings");

            migrationBuilder.DropTable(
                name: "TbRequestComments");

            migrationBuilder.DropTable(
                name: "TbRequestDocuments");

            migrationBuilder.DropTable(
                name: "TbReviewReports");

            migrationBuilder.DropTable(
                name: "TbReviewVotes");

            migrationBuilder.DropTable(
                name: "TbSalesReviews");

            migrationBuilder.DropTable(
                name: "TbSellerPerformanceMetrics");

            migrationBuilder.DropTable(
                name: "TbSellerTierBenefits");

            migrationBuilder.DropTable(
                name: "TbSettings");

            migrationBuilder.DropTable(
                name: "TbShippingDetails");

            migrationBuilder.DropTable(
                name: "TbShoppingCartItems");

            migrationBuilder.DropTable(
                name: "TbSupportTicketMessages");

            migrationBuilder.DropTable(
                name: "TbSuppressionReasons");

            migrationBuilder.DropTable(
                name: "TbUnitConversions");

            migrationBuilder.DropTable(
                name: "TbUserNotifications");

            migrationBuilder.DropTable(
                name: "TbUserOfferRatings");

            migrationBuilder.DropTable(
                name: "TbVendorTierHistories");

            migrationBuilder.DropTable(
                name: "TbVideoProviders");

            migrationBuilder.DropTable(
                name: "TbVisibilityLogs");

            migrationBuilder.DropTable(
                name: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TbCombinationAttributesValues");

            migrationBuilder.DropTable(
                name: "TbHomepageBlocks");

            migrationBuilder.DropTable(
                name: "TbBrandRegistrationRequests");

            migrationBuilder.DropTable(
                name: "TbCampaigns");

            migrationBuilder.DropTable(
                name: "TbDisputes");

            migrationBuilder.DropTable(
                name: "TbFlashSales");

            migrationBuilder.DropTable(
                name: "TbCustomerLoyalties");

            migrationBuilder.DropTable(
                name: "TbContentAreas");

            migrationBuilder.DropTable(
                name: "TbPaymentMethods");

            migrationBuilder.DropTable(
                name: "TbOrderDetails");

            migrationBuilder.DropTable(
                name: "TbOrderShipments");

            migrationBuilder.DropTable(
                name: "TbSellerRequests");

            migrationBuilder.DropTable(
                name: "TbProductReviews");

            migrationBuilder.DropTable(
                name: "TbShoppingCarts");

            migrationBuilder.DropTable(
                name: "TbSupportTickets");

            migrationBuilder.DropTable(
                name: "TbProductVisibilityRules");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TbSellerTiers");

            migrationBuilder.DropTable(
                name: "TbCustomerWallets");

            migrationBuilder.DropTable(
                name: "TbRefundRequests");

            migrationBuilder.DropTable(
                name: "TbVendorWallets");

            migrationBuilder.DropTable(
                name: "TbAttributes");

            migrationBuilder.DropTable(
                name: "TbCombinationAttributes");

            migrationBuilder.DropTable(
                name: "TbLoyaltyTiers");

            migrationBuilder.DropTable(
                name: "TbOfferCombinationPricings");

            migrationBuilder.DropTable(
                name: "TbWarehouses");

            migrationBuilder.DropTable(
                name: "TbOrders");

            migrationBuilder.DropTable(
                name: "TbCurrencies");

            migrationBuilder.DropTable(
                name: "TbItemCombinations");

            migrationBuilder.DropTable(
                name: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbVendors");

            migrationBuilder.DropTable(
                name: "TbCouponCodes");

            migrationBuilder.DropTable(
                name: "TbCustomerAddresses");

            migrationBuilder.DropTable(
                name: "TbShippingCompanies");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TbItems");

            migrationBuilder.DropTable(
                name: "TbOfferConditions");

            migrationBuilder.DropTable(
                name: "TbWarranties");

            migrationBuilder.DropTable(
                name: "TbCustomers");

            migrationBuilder.DropTable(
                name: "TbBrands");

            migrationBuilder.DropTable(
                name: "TbCategories");

            migrationBuilder.DropTable(
                name: "TbUnits");

            migrationBuilder.DropTable(
                name: "TbCities");

            migrationBuilder.DropTable(
                name: "TbPricingSystemSettings");

            migrationBuilder.DropTable(
                name: "TbStates");

            migrationBuilder.DropTable(
                name: "TbCountries");
        }
    }
}
