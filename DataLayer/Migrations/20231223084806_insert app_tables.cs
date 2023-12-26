using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class insertapp_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "bedacef9-daaa-431d-b395-d87e58e878cd");

            migrationBuilder.InsertData(
                table: "BASLookup",
                columns: new[] { "Id", "Aux", "Aux2", "Code", "Description", "Title", "Type" },
                values: new object[,]
                {
                    { 20L, null, null, 0, "جدول نقش های سیستم", "AspNetRoles", "App_Table" },
                    { 21L, null, null, 1, "جدول تمام کاربران سیستم", "AspNetUsers", "App_Table" },
                    { 22L, null, null, 2, "جدول تمام کامنت های سیستم", "Comments", "App_Table" },
                    { 23L, null, null, 3, "جدول تمام پایان نامه های سیستم", "Dissertations", "App_Table" },
                    { 24L, null, null, 4, "جدول تمام لاگ های سیستم", "Logs", "App_Table" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "BASLookup",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2905fb1d-c703-49f8-8885-53e19eee51fd");
        }
    }
}
