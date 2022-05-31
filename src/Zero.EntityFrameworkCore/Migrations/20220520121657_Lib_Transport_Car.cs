using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Transport_Car : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lib_Transport_Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    CarTypeId = table.Column<int>(type: "int", nullable: true),
                    CarGroupId = table.Column<int>(type: "int", nullable: true),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    RfidTypeId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    Quota = table.Column<double>(type: "float", nullable: false),
                    SpeedLimit = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Lib_Transport_Car", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Car_Lib_Basic_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Lib_Basic_Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Car_Lib_Basic_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Lib_Basic_Driver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Car_Lib_Basic_RfidType_RfidTypeId",
                        column: x => x.RfidTypeId,
                        principalTable: "Lib_Basic_RfidType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Car_Lib_Transport_CarGroup_CarGroupId",
                        column: x => x.CarGroupId,
                        principalTable: "Lib_Transport_CarGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_Car_Lib_Transport_CarType_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "Lib_Transport_CarType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Car_CarGroupId",
                table: "Lib_Transport_Car",
                column: "CarGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Car_CarTypeId",
                table: "Lib_Transport_Car",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Car_DeviceId",
                table: "Lib_Transport_Car",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Car_DriverId",
                table: "Lib_Transport_Car",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Car_RfidTypeId",
                table: "Lib_Transport_Car",
                column: "RfidTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lib_Transport_Car");
        }
    }
}
