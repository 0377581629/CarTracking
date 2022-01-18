using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class PostTagDetail_Update_Colum_IsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Cms_Post_Tag_Detail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Cms_Post_Tag_Detail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Cms_Post_Tag_Detail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Cms_Post_Tag_Detail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cms_Post_Tag_Detail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Cms_Post_Tag_Detail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Cms_Post_Tag_Detail",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Cms_Post_Tag_Detail");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Cms_Post_Tag_Detail");
        }
    }
}
