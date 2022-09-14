using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSDProjectAPI.Migrations
{
    public partial class UserTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassworfSalt",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "Medicines",
                newName: "ExpirationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Users",
                newName: "PassworfSalt");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Medicines",
                newName: "ExpireDate");
        }
    }
}
