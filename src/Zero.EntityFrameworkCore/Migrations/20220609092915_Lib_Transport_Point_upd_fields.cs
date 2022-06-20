using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Transport_Point_upd_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Lib_Transport_Point",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Lib_Transport_Point",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Lib_Transport_Point",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Lib_Transport_Point",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Lib_Transport_Point",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ManagementUnitId",
                table: "Lib_Transport_Point",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Lib_Transport_Point",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointTypeId",
                table: "Lib_Transport_Point",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Point_ManagementUnitId",
                table: "Lib_Transport_Point",
                column: "ManagementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_Point_PointTypeId",
                table: "Lib_Transport_Point",
                column: "PointTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lib_Transport_Point_Lib_Basic_ManagementUnit_ManagementUnitId",
                table: "Lib_Transport_Point",
                column: "ManagementUnitId",
                principalTable: "Lib_Basic_ManagementUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lib_Transport_Point_Lib_Transport_PointType_PointTypeId",
                table: "Lib_Transport_Point",
                column: "PointTypeId",
                principalTable: "Lib_Transport_PointType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lib_Transport_Point_Lib_Basic_ManagementUnit_ManagementUnitId",
                table: "Lib_Transport_Point");

            migrationBuilder.DropForeignKey(
                name: "FK_Lib_Transport_Point_Lib_Transport_PointType_PointTypeId",
                table: "Lib_Transport_Point");

            migrationBuilder.DropIndex(
                name: "IX_Lib_Transport_Point_ManagementUnitId",
                table: "Lib_Transport_Point");

            migrationBuilder.DropIndex(
                name: "IX_Lib_Transport_Point_PointTypeId",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "ManagementUnitId",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Lib_Transport_Point");

            migrationBuilder.DropColumn(
                name: "PointTypeId",
                table: "Lib_Transport_Point");
        }
    }
}
