using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class SddataAbout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Abouts",
                columns: new[] { "Id", "Description", "Image", "IsDeactive", "SubTitle", "Title" },
                values: new object[] { 3, "rtyuiof", "about.png", false, "<h3>drtfgyhuj</h3>", "<h2>bizim haqqimizda melumat</h2>" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Abouts",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
