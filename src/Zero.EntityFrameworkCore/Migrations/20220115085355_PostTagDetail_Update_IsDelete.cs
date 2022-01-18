using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class PostTagDetail_Update_IsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Cms_Post_Category_Detail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Cms_Post_Category_Detail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Cms_Post_Category_Detail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Cms_Post_Category_Detail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cms_Post_Category_Detail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Cms_Post_Category_Detail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Cms_Post_Category_Detail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Cms_Post",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post",
                column: "CategoryId",
                principalTable: "Cms_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Cms_Post_Category_Detail");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Cms_Post",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post",
                column: "CategoryId",
                principalTable: "Cms_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
