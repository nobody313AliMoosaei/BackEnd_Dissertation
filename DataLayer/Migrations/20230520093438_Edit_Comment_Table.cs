using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Comment_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UesrId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UesrId",
                schema: "dbo",
                table: "Comments",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UesrId",
                schema: "dbo",
                table: "Comments",
                newName: "IX_Comments_SenderId");

            migrationBuilder.AddColumn<decimal>(
                name: "CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                column: "CommentsComment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_SenderId",
                schema: "dbo",
                table: "Comments",
                column: "SenderId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                column: "CommentsComment_Id",
                principalSchema: "dbo",
                principalTable: "Comments",
                principalColumn: "Comment_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_SenderId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CommentsComment_Id",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CommentsComment_Id",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "dbo",
                table: "Comments",
                newName: "UesrId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_SenderId",
                schema: "dbo",
                table: "Comments",
                newName: "IX_Comments_UesrId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UesrId",
                schema: "dbo",
                table: "Comments",
                column: "UesrId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
