using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeToList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationsDissertations_UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmationsDissertations_Users_UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmationsDissertations_Users_UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationsDissertations_UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "ConfirmationsDissertations");
        }
    }
}
