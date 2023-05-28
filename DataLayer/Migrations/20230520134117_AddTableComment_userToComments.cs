using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddTableComment_userToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "Comment_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_Users_UserId",
                schema: "dbo",
                table: "Comment_User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment_User",
                schema: "dbo",
                table: "Comment_User");

            migrationBuilder.RenameTable(
                name: "Comment_User",
                schema: "dbo",
                newName: "CommentsUser",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "CommentsUser",
                newName: "User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_User_UserId",
                schema: "dbo",
                table: "CommentsUser",
                newName: "IX_CommentsUser_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_User_CommentsComment_Id",
                schema: "dbo",
                table: "CommentsUser",
                newName: "IX_CommentsUser_CommentsComment_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentsUser",
                schema: "dbo",
                table: "CommentsUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsUser_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "CommentsUser",
                column: "CommentsComment_Id",
                principalSchema: "dbo",
                principalTable: "Comments",
                principalColumn: "Comment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsUser_Users_User_Id",
                schema: "dbo",
                table: "CommentsUser",
                column: "User_Id",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentsUser_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "CommentsUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsUser_Users_User_Id",
                schema: "dbo",
                table: "CommentsUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentsUser",
                schema: "dbo",
                table: "CommentsUser");

            migrationBuilder.RenameTable(
                name: "CommentsUser",
                schema: "dbo",
                newName: "Comment_User",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                schema: "dbo",
                table: "Comment_User",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsUser_User_Id",
                schema: "dbo",
                table: "Comment_User",
                newName: "IX_Comment_User_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsUser_CommentsComment_Id",
                schema: "dbo",
                table: "Comment_User",
                newName: "IX_Comment_User_CommentsComment_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment_User",
                schema: "dbo",
                table: "Comment_User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "Comment_User",
                column: "CommentsComment_Id",
                principalSchema: "dbo",
                principalTable: "Comments",
                principalColumn: "Comment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_Users_UserId",
                schema: "dbo",
                table: "Comment_User",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
