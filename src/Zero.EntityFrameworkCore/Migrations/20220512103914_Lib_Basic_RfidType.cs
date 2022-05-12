using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Lib_Basic_RfidType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lib_Basic_RfidType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardDer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    IsBlackList = table.Column<bool>(type: "bit", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Lib_Basic_RfidType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lib_Basic_RfidType_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lib_Basic_RfidType_UserId",
                table: "Lib_Basic_RfidType",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lib_Basic_RfidType");
        }
    }
}
