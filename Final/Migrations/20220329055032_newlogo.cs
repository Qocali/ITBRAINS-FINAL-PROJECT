using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class newlogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Logo",
                value: "footer-logo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Logo",
                value: "logo2.png");
        }
    }
}
