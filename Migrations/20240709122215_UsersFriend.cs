using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkStream_API.Migrations
{
    /// <inheritdoc />
    public partial class UsersFriend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserUid",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserUid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserUid",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserUid = table.Column<string>(type: "varchar(36)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Friend_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserUid",
                table: "Friend",
                column: "UserUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.AddColumn<string>(
                name: "UserUid",
                table: "Users",
                type: "varchar(36)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserUid",
                table: "Users",
                column: "UserUid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserUid",
                table: "Users",
                column: "UserUid",
                principalTable: "Users",
                principalColumn: "Uid");
        }
    }
}
