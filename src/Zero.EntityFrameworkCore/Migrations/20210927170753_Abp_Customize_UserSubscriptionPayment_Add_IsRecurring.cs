using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Abp_Customize_UserSubscriptionPayment_Add_IsRecurring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "AppUserSubscriptionPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "AppUserSubscriptionPayments");
        }
    }
}
