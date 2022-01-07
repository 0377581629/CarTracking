using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Cms_Menu_Add_ParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Cms_Menu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Menu_ParentId",
                table: "Cms_Menu",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cms_Menu_Cms_Menu_ParentId",
                table: "Cms_Menu",
                column: "ParentId",
                principalTable: "Cms_Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cms_Menu_Cms_Menu_ParentId",
                table: "Cms_Menu");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Menu_ParentId",
                table: "Cms_Menu");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Cms_Menu");
        }
    }
}
