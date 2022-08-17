using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Bank_FinalProject.Migrations
{
    public partial class Balanceadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Account",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Account");
        }
    }
}
