using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class nullgiventocourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CourseCatigoriesId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses",
                column: "CourseCatigoriesId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "CourseCatigoriesId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses",
                column: "CourseCatigoriesId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
