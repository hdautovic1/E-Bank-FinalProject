using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Bank_FinalProject.Migrations
{
    public partial class Accountnumberadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Account",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Account");
        }
    }
}
