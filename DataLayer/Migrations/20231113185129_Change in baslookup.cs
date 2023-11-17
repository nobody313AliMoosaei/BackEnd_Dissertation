using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Changeinbaslookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aux",
                table: "BASLookup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Aux2",
                table: "BASLookup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "730f5db1-147b-4552-8428-4685e05671a9");

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "Aux", "Aux2" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "Register", "ثبت اوليه پایان نامه" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationGuideMaster", "تاییدیه استاد راهنمای اول" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationGuideMaster2", "تاییدیه استاد راهنمای دوم" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationGuideMaster3", "تاییدیه استاد راهنمای سوم" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationEducationExpert", "تاییدیه کارشناس آموزش" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationPostgraduateEducationExpert", "تاییدیه کارشناس تحصیلات تکمیلی" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ConfirmationDissertationExpert", "تاییدیه کارشناس امور پایان نامه" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "Aux", "Aux2", "Description", "Title" },
                values: new object[] { null, null, "ExpirDissertation", "رد پایان نامه" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aux",
                table: "BASLookup");

            migrationBuilder.DropColumn(
                name: "Aux2",
                table: "BASLookup");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ConcurrencyStamp",
                value: "55261ef8-3887-43a2-869c-76c5fd2e0590");

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "ثبت اوليه پایان نامه", "Register" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه استاد راهنمای اول", "ConfirmationGuideMaster" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه استاد راهنمای دوم", "ConfirmationGuideMaster2" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه استاد راهنمای سوم", "ConfirmationGuideMaster3" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه کارشناس آموزش", "ConfirmationEducationExpert" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه کارشناس تحصیلات تکمیلی", "ConfirmationPostgraduateEducationExpert" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تاییدیه کارشناس امور پایان نامه", "ConfirmationDissertationExpert" });

            migrationBuilder.UpdateData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رد پایان نامه", "ExpirDissertation" });
        }
    }
}
