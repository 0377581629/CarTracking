using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Abp_Customize_Add_DashboardWidgets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpDashboardWidgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WidgetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<byte>(type: "tinyint", nullable: false),
                    Width = table.Column<byte>(type: "tinyint", nullable: false),
                    PositionX = table.Column<byte>(type: "tinyint", nullable: false),
                    PositionY = table.Column<byte>(type: "tinyint", nullable: false),
                    ViewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CssPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDashboardWidgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEditionDashboardWidgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditionId = table.Column<int>(type: "int", nullable: false),
                    DashboardWidgetId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEditionDashboardWidgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleDashboardWidgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    DashboardWidgetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleDashboardWidgets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpDashboardWidgets");

            migrationBuilder.DropTable(
                name: "AbpEditionDashboardWidgets");

            migrationBuilder.DropTable(
                name: "AbpRoleDashboardWidgets");
        }
    }
}
