using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Transport_Route : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lib_Transport_Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagementUnitId = table.Column<int>(type: "int", nullable: true),
                    ListPoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPermanentRoute = table.Column<bool>(type: "bit", nullable: false),
                    MinuteLate = table.Column<double>(type: "float", nullable: false),
                    Range = table.Column<double>(type: "float", nullable: false),
                    HasConstraintTime = table.Column<bool>(type: "bit", nullable: false),
                    RouteType = table.Column<int>(type: "int", nullable: false),
                    EstimateDistance = table.Column<double>(type: "float", nullable: false),
                    EstimatedTime = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_Lib_Transport_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Route_Lib_Basic_ManagementUnit_ManagementUnitId",
                        column: x => x.ManagementUnitId,
                        principalTable: "Lib_Basic_ManagementUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Route_ManagementUnitId",
                table: "Lib_Transport_Route",
                column: "ManagementUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lib_Transport_Route");
        }
    }
}
