using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTeacherNavigationinUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserTB_Us__Teach__267ABA7A",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherID",
                table: "Teachers");

            migrationBuilder.AddColumn<long>(
                name: "TeacherNavigationId",
                table: "Teachers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherNavigationId",
                table: "Teachers",
                column: "TeacherNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_TeacherNavigationId",
                table: "Teachers",
                column: "TeacherNavigationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_TeacherNavigationId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherNavigationId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TeacherNavigationId",
                table: "Teachers");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherID",
                table: "Teachers",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK__UserTB_Us__Teach__267ABA7A",
                table: "Teachers",
                column: "TeacherID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
