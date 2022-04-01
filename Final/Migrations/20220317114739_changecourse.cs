using Microsoft.EntityFrameworkCore.Migrations;

namespace Final.Migrations
{
    public partial class changecourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseCatigoriesId",
                table: "Courses",
                newName: "CourseCategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CourseCatigoriesId",
                table: "Courses",
                newName: "IX_Courses_CourseCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoriesId",
                table: "Courses",
                column: "CourseCategoriesId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoriesId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseCategoriesId",
                table: "Courses",
                newName: "CourseCatigoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CourseCategoriesId",
                table: "Courses",
                newName: "IX_Courses_CourseCatigoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCatigoriesId",
                table: "Courses",
                column: "CourseCatigoriesId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
