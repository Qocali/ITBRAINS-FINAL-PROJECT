using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class logochange3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Logo",
                value: "logo2.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Logo",
                value: "logo.png");
        }
    }
}
