using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTimeTODissertation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Word_English",
                schema: "dbo",
                table: "KeyWords");

            migrationBuilder.DropColumn(
                name: "Insert_DateTime",
                schema: "dbo",
                table: "Dissertations");

            migrationBuilder.RenameColumn(
                name: "Word_Persion",
                schema: "dbo",
                table: "KeyWords",
                newName: "Word");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                schema: "dbo",
                table: "Dissertations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                schema: "dbo",
                table: "Dissertations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "dbo",
                table: "Dissertations");

            migrationBuilder.DropColumn(
                name: "Time",
                schema: "dbo",
                table: "Dissertations");

            migrationBuilder.RenameColumn(
                name: "Word",
                schema: "dbo",
                table: "KeyWords",
                newName: "Word_Persion");

            migrationBuilder.AddColumn<string>(
                name: "Word_English",
                schema: "dbo",
                table: "KeyWords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Insert_DateTime",
                schema: "dbo",
                table: "Dissertations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
