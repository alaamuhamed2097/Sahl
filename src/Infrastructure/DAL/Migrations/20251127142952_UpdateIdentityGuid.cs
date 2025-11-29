using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityGuid : Migration
    {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                // Step 1: Drop all foreign key constraints that reference AspNetUsers.Id
                migrationBuilder.DropForeignKey(name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId", table: "TbPriceHistories");
                migrationBuilder.DropForeignKey(name: "FK_TbMortems_AspNetUsers_UserId", table: "TbMortems");
                migrationBuilder.DropForeignKey(name: "FK_TbMoitems_AspNetUsers_UserId", table: "TbMoitems");
                migrationBuilder.DropForeignKey(name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId", table: "TbCampaignVendors");
                migrationBuilder.DropForeignKey(name: "FK_TbOffers_AspNetUsers_UserId", table: "TbOffers");
                migrationBuilder.DropForeignKey(name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId", table: "TbCampaignProducts");
                migrationBuilder.DropForeignKey(name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId", table: "TbBrandDocuments");
                migrationBuilder.DropForeignKey(name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId", table: "TbVisibilityLogs");
                migrationBuilder.DropForeignKey(name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId", table: "TbBrandRegistrationRequests");
                migrationBuilder.DropForeignKey(name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId", table: "TbWalletTransactions");
                migrationBuilder.DropForeignKey(name: "FK_TbUserNotifications_AspNetUsers_UserId", table: "TbUserNotifications");
                migrationBuilder.DropForeignKey(name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId", table: "TbAuthorizedDistributors");
                migrationBuilder.DropForeignKey(name: "FK_TbOrders_AspNetUsers_UserId", table: "TbOrders");
                migrationBuilder.DropForeignKey(name: "FK_NotificationPreferences_AspNetUsers_UserId", table: "NotificationPreferences");
                migrationBuilder.DropForeignKey(name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId", table: "TbSellerRequests");
                migrationBuilder.DropForeignKey(name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId", table: "TbRequestDocuments");
                migrationBuilder.DropForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId", table: "AspNetUserTokens");
                migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId", table: "AspNetUserRoles");
                migrationBuilder.DropForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId", table: "AspNetUserLogins");
                migrationBuilder.DropForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId", table: "AspNetUserClaims");
                migrationBuilder.DropForeignKey(name: "FK_TbRequestComments_AspNetUsers_UserId", table: "TbRequestComments");
                migrationBuilder.DropForeignKey(name: "FK_TbUserOfferRatings_AspNetUsers_UserId", table: "TbUserOfferRatings");
                migrationBuilder.DropForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests");
                migrationBuilder.DropForeignKey(name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId", table: "TbProductVisibilityRules");

                // Step 2: Drop foreign key constraints that reference AspNetRoles.Id
                migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId", table: "AspNetUserRoles");
                migrationBuilder.DropForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", table: "AspNetRoleClaims");

                // Step 3: Drop primary keys
                migrationBuilder.DropPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers");
                migrationBuilder.DropPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles");
                migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles");
                migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens");
                migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserLogins", table: "AspNetUserLogins");

                // Step 4: Alter columns to GUID
                // AspNetUsers
                migrationBuilder.AlterColumn<Guid>(
                    name: "Id",
                    table: "AspNetUsers",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetRoles
                migrationBuilder.AlterColumn<Guid>(
                    name: "Id",
                    table: "AspNetRoles",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetUserRoles
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: "AspNetUserRoles",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                migrationBuilder.AlterColumn<Guid>(
                    name: "RoleId",
                    table: "AspNetUserRoles",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetUserClaims
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: "AspNetUserClaims",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetUserLogins
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: "AspNetUserLogins",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetUserTokens
                migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: "AspNetUserTokens",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // AspNetRoleClaims
                migrationBuilder.AlterColumn<Guid>(
                    name: "RoleId",
                    table: "AspNetRoleClaims",
                    type: "uniqueidentifier",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // Step 5: Alter foreign key columns in custom tables
                migrationBuilder.AlterColumn<Guid>(name: "ChangedByUserId", table: "TbPriceHistories", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbMortems", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbMoitems", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "ApprovedByUserId", table: "TbCampaignVendors", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbOffers", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "ApprovedByUserId", table: "TbCampaignProducts", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "VerifiedByUserId", table: "TbBrandDocuments", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "ChangedByUserId", table: "TbVisibilityLogs", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "ReviewedByUserId", table: "TbBrandRegistrationRequests", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "ProcessedByUserId", table: "TbWalletTransactions", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbUserNotifications", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "VerifiedByUserId", table: "TbAuthorizedDistributors", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbOrders", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "NotificationPreferences", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "ReviewedByUserId", table: "TbSellerRequests", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "UploadedByUserId", table: "TbRequestDocuments", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbRequestComments", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "UserId", table: "TbUserOfferRatings", type: "uniqueidentifier", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)");
                migrationBuilder.AlterColumn<Guid>(name: "AdminUserId", table: "TbRefundRequests", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
                migrationBuilder.AlterColumn<Guid>(name: "SuppressedByUserId", table: "TbProductVisibilityRules", type: "uniqueidentifier", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);

                // Step 6: Recreate primary keys
                migrationBuilder.AddPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers", column: "Id");
                migrationBuilder.AddPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles", column: "Id");
                migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles", columns: new[] { "UserId", "RoleId" });
                migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens", columns: new[] { "UserId", "LoginProvider", "Name" });
                migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserLogins", table: "AspNetUserLogins", columns: new[] { "LoginProvider", "ProviderKey" });

                // Step 7: Recreate foreign key constraints
                migrationBuilder.AddForeignKey(name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId", table: "TbPriceHistories", column: "ChangedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbMortems_AspNetUsers_UserId", table: "TbMortems", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbMoitems_AspNetUsers_UserId", table: "TbMoitems", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId", table: "TbCampaignVendors", column: "ApprovedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbOffers_AspNetUsers_UserId", table: "TbOffers", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId", table: "TbCampaignProducts", column: "ApprovedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId", table: "TbBrandDocuments", column: "VerifiedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId", table: "TbVisibilityLogs", column: "ChangedByUserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId", table: "TbBrandRegistrationRequests", column: "ReviewedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId", table: "TbWalletTransactions", column: "ProcessedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbUserNotifications_AspNetUsers_UserId", table: "TbUserNotifications", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId", table: "TbAuthorizedDistributors", column: "VerifiedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbOrders_AspNetUsers_UserId", table: "TbOrders", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_NotificationPreferences_AspNetUsers_UserId", table: "NotificationPreferences", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId", table: "TbSellerRequests", column: "ReviewedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId", table: "TbRequestDocuments", column: "UploadedByUserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId", table: "AspNetUserTokens", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId", table: "AspNetUserRoles", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId", table: "AspNetUserLogins", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId", table: "AspNetUserClaims", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbRequestComments_AspNetUsers_UserId", table: "TbRequestComments", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbUserOfferRatings_AspNetUsers_UserId", table: "TbUserOfferRatings", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests", column: "AdminUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId", table: "TbProductVisibilityRules", column: "SuppressedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
                migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId", table: "AspNetUserRoles", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                migrationBuilder.AddForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", table: "AspNetRoleClaims", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Drop all foreign key constraints
            migrationBuilder.DropForeignKey(name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId", table: "TbPriceHistories");
            migrationBuilder.DropForeignKey(name: "FK_TbMortems_AspNetUsers_UserId", table: "TbMortems");
            migrationBuilder.DropForeignKey(name: "FK_TbMoitems_AspNetUsers_UserId", table: "TbMoitems");
            migrationBuilder.DropForeignKey(name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId", table: "TbCampaignVendors");
            migrationBuilder.DropForeignKey(name: "FK_TbOffers_AspNetUsers_UserId", table: "TbOffers");
            migrationBuilder.DropForeignKey(name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId", table: "TbCampaignProducts");
            migrationBuilder.DropForeignKey(name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId", table: "TbBrandDocuments");
            migrationBuilder.DropForeignKey(name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId", table: "TbVisibilityLogs");
            migrationBuilder.DropForeignKey(name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId", table: "TbBrandRegistrationRequests");
            migrationBuilder.DropForeignKey(name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId", table: "TbWalletTransactions");
            migrationBuilder.DropForeignKey(name: "FK_TbUserNotifications_AspNetUsers_UserId", table: "TbUserNotifications");
            migrationBuilder.DropForeignKey(name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId", table: "TbAuthorizedDistributors");
            migrationBuilder.DropForeignKey(name: "FK_TbOrders_AspNetUsers_UserId", table: "TbOrders");
            migrationBuilder.DropForeignKey(name: "FK_NotificationPreferences_AspNetUsers_UserId", table: "NotificationPreferences");
            migrationBuilder.DropForeignKey(name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId", table: "TbSellerRequests");
            migrationBuilder.DropForeignKey(name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId", table: "TbRequestDocuments");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId", table: "AspNetUserTokens");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId", table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId", table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_TbRequestComments_AspNetUsers_UserId", table: "TbRequestComments");
            migrationBuilder.DropForeignKey(name: "FK_TbUserOfferRatings_AspNetUsers_UserId", table: "TbUserOfferRatings");
            migrationBuilder.DropForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests");
            migrationBuilder.DropForeignKey(name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId", table: "TbProductVisibilityRules");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", table: "AspNetRoleClaims");

            // Step 2: Drop primary keys
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserLogins", table: "AspNetUserLogins");

            // Step 3: Alter columns back to string
            // AspNetUsers
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetRoles
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetUserRoles
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetUserClaims
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetUserLogins
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetUserTokens
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // AspNetRoleClaims
            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Step 4: Alter foreign key columns in custom tables back to string
            migrationBuilder.AlterColumn<string>(name: "ChangedByUserId", table: "TbPriceHistories", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbMortems", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbMoitems", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "ApprovedByUserId", table: "TbCampaignVendors", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbOffers", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "ApprovedByUserId", table: "TbCampaignProducts", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "VerifiedByUserId", table: "TbBrandDocuments", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "ChangedByUserId", table: "TbVisibilityLogs", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "ReviewedByUserId", table: "TbBrandRegistrationRequests", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "ProcessedByUserId", table: "TbWalletTransactions", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbUserNotifications", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "VerifiedByUserId", table: "TbAuthorizedDistributors", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbOrders", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "NotificationPreferences", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "ReviewedByUserId", table: "TbSellerRequests", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UploadedByUserId", table: "TbRequestDocuments", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbRequestComments", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbUserOfferRatings", type: "nvarchar(450)", nullable: false, oldClrType: typeof(Guid), oldType: "uniqueidentifier");
            migrationBuilder.AlterColumn<string>(name: "AdminUserId", table: "TbRefundRequests", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "SuppressedByUserId", table: "TbProductVisibilityRules", type: "nvarchar(450)", nullable: true, oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);

            // Step 5: Recreate primary keys
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles", columns: new[] { "UserId", "RoleId" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens", columns: new[] { "UserId", "LoginProvider", "Name" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserLogins", table: "AspNetUserLogins", columns: new[] { "LoginProvider", "ProviderKey" });

            // Step 6: Recreate foreign key constraints
            migrationBuilder.AddForeignKey(name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId", table: "TbPriceHistories", column: "ChangedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbMortems_AspNetUsers_UserId", table: "TbMortems", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbMoitems_AspNetUsers_UserId", table: "TbMoitems", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId", table: "TbCampaignVendors", column: "ApprovedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbOffers_AspNetUsers_UserId", table: "TbOffers", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId", table: "TbCampaignProducts", column: "ApprovedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId", table: "TbBrandDocuments", column: "VerifiedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId", table: "TbVisibilityLogs", column: "ChangedByUserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId", table: "TbBrandRegistrationRequests", column: "ReviewedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId", table: "TbWalletTransactions", column: "ProcessedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbUserNotifications_AspNetUsers_UserId", table: "TbUserNotifications", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId", table: "TbAuthorizedDistributors", column: "VerifiedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbOrders_AspNetUsers_UserId", table: "TbOrders", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_NotificationPreferences_AspNetUsers_UserId", table: "NotificationPreferences", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId", table: "TbSellerRequests", column: "ReviewedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId", table: "TbRequestDocuments", column: "UploadedByUserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId", table: "AspNetUserTokens", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId", table: "AspNetUserRoles", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId", table: "AspNetUserLogins", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId", table: "AspNetUserClaims", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbRequestComments_AspNetUsers_UserId", table: "TbRequestComments", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbUserOfferRatings_AspNetUsers_UserId", table: "TbUserOfferRatings", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests", column: "AdminUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId", table: "TbProductVisibilityRules", column: "SuppressedByUserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId", table: "AspNetUserRoles", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId", table: "AspNetRoleClaims", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
        }
    }
}
