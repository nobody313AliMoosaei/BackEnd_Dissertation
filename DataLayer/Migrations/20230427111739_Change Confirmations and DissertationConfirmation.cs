using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConfirmationsandDissertationConfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confirmations_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Confirmations",
                schema: "dbo",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dbo",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "NormalizeName",
                schema: "dbo",
                table: "Confirmations");

            migrationBuilder.RenameTable(
                name: "Confirmations",
                schema: "dbo",
                newName: "Confirmation",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Confirmations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation",
                newName: "IX_Confirmation_ConfirmationsDissertationsId");

            migrationBuilder.AddColumn<int>(
                name: "Code_Dissertation_Confirmation",
                schema: "dbo",
                table: "Confirmation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Confirmation",
                schema: "dbo",
                table: "Confirmation",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Confirmation",
                columns: new[] { "Id", "Code_Dissertation_Confirmation", "ConfirmationsDissertationsId", "Name", "PersianName" },
                values: new object[,]
                {
                    { 1, 1, null, "ConfirmationGuideMaster", "تاییدیه استاد راهنمای اول" },
                    { 2, 2, null, "ConfirmationGuideMaster2", "تاییدیه استاد راهنمای دوم" },
                    { 3, 3, null, "ConfirmationGuideMaster3", "تاییدیه استاد راهنمای سوم" },
                    { 4, 4, null, "ConfirmationEducationExpert", "تاییدیه کارشناس آموزش" },
                    { 5, 5, null, "ConfirmationPostgraduateEducationExpert", "تاییدیه کارشناس تحصیلات تکمیلی" },
                    { 6, 6, null, "ConfirmationDissertationExpert", "تاییدیه کارشناس امور پایان نامه" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Confirmation_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation",
                column: "ConfirmationsDissertationsId",
                principalSchema: "dbo",
                principalTable: "ConfirmationsDissertations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confirmation_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Confirmation",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "Code_Dissertation_Confirmation",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.RenameTable(
                name: "Confirmation",
                schema: "dbo",
                newName: "Confirmations",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Confirmation_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmations",
                newName: "IX_Confirmations_ConfirmationsDissertationsId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizeName",
                schema: "dbo",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Confirmations",
                schema: "dbo",
                table: "Confirmations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Confirmations_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmations",
                column: "ConfirmationsDissertationsId",
                principalSchema: "dbo",
                principalTable: "ConfirmationsDissertations",
                principalColumn: "Id");
        }
    }
}
