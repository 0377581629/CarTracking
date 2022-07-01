using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Transport_RouteDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lib_Transport_RouteDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    EndPointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lib_Transport_RouteDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_RouteDetail_Lib_Transport_Point_EndPointId",
                        column: x => x.EndPointId,
                        principalTable: "Lib_Transport_Point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lib_Transport_RouteDetail_Lib_Transport_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Lib_Transport_Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_RouteDetail_EndPointId",
                table: "Lib_Transport_RouteDetail",
                column: "EndPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Transport_RouteDetail_RouteId",
                table: "Lib_Transport_RouteDetail",
                column: "RouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lib_Transport_RouteDetail");
        }
    }
}
