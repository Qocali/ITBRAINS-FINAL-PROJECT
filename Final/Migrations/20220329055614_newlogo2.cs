using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class newlogo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "bios",
                columns: new[] { "Id", "Facebook", "Logo", "Pinterest", "Twitter", "Vimeo" },
                values: new object[] { 3, "https://www.facebook.com/qocheli.babayev", "footer-logo", "https://www.Pinterest.com/in/qocheli-babayev-141009218", "https://www.linkedin.com/in/qocheli-babayev-141009218", "https://www.Vimeo.com/qocheli.babayev" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "bios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "bios",
                columns: new[] { "Id", "Facebook", "Logo", "Pinterest", "Twitter", "Vimeo" },
                values: new object[] { 2, "https://www.facebook.com/qocheli.babayev", "footer-logo", "https://www.Pinterest.com/in/qocheli-babayev-141009218", "https://www.linkedin.com/in/qocheli-babayev-141009218", "https://www.Vimeo.com/qocheli.babayev" });
        }
    }
}
