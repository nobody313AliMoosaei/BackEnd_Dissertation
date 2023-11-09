using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddDissertation_StatusToBaslookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BASLookup",
                columns: new[] { "Id", "Code", "Description", "Title", "Type" },
                values: new object[,]
                {
                    { 12L, 0, "ثبت اوليه پایان نامه", "Register", "DissertationStatus" },
                    { 13L, 1, "تاییدیه استاد راهنمای اول", "ConfirmationGuideMaster", "DissertationStatus" },
                    { 14L, 2, "تاییدیه استاد راهنمای دوم", "ConfirmationGuideMaster2", "DissertationStatus" },
                    { 15L, 3, "تاییدیه استاد راهنمای سوم", "ConfirmationGuideMaster3", "DissertationStatus" },
                    { 16L, 4, "تاییدیه کارشناس آموزش", "ConfirmationEducationExpert", "DissertationStatus" },
                    { 17L, 5, "تاییدیه کارشناس تحصیلات تکمیلی", "ConfirmationPostgraduateEducationExpert", "DissertationStatus" },
                    { 18L, 6, "تاییدیه کارشناس امور پایان نامه", "ConfirmationDissertationExpert", "DissertationStatus" },
                    { 19L, -3333, "رد پایان نامه", "ExpirDissertation", "DissertationStatus" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 19L);
        }
    }
}
