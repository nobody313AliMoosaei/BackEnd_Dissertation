using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ReplayComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Comments__Invers__7F2BE32F",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_InversCommentRef",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "InversCommentRef",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "8657a778-a0fb-4a6f-918d-6ceb4e3f31ff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InversCommentRef",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1c9a2dfc-df1c-4c84-8e32-c3412cba7986");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InversCommentRef",
                table: "Comments",
                column: "InversCommentRef");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__Invers__7F2BE32F",
                table: "Comments",
                column: "InversCommentRef",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
