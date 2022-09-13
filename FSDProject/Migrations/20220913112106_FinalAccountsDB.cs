using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSDProjectAPI.Migrations
{
    public partial class FinalAccountsDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Accounts");
        }
    }
}
