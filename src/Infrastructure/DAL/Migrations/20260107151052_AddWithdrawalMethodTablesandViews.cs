using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddWithdrawalMethodTablesandViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "TbVendors",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "TbWithdrawalMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWithdrawalMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    WithdrawalMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFields_TbWithdrawalMethods_WithdrawalMethodId",
                        column: x => x.WithdrawalMethodId,
                        principalTable: "TbWithdrawalMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbUserWithdrawalMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    WithdrawalMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserWithdrawalMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserWithdrawalMethods_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbUserWithdrawalMethods_TbWithdrawalMethods_WithdrawalMethodId",
                        column: x => x.WithdrawalMethodId,
                        principalTable: "TbWithdrawalMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbWithdrawalMethodFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserWithdrawalMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWithdrawalMethodFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWithdrawalMethodFields_TbFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "TbFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbWithdrawalMethodFields_TbUserWithdrawalMethods_UserWithdrawalMethodId",
                        column: x => x.UserWithdrawalMethodId,
                        principalTable: "TbUserWithdrawalMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbFields_FieldType",
                table: "TbFields",
                column: "FieldType");

            migrationBuilder.CreateIndex(
                name: "IX_TbFields_IsDeleted",
                table: "TbFields",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbFields_WithdrawalMethodId",
                table: "TbFields",
                column: "WithdrawalMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserWithdrawalMethods_IsDeleted",
                table: "TbUserWithdrawalMethods",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserWithdrawalMethods_UserId",
                table: "TbUserWithdrawalMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserWithdrawalMethods_WithdrawalMethodId",
                table: "TbUserWithdrawalMethods",
                column: "WithdrawalMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWithdrawalMethodFields_FieldId",
                table: "TbWithdrawalMethodFields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWithdrawalMethodFields_IsDeleted",
                table: "TbWithdrawalMethodFields",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbWithdrawalMethodFields_UserWithdrawalMethodId",
                table: "TbWithdrawalMethodFields",
                column: "UserWithdrawalMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWithdrawalMethods_IsDeleted",
                table: "TbWithdrawalMethods",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbWithdrawalMethodFields");

            migrationBuilder.DropTable(
                name: "TbFields");

            migrationBuilder.DropTable(
                name: "TbUserWithdrawalMethods");

            migrationBuilder.DropTable(
                name: "TbWithdrawalMethods");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "TbVendors",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
