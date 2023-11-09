using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class BASLookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CollegeRef",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BASLookup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BASLooku__3214EC07A4E6B86E", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BASLookup",
                columns: new[] { "Id", "Code", "Description", "Title", "Type" },
                values: new object[,]
                {
                    { 1L, 1, null, "مهندسي کامپيوتر", "CollegesUni" },
                    { 2L, 2, null, "مهندسي برق", "CollegesUni" },
                    { 3L, 3, null, "مهندسي مکانيک", "CollegesUni" },
                    { 4L, 4, null, "مهندسي عمران", "CollegesUni" },
                    { 5L, 5, null, "مهندسي معماري و شهرسازی", "CollegesUni" },
                    { 6L, 6, null, "مهندسي مواد و علوم ميان رشته‌ای", "CollegesUni" },
                    { 7L, 7, null, "حوضه علوم اسلامي", "CollegesUni" },
                    { 8L, 8, null, "مرکز آموزش الکترونيکي", "CollegesUni" },
                    { 9L, 9, null, "علوم پايه", "CollegesUni" },
                    { 10L, 10, null, "علوم ورزشي", "CollegesUni" },
                    { 11L, 11, null, "علوم انساني", "CollegesUni" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CollegeRef",
                table: "AspNetUsers",
                column: "CollegeRef");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex1",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_aspnetUsers_BasLookup",
                table: "AspNetUsers",
                column: "CollegeRef",
                principalTable: "BASLookup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aspnetUsers_BasLookup",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BASLookup");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CollegeRef",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CollegeRef",
                table: "AspNetUsers");
        }
    }
}
