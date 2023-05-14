using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class editconfirmationdissertation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confirmation_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.DropIndex(
                name: "IX_Confirmation_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.DropColumn(
                name: "ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationsDissertations_ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                column: "ConfirmationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationsDissertations_Confirmation_ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                column: "ConfirmationId",
                principalSchema: "dbo",
                principalTable: "Confirmation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationsDissertations_Confirmation_ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationsDissertations_ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations");

            migrationBuilder.DropColumn(
                name: "ConfirmationId",
                schema: "dbo",
                table: "ConfirmationsDissertations");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Confirmation",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConfirmationsDissertationsId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Confirmation_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation",
                column: "ConfirmationsDissertationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Confirmation_ConfirmationsDissertations_ConfirmationsDissertationsId",
                schema: "dbo",
                table: "Confirmation",
                column: "ConfirmationsDissertationsId",
                principalSchema: "dbo",
                principalTable: "ConfirmationsDissertations",
                principalColumn: "Id");
        }
    }
}
