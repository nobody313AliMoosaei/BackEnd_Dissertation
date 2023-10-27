using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class SetRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name_Persian",
                table: "AspNetRoles",
                newName: "PersianName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "PersianName" },
                values: new object[,]
                {
                    { 1L, null, "Administrator", "ADMINISTRATOR", "مالک" },
                    { 2L, null, "Student", "STUDENT", "دانشجو" },
                    { 3L, null, "GuideMaster", "GUIDEMASTER", "استاد راهنما" },
                    { 4L, null, "Adviser", "ADVISER", "مشاور" },
                    { 5L, null, "EducationExpert", "EDUCATIONEXPERT", "کارشناس آموزش" },
                    { 6L, null, "PostgraduateEducationExpert", "POSTGRADUATEEDUCATIONEXPERT", "کارشناس تحصیلات تکمیلی" },
                    { 7L, null, "DissertationExpert", "DISSERTATIONEXPERT", "کارشناس امور پایان نامه" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.RenameColumn(
                name: "PersianName",
                table: "AspNetRoles",
                newName: "Name_Persian");
        }
    }
}
