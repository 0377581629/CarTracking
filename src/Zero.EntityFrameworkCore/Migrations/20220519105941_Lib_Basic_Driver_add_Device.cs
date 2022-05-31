using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Basic_Driver_add_Device : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Lib_Basic_Driver",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Basic_Driver_DeviceId",
                table: "Lib_Basic_Driver",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lib_Basic_Driver_Lib_Basic_Device_DeviceId",
                table: "Lib_Basic_Driver",
                column: "DeviceId",
                principalTable: "Lib_Basic_Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lib_Basic_Driver_Lib_Basic_Device_DeviceId",
                table: "Lib_Basic_Driver");

            migrationBuilder.DropIndex(
                name: "IX_Lib_Basic_Driver_DeviceId",
                table: "Lib_Basic_Driver");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Lib_Basic_Driver");
        }
    }
}
