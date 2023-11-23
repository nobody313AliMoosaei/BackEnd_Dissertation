using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCommentsTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Comments__Disser__3B75D760",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK__Comments__UserRe__3A81B327",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Replay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Insert_DateTime",
                table: "Comments",
                newName: "InsertDateTime");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comments",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comments",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InversCommentRef",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Comments__3214EC077B918307",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InversCommentRef",
                table: "Comments",
                column: "InversCommentRef");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__Disser__7E37BEF6",
                table: "Comments",
                column: "DissertationRef",
                principalTable: "Dissertations",
                principalColumn: "Dissertation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__Invers__7F2BE32F",
                table: "Comments",
                column: "InversCommentRef",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__UserRe__7D439ABD",
                table: "Comments",
                column: "UserRef",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Comments__Disser__7E37BEF6",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK__Comments__Invers__7F2BE32F",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK__Comments__UserRe__7D439ABD",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Comments__3214EC077B918307",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_InversCommentRef",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "InversCommentRef",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "InsertDateTime",
                table: "Comments",
                newName: "Insert_DateTime");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateTable(
                name: "Replay",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentRef = table.Column<long>(type: "bigint", nullable: true),
                    ReplayId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replay", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Replay__CommentR__3E52440B",
                        column: x => x.CommentRef,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK__Replay__ReplayId__3F466844",
                        column: x => x.ReplayId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Replay_CommentRef",
                table: "Replay",
                column: "CommentRef");

            migrationBuilder.CreateIndex(
                name: "IX_Replay_ReplayId",
                table: "Replay",
                column: "ReplayId");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__Disser__3B75D760",
                table: "Comments",
                column: "DissertationRef",
                principalTable: "Dissertations",
                principalColumn: "Dissertation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Comments__UserRe__3A81B327",
                table: "Comments",
                column: "UserRef",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
