using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class addcolumnIsDeactivetocoursecatigories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "CourseCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "CourseCategories");
        }
    }
}
