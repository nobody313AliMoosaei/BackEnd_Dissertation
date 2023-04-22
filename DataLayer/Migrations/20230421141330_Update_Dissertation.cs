using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Update_Dissertation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status_Dissertation",
                schema: "dbo",
                table: "Dissertations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KeyWords_DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords",
                column: "DissertationsDissertation_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyWords_Dissertations_DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords",
                column: "DissertationsDissertation_Id1",
                principalSchema: "dbo",
                principalTable: "Dissertations",
                principalColumn: "Dissertation_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyWords_Dissertations_DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords");

            migrationBuilder.DropIndex(
                name: "IX_KeyWords_DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords");

            migrationBuilder.DropColumn(
                name: "DissertationsDissertation_Id1",
                schema: "dbo",
                table: "KeyWords");

            migrationBuilder.DropColumn(
                name: "Status_Dissertation",
                schema: "dbo",
                table: "Dissertations");
        }
    }
}
