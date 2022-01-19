using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Cms_Post_Update_Colum_CategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cms_Post_Cms_Category_CategoryId",
                table: "Cms_Post");

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
    }
}
