using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class addcolumnbgimagetoslider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sliderimages");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage",
                table: "sliders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "sliders");

            migrationBuilder.CreateTable(
                name: "sliderimages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sliderimages", x => x.Id);
                });
        }
    }
}
