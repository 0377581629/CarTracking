using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Abp_Customize_EmailTemplate_Add_EmailTemplateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateType",
                table: "AbpEmailTemplates",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailTemplateType",
                table: "AbpEmailTemplates");
        }
    }
}
