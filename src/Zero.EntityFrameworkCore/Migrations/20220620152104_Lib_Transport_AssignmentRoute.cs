using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Transport_AssignmentRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lib_Transport_AssignmentRoute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagementUnitId = table.Column<int>(type: "int", nullable: true),
                    CarId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    TreasurerId = table.Column<int>(type: "int", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    Guard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayOfWeeks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAssignment = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Lib_Transport_AssignmentRoute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Basic_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Lib_Basic_Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Basic_ManagementUnit_ManagementUnitId",
                        column: x => x.ManagementUnitId,
                        principalTable: "Lib_Basic_ManagementUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Basic_Technician_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Lib_Basic_Technician",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Basic_Treasurer_TreasurerId",
                        column: x => x.TreasurerId,
                        principalTable: "Lib_Basic_Treasurer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Transport_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Lib_Transport_Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_AssignmentRoute_Lib_Transport_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Lib_Transport_Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_CarId",
                table: "Lib_Transport_AssignmentRoute",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_DriverId",
                table: "Lib_Transport_AssignmentRoute",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_ManagementUnitId",
                table: "Lib_Transport_AssignmentRoute",
                column: "ManagementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_RouteId",
                table: "Lib_Transport_AssignmentRoute",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_TechnicianId",
                table: "Lib_Transport_AssignmentRoute",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_AssignmentRoute_TreasurerId",
                table: "Lib_Transport_AssignmentRoute",
                column: "TreasurerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lib_Transport_AssignmentRoute");
        }
    }
}
