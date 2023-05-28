using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddComment_UserToCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Comment_User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CommentsComment_Id = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_User_Comments_CommentsComment_Id",
                        column: x => x.CommentsComment_Id,
                        principalSchema: "dbo",
                        principalTable: "Comments",
                        principalColumn: "Comment_Id");
                    table.ForeignKey(
                        name: "FK_Comment_User_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_User_CommentsComment_Id",
                schema: "dbo",
                table: "Comment_User",
                column: "CommentsComment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_User_UserId",
                schema: "dbo",
                table: "Comment_User",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment_User",
                schema: "dbo");

            migrationBuilder.AddColumn<decimal>(
                name: "CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                column: "CommentsComment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Comments_CommentsComment_Id",
                schema: "dbo",
                table: "Users",
                column: "CommentsComment_Id",
                principalSchema: "dbo",
                principalTable: "Comments",
                principalColumn: "Comment_Id");
        }
    }
}
