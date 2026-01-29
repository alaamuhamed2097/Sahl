using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateTbBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbAuthorizedDistributors");

            migrationBuilder.DropTable(
                name: "TbBrandDocuments");

            migrationBuilder.DropTable(
                name: "TbBrandRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "TbBrands");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbAttributeOptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbAttributeOptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "TbAttributeOptions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_NameAr",
                table: "TbBrands",
                column: "NameAr");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_NameEn",
                table: "TbBrands",
                column: "NameEn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbBrands_NameAr",
                table: "TbBrands");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_NameEn",
                table: "TbBrands");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "TbAttributeOptions");

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "TbBrands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbAttributeOptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbAttributeOptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TbAuthorizedDistributors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorizationDocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AuthorizationEndDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuthorizationStartDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
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
                    ApprovedBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandType = table.Column<int>(type: "int", nullable: false),
                    CommercialRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OfficialWebsite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    TrademarkExpiryDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    TrademarkNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                name: "TbBrandDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandRegistrationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    VerificationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
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
                name: "IX_TbAuthorizedDistributors_IsActive",
                table: "TbAuthorizedDistributors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_IsDeleted",
                table: "TbAuthorizedDistributors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VendorId",
                table: "TbAuthorizedDistributors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId_DocumentType",
                table: "TbBrandDocuments",
                columns: new[] { "BrandRegistrationRequestId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_DocumentType",
                table: "TbBrandDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_IsDeleted",
                table: "TbBrandDocuments",
                column: "IsDeleted");

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
                name: "IX_TbBrandRegistrationRequests_IsDeleted",
                table: "TbBrandRegistrationRequests",
                column: "IsDeleted");

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
        }
    }
}
