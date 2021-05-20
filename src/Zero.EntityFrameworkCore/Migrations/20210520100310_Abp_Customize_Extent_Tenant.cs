using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Abp_Customize_Extent_Tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginBackgroundFileType",
                table: "AbpTenants",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LoginBackgroundId",
                table: "AbpTenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginLogoFileType",
                table: "AbpTenants",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LoginLogoId",
                table: "AbpTenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuLogoFileType",
                table: "AbpTenants",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MenuLogoId",
                table: "AbpTenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "AbpTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebAuthor",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebDescription",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebFavicon",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebKeyword",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebTitle",
                table: "AbpTenants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "LoginBackgroundFileType",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "LoginBackgroundId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "LoginLogoFileType",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "LoginLogoId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "MenuLogoFileType",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "MenuLogoId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "WebAuthor",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "WebDescription",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "WebFavicon",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "WebKeyword",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "WebTitle",
                table: "AbpTenants");
        }
    }
}
