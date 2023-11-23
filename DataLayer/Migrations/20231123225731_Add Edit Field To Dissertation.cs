using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddEditFieldToDissertation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Dissertations",
                newName: "RegisterDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDateTime",
                table: "Dissertations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateCnt",
                table: "Dissertations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ba95b2d7-be51-4dfc-a50d-833540df2672", "K7JCQNNN4ULGGODXGAHOHXHF2MHWMYZU" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditDateTime",
                table: "Dissertations");

            migrationBuilder.DropColumn(
                name: "UpdateCnt",
                table: "Dissertations");

            migrationBuilder.RenameColumn(
                name: "RegisterDateTime",
                table: "Dissertations",
                newName: "DateTime");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "70a52898-8da8-4928-a01a-0bf263f3ed06", null });
        }
    }
}
